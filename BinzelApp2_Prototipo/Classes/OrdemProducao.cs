using System;
using SQLite;

namespace BinzelApp2_Prototipo
{
    [Table("OrdemProducao")]
    public class OrdemProducao
    {
        [PrimaryKey]
        public int NumOP { get; set; }
        public string CodProduto { get; set; }
        public int QtdTotal { get; set; }
        public string Cliente { get; set; }
        public string DescricaoOP { get; set; }
        public DateTime DtHrCriacao { get; set; }
        public DateTime DtHrFinalOP { get; set; }
        public string DesenhoTecnico { get; set; }
        /// <summary> 0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado </summary>
        public int StatusOP { get; set; }

        /**CONSTRUTORES*/
        public OrdemProducao() { }

        public OrdemProducao(int num, string codProduto, int qtd, string cliente, string desc)
        {
            this.NumOP = num;
            this.CodProduto = codProduto;
            this.QtdTotal = qtd;
            this.Cliente = cliente;
            this.DescricaoOP = desc;

            this.DtHrCriacao = DateTime.Now;
            this.StatusOP = 0; //não_iniciado
        }

    }
}