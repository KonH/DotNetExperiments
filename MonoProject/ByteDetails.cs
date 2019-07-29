using System;
using System.Diagnostics;

namespace MonoProject {
	static class ByteDetails {
		public static void Run() {
			ToByte(byte.MinValue);
			ToByte(byte.MaxValue);
			Console.WriteLine($"Is checked mode enabled by default? {IsCheckedModeEnabled()}");
			{
				var b1 = 255;
				var b2 = 1;
				Console.WriteLine($"Overflow (unchecked): {Utils.Try(() => (byte)(b1 + b2))}");
			}
			{
				byte b1 = 255;
				byte b2 = 1;
				Console.WriteLine($"Overflow (checked): {Utils.Try(() => checked((byte) (b1 + b2)))}");
			}
			{
				byte b1 = 0;
				byte b2 = 1;
				Console.WriteLine($"Underflow (unchecked): {Utils.Try(() => (byte)(b1 - b2))}");
			}
			{
				byte b1 = 0;
				byte b2 = 1;
				Console.WriteLine($"Underflow (checked): {Utils.Try(() => checked((byte) (b1 - b2)))}");
			}
			{
				CheckedByte b1 = 255;
				byte b2 = 1;
				Console.WriteLine($"Overflow (always checked): {Utils.Try(() => (b1 + b2))}");
			}
			{
				CheckedByte b1 = 0;
				byte b2 = 1;
				Console.WriteLine($"Underflow (always checked): {Utils.Try(() => (b1 - b2))}");
			}
			Console.WriteLine($"Parse: {byte.Parse("255")}");
			Console.WriteLine($"Parse (overflow): {Utils.Try(() => byte.Parse("256"))}");
		}
		
		static void ToByte(byte value) {
			Console.WriteLine($"{value,-3} = {value,-2:X} = {Utils.GetBitString(value)}");
		}

		static bool IsCheckedModeEnabled() {
			try {
				byte b1 = 0;
				byte b2 = 1;
				var result = (byte)(b1-b2);
				Debug.Assert(result == byte.MaxValue);
				return false;
			} catch (OverflowException) {
				return true;
			}
		}
		
		// Debug/Release differences:
		// .locals: 3 vs 1 (b1, b2, return value vs b2 only)
		// Less operations
		
		// Overflow check off/on:
		// add/add.ovf
		// conv.u1/conv.ovf.u1
		
		// 7 op codes
		
		public static byte JustAddBytes() {
			byte b1 = 1;
			byte b2 = 1;
			return (byte)(b1 + b2);
		}
		
		public static byte JustAddBytesChecked() {
			checked {
				byte b1 = 1;
				byte b2 = 1;
				return (byte) (b1 + b2);
			}
		}
		
		// 8 op codes, 3 calls included
		// 3 + 3 + 13 = 19 op codes inside calls
		// Roughly 17 op codes, +10 in difference
		
		public static CheckedByte JustAddCheckedBytes() {
			CheckedByte b1 = 1;
			CheckedByte b2 = 1;
			return b1 + b2;
		}
		
		// x62 slower
		//|            Method |       Mean |     Error |    StdDev |
		//|------------------ |-----------:|----------:|----------:|
		//|              Byte |  0.3435 ns | 0.0031 ns | 0.0029 ns |
		//|  ByteCheckedBlock |  0.3526 ns | 0.0058 ns | 0.0054 ns |
		//| CheckedByteStruct | 21.7849 ns | 0.0337 ns | 0.0299 ns |
		
		// x30 slower with aggressive inlining
		//|            Method |       Mean |     Error |    StdDev |
		//|------------------ |-----------:|----------:|----------:|
		//|              Byte |  0.4148 ns | 0.0937 ns | 0.1002 ns |
		//|  ByteCheckedBlock |  0.4023 ns | 0.0608 ns | 0.0539 ns |
		//| CheckedByteStruct | 12.3198 ns | 0.2624 ns | 0.2455 ns |
	}
}