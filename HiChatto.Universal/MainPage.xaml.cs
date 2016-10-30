using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Net.Sockets;
using Windows.UI.Xaml.Documents;
using Windows.UI;
using System.Text;
using Windows.UI.Popups;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HiChatto.Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        Socket socket;
        SocketAsyncEventArgs receive_ev;
        SocketAsyncEventArgs send_ev;
        IPEndPoint ipe;
        byte[] rc_buff;
        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            rc_buff = new byte[8096];
            MessageDialog dia = new MessageDialog("Connected");
            await dia.ShowAsync();
            ipe = new IPEndPoint(IPAddress.Parse(txtIP.Text), 6969);
            if (socket != null)
            {
                socket.Dispose();
            }
            SocketAsyncEventArgs connect_ev = new SocketAsyncEventArgs();
            connect_ev.RemoteEndPoint = ipe;
            connect_ev.Completed += Connect_Completed;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.ConnectAsync(connect_ev);
            send_ev = new SocketAsyncEventArgs();
            send_ev.Completed += SendAsync_Completed;
            receive_ev = new SocketAsyncEventArgs();
            receive_ev.Completed += ReceiveAsync_Completed;
            receive_ev.SetBuffer(rc_buff,0,rc_buff.Length);
            btnSend.IsEnabled = true;
            RecivedAsync();
        }

        private void ReceiveAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            ShowMessage(string.Format("Received {0} bytes", e.BytesTransferred, ipe.ToString()), Colors.Red, Colors.White);
            RecivedAsync();
        }
        private void Connect_Completed(object sender, SocketAsyncEventArgs e)
        {
            ShowMessage(string.Format("Connected to {0}", ipe.ToString()), Colors.Green, Colors.White);
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            byte[] buff = UnicodeEncoding.UTF8.GetBytes(txtMess.Text);
            SendAsync(buff);
        }
        void RecivedAsync()
        {
            socket.ReceiveAsync(receive_ev);
        }
        private void SendAsync(byte[] buff)
        {
            send_ev.SetBuffer(buff, 0, buff.Length);
            socket.SendAsync(send_ev);
        }
        private void SendAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            ShowMessage(string.Format("Sent {0} bytes to {1}", e.BytesTransferred, ipe.ToString()), Colors.Cyan, Colors.White);
        }
        private async void ShowMessage(string msg, Color color, Color background)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Paragraph p = new Paragraph();
                Border b = new Border();
                b.Padding = new Thickness(0, 0, 0, 0);
                b.Margin = new Thickness(0, 0, 0, 0);
                b.Background = new SolidColorBrush(background);
                TextBlock t = new TextBlock();
                t.Foreground = new SolidColorBrush(color);
                t.Text = msg;
                t.VerticalAlignment = VerticalAlignment.Bottom;
                b.Child = t;
                p.Inlines.Add(new InlineUIContainer() { Child = b });
                rtxtResult.Blocks.Add(p);
            });
        }

    }
}
