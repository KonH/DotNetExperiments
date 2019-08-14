using System;
using System.Linq;

namespace MonoProject {
	static class SbyteDetails {
		// SByte = 1 byte

		public static void Run() {
			ToByte(sbyte.MinValue);
			ToByte(-1);
			ToByte(0);
			ToByte(1);
			ToByte(sbyte.MaxValue);
			ChangeSign(10, -10);
			ChangeSign(-10, 10);
			ChangeSign(0, 0);
			ChangeSign(127, -127);
			ChangeSign(-128,-128);
		}

		static void ToByte(sbyte value) {
			Console.WriteLine($"{value,4} = {value,2:X} = {Utils.GetBitString(value)}");
		}

		// Negative numbers presented using two’s complement

		static void ChangeSign(sbyte originalValue, sbyte expectedValue) {
			Console.WriteLine($"Change sign ({originalValue}):");
			ToByte(originalValue);
			Console.WriteLine("Result:");
			ToByte(ChangeSign(originalValue));
			Console.WriteLine("Should be:");
			ToByte(expectedValue);
		}

		static sbyte ChangeSign(sbyte value) {
			var bits = Utils.GetBits(value);
			var inverseBits = bits.Select(b => !b).ToArray();
			var inverseByte = Utils.GetByte(inverseBits);
			inverseByte++;
			return (sbyte)inverseByte;
		}
	}
}