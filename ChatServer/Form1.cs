using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ChatLibrary;

namespace ChatServer{
    public partial class Form1 : Form{		
		
		static object myLock = new object();
		static TextBox log;
		
		public static Dictionary<string, Client> clientsConnectionDict =  new Dictionary<string, Client>();
		public static List<string> friendsList = new List<string>();

		CancellationTokenSource tokenSource = null;

		Task runServerTask;

		public static bool isFormClosing = false;

        public Form1(){
            InitializeComponent();
			log = tbLog; 
        }

		static int port;
		static TcpListener chatServer = null;
        private void btnRun_Click(object sender, EventArgs e){

			if(!int.TryParse(tbPort.Text, out port)){
				port = Code.DEFAULT_PORT;
			}
			else{
				if(port < 0 || port > short.MaxValue){
					MessageBox.Show("Port out-of-range" + Environment.NewLine + "Port range: 1 - 65523", "Port Range Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tbPort.Clear();
					tbLog.InvokedIfRequired(() => { this.tbLog.Text = "WARNING: Chat Server NOT Started."; });
					return;
				}
			}

			if(chatServer == null){
				chatServer = new TcpListener(IPAddress.Any, port);
			}
			
			chatServer.Server.NoDelay = true;
			chatServer.Server.LingerState = new LingerOption(false, 0);

			tbLog.InvokedIfRequired(()=>{ this.tbLog.AppendText("Chat Server Started." + Environment.NewLine); });
			tokenSource = new CancellationTokenSource();
			runServerTask = Task.Factory.StartNew(() => { RunServer(); }, tokenSource.Token);
			

			btnRun.Enabled = false;
			btnStop.Enabled = true;
			tbPort.Text = port.ToString();
			tbLog.Focus();
	    }

        void RunServer(){
			TcpClient clientSocket;            
			
			try{
				chatServer.Start(24);
			}
			catch(Exception ex){
				tbLog.InvokedIfRequired(()=>{ this.tbLog.AppendText("ERROR: Chat server cannot start!" + Environment.NewLine + ex.Message + Environment.NewLine); });
				btnRun.InvokedIfRequired(()=>{ this.btnRun.Enabled = true; this.btnRun.Focus();});
				btnStop.InvokedIfRequired(()=>{ this.btnStop.Enabled = false; });
				return;
			}
			
            while(true){
				if(tokenSource.IsCancellationRequested){ 
					chatServer.Stop();
				}
				else{
					try{
						clientSocket = chatServer.AcceptTcpClient();
					
						clientSocket.NoDelay = true;
						clientSocket.Client.LingerState = new LingerOption(false, 0);

						DataPackage package;
						using(MemoryStream ms = new MemoryStream()){
							BinaryFormatter bf = new BinaryFormatter();
							byte[] buffer = new byte[Code.BUFFER_SIZE];
							int bytesRead = 0;
							do{
								bytesRead = clientSocket.GetStream().Read(buffer, 0, Code.BUFFER_SIZE);
								ms.Write(buffer, 0, bytesRead);
							}while(clientSocket.GetStream().DataAvailable);

							ms.Seek(0, SeekOrigin.Begin);
							package = (DataPackage)bf.Deserialize(ms);
						}

						Client client = new Client(clientSocket);
						client.UserNick = package.userNick;

						lock(myLock){
							clientsConnectionDict.Add(package.userNick, client);
							friendsList.Add(package.userNick);
							tbLog.InvokedIfRequired(() => { this.tbLog.AppendText(clientSocket.Client.RemoteEndPoint.ToString() + " connected to chat server" + Environment.NewLine); });
						}				
				
						ChatManager(package);

						// keep the socket connection alive 
						ServiceClient sc = new ServiceClient();
						// must check if there's data just in case an exception happens during the process of adding in new user.
						if(clientsConnectionDict.ContainsKey(package.userNick))
							sc.StartService(client);
					}
					catch(SocketException se){
						log.InvokedIfRequired(()=>{ log.AppendText(Environment.NewLine + se.Message + Environment.NewLine); });
						break;
					}
					catch(Exception ex){
						log.InvokedIfRequired(()=>{ log.AppendText(Environment.NewLine + ex.Message + Environment.NewLine); });
						this.InvokedIfRequired(()=>{ this.btnStop_Click(this, null); });
					}
				}
            }// while
        }

		public static void ChatManager(DataPackage package){
			if(clientsConnectionDict.Count > 0){
				switch(package.status){
					case Code.SessionStatus.JOINED:
						Broadcast(package);
						break;

					case Code.SessionStatus.IN_SESSION:
						Broadcast(package);
						break;

					case Code.SessionStatus.DISCONNECT:
						// remove user from both dictionary
						// and then broadcast to everyone that user has disconnected
						Client temp = clientsConnectionDict[package.userNick].DeepClone();
						
						lock(myLock){
							clientsConnectionDict.Remove(package.userNick);
							friendsList.Remove(package.userNick);
						}
						Broadcast(package);					
						log.InvokedIfRequired(()=>{ log.AppendText(temp.IP + " disconnected from chat server" + Environment.NewLine); });				
						break;
				}
			}
		}
		
		
        static void Broadcast(DataPackage package){
			if(clientsConnectionDict.Count > 0){
				Client temp = null;
				try{
					lock(myLock){
						byte[] buffer;
						using(MemoryStream ms = new MemoryStream()){
							package.friendsList = Form1.friendsList.ToArray();
							BinaryFormatter bf = new BinaryFormatter();
							bf.Serialize(ms, package);
							ms.Seek(0, SeekOrigin.Begin);
							buffer = ms.ToArray();
						}

						switch(package.status){
							case Code.SessionStatus.IN_SESSION:
								foreach(var pair in clientsConnectionDict){
									if(pair.Value.UserNick != package.userNick){
										temp = pair.Value;
										temp.TcpSocket.GetStream().Write(buffer, 0, buffer.Length);
									}
								}
								break;

							default:
								foreach(var pair in clientsConnectionDict){
									temp = pair.Value;
									temp.TcpSocket.GetStream().Write(buffer, 0, buffer.Length);
								}
								break;						
						}// switch
					}// lock
				}// try
				catch(Exception){  // on any kind of error close connection
					if(!temp.TcpSocket.Client.IsConnected()){
						DataPackage dp = new DataPackage(){ userNick = temp.UserNick, status = Code.SessionStatus.DISCONNECT };
 						ChatManager(dp);
					}
				}
			}// if
		}


		public class ServiceClient{
			// keeping each connection alive
			Client client;

			public void StartService(Client client){
				this.client = client;
				Thread newServiceThread = new Thread(ServiceChat);
				newServiceThread.Start();
			}

			private void ServiceChat(){
				int bytesRead = 0;
				bool stopThread = false;
				byte[] buffer = new byte[Code.BUFFER_SIZE];
				BinaryFormatter bf = new BinaryFormatter();

				while(true){
					try{
						if(client.TcpSocket.Client.IsConnected()){
							NetworkStream ns = client.TcpSocket.GetStream();
							if(ns.DataAvailable){
								using(MemoryStream ms = new MemoryStream()){
									do{
										bytesRead = ns.Read(buffer, 0, Code.BUFFER_SIZE);
										if(bytesRead == 0)
											break;
										ms.Write(buffer, 0, bytesRead);
									}while(ns.DataAvailable);

									ms.Seek(0, SeekOrigin.Begin);

									log.InvokedIfRequired(()=>{ Form1.log.AppendText("Incoming data: " + client.IP + Environment.NewLine); });

									Form1.ChatManager((DataPackage)bf.Deserialize(ms));

									log.InvokedIfRequired(()=>{ Form1.log.AppendText("Data sent: " + client.IP + Environment.NewLine); });
								}
							}
						}
						else{
							throw new Exception();
						}
					}
					catch(Exception){
						if(!Form1.isFormClosing){
							client.TcpSocket.Client.Shutdown(SocketShutdown.Both);
							client.TcpSocket.Client.Close();
							client.TcpSocket.Client.Dispose();
							client.TcpSocket = null;						
							DataPackage package = new DataPackage(){ userNick = client.UserNick, status = Code.SessionStatus.DISCONNECT };
							Form1.ChatManager(package);
						}
						stopThread = true;
					}

					if(stopThread)
						break;
				}
			}
		}

		private void btnStop_Click(object sender, EventArgs e) {
			DialogResult dr;
			dr = MessageBox.Show("Stopping server" + Environment.NewLine + "Continue?", "", MessageBoxButtons.YesNo);
			if(dr == DialogResult.Yes){
				lock(myLock){
					// send Status.Code.ServerStopped to all connection
					using(MemoryStream ms = new MemoryStream()){
						BinaryFormatter bf = new BinaryFormatter();
						bf.Serialize(ms, new DataPackage(){status = Code.SessionStatus.SERVER_STOP });
						ms.Seek(0, SeekOrigin.Begin);
						byte[] buffer = ms.ToArray();
						foreach(var pair in clientsConnectionDict){
							try{
								pair.Value.TcpSocket.GetStream().Write(buffer, 0, buffer.Length);
							}
							catch(Exception){
							}
						}
					}

					// close all connection
					foreach(var pair in clientsConnectionDict){
						pair.Value.TcpSocket.Client.Shutdown(SocketShutdown.Both);
						pair.Value.TcpSocket.Client.Close();
						pair.Value.TcpSocket.Client.Dispose();
						pair.Value.TcpSocket = null;
					}

					clientsConnectionDict.Clear();
					tokenSource.Cancel();
				}// lock

				tbLog.AppendText("Server stopped." + Environment.NewLine);
				btnStop.Enabled = false;
				btnRun.Enabled = true;
				btnRun.Focus();
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e){
			isFormClosing = true;

			if(tokenSource != null)
				tokenSource.Cancel();

			// send Status.Code.ServerStopped to all connection
			using(MemoryStream ms = new MemoryStream()){
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(ms, new DataPackage(){status = Code.SessionStatus.SERVER_STOP });
				ms.Seek(0, SeekOrigin.Begin);
				byte[] buffer = ms.ToArray();
				foreach(var pair in clientsConnectionDict){
					try{
						pair.Value.TcpSocket.GetStream().Write(buffer, 0, buffer.Length);
					}
					catch(Exception){
					}
				}
			}			

			if(clientsConnectionDict.Count > 0){
				foreach(var pair in clientsConnectionDict){
					try{
						pair.Value.TcpSocket.Client.Shutdown(SocketShutdown.Both);
						pair.Value.TcpSocket.Client.Close();
						pair.Value.TcpSocket.Client.Dispose();
						pair.Value.TcpSocket = null;
					}
					catch(Exception){}
				}
				clientsConnectionDict.Clear();
			}
		}		
    }// end Form1


	public static class ControlExtension{ 
		public static void InvokedIfRequired(this Control control, Action action){
			if(control.InvokeRequired){
				control.Invoke(action);
			}
			else{
				action();
			}
		}
	}


	public static class SocketExtension{
		public static bool IsConnected(this Socket clientSocket){
			bool blockingState = clientSocket.Blocking;
			bool objectDisposedExceptionThrown = false;
			try{
				clientSocket.Blocking = false;
				byte[] testData = new byte[1];
				clientSocket.Send(testData, 0, 0, SocketFlags.None);
				return true;
			}
			catch(SocketException se){
				// WSAEWOULDBLOCK = 10035
				if(se.NativeErrorCode == 10035)
					return true;
				else
					return false;
			}
			catch(ObjectDisposedException){
				objectDisposedExceptionThrown = true;
				return false;
			}
			finally{
				if(!objectDisposedExceptionThrown)
					clientSocket.Blocking = blockingState;
			}
		}
	}


}
