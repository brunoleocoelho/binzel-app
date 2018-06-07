using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BinzelApp3_Prototipo
{
    [Activity(Label = "@string/menuGer_titulo", Theme = "@style/Binzel")]
    public class MenuGerencial : Activity
    {
        private Button btnOrdProd;
        private Button btnFuncRel;
        private Button btnRelat;
        private TextView userMenu;
        private Usuario usr = new Usuario();
        private Bundle bld = new Bundle();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuGerencial);

            //recebendo as variaveis passadas da Activity anterior
            usr.IdUsuario = Intent.GetStringExtra("usrId");
            usr.Nome = Intent.GetStringExtra("usrNome");
            usr.Cargo = Intent.GetStringExtra("usrCargo");
            usr.Setor = Intent.GetStringExtra("usrSetor");
            //usr.NivelAcesso = Intent.GetIntExtra("usrNivel", 0);

            //Variavel criada recebe a toolbar criada, e é setada pelo SetActionBar
            //após isso, pode sser referenciada chamando "ActionBar"
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar: toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.menuGer_titulo);

            //botão de Ordens de Produção
            btnOrdProd = FindViewById<Button>(Resource.Id.btnRelOPs);
            btnOrdProd.Click += (sender, e) => {
                Intent intent = new Intent(Application.Context, typeof(ListaOrdemProducao));
                intent.PutExtras(bld);                
                StartActivity( intent );
            };
            //botão de Relatório de Funcionários
            btnFuncRel = FindViewById<Button>(Resource.Id.btnRelFunc);
            //botão de Relatórios
            btnRelat = FindViewById<Button>(Resource.Id.btnReports);

            //label contendo usuario e setor logado
            userMenu = FindViewById<TextView>(Resource.Id.menuProd_User);
            userMenu.Text = string.Format("{0}, {1} ({2})", usr.Nome, usr.Cargo, usr.Setor);

            //criando variaveis de passagem de Activities
            bld.PutString("usrId", usr.IdUsuario);
            bld.PutString("usrNome", usr.Nome);
            bld.PutString("usrCargo", usr.Cargo);
            bld.PutString("usrSetor", usr.Setor);
            //bld.PutInt("usrNivel", usr.NivelAcesso);
        }

        //manipula a saída do app
        public override void OnBackPressed()
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);
            alerta.SetTitle(Resources.GetString(Resource.String.warning)); //definindo titulo como AVISO
            alerta.SetIcon(Resources.GetDrawable(Resource.Drawable.binzel_app_icon_light));
            alerta.SetMessage(Resources.GetString(Resource.String.exit_message) + " " + Resources.GetString(Resource.String.app_name) + "?");//mensagem se deseja fechar o app

            //definindo opção de OK (fechar app)
            alerta.SetPositiveButton(Resource.String.btn_ok, delegate {
                alerta.Dispose();
                Toast.MakeText(this,
                    string.Format("{0} {1}", Resources.GetString(Resource.String.close_app_message), Resources.GetString(Resource.String.app_name)),
                    ToastLength.Short).Show();
                Finish();
            });

            //definindo opção de cancela
            alerta.SetNegativeButton(Resource.String.btn_cancel, delegate {
                alerta.Dispose();
            });

            alerta.Show();
        }
        // Carrega o menu customizado e "infla" na tela
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menuGer_bar, menu); //MENU1_BAR.XML
            return base.OnCreateOptionsMenu(menu);
        }

        /// Direciona as ações para cada item do menu da ActionBar
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            //menu icone X SAIR
            if (id == Resource.Id.menuOptExit) {
                this.OnBackPressed();
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}