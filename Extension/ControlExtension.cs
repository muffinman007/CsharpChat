using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer{
	public static class ControlExtension{ 
		public static void InvokedIfRequired(this Control control, Action action){
			if(control.InvokeRequired){
				control.Invoke(action);
			}
			else{
				action();
			}
		}

		public static void AppendTextNewLine(this TextBox control, string text){
			control.AppendText(text + Environment.NewLine);
		}
	}
}
