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

namespace BinzelApp_Prototype
{
    [Activity(Label = "OPEmProducao", Theme = "@style/Binzel")]
    public class OPEmProducao : Activity
    {
        private Usuario usr = new Usuario();
        private Bundle bld = new Bundle();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OPEmProducao);

            //definindo toolbar customizada
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar: toolbar);
            ActionBar.Title = "";
            ActionBar.SetDisplayHomeAsUpEnabled(true);//define aparição do botão de voltar
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home)
            { //back button
                this.OnBackPressed();
            }
            return base.OnMenuItemSelected(featureId, item);
        }
    }
}