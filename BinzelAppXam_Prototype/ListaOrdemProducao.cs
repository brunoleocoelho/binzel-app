using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BinzelApp_Prototype;

namespace BinzelApp_Prototype
{
    [Activity(Label = "@string/menuProd_tituloListaOPs", Theme = "@style/Binzel")]
    public class ListaOrdemProducao : Activity
    {
        private List<OrdemProducao> orders = new List<OrdemProducao>();
        private ListView listaNaTela;
        private TextView userMenu;
        private Usuario usr = new Usuario();
        private Bundle bld = new Bundle();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListaOrdemProducao);

            //recebendo as variaveis passadas da Acrivity anterior
            usr.User_id = Intent.GetIntExtra("usrId", 0);
            usr.Nome = Intent.GetStringExtra("usrNome");
            usr.Sobrenome = Intent.GetStringExtra("usrSobreNome");
            usr.Cargo = Intent.GetStringExtra("usrCargo");
            usr.Setor = Intent.GetStringExtra("usrSetor");
            usr.NivelAcesso = Intent.GetIntExtra("usrNivel", 0);

            //Variavel criada recebe a toolbar criada, e é definida pelo SetActionBar
            //após isso, pode ser referenciada chamando "ActionBar"
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar: toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.menuProd_tituloListaOPs);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            //criando variaveis de passagem de Activities
            bld.PutInt("usrId", usr.User_id);
            bld.PutString("usrNome", usr.Nome);
            bld.PutString("usrSobreNome", usr.Sobrenome);
            bld.PutString("usrCargo", usr.Cargo);
            bld.PutString("usrSetor", usr.Setor);
            bld.PutInt("usrNivel", usr.NivelAcesso);

            listaNaTela = FindViewById<ListView>(Resource.Id.listView_OPs);
            userMenu = FindViewById<TextView>(Resource.Id.menuProd_User);
            userMenu.Text = string.Format("{0} {1}, {2}", usr.Nome, usr.Sobrenome, usr.Setor);

            DB_PCP con = new DB_PCP();            
            //todos=0, aberto=1, producao=2, parado=3, completo=4, cancelado=5
            switch (usr.NivelAcesso)
            {
                case 1: //produção
                    orders.AddRange(con.GetOrdensProducao(3));
                    orders.AddRange(con.GetOrdensProducao(2));
                    orders.AddRange(con.GetOrdensProducao(1));
                    break;
                case 2: //gerencial
                case 0:
                    orders = con.GetOrdensProducao(0);
                    break;
            }

            OPListViewAdapter adapter = new OPListViewAdapter(this, orders);
            listaNaTela.Adapter = adapter;
            listaNaTela.ItemClick += (sender, e) => {
                this.MostraDetalhesItem(e.Position);
            };
            
        }

        //manipula os itens selecionados do menu da toolbar
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home) {
                this.OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }

        private void MostraDetalhesItem(int position)
        {
            var op = orders[position].NumOP.ToString();
            var desc = orders[position].DescricaoOP;
            var cl = orders[position].Cliente;
            var st = orders[position].StatusOP;

            AlertDialog.Builder alerta = new AlertDialog.Builder(this);
            alerta.SetTitle(Resource.String.detailsOP_titulo);
            alerta.SetMessage(
                "OP: " + op + 
                "\nStatus: " + st +
                "\nDesc: " + desc + 
                "\nCliente: " + cl + 
                "\n\n"+ Resources.GetString(Resource.String.menuGer_msgAbrirOP)); //ABRIR DETALHES DA OP?

            alerta.SetPositiveButton(Resource.String.btn_ok, delegate {
                alerta.Dispose();
                Intent intent = new Intent(this, typeof(OPDetails));
                bld.PutInt("op", orders[position].NumOP);
                intent.PutExtras(bld);
                StartActivity(intent);
            });
            alerta.SetNegativeButton(Resource.String.btn_cancel, delegate {
                alerta.Dispose();
            });

            alerta.Show();
        }
    }    
}