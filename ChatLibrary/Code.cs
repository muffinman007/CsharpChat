using System;

namespace ChatLibrary{
	public static class Code{
		public static readonly int BUFFER_SIZE = 1024;
		public static readonly int DEFAULT_PORT = 12345;

		public enum SessionStatus{ JOINED, IN_SESSION, DISCONNECT, SERVER_STOP, NOP }
	}
}

