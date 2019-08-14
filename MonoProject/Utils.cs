using System;
using System.Linq;

namespace MonoProject {
	static class Utils {
		public static bool[] GetBits(byte b) {
			var bits = new bool[8];
			for (var i = 0; i < bits.Length; i++) {
				bits[i] = GetBit(b, i);
			}
			return bits;
		}

		public static bool[] GetBits(sbyte b) {
			return GetBits((byte)b);
		}

		public static string GetBitString(byte b) {
			var chars = GetBits(b).Reverse().Select(bit => bit ? '1' : '0').ToArray();
			return new string(chars);
		}

		public static string GetBitString(sbyte b) {
			return GetBitString((byte)b);
		}

		public static string GetBitString(byte[] bytes) {
			var parts = bytes.Select(GetBitString).Reverse();
			return string.Join(" ", parts);
		}

		public static bool GetBit(byte wantedByte, int index) {
			var byteWithBitSet = (1 << index);
			var bitwiseAnd = wantedByte & byteWithBitSet;
			var bitIsSet = bitwiseAnd != 0;
			return bitIsSet;
		}

		public static byte GetByte(bool[] bits) {
			byte acc = 0;
			for (var i = 0; i < 8; i++) {
				var bitSet = (byte)(1 << i);
				acc += bits[i] ? bitSet : (byte)0;
			}
			return acc;
		}

		public static string Try<T>(Func<T> func) {
			try {
				var result = func();
				return result.ToString();
			} catch (Exception e) {
				return e.GetType().Name;
			}
		}
	}
}