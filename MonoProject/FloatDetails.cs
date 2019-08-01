using System;

namespace MonoProject {
	static class FloatDetails {
		// Single = 4 bytes
		// IEEE 754
		
		// Part	                                 Bits
		// Significand or mantissa	             0-22
		// Exponent	                            23-30
		// Sign (0 = Positive, 1 = Negative)	   31
		
		// num = mantissa * pow(basement, exponent)
		// basement = 2
		
		public static void Run() {
			ToBytes("MinValue", float.MinValue);
			ToBytes("MaxValue", float.MaxValue);
			ToBytes("Epsilon", float.Epsilon);
			ToBytes("NegativeInfinity", float.NegativeInfinity);
			ToBytes("PositiveInfinity", float.PositiveInfinity);
			ToBytes("NaN", float.NaN);
			ToBytes("10", 10);
			ToBytes("1000", 1000);
			ToBytes("0.5", 0.5f);
		}
		
		static void ToBytes(string name, float value) {
			var bytes = BitConverter.GetBytes(value);
			Console.WriteLine($"{name,16}: {value,24} = {Utils.GetBitString(bytes)}");
		}
	}
}