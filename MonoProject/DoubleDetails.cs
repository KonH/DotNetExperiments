using System;

namespace MonoProject {
	static class DoubleDetails {
		// Double = 8 bytes
		// IEEE 754
		
		// Part	                                 Bits
		// Significand or mantissa	             0-51
		// Exponent	                            52-62
		// Sign (0 = Positive, 1 = Negative)	   63
		
		// num = mantissa * pow(basement, exponent)
		// basement = 2
		
		public static void Run() {
			ToBytes("MinValue", double.MinValue);
			ToBytes("MaxValue", double.MaxValue);
			ToBytes("Epsilon", double.Epsilon);
			ToBytes("NegativeInfinity", double.NegativeInfinity);
			ToBytes("PositiveInfinity", double.PositiveInfinity);
			ToBytes("NaN", double.NaN);
			ToBytes("10", 10);     //   5 * pow(2,  1) =   5 * 2 =   10
			ToBytes("1000", 1000); // 125 * pow(2,  3) = 125 * 8 = 1000
			ToBytes("0.5", 0.5);   //   1 * pow(2, -1) = 1 * 0.5 =  0.5
		}
		
		static void ToBytes(string name, double value) {
			var bytes = BitConverter.GetBytes(value);
			Console.WriteLine($"{name,16}: {value,24} = {Utils.GetBitString(bytes)} = {ToExactString(value)}");
		}
		
		static string ToExactString (double d) {
			// Translate the double into sign, exponent and mantissa.
			var bits = BitConverter.DoubleToInt64Bits(d);
			// Note that the shift is sign-extended, hence the test against -1 not 1
			var negative = (bits & (1L << 63)) != 0;
			var exponent = (int) ((bits >> 52) & 0x7ffL);
			var mantissa = bits & 0xfffffffffffffL;
			
			if ( exponent == 0 ) {
				// Subnormal numbers; exponent is effectively one higher,
				// but there's no extra normalisation bit in the mantissa
				exponent++;
			} else {
				// Normal numbers; leave exponent as it is but add extra
				// bit to the front of the mantissa
				mantissa |= (1L << 52);
			}

			// Bias the exponent. It's actually biased by 1023, but we're
			// treating the mantissa as m.0 rather than 0.m, so we need
			// to subtract another 52 from it.
			exponent -= 1075;

			if ( mantissa == 0 ) {
				return negative ? "-0" : "0";
			}

			/* Normalize */
			while ( (mantissa & 1) == 0 ) {
				/*  i.e., Mantissa is even */
				mantissa >>= 1;
				exponent++;
			}
			return $"sign: {(negative ? "-" : "+")}, mantissa: {mantissa,17}, exponent: {exponent}";
		}
	}
}