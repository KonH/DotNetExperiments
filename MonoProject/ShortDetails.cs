using System;

namespace MonoProject {
	static class ShortDetails {
		// Int16 = 2 bytes

		public static void Run() {
			ToBytes(short.MinValue);
			ToBytes(-1);
			ToBytes(0);
			ToBytes(1);
			ToBytes(short.MaxValue);
		}

		static void ToBytes(short value) {
			var bytes = BitConverter.GetBytes(value);
			Console.WriteLine($"{value,6} = {value,4:X} = {Utils.GetBitString(bytes)}");
		}
	}
}