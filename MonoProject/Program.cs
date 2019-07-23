using BenchmarkDotNet.Running;

namespace MonoProject {
	static class Program {
		public static void Main(string[] args) {
			RunBenchmarks();
		}

		static void RunDetails() {
			DetailsWrapper.Run("byte", ByteDetails.Run);
			DetailsWrapper.Run("boolean", BooleanDetails.Run);
		}

		static void RunBenchmarks() {
			BenchmarkRunner.Run<ByteVsCheckedByte>();
		}
	}
}