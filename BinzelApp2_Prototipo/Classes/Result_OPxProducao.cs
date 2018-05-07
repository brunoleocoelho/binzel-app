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

namespace BinzelApp2_Prototipo
{
    public class Result_OPxProducao
    {
        public int NumOP { get; set; }
        public string Cliente { get; set; }
        public string DescricaoOP { get; set; }
        public int StatusOP { get; set; }
        public int CodProduto { get; set; }
        public string Descricao { get; set; }
        public DateTime DtHrInicial { get; set; }
        public DateTime DtHrFinal { get; set; }

        public Result_OPxProducao() { }

    }
}