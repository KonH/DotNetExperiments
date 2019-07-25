using System;
using System.Text;

namespace SampleProject {
	public class StringConverter {
		readonly string _source;

		public StringConverter(string source) {
			_source = source;
			Console.WriteLine("Managed instance created.");
		}

		public string ConvertWith(string method) {
			var bytes = Encoding.UTF8.GetBytes(_source);
			switch ( method ) {
				case "Base64": return Convert.ToBase64String(bytes);
				default: throw new NotSupportedException($"Method '{method}' is not yet implemented!");
			}
		}

		public override string ToString() {
			return $"StringConverter with '{_source}'";
		}
	}
}