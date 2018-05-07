using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BinzelApp_Prototype
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Splash); //chamada não necessária
        }

        //Metodo sobrescrito executado quando o app é chamado a atividade
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // sobrescrevendo método de "back_button" como vazio para previnir saída durante splash
        public override void OnBackPressed() { }

        // Metodo para exibir uma tela de inicialização
        async void SimulateStartup()
        {
            //Toast.MakeText(this,"App sendo iniciado!",ToastLength.Long).Show();
            await Task.Delay(2000); //mantendo splash screen por 2s
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}