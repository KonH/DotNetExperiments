using System;

namespace MonoProject {
	static class BooleanDetails {
		public static void Run() {
			var falseValue = False();
			var trueValue = True();
			ToByte(falseValue);
			ToByte(trueValue);
			Console.WriteLine($"Always false: {Compare(True(), False())}");
		}

		static void ToByte(bool value) {
			var b = Convert.ToByte(value);
			Console.WriteLine($"{value, -5} = {b:N} = {b:X} = {Utils.GetBitString(b)}");
		}

		static bool False() {
			return false;
		}

		static bool True() {
			return true;
		}

		static bool Compare(bool x, bool y) {
			return x == y;
		}
	}
}