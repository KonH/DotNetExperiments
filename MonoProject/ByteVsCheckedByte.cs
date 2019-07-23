using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace MonoProject {
	[MonoJob]
	public class ByteVsCheckedByte {
		[Benchmark]
		public byte Byte() => ByteDetails.JustAddBytes();
		[Benchmark]
		public byte ByteCheckedBlock() => ByteDetails.JustAddBytesChecked();
		[Benchmark]
		public CheckedByte CheckedByteStruct() => ByteDetails.JustAddCheckedBytes();
	}
}