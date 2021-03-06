﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BinzelApp2_Prototipo
{
    [Activity(Label = "@string/menuProd_titulo", Theme ="@style/Binzel")]
    public class MenuProducao : Activity
    {
        private Button btnOPs;
        private Button btnEventos;
        private TextView userMenu;
        private Colaborador usr = new Colaborador();
        private Bundle bld = new Bundle();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MenuProducao);

            //recebendo as variaveis passadas da Activity anterior
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
            ActionBar.Title = Resources.GetString(Resource.String.menuProd_titulo) ;

            //botão de Ordens de Produção
            btnOPs = FindViewById<Button>(Resource.Id.btnOrdens);
            btnOPs.Click += (sender, e) => {
                Intent intentOP = new Intent(Application.Context, typeof(ListaOrdemProducao));                
                intentOP.PutExtras(bld);
                StartActivity(intentOP);
            };

            //botão de Eventos
            btnEventos = FindViewById<Button>(Resource.Id.btnEventos);
            //btnEventos.Click += (sender, e) => {
            //    StartActivity(new Intent(Application.Context, typeof(........)));                
            //};

            //label contendo usuario e setor logado
            userMenu = FindViewById<TextView>(Resource.Id.menuProd_User);
            userMenu.Text = string.Format("{0} {1}, {2} ({3})", usr.Nome, usr.Sobrenome, usr.Cargo, usr.Setor);

            //criando variaveis de passagem de Activities
            bld.PutInt("usrMatr", usr.Matricula);
            bld.PutString("usrNome", usr.Nome);
            bld.PutString("usrSobreNome", usr.Sobrenome);
            bld.PutString("usrCargo", usr.Cargo);
            bld.PutString("usrSetor", usr.Setor);
            bld.PutInt("usrNivel", usr.NivelAcesso);
        }

        /// <summary>
        /// Carrega o menu e "infla" na tela
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menuProd_bar, menu); //MENUPROD_BAR.XML            
            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary> 
        /// define o que acontece ao pressionar 'BACKBUTTON" (VOLTAR) 
        /// </summary>
        public override void OnBackPressed()
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);            
            alerta.SetTitle(Resources.GetString(Resource.String.warning)); //definindo titulo como AVISO 
            alerta.SetIcon(Resources.GetDrawable(Resource.Drawable.binzel_app_icon_light));
            alerta.SetMessage(Resources.GetString(Resource.String.exit_message) + " " + Resources.GetString(Resource.String.app_name) + "?"); //mensagem se deseja fechar o app
            
            //opção de OK (fechar app)
            alerta.SetPositiveButton(Resource.String.btn_ok, delegate {
                alerta.Dispose();
                Toast.MakeText(this,
                    string.Format("{0} {1}", Resources.GetString(Resource.String.close_app_message), Resources.GetString(Resource.String.app_name)),
                    ToastLength.Short).Show();
                Finish();
            });
            
            //opção de cancelar
            alerta.SetNegativeButton(Resource.String.btn_cancel, delegate {
                alerta.Dispose();                
            });

            alerta.Show();
        }

        /// <summary>
        /// Direciona as ações para cada item do menu da ActionBar
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            //menu icone X SAIR
            if (id == Resource.Id.menuOptExit){
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