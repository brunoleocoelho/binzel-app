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

namespace BinzelApp2_Prototipo
{
    [Activity(Label = "@string/menuProd_tituloListaOPs", Theme = "@style/Binzel")]
    public class ListaOrdemProducao : Activity
    {
        private List<OrdemProducao> orders = new List<OrdemProducao>();
        private ListView listaNaTela;
        private TextView userMenu;
        private Colaborador usr = new Colaborador();
        private Bundle bld = new Bundle();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListaOrdemProducao);

            //recebendo as variaveis passadas da Acrivity anterior
            usr.Matricula = Intent.GetIntExtra("usrMatr", 0);
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

            listaNaTela = FindViewById<ListView>(Resource.Id.listView_OPs);

            userMenu = FindViewById<TextView>(Resource.Id.menuProd_User);
            userMenu.Text = string.Format("{0} {1}, {2} ({3})", usr.Nome, usr.Sobrenome, usr.Cargo, usr.Setor);

            DB_PCP con = new DB_PCP();          

            ////0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado
            switch (usr.NivelAcesso)
            {
                case 1: //produção
                    //buscando tarefas associadas ao usuário
                    
                    //1º as já iniciadas
                    var trf = con.GetTarefasPorColaborador(usr.Matricula, 1);
                    if (trf != null)
                    {
                        foreach (var item in trf){
                            orders.Add(con.GetOrdemProducao(item.NumOP));
                        }
                        trf.Clear();
                    }
                    
                    //2º as NÃO iniciadas
                    trf = con.GetTarefasPorColaborador(usr.Matricula, 0);
                    if (trf != null)
                    {
                        foreach (var item in trf) {
                            orders.Add(con.GetOrdemProducao(item.NumOP));
                        }
                    }
                    break;

                case 2: //supervisão
                case 3: //gerencial
                    orders.AddRange(con.GetOrdensProducaoPorStatus(1));
                    orders.AddRange(con.GetOrdensProducaoPorStatus(2));
                    orders.AddRange(con.GetOrdensProducaoPorStatus(0));
                    orders.AddRange(con.GetOrdensProducaoPorStatus(3));                    
                    break;
            }

            OP_ListViewAdapter adapter = new OP_ListViewAdapter(this, orders);
            listaNaTela.Adapter = adapter;
            listaNaTela.ItemClick += (sender, e) => {
                this.MostraDetalhesItem(e.Position);
            };

            //criando variaveis de passagem de Activities
            bld.PutInt("usrMatr", usr.Matricula);
            bld.PutString("usrNome", usr.Nome);
            bld.PutString("usrSobreNome", usr.Sobrenome);
            bld.PutString("usrCargo", usr.Cargo);
            bld.PutString("usrSetor", usr.Setor);
            bld.PutInt("usrNivel", usr.NivelAcesso);            
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
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);
            alerta.SetTitle(Resource.String.detailsOP_titulo);
            alerta.SetMessage(
                "OP: " + orders[position].NumOP.ToString() + 
                "\nStatus: " + orders[position].StatusOP +
                "\nProduto: " + orders[position].CodProduto + 
                "\nCliente: " + orders[position].Cliente + 
                "\n\n"+ Resources.GetString(Resource.String.menuGer_msgAbrirOP)  //ABRIR DETALHES DA OP?
            );
            alerta.SetPositiveButton(Resource.String.btn_ok, delegate {
                alerta.Dispose();
                //Toast.MakeText(this, "Clicou ABRIR " + orders[position].NumOP, ToastLength.Short).Show();
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