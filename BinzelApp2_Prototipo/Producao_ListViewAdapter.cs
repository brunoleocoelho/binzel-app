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

namespace BinzelApp2_Prototipo
{
    class Producao_ListViewAdapter : BaseAdapter<Producao>
    {
        private List<Producao> mItens;
        private Context mContext;
        private List<Apontamento> listaApont;
        //propriedade que conta quantos itens para
        //retornar a quantidade de linhas para ListView
        public override int Count => mItens.Count;

        //propriedade retorna na hora do instanciamento a posição de um item
        //da lista armazenada (mItems) para construir a ListView
        public override Producao this[int position] => mItens[position];

        //construtor recebe o contexto, e a lista de itens
        public Producao_ListViewAdapter(Context ctxt, List<Producao> itens)  {
            this.mItens = itens;
            this.mContext = ctxt;
            DB_PCP db = new DB_PCP();
            listaApont = new List<Apontamento>();
            listaApont = db.GetApontamentos();
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
            TextView codigo = row.FindViewById<TextView>(Resource.Id.apTxt_codigo);
            codigo.Text = mItens[position].CodApont.ToString();

            var aux = listaApont.Where(x => x.CodApont == mItens[position].CodApont).First();
            TextView descricao = row.FindViewById<TextView>(Resource.Id.apTxt_descricao);
            descricao.Text = aux.Descricao;

            TextView dtInicio = row.FindViewById<TextView>(Resource.Id.apTxt_dtInicial);
            dtInicio.Text = mItens[position].DtHrInicial.ToShortDateString();

            TextView hrInicio = row.FindViewById<TextView>(Resource.Id.apTxt_hrInicial);
            hrInicio.Text = mItens[position].DtHrInicial.ToShortTimeString();

            TextView dtFinal = row.FindViewById<TextView>(Resource.Id.apTxt_dtFim);
            TextView hrFinal = row.FindViewById<TextView>(Resource.Id.apTxt_hrFim);
            if (mItens[position].DtHrFinal.Year == 1)
            {
                dtFinal.Text = "-";
                hrFinal.Text = "-";
            }
            else
            {
                dtFinal.Text = mItens[position].DtHrFinal.ToShortDateString();
                hrFinal.Text = mItens[position].DtHrFinal.ToShortTimeString();
            }

            //TextView txtStatus = row.FindViewById<TextView>(Resource.Id.tarefaTxt_status);
            //txtStatus.Text = mItens[position].StatusTarefa;

            //1-iniciado, 2-fechado
            int status;
            switch (mItens[position].Status)
            {
                case 1: status = Resource.Drawable.sign_blue_in_process; break;   //iniciado
                case 2: status = Resource.Drawable.sign_green_dot; break;         //fechado
                default: status = Resource.Drawable.sign_blue_in_process; break;
            }
            ImageView imgStatus = row.FindViewById<ImageView>(Resource.Id.apImg_Status);
            imgStatus.SetImageResource(status);

            return row;
        }

    }
}