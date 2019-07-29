using System;

namespace MonoProject {
	static class LongDetails {
		// Int64 = 8 bytes
		
		public static void Run() {
			ToBytes(long.MinValue);
			ToBytes(long.MaxValue);
		}
		
		static void ToBytes(long value) {
			var bytes = BitConverter.GetBytes(value);
			Console.WriteLine($"{value,20} = {value,16:X} = {Utils.GetBitString(bytes)}");
		}
	}
}