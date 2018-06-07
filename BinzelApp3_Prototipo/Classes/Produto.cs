using System;
using SQLite;

namespace BinzelApp3_Prototipo
{
    public class RootProduto
    {
        public int Status { get; set; }
        public string Descricao { get; set; }
        public int Registros { get; set; }
        public Object[] Objeto { get; set; }
    }

    [Table("Produto")]
    public class Produto
    {
        [PrimaryKey, Column("CodProduto")]
        public string CodProduto { get; set; }
        public string Descricao { get; set; }
        public string UnidMedida { get; set; }

        //public string _classname { get; set; }
        //public string CCODIGO { get; set; }
        //public string CDESCRICAO { get; set; }
        //public object CSTATUS { get; set; }
        //public string CUM { get; set; }
        //public string CGRUPO { get; set; }
        //public string NSALDO { get; set; }

        public Produto() { }

        public Produto(string cod, string desc, string unit)
        {
            this.CodProduto = cod;
            this.Descricao = desc;
            this.UnidMedida = unit.ToUpper();
        }
    }
}