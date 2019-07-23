using System;

namespace MonoProject {
	static class Utils {
		public static string GetBitString(byte b) {
			var chars = new char[8];
			for (var i = 0; i < chars.Length; i++) {
				chars[i] = GetBit(b, i) ? '1' : '0';
			}
			return new string(chars);
		}

		public static bool GetBit(byte wantedByte, int index) {
			var byteWithBitSet = (1 << index);
			var bitwiseAnd = wantedByte & byteWithBitSet;
			var bitIsSet = bitwiseAnd != 0;
			return bitIsSet;
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