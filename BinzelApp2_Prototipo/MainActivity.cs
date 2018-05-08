/** Tabelas possivelmente a serem usadas
 * http://tdn.totvs.com/pages/releaseview.action?pageId=286729861
 * http://tdn.totvs.com/display/public/PROT/Fluxo+Operacional+-+SIGAPCP
 * SH6 - Movimentação de Produção
 * CVL - Controle Diário;
 * SB1 - Cadastro de produtos;
 * SB2 - Saldos físicos e financeiros;
 * SB3 - Demandas;
 * SB9 - Saldos iniciais;
 * SC2 - Ordens de produção;
 * SD3 - Movimentações internas;
 * SD4 - Requisições empenhadas;
 * SF5 - Tipos de movimentação;
 * SI1 - Plano de contas;
 * SI2 - Lançamentos Contábeis;
 * SI5 - Lançamentos Padronizados;
 * SI6 - Totais de lote;
 * SI7 - Plano de contas em outras moedas;
 * SHD - Operações alocadas e sacramentadas.
 * 
 * 
*/

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

namespace BinzelApp2_Prototipo
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/binzel_icon", Theme = "@style/Binzel", NoHistory = true)]
    public class MainActivity : Activity
    {
        ////variaveis de objetos da tela
        private EditText nameLogin;
        private EditText password;
        private Button btnLoginOk;
        private DB_PCP con = new DB_PCP();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            //Variavel criada recebe a toolbar criada, e é setada pelo SetActionBar
            //após isso, pode ser referenciada chamando "ActionBar"
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.app_name); //"Binzel App (Prototype)"
            //ActionBar.SetLogo(Resource.Drawable.binzel_app_icon_light);

            //instanciando objetos da tela
            nameLogin = FindViewById<EditText>(Resource.Id.txtName);
            password = FindViewById<EditText>(Resource.Id.txtPwd);
            btnLoginOk = FindViewById<Button>(Resource.Id.btnLoginOk);
            btnLoginOk.Click += LoginSystem;

        }

        /// <summary>
        /// Faz a verificação de login e senha de usuário e encaminha para menu inicial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginSystem(object sender, System.EventArgs e)
        {
            Toast mensagem;
            Colaborador usr = new Colaborador();
            usr = con.GetColaborador(this.nameLogin.Text, this.password.Text);

            if (usr != null)
            {
                mensagem = Toast.MakeText(this, "Bem-Vindo " + usr.Nome + " " + usr.Sobrenome, ToastLength.Long);

                Bundle bld = new Bundle();
                bld.PutInt("usrMatr", usr.Matricula);
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
                    case 2: //pcp
                    case 3: //gerencial
                        Intent intentGer = new Intent(Application.Context, typeof(MenuGerencial));
                        intentGer.PutExtras(bld);
                        StartActivity(intentGer);
                        break;
                }
            }
            else
            {
                mensagem = Toast.MakeText(this, "USUARIO/SENHA INCORRETOS", ToastLength.Long);
                this.nameLogin.Text = "";
                this.password.Text = "";
            }

            mensagem.Show();
        }
    }
}
