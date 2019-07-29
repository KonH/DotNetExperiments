using BenchmarkDotNet.Running;

namespace MonoProject {
	static class Program {
		public static void Main(string[] args) {
			RunDetails();
		}

		static void RunDetails() {
			DetailsWrapper.Run("byte", ByteDetails.Run);
			DetailsWrapper.Run("boolean", BooleanDetails.Run);
			DetailsWrapper.Run("sbyte", SbyteDetails.Run);
			DetailsWrapper.Run("short", ShortDetails.Run);
			DetailsWrapper.Run("int", IntDetails.Run);
			DetailsWrapper.Run("long", LongDetails.Run);
		}

		static void RunBenchmarks() {
			BenchmarkRunner.Run<ByteVsCheckedByte>();
		}
	}
}