using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using System.Linq;
using BinzelApp_Prototype;


namespace BinzelApp_Prototype
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/binzel_icon", Theme = "@style/Binzel", NoHistory = true)]
    public class MainActivity : Activity
    {          
        //variaveis de objetos da tela
        private EditText nameLogin;
        private EditText password;
        private Button btnLoginOk;
        private DB_PCP con;
        private Usuario usr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            //Variavel criada recebe a toolbar criada, e é setada pelo SetActionBar
            //após isso, pode sser referenciada chamando "ActionBar"
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar( toolbar );
            ActionBar.Title = Resources.GetString(Resource.String.app_name); //"Binzel App (Prototype)"
                                                                             //ActionBar.SetLogo(Resource.Drawable.binzel_app_icon_light);
            //instanciando objetos da tela
            btnLoginOk = FindViewById<Button>(Resource.Id.btnLoginOk);
            btnLoginOk.Click += LoginSystem;
            nameLogin = FindViewById<EditText>(Resource.Id.txtName);
            password = FindViewById<EditText>(Resource.Id.txtPwd);

            con = new DB_PCP();
        }

        /// <summary>
        /// Carrega o menu e "infla" na tela
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menuHome_bar, menu);
            //return base.OnCreateOptionsMenu(menu);
            return true;
        }

        /// <summary>
        /// Direciona as ações para cada item do menu da ActionBar
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.menuOptExit)    {
                AlertDialog.Builder alerta = new AlertDialog.Builder(this);                
                alerta.SetTitle(Resources.GetString(Resource.String.warning)); //definindo titulo como AVISO
                alerta.SetIcon(Resources.GetDrawable(Resource.Drawable.binzel_app_icon_light));
                alerta.SetMessage(Resources.GetString(Resource.String.exit_message) +" "+ Resources.GetString(Resource.String.app_name) + "?"); //mensagem se deseja fechar o app
                //opção de OK (fechar app)
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
            //else if (id == Resource.Id.menuOptSearch) {
            //    Toast.MakeText(this, "Clicou em SEARCH", ToastLength.Short).Show();
            //}
            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// Faz a verificação de login e senha de usuário e encaminha para menu inicial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginSystem(object sender, System.EventArgs e)
        {
            Toast mensagem;            
            usr = con.GetUsuario(this.nameLogin.Text, this.password.Text);

            if ( usr != null ) {
                mensagem = Toast.MakeText(this, "Bem-Vindo " + usr.Nome + " "+ usr.Sobrenome, ToastLength.Long);
                Bundle bld = new Bundle();
                bld.PutInt("usrId", usr.User_id);
                bld.PutString("usrNome", usr.Nome);
                bld.PutString("usrSobreNome", usr.Sobrenome);
                bld.PutString("usrCargo", usr.Cargo);
                bld.PutString("usrSetor", usr.Setor);
                bld.PutInt("usrNivel", usr.NivelAcesso);
                switch (usr.NivelAcesso)
                {
                    case 1: //produção  
                        Intent intentProd = new Intent(Application.Context, typeof(MenuProducao));
                        intentProd.PutExtras(bld);
                        StartActivity(intentProd);
                        break;
                    case 2: //gerencial
                        Intent intentGer = new Intent(Application.Context, typeof(MenuGerencial));
                        intentGer.PutExtras(bld);
                        StartActivity(intentGer);
                        break;
                }
            }       
            else {
                mensagem = Toast.MakeText(this, "USUARIO/SENHA INCORRETOS", ToastLength.Long);
                this.LimparCampos();
            }

            mensagem.Show();
        }

    //Limpa campos da tela de login
    private void LimparCampos()
        {
            this.nameLogin.Text = "";
            this.password.Text = "";
        }

    }
}

