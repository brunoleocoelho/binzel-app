using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BinzelApp3_Prototipo
{
    [Activity(Label = "Desenho", Theme ="@style/Binzel")]
    public class Desenho : Activity
    {
        private Usuario usr = new Usuario();      
        private ImageView imgDesenho;
        private string descDesenho;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Desenho);

            //recebendo as variaveis passadas da activity anterior
            descDesenho = Intent.GetStringExtra("descricao");

            //definindo toolbar personalizada
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar: toolbar);
            ActionBar.Title = "OP: "+ descDesenho;
            ActionBar.SetDisplayHomeAsUpEnabled(true);//define aparição do botão de voltar

            imgDesenho = FindViewById<ImageView>(Resource.Id.imageView1);
            imgDesenho.SetImageResource(Resource.Drawable.desenho1);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home) { //back button
                this.OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}