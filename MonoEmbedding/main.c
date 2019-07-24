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
	
	const char* assembly_path = "../SampleProject/bin/Debug/SampleProject.exe"; 
	MonoAssembly *assembly = mono_domain_assembly_open(domain, assembly_path);
	if ( !assembly ) {
		printf("Failed to load assembly.\n");
		return clean_up_and_exit(domain, 1);
	}
	printf("Initialized MonoAssembly address: %p.\n", (void*)&assembly);
	
	printf("Executing entry point.\n");
	mono_jit_exec(domain, assembly, argc, argv);

	return clean_up_and_exit(domain, 0);
}