using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BinzelApp_Prototype
{
    class OPListViewAdapter : BaseAdapter<OrdemProducao>
    {
        private List<OrdemProducao> mItens;
        private Context mContext;

        //propriedade que conta quantos itens para
        //retornar a quantidade de linhas para ListView
        public override int Count => mItens.Count;

        //propriedade retorna na hora do instanciamento a posição de um item
        //da lista armazenada (mItems) para construir a ListView
        public override OrdemProducao this[int position] => mItens[position];

        //construtor recebe o contexto, e a lista de itens
        public OPListViewAdapter(Context ctxt, List<OrdemProducao> itens)  {
            this.mItens = itens;
            this.mContext = ctxt;
        }

        //retorna a posição de um item da ListView
        public override long GetItemId(int position)  {            
            return position;
        }

        //cria as linhas e define as os textos, imagens, e outros objetos da view
        //retornando essa linha montada para a ListView
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //ciando uma view local para montar
            View row = convertView;

            //se "row" não estiver sendo usado, infla-lo c/ o rowOP.xaml
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.rowOP, null, false);
                //row.Click += (sender, e) => {
                //    this.MostraDetalhesItem(position);
                //};
            }

            //criando objetos c/ propriedades de cada componente de "rowOP.xaml"
            //cada objeto receberá o conteúdo em mItens[position]
            TextView numOP = row.FindViewById<TextView>(Resource.Id.numOP);
            numOP.Text = mItens[position].NumOP.ToString();

            TextView txtCliente = row.FindViewById<TextView>(Resource.Id.txtCliente);
            txtCliente.Text = mItens[position].Cliente;

            TextView txtPrjDesc = row.FindViewById<TextView>(Resource.Id.txtProjDesc);
            txtPrjDesc.Text = mItens[position].DescricaoOP;

            TextView dtInicio = row.FindViewById<TextView>(Resource.Id.dtInicial);
            dtInicio.Text = mItens[position].DtHrInicialOP.ToShortDateString();

            TextView hrInicio = row.FindViewById<TextView>(Resource.Id.hrInicial);
            hrInicio.Text = mItens[position].DtHrInicialOP.ToShortTimeString();

            TextView dtFinal = row.FindViewById<TextView>(Resource.Id.dtFim);
            TextView hrFinal = row.FindViewById<TextView>(Resource.Id.hrFim);
            if (mItens[position].DtHrFinalOP.Year == 1) {
                dtFinal.Text = "-";
                hrFinal.Text = "-";
            }
            else {
                dtFinal.Text = mItens[position].DtHrFinalOP.ToShortDateString();
                hrFinal.Text = mItens[position].DtHrFinalOP.ToShortTimeString();
            }

            //aberto=1, producao=2, parado=3, completo=4, cancelado=5
            int status;
            var sts = mItens[position].StatusOP;
            switch (sts) {
                case "aberto":          status = Resource.Drawable.sign_gray_play;          break;
                case "producao":        status = Resource.Drawable.sign_blue_in_process;    break;
                case "parado":          status = Resource.Drawable.sign_yellow_warning;     break;
                case "completo":        status = Resource.Drawable.sign_green_dot;          break;
                case "cancelado":       status = Resource.Drawable.sign_red_cancel;         break;
                default:                status = Resource.Drawable.sign_gray_play;          break;
            }
            ImageView imgStatus = row.FindViewById<ImageView>(Resource.Id.imgStatus);
            imgStatus.SetImageResource(status);

            TextView txtStatus = row.FindViewById<TextView>(Resource.Id.txtStatus);
            txtStatus.Text = mItens[position].StatusOP;

            return row;
        }

    }
}