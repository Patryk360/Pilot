using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Android.App;
using Android.Content;
using Android.Widget;
using WebSocketSharp;

namespace Pilot
{
    public class WebSocket
    {
        public event EventHandler ConnectionClosed;
        private Context context;
        private Activity activity;
        private WebSocketSharp.WebSocket webSocket;
        public WebSocket(Context context, Activity activity)
        {
            this.context = context;
            this.activity = activity;
            Start();
        }
        public void Start()
        {
            Task.Run(() => {
                InitializeWebSocket();
            });
        }
        private void InitializeWebSocket()
        {
            webSocket = new WebSocketSharp.WebSocket("ws://192.168.0.3:8080");
            webSocket.WaitTime = TimeSpan.FromSeconds(3);
            webSocket.OnMessage += WebSocket_OnMessage;
            webSocket.OnError += WebSocket_OnError;
            webSocket.OnClose += WebSocket_OnClose;

            webSocket.Connect();
        }
        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            activity.RunOnUiThread(() => {
                ShowToast(e.Data);
            });
            Debug.WriteLine("Received message: " + e.Data);
        }
        private void WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Close();
            Debug.WriteLine("Error: " + e.Message);
        }
        private void WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }
        private void ShowToast(string message)
        {
            Toast.MakeText(context, message, ToastLength.Short).Show();
        }
        public void SendMessage(string message)
        {
            if (webSocket.IsAlive)
            {
                ShowToast("send message");
                webSocket.Send(message);
            }
            else
            {
                ShowToast("connection error");
            }
        }
        public void Close()
        {
            if (webSocket.IsAlive)
            {
                webSocket.Close();
            }
        }
    }
}