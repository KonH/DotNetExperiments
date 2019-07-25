#include <stdio.h>
#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>

int clean_up_and_exit(MonoDomain *domain, int exit_code) {
	mono_jit_cleanup(domain);
	printf("Shutting down application.\n");
	return exit_code;
}

int main(int argc, char** argv) {
	printf("Starting application.\n");
	MonoDomain *domain = mono_jit_init("DomainName");
	if ( !domain ) {
		printf("Failed to initialize MonoDomain.\n");
		return 1;
	}
	printf("Initialized MonoDomain address: %p.\n", (void*)&domain);
	
	const char *assembly_path = "../SampleProject/bin/Debug/SampleProject.exe"; 
	MonoAssembly *assembly = mono_domain_assembly_open(domain, assembly_path);
	if ( !assembly ) {
		printf("Failed to load assembly.\n");
		return clean_up_and_exit(domain, 1);
	}
	printf("Initialized MonoAssembly address: %p.\n", (void*)&assembly);
	
	printf("Executing entry point.\n");
	mono_jit_exec(domain, assembly, argc, argv);

	printf("Creating managed class type.\n");
	MonoImage *image = mono_assembly_get_image(assembly);
	if ( !image ) {
		printf("Failed to get image.\n");
		return clean_up_and_exit(domain, 1);
	}
	MonoClass *managed_class = mono_class_from_name(image, "SampleProject", "StringConverter");
	if ( !managed_class ) {
		printf("Failed to get class.\n");
		return clean_up_and_exit(domain, 1);
	}
	const char *class_name = mono_class_get_name(managed_class);
	if ( !class_name ) {
		printf("Failed to get class name.\n");
		return clean_up_and_exit(domain, 1);
	}
	printf("Mono class name is '%s'\n", class_name);

	printf("Lookup methods:\n");
	MonoMethod *method;
	void *iter = NULL;
	while ((method = mono_class_get_methods(managed_class, &iter))) {
		const char *method_name = mono_method_get_name(method);
		MonoMethodSignature *method_signature = mono_method_signature(method);
		uint32_t method_param_count = mono_signature_get_param_count(method_signature);
		printf(" - %s (%i)\n", method_name, method_param_count);
	}

	printf("Find constructor.\n");
	MonoMethod *constructor = mono_class_get_method_from_name(managed_class, ".ctor", 1);
	if ( !constructor ) {
		printf("Failed to get constructor.\n");
		return clean_up_and_exit(domain, 1);
	}

	printf("Find ConvertWith method.\n");
	MonoMethod *convert_method = mono_class_get_method_from_name(managed_class, "ConvertWith", 1);
	if ( !convert_method ) {
		printf("Failed to get ConvertWith method.\n");
		return clean_up_and_exit(domain, 1);
	}

	printf("Find ToString method.\n");
	MonoMethod *to_string_method = mono_class_get_method_from_name(managed_class, "ToString", 0);
	if ( !to_string_method ) {
		printf("Failed to get ToString method.\n");
		return clean_up_and_exit(domain, 1);
	}

	printf("Creating managed instance.\n");
	MonoObject *class_instance = mono_object_new(domain, managed_class);
	if ( !class_instance ) {
		printf("Failed to create managed instance.\n");
		return clean_up_and_exit(domain, 1);
	}

	{
		printf("Initialize managed instance.\n");
		MonoString *constructor_arg = mono_string_new(domain, "Some string");
		void *constructor_args[1];
		constructor_args[0] = constructor_arg;
		mono_runtime_invoke(constructor, class_instance, constructor_args, NULL);
	}

	{
		printf("Calling ToString method.\n");
		void *to_string_args[0];
		MonoObject *to_string_result = mono_runtime_invoke(to_string_method, class_instance, to_string_args, NULL);
		MonoString *to_string_result_str = (MonoString*)to_string_result;
		char *to_string_result_chars = mono_string_to_utf8(to_string_result_str);
		printf("Result is '%s'\n", to_string_result_chars);
	}

	{
		printf("Calling ConvertWith method.\n");
		MonoString *convert_arg = mono_string_new(domain, "Base64");
		void *convert_args[1];
		convert_args[0] = convert_arg;
		MonoObject *convert_result = mono_runtime_invoke(convert_method, class_instance, convert_args, NULL);
		MonoString *convert_result_str = (MonoString*)convert_result;
		char *convert_result_chars = mono_string_to_utf8(convert_result_str);
		printf("Result is '%s'\n", convert_result_chars);
	}

	{
		printf("Calling ConvertWith method.\n");
		MonoString *convert_arg = mono_string_new(domain, "Other");
		void *convert_args[1];
		convert_args[0] = convert_arg;
		MonoObject *exc = NULL;
		MonoObject *convert_result = mono_runtime_invoke(convert_method, class_instance, convert_args, &exc);
		if ( exc ) {
			MonoException* mono_exc = (MonoException*)exc;
			MonoClass *exc_class = mono_object_get_class(exc);
			const char *exc_name = mono_class_get_name(exc_class);
			printf("Exception caught: '%s'.\n", exc_name);
		}
	}

	return clean_up_and_exit(domain, 0);
}