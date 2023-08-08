using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace Pilot
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private WebSocket ws;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Button button = FindViewById<Button>(Resource.Id.click_me);
            button.Click += Button_Click;
            
            ws = new WebSocket(this, this);
            ws.ConnectionClosed += (sender, args) =>
            {
                RunOnUiThread(() => {
                    Toast.MakeText(this, "Połączenie zostało zamknięte", ToastLength.Short).Show();
                });
            };
        }
        private void Button_Click(object sender, System.EventArgs e)
        {
            ws.SendMessage("hejka");
        }
    }
}