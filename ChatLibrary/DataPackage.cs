using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLibrary{

	[Serializable()]
	public class DataPackage{
		public string userNick = string.Empty;
		public byte[] data = null;

		public string[] friendsList = null;

		public Code.SessionStatus status = Code.SessionStatus.NOP;		
	}
}
