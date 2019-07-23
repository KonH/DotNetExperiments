using System;

namespace MonoProject {
	static class DetailsWrapper {
		public static void Run(string name, Action act) {
			Console.WriteLine($"{name}:");
			Console.WriteLine();
			act();
			Console.WriteLine();
		}
	}
}