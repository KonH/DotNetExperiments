using System;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Attributes;

namespace MonoProject {
/*
	             Method |      Mean |     Error |    StdDev | Scaled | ScaledSD |  Gen 0 |
----------------------- |----------:|----------:|----------:|-------:|---------:|-------:|
               Baseline | 0.0473 ns | 0.0002 ns | 0.0002 ns |   1.00 |     0.00 |      - |
                Closure | 0.3850 ns | 0.0025 ns | 0.0021 ns |   8.14 |     0.05 | 0.0368 |
       ValueFuncClosure | 0.3293 ns | 0.0028 ns | 0.0026 ns |   6.96 |     0.06 | 0.0315 |
 ValueFuncClosureCached | 0.0793 ns | 0.0002 ns | 0.0001 ns |   1.68 |     0.01 |      - |
*/
	[MonoJob]
	[MemoryDiagnoser]
	public class ClosureVsValueClosure {
		[Benchmark(Baseline = true, OperationsPerInvoke = 100)]
		public int Baseline() => ClosureTest.JustCall();

		[Benchmark(OperationsPerInvoke = 100)]
		public int Closure() => ClosureTest.Closure();

		[Benchmark(OperationsPerInvoke = 100)]
		public int ValueFuncClosure() => ClosureTest.ValueFuncClosure();

		[Benchmark(OperationsPerInvoke = 100)]
		public int ValueFuncClosureCached() => ClosureTest.ValueFuncClosureCached();
	}

	public static class ClosureTest {
		struct ValueFuncInvoker<T1, T2> {
			public readonly Func<T1, T2> Method;
			public T1 Value;

			public ValueFuncInvoker(Func<T1, T2> method) {
				Method = method;
				Value = default;
			}

			public void Apply() {
				Method(Value);
			}
		}

		static int SomeMethod(int value) {
			return value + 1;
		}

		public static void Run() {
			JustCall();
			Console.WriteLine(Closure());
			Console.WriteLine(ValueFuncClosure());
			Console.WriteLine(ValueFuncClosureCached());
		}

		public static int JustCall() {
			var value = 50;
			value++;
			return SomeMethod(value);
		}

		public static int Closure() {
			var value = 100;
			Func<int> closure = () => SomeMethod(value); // Allocation for internal class with given field
			value++;
			return closure(); // Gets changed value because of modified closure class member
		}

		// Faster, little bit less GC pressure
		public static int ValueFuncClosure() {
			var value = 150;
			var invoker = new ValueFuncInvoker<int, int>(SomeMethod); // Allocation for Action, can be cached
			invoker.Value = value;
			value++;
			invoker.Apply(); // Get old value because of pass by value
			return value;
		}

		static ValueFuncInvoker<int, int> _funcInvoker = new ValueFuncInvoker<int, int>(SomeMethod);

		// Faster, no GC allocation
		public static int ValueFuncClosureCached() {
			var value = 200;
			var invoker = _funcInvoker;
			invoker.Value = value;
			value++;
			invoker.Apply(); // Get old value because of pass by value
			return value;
		}
	}
}