using BenchmarkDotNet.Running;

namespace MonoProject {
	static class Program {
		public static void Main(string[] args) {
			ClosureTest.Run();
			RunClosureBenchmark();
		}

		static void RunDetails() {
			DetailsWrapper.Run("byte", ByteDetails.Run);
			DetailsWrapper.Run("boolean", BooleanDetails.Run);
			DetailsWrapper.Run("sbyte", SbyteDetails.Run);
			DetailsWrapper.Run("short", ShortDetails.Run);
			DetailsWrapper.Run("int", IntDetails.Run);
			DetailsWrapper.Run("long", LongDetails.Run);
			DetailsWrapper.Run("double", DoubleDetails.Run);
			DetailsWrapper.Run("float", FloatDetails.Run);
		}

		static void RunByteVsCheckedByteBenchmark() {
			BenchmarkRunner.Run<ByteVsCheckedByte>();
		}

		static void RunClosureBenchmark() {
			BenchmarkRunner.Run<ClosureVsValueClosure>();
		}
	}
}