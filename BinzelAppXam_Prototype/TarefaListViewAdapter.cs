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
    class TarefaListViewAdapter : BaseAdapter<Tarefa>
    {
        private List<Tarefa> mItens;
        private Context mContext;

        //propriedade que conta quantos itens para
        //retornar a quantidade de linhas para ListView
        public override int Count => mItens.Count;

        //propriedade retorna na hora do instanciamento a posição de um item
        //da lista armazenada (mItems) para construir a ListView
        public override Tarefa this[int position] => mItens[position];

        //construtor recebe o contexto, e a lista de itens
        public TarefaListViewAdapter(Context ctxt, List<Tarefa> itens)  {
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.rowTarefa, null, false);
            }

            //criando objetos c/ propriedades de cada componente de "rowTarefa.xaml" cada objeto recebe o conteúdo em mItens[position]
            TextView codigo = row.FindViewById<TextView>(Resource.Id.tarefaTxt_codigo);
            codigo.Text = mItens[position].CodApont.ToString();

            //TextView descricao = row.FindViewById<TextView>(Resource.Id.tarefaTxt_descricao);
            //descricao.Text = mItens[position].des;

            TextView dtInicio = row.FindViewById<TextView>(Resource.Id.tarefaTxt_dtInicial);
            dtInicio.Text = mItens[position].DtHrInicioTarefa.ToShortDateString();

            TextView hrInicio = row.FindViewById<TextView>(Resource.Id.tarefaTxt_hrInicial);
            hrInicio.Text = mItens[position].DtHrInicioTarefa.ToShortTimeString();

            TextView dtFinal = row.FindViewById<TextView>(Resource.Id.tarefaTxt_dtFim);
            TextView hrFinal = row.FindViewById<TextView>(Resource.Id.tarefaTxt_hrFim);
            if (mItens[position].DtHrFinalTarefa.Year == 1) {
                dtFinal.Text = "-";
                hrFinal.Text = "-";
            }
            else {
                dtFinal.Text = mItens[position].DtHrFinalTarefa.ToShortDateString();
                hrFinal.Text = mItens[position].DtHrFinalTarefa.ToShortTimeString();
            }

            //TextView txtStatus = row.FindViewById<TextView>(Resource.Id.tarefaTxt_status);
            //txtStatus.Text = mItens[position].StatusTarefa;

            //aberto=1, producao=2, parado=3, completo=4, cancelado=5
            int status;
            var sts = mItens[position].StatusTarefa;
            switch (sts) {
                case "aberto":          status = Resource.Drawable.sign_gray_play;          break;
                case "producao":        status = Resource.Drawable.sign_blue_in_process;    break;
                case "parado":          status = Resource.Drawable.sign_yellow_warning;     break;
                case "completo":        status = Resource.Drawable.sign_green_dot;          break;
                case "cancelado":       status = Resource.Drawable.sign_red_cancel;         break;
                default:                status = Resource.Drawable.sign_gray_play;          break;
            }
            ImageView imgStatus = row.FindViewById<ImageView>(Resource.Id.tarefaImg_Status);
            imgStatus.SetImageResource(status);

            return row;
        }

    }
}