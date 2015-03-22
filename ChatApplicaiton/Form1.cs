using System;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


using ChatLibrary;

namespace ChatApplicaiton{
    public partial class Form1 : Form{

		System.Windows.Forms.Timer connectionEnableTimer = new System.Windows.Forms.Timer();
		System.Windows.Forms.Timer disconnectTimer = new System.Windows.Forms.Timer();

		List<string> friendsList = new List<string>();
		string userNick = string.Empty;
		DataPackage messagePackage = new DataPackage();
		static byte[] buffer;

		bool socketErr = false;

		static object myLock = new object();
		
        public Form1(){
            InitializeComponent();

			connectionEnableTimer.Interval = 200;
			connectionEnableTimer.Tick += ConnectEnable_Tick;
			connectionEnableTimer.Start();

			disconnectTimer.Interval = 15;
			disconnectTimer.Tick += Disconnect_Tick;

			tbPort.Text = Code.DEFAULT_PORT.ToString();
        }

		void ConnectEnable_Tick(object sender, EventArgs e){
			if(!string.IsNullOrEmpty(tbIP.Text) && !string.IsNullOrEmpty(tbPort.Text) && !string.IsNullOrEmpty(tbNick.Text.Trim()))
					btnConnect.Enabled = true;
		}

		void Disconnect_Tick(object sender, EventArgs e){
			btnDisconnect_Click(this, null);
			disconnectTimer.Stop();
		}		


		TcpClient clientSocket;
		CancellationTokenSource tokenSource = new CancellationTokenSource();
		Task chatServiceTask;
		private void btnConnect_Click(object sender, EventArgs e){
			IPAddress serverIP;
			if(!IPAddress.TryParse(tbIP.Text, out serverIP)){
				MessageBox.Show("Valid IP Address format:" + Environment.NewLine + "xxx.xxx.xxx.xxx" + Environment.NewLine + "Each x represent a digit between 0 - 9.",
								"IP Adresss Format Error", 
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
				tbIP.Focus();
				return;
			}

			int serverPort;
			if(!int.TryParse(tbPort.Text, out serverPort)){
				MessageBox.Show("Port number must be in the range of:" + Environment.NewLine + "1 - 65535",
								"Port Out-Of-Range",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
				tbPort.Focus();
				return;
			}

			tbNick.Text = tbNick.Text.Trim();
			if(tbNick.TextLength < 2){
				MessageBox.Show("Nick has to be at least 2 characters long.","",MessageBoxButtons.OK); 
				tbNick.Focus();
				return;
			}

			userNick = tbNick.Text;
			messagePackage.userNick = userNick;
			messagePackage.status = Code.SessionStatus.IN_SESSION;

			clientSocket = new TcpClient();
			clientSocket.NoDelay = true;
			clientSocket.Client.LingerState = new LingerOption(false, 0);
			
			try{
				clientSocket.Connect(serverIP, serverPort);
			}
			catch(Exception){
				MessageBox.Show("Cannot connect to chat server" + Environment.NewLine + "Troubleshoot suggestion:" + Environment.NewLine +
					            "Check network connection, network line, and/or server IP address/port",
								"ERROR",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
				btnConnect.Focus();
				return;
			}

			DataPackage package = new DataPackage(){ userNick = this.userNick, status = Code.SessionStatus.JOINED };
			using(MemoryStream ms = new MemoryStream()){
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(ms, package);
				ms.Seek(0, SeekOrigin.Begin);
				ms.CopyTo(clientSocket.GetStream());
			}

			tbChatWindow.InvokedIfRequired(()=>{ this.tbChatWindow.AppendText("Connected with Chat Server successful." + Environment.NewLine); });
			
			chatServiceTask = Task.Factory.StartNew(()=>{ ChatManager(); }, tokenSource.Token);
			
			connectionEnableTimer.Stop();
			btnConnect.Enabled = false;
			btnDisconnect.Enabled = true;
			btnSend.Enabled = true;
			tbMessage.Focus();
		}


		private void ChatManager(){
			byte[] buffer = new byte[Code.BUFFER_SIZE];
			bool stopThread = false;
			int bytesRead = 0;
			DataPackage package;
			BinaryFormatter bf;

			while(true){
				if(!tokenSource.IsCancellationRequested){
					if(!stopThread){
						try{
							// code better to handel this line expection, also to handle memorystream exception
							NetworkStream ns = clientSocket.GetStream();
							if(ns.DataAvailable){
								using(MemoryStream ms = new MemoryStream()){
									do{
										bytesRead = ns.Read(buffer, 0, Code.BUFFER_SIZE);
										ms.Write(buffer, 0, bytesRead);
									}while(ns.DataAvailable);

									ms.Seek(0, SeekOrigin.Begin);
									bf = new BinaryFormatter();
									package = (DataPackage)bf.Deserialize(ms);
									bf = null;
								}

								switch(package.status){
									case Code.SessionStatus.JOINED:
										tbChatWindow.InvokedIfRequired(()=>{ this.tbChatWindow.AppendText(package.userNick + " joined the chat room" + Environment.NewLine); });
										foreach(string name in package.friendsList){
											if(!friendsList.Contains(name))
												friendsList.Add(name);
										}										
										DisplayFriendsList();
										break;

									case Code.SessionStatus.IN_SESSION:
										tbChatWindow.InvokedIfRequired(()=>{
											this.tbChatWindow.AppendText(package.userNick + ": " + System.Text.Encoding.UTF8.GetString(package.data) + Environment.NewLine);
										});
										bool friendsListChange = false;
										foreach(string name in package.friendsList){
											if(!friendsList.Contains(name)){
												friendsList.Add(name);
												friendsListChange = true;
											}
										}
										if(friendsListChange)
											DisplayFriendsList();
										break;

									case Code.SessionStatus.DISCONNECT:
										lock(myLock){
											this.friendsList.Remove(package.userNick);
										}
										tbChatWindow.InvokedIfRequired(()=>{ this.tbChatWindow.AppendText(package.userNick + " left chat room" + Environment.NewLine); });
										DisplayFriendsList();
										break;

									case Code.SessionStatus.SERVER_STOP:
										tbChatWindow.InvokedIfRequired(()=>{ this.tbChatWindow.AppendText("Server stop responding..." + Environment.NewLine); });
										stopThread = true;
										this.InvokedIfRequired(()=>{ this.disconnectTimer.Start(); });
										break;									
								}

								package = null;
							}// if data available
						}// try
						catch(Exception){
							socketErr = true;
							break;
						}
					}// stopTread
				}// tokenSource
				else{
					// Cancellation request
					break;
				}
			}// while

			if(socketErr && !tokenSource.IsCancellationRequested){
				tbChatWindow.InvokedIfRequired(()=>{ 
					tbChatWindow.AppendText("ERROR: Communication with server stopped." + Environment.NewLine);
					btnDisconnect_Click(this, null);
				});

				socketErr = false;
			}
		}

		void DisplayFriendsList(){			
			tbNickList.InvokedIfRequired(()=>{ 
				this.tbNickList.Clear();
				foreach(string nick in friendsList){
					this.tbNickList.AppendText(nick + Environment.NewLine); 
				}
			});
		}


		private void btnDisconnect_Click(object sender, EventArgs e){
			tokenSource.Cancel();

			if(!socketErr){
				messagePackage.data = null;
				messagePackage.status = Code.SessionStatus.DISCONNECT;
				messagePackage.friendsList = null;
				using(MemoryStream ms = new MemoryStream()){
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(ms, messagePackage);
					ms.Seek(0, SeekOrigin.Begin);
					byte[] buffer = ms.ToArray();
					try{
						clientSocket.GetStream().Write(buffer, 0, buffer.Length);
					}
					catch(Exception){}
				}
				
				clientSocket.Client.Shutdown(SocketShutdown.Both);
				clientSocket.Client.Close();
				clientSocket.Client.Dispose();
				clientSocket = null;
			}

			chatServiceTask = null;

			lock(myLock){
				friendsList.Clear();
			}
			tbNickList.Clear();

			tbChatWindow.InvokedIfRequired(()=>{ this.tbChatWindow.AppendText("Disconnected from chat server." + Environment.NewLine); });

			btnSend.Enabled = false;
			btnDisconnect.Enabled = false;
			connectionEnableTimer.Start();
			btnConnect.Focus();
		}

		
		private void btnSend_Click(object sender, EventArgs e){
			if(string.IsNullOrEmpty(tbMessage.Text))
				return;

			messagePackage.data = System.Text.Encoding.UTF8.GetBytes(tbMessage.Text);

			using(MemoryStream ms = new MemoryStream()){
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(ms, messagePackage);
				ms.Seek(0, SeekOrigin.Begin);
				buffer = ms.ToArray();
				try{
					clientSocket.GetStream().Write(buffer, 0, buffer.Length);
				}
				catch(SocketException){
					tbChatWindow.AppendText("ERROR: message not sent, connection issue" + Environment.NewLine);
					btnSend.Focus();
					return;
				}
				buffer = null;

				tbMessage.Focus();
			}

			
			// write user message to tbChatWindow
			tbChatWindow.AppendText(": " + tbMessage.Text + Environment.NewLine);
			tbMessage.Clear();
			tbMessage.Focus();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e){
			if(chatServiceTask != null){
				if(chatServiceTask.Status == TaskStatus.Running)
					tokenSource.Cancel();
			}

			if(clientSocket != null){
				messagePackage.data = null;
				messagePackage.status = Code.SessionStatus.DISCONNECT;
				messagePackage.friendsList = null;
				using(MemoryStream ms = new MemoryStream()){
					BinaryFormatter bf = new BinaryFormatter();
					bf.Serialize(ms, messagePackage);
					ms.Seek(0, SeekOrigin.Begin);
					buffer = ms.ToArray();
					try{
						clientSocket.GetStream().Write(buffer, 0, buffer.Length);
					}
					catch(Exception){}
				}			
				messagePackage = null;

				if(!socketErr){
					clientSocket.Client.Shutdown(SocketShutdown.Both);
					clientSocket.Client.Close();
					clientSocket.Client.Dispose();
					clientSocket = null;
				}
			}

			friendsList.Clear();
			tbNickList.Clear();			
		}

		private void tbMessage_TextChanged(object sender, EventArgs e) {
			lbCounter.Text = tbMessage.TextLength.ToString();
		}

		//private void tbMessage_KeyPress(object sender, KeyPressEventArgs e) {
		//	if(e.KeyChar == (char)Keys.Return && Control.ModifierKeys == Keys.Shift){
		//		btnSend_Click(this, null);
		//		return;
		//	}
		//} 

    }// end form

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


}
