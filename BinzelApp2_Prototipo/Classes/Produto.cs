using System;
using SQLite;

namespace BinzelApp2_Prototipo
{
    [Table("Produto")]
    public class Produto
    {
        [PrimaryKey, Column("CodProduto")]
        public string CodProduto { get; set; }
        public string Descricao { get; set; }
        public string UnidMedida { get; set; }

        public Produto() { }

        public Produto(string cod, string desc, string unit)
        {
            this.CodProduto = cod;
            this.Descricao = desc;
            this.UnidMedida = unit.ToUpper();
        }
    }
}