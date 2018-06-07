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
    [Activity(Label = "OPEmProducao", Theme = "@style/Binzel")]
    public class OPEmProducao : Activity
    {
        private int OP;
        private int qtdOp;
        private DB_PCP db = new DB_PCP();
        private Producao registro = new Producao();
        private Usuario usr = new Usuario();
        private Bundle bld = new Bundle();

        TextView txtOP;
        TextView txtCliente;
        TextView txtProduto;
        TextView txtApont;
        //TextView dtIni;
        //TextView hrIni;
        //TextView dtFim;
        //TextView hrFim;
        Button btnParada;
        Button btnFinalizar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.OPEmProducao);

            //recebendo as variaveis passadas da Activity anterior
            usr.IdUsuario = Intent.GetStringExtra("usrId");
            usr.Nome = Intent.GetStringExtra("usrNome");
            usr.Cargo = Intent.GetStringExtra("usrCargo");
            usr.Setor = Intent.GetStringExtra("usrSetor");
            //usr.NivelAcesso = Intent.GetIntExtra("usrNivel", 0);
            OP = Intent.GetIntExtra("OP", 0);
            qtdOp = Intent.GetIntExtra("QtdOP", 0);
            var cliente = Intent.GetStringExtra("Cliente");
            var produto = Intent.GetStringExtra("Produto");
            var apontDesc = Intent.GetStringExtra("ApontDesc");
            var apontCod = Intent.GetStringExtra("ApontCod");
            var regAux = Intent.GetStringArrayExtra("regProd");
            registro = new Producao
            {
                RegProducao = int.Parse(regAux[0]),
                CodApont = int.Parse(regAux[1]),
                DtHrFinal = DateTime.Parse(regAux[2]),
                DtHrInicial = DateTime.Parse(regAux[3]),
                IdTarefa = int.Parse(regAux[4]),
                QtdProduzida = int.Parse(regAux[5]),
                Status = int.Parse(regAux[6])
            };

            //definindo toolbar customizada
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar: toolbar);
            ActionBar.Title = "";
            ActionBar.SetDisplayHomeAsUpEnabled(true);//define aparição do botão de voltar

            //instanciando itens da tela
            txtOP = FindViewById<TextView>(Resource.Id.numOP);
            txtOP.Text = "Ordem de Produção: " + OP.ToString();

            txtCliente = FindViewById<TextView>(Resource.Id.txtCliente);
            txtCliente.Text = cliente;

            txtProduto = FindViewById<TextView>(Resource.Id.txtProduto);
            txtProduto.Text = produto;

            txtApont = FindViewById<TextView>(Resource.Id.txtApont);
            txtApont.Text = apontCod + " - " + apontDesc;

            //dtIni = FindViewById<TextView>(Resource.Id.dtInicial);
            //dtIni.Text = registro.DtHrInicial.ToShortDateString();
            //hrIni = FindViewById<TextView>(Resource.Id.hrInicial);
            //hrIni.Text = registro.DtHrInicial.ToShortTimeString();

            //dtFim = FindViewById<TextView>(Resource.Id.dtFim);
            //hrFim = FindViewById<TextView>(Resource.Id.hrFim);
            //if (registro.DtHrFinal.Year > 1) {
            //    dtFim.Text = registro.DtHrFinal.ToShortDateString();
            //    hrFim.Text = registro.DtHrFinal.ToShortTimeString();
            //}
            //else {
            //    dtFim.Text = "-";
            //    hrFim.Text = "-";
            //}

            btnParada = FindViewById<Button>(Resource.Id.btn_Parada);
            //btnParada.Click += BtnParada_Click;

            btnFinalizar = FindViewById<Button>(Resource.Id.btn_Finalizar);
            btnFinalizar.Click += (sender, e) =>
            {
                this.Finish();
            };
        }

        //private void BtnParada_Click(object sender, EventArgs e)
        //{            
        //    var objOP = db.GetOrdemProducao(OP);
        //    NumberPicker nPkr = new NumberPicker(this);
        //    nPkr.MaxValue = qtdOp;
        //    nPkr.MinValue = 0;

        //    AlertDialog.Builder alerta = new AlertDialog.Builder(this);
        //    alerta.SetTitle("Parada");
        //    alerta.SetMessage(Resources.GetString(Resource.String.lbl_QtdProd)); //qtd produzida
        //    alerta.SetView(nPkr);
        //    alerta.SetPositiveButton(Resource.String.btn_ok, delegate {
        //        Toast.MakeText(this, Resources.GetString(Resource.String.lbl_QtdProd) + ": " + nPkr.Value.ToString(), ToastLength.Long);
        //        //alerta.Dispose();
        //    });
        //    alerta.SetNegativeButton(Resource.String.btn_cancel, delegate {
        //        alerta.Dispose();
        //        Toast.MakeText(this, Resources.GetString(Resource.String.lbl_QtdProd) +": "+ nPkr.Value.ToString(), ToastLength.Long);
        //    });
        //    alerta.Show();
        //    //registro.QtdProduzida = objOP.QtdTotal;
        //    //db.UpdateProducaoApont(registro);
        //    //this.Finish();
        //}

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home)
            { //back button
                this.OnBackPressed();
            }
            return base.OnMenuItemSelected(featureId, item);
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);
            alerta.SetTitle("Aviso");
            alerta.SetMessage("Qual o status de finalização da tarefa?");
            alerta.SetPositiveButton("Completa", (s, e) =>
            {
                //Toast.MakeText(this, "Tarefa Finalizada COMPLETA: " + registro.RegProducao.ToString() + ")", ToastLength.Short).Show();                
                //var objOP = db.GetOrdemProducao(OP);
                //registro.QtdProduzida = objOP.QtdTotal;                
                db.UpdateProducaoApont(registro);
                this.Finish();
            });
            alerta.SetNegativeButton("Pausa", (s, e) =>
            {
                Toast.MakeText(this, "Tarefa Finalizada PARCIAL (Registro: " + registro.RegProducao.ToString() + ")", ToastLength.Short).Show();
                this.Finish();
            });
            alerta.SetNeutralButton("Cancelar", (s, e) =>
            {
                alerta.Dispose();
            });
            alerta.Show();

            //base.OnBackPressed();   
        }


    }
}
