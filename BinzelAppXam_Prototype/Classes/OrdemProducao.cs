using System;
using SQLite;

namespace BinzelApp_Prototype
{
    [Table("OrdemProducao")]
    public class OrdemProducao
    {
        [PrimaryKey, Unique, Column("numOP")]
        public int NumOP { get; set; }
        public string DescricaoOP { get; set; }
        public string Cliente { get; set; }
        public DateTime DtHrInicialOP { get; set; }
        public DateTime DtHrFinalOP { get; set; }
        public string StatusOP { get; set; }
        //aberto=1, producao=2, parado=3, completo=4, cancelado=5

        //construtores
        public OrdemProducao() { }

        /// <summary>
        /// Cria uma ordem de produção já declarando seus atributos
        /// </summary>
        /// <param name="num">Numero da OP (Único)</param>
        /// <param name="desc">Descricao da OP</param>
        /// <param name="client">Cliente da OP</param>
        /// <param name="ini">Data e Hora de abertura da OP</param>
        /// <param name="fin">Data e Hora de fechamento da OP</param>
        /// <param name="status">Status da OP (</param>
        public OrdemProducao(int num, string desc, string client, DateTime ini, DateTime fin, string status)
        {
            this.NumOP = num;
            this.DescricaoOP = desc;
            this.Cliente = client;
            this.DtHrInicialOP = ini;
            this.DtHrFinalOP = fin;
            this.StatusOP = status;
        }

    }
}