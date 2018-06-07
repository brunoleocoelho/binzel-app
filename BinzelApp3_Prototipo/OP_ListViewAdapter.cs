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

namespace BinzelApp3_Prototipo
{
    class OP_ListViewAdapter : BaseAdapter<OrdemProducao>
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
        public OP_ListViewAdapter(Context ctxt, List<OrdemProducao> itens)
        {
            this.mItens = itens;
            this.mContext = ctxt;
        }

        //retorna a posição de um item da ListView
        public override long GetItemId(int position)
        {
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
            }

            if (mItens != null)
            {
                //criando objetos c/ propriedades de cada componente de "rowOP.xaml"
                //cada objeto receberá o conteúdo em mItens[position]
                TextView numOP = row.FindViewById<TextView>(Resource.Id.numOP);
                numOP.Text = mItens[position].NUMOP;

                //TextView txtCliente = row.FindViewById<TextView>(Resource.Id.txtCliente);
                //txtCliente.Text = mItens[position].Cliente;

                TextView txtProdDesc = row.FindViewById<TextView>(Resource.Id.txtProdDesc);
                txtProdDesc.Text = mItens[position].CODPRODUTO;

                TextView dtInicio = row.FindViewById<TextView>(Resource.Id.dtInicial);
                dtInicio.Text = mItens[position].PREVISAOINICIO; //.ToShortDateString();

                //TextView hrInicio = row.FindViewById<TextView>(Resource.Id.hrInicial);
                //hrInicio.Text = mItens[position].DtHrCriacao.ToShortTimeString();

                TextView dtFinal = row.FindViewById<TextView>(Resource.Id.dtFim);
                dtFinal.Text = mItens[position].PREVISAOENTREGA; //.ToShortDateString();

                //TextView hrFinal = row.FindViewById<TextView>(Resource.Id.hrFim);
                //if (mItens[position].DtHrFinalOP.Year <= 1) {
                //    dtFinal.Text = "-";
                //    hrFinal.Text = "-";
                //}
                //else {
                //    dtFinal.Text = mItens[position].DtHrFinalOP.ToShortDateString();
                //    hrFinal.Text = mItens[position].DtHrFinalOP.ToShortTimeString();
                //}

                //0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado
                int statusImgRsrc;
                string statusText;
                var sts = mItens[position].STATUS;
                switch (sts)
                {
                    //case 0:
                    //    statusImgRsrc = Resource.Drawable.sign_gray_not_started;
                    //    statusText = "Não Iniciado";
                    //    break;
                    case "N":
                        statusImgRsrc = Resource.Drawable.sign_blue_in_process;
                        statusText = "Normal";
                        break;
                    case "S":
                        statusImgRsrc = Resource.Drawable.sign_green_dot;
                        statusText = "Sacramentada";
                        break;
                    case "U":
                        statusImgRsrc = Resource.Drawable.sign_red_cancel;
                        statusText = "Suspensa";
                        break;
                    default:
                        statusImgRsrc = Resource.Drawable.sign_blue_in_process;
                        statusText = "Normal";
                        break;
                }
                ImageView imgStatus = row.FindViewById<ImageView>(Resource.Id.imgStatus);
                imgStatus.SetImageResource(statusImgRsrc);

                TextView txtStatus = row.FindViewById<TextView>(Resource.Id.txtStatus);
                txtStatus.Text = statusText;
            }

            return row;
        }

    }
}
