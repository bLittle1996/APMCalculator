
namespace APMCalculator
{
	public class KeyboardEvent
	{
		public int keyCode;
		public string key;
		public long timestamp;
		private static KeysConverter keysConverter = new KeysConverter();
		public KeyboardEvent(int keyCode, DateTimeOffset timestamp)
		{
			this.keyCode = keyCode;
			this.key = keysConverter.ConvertToString(keyCode);
			this.timestamp = timestamp.ToUnixTimeMilliseconds();
		}

		// Returns CSV format for timestamp of the KeyboardEvent, and the keyCode pressed
		public override string ToString()
		{
			return $"{this.timestamp},{this.keyCode},{this.key}";
		}
	}
}