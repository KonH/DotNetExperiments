using System;

namespace MonoProject {
	static class IntDetails {
		// Int32 = 4 bytes
		
		public static void Run() {
			ToBytes(int.MinValue);
			ToBytes(int.MaxValue);
		}
		
		static void ToBytes(int value) {
			var bytes = BitConverter.GetBytes(value);
			Console.WriteLine($"{value,11} = {value,8:X} = {Utils.GetBitString(bytes)}");
		}
	}
}