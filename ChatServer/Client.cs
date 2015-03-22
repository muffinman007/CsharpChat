using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ChatServer{
	public class Client{
		public Client(TcpClient socket){
			TcpSocket = socket;

			if(socket != null)  
				IP = socket.Client.RemoteEndPoint.ToString();
		}

		public string IP{ get; private set; }
		public TcpClient TcpSocket{ get; set; }
		
		public string UserNick{ get; set; }

		public Client DeepClone(){
			Client temp = new Client(null);

			temp.IP = this.IP;
			temp.TcpSocket = this.TcpSocket;
			temp.UserNick = this.UserNick;
			return temp;
		}

		
	}
}
