#include <stdio.h>
#include <stdbool.h>
#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>

// Summary:
// Just run mono and execute methods on managed class
// Instead of huge number of null checks just shutdown where it appropriate
// (not obvious solution, but leads to less code)

// Manage runtime

MonoDomain *_domain = NULL;

void try_clean_up() {
	if ( _domain ) {
		mono_jit_cleanup(_domain);
		printf("Shutting down Mono runtime.\n");
		_domain = NULL;
	}
}

// Interrupt execution and clean up resources
void fail(const char *reason) {
	printf("%s", reason);
	try_clean_up();
	exit(1);
}

MonoDomain* init_mono_domain(const char *domain_name) {
	printf("Starting Mono runtime.\n");
	MonoDomain *mono_domain = mono_jit_init(domain_name);
	if ( !mono_domain ) {
		fail("Failed to initialize MonoDomain.\n");
	}
	return mono_domain;
}

MonoAssembly* load_mono_assembly(MonoDomain *domain, const char *assembly_path) { 
	MonoAssembly *assembly = mono_domain_assembly_open(domain, assembly_path);
	if ( !assembly ) {
		fail("Failed to load assembly.\n");
	}
	return assembly;
}

void execute_entry_point(MonoDomain* domain, MonoAssembly *assembly, int argc, char **argv) {
	printf("Executing entry point.\n");
	mono_jit_exec(domain, assembly, argc, argv);
}

MonoImage* get_mono_image(MonoAssembly *assembly) {
	printf("Load mono assembly.\n");
	MonoImage *image = mono_assembly_get_image(assembly);
	if ( !image ) {
		fail("Failed to get image.\n");
	}
	return image;
}

MonoImage* init_mono_image(int argc, char **argv, const char *domain_name, const char *assembly_path) {
	_domain = init_mono_domain(domain_name);
	MonoAssembly *assembly = load_mono_assembly(_domain, assembly_path);
	execute_entry_point(_domain, assembly, argc, argv);
	return get_mono_image(assembly);
}

// Manage classes

MonoClass* get_mono_class(MonoImage *image, const char *namespace, const char *name) {
	printf("Creating managed class type.\n");
	MonoClass *managed_class = mono_class_from_name(image, namespace, name);
	if ( managed_class ) {
		const char *class_name = mono_class_get_name(managed_class);
		printf("Mono class name is '%s'\n", class_name);
	} else {
		fail("Failed to get class.\n");
	}
	return managed_class;
}

void print_class_methods(MonoClass *managed_class) {
	printf("Lookup methods:\n");
	MonoMethod *method;
	void *iter = NULL;
	while ((method = mono_class_get_methods(managed_class, &iter))) {
		const char *method_name = mono_method_get_name(method);
		MonoMethodSignature *method_signature = mono_method_signature(method);
		uint32_t method_param_count = mono_signature_get_param_count(method_signature);
		printf(" - %s (%i)\n", method_name, method_param_count);
	}
}

MonoMethod* get_method(MonoClass *managed_class, const char *name, int param_count) {
	printf("Find method '%s'.\n", name);
	MonoMethod *method = mono_class_get_method_from_name(managed_class, name, param_count);
	if ( !method ) {
		fail("Failed to get method.\n");
	}
	return method;
}

MonoMethod* get_constructor(MonoClass *managed_class, int param_count) {
	return get_method(managed_class, ".ctor", param_count);
}

MonoObject* create_new_object(MonoClass *managed_class, int param_count, void *params) {
	printf("Creating managed instance.\n");
	MonoObject *class_instance = mono_object_new(_domain, managed_class);
	if ( !class_instance ) {
		fail("Failed to create managed instance.\n");
	}
	MonoMethod* constructor = get_constructor(managed_class, param_count);
	printf("Initialize managed instance.\n");
	mono_runtime_invoke(constructor, class_instance, params, NULL);
	return class_instance;
}

bool run_method(MonoClass *managed_class, MonoObject* class_instance, const char *method_name, int param_count, void *params, MonoObject **result) {
	MonoMethod* method = get_method(managed_class, method_name, param_count);
	MonoObject *exc = NULL;
	printf("Calling '%s' method.\n", method_name);
	*result = mono_runtime_invoke(method, class_instance, params, &exc);
	if ( exc ) {
		MonoClass *exc_class = mono_object_get_class(exc);
		const char *exc_name = mono_class_get_name(exc_class);
		printf("Exception in '%s' caught: '%s'.\n", method_name, exc_name);
		return false;
	}
	return true;
}

void run_method_with_string_result(MonoClass *managed_class, MonoObject* class_instance, const char *method_name, int param_count, void *params) {
	MonoObject* result = NULL;
	bool success = run_method(managed_class, class_instance, method_name, param_count, params, &result);
	if ( success ) {
		MonoString *result_str = (MonoString*)result;
		const char *result_chars = mono_string_to_utf8(result_str);
		printf("Result of '%s' is '%s'\n", method_name, result_chars);
	}
}

// Entry point

int main(int argc, char** argv) {
	MonoImage *image = init_mono_image(argc, argv, "DomainName", "../SampleProject/bin/Debug/SampleProject.exe");
	MonoClass *managed_class = get_mono_class(image, "SampleProject", "StringConverter");
	print_class_methods(managed_class);
	
	MonoMethod *convert_with_method = get_method(managed_class, "ConvertWith", 1);
	MonoMethod *to_string_method    = get_method(managed_class, "ToString", 0);

	void *constructor_params[1] = { 
		mono_string_new(_domain, "Some string")
	};
	MonoObject *class_instance = create_new_object(managed_class, 1, constructor_params);
	
	void *to_string_params[0];
	run_method_with_string_result(managed_class, class_instance, "ToString", 0, to_string_params);

	{
		void *convert_params[1] = {
			mono_string_new(_domain, "Base64")
		};
		run_method_with_string_result(managed_class, class_instance, "ConvertWith", 1, convert_params);
	}
	{
		void *convert_params[1] = {
			mono_string_new(_domain, "Other")
		};
		run_method_with_string_result(managed_class, class_instance, "ConvertWith", 1, convert_params);
	}

	try_clean_up();
	return 0;
}