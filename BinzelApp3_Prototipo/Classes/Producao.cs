using System;
using SQLite;

namespace BinzelApp3_Prototipo
{
    [Table("Producao")]
    public class Producao
    {
        [PrimaryKey, AutoIncrement, Column("RegProducao")]
        public int RegProducao { get; set; }
        public int IdTarefa { get; set; }
        public int CodApont { get; set; }
        public DateTime DtHrInicial { get; set; }
        public DateTime DtHrFinal { get; set; }
        public int QtdProduzida { get; set; }
        /// <summary> Status registro 1-iniciado, 2-fechado </summary>
        public int Status { get; set; }

        /**CONSTRUTORES*/
        public Producao() { }

        //Inicia produção da tarefa em questão pelo usuario
        public Producao(int tarefa, int codApont)
        {
            this.IdTarefa = tarefa;
            this.CodApont = codApont;
            this.DtHrInicial = DateTime.Now;
            this.Status = 1; //iniciado
        }

        //Fecha o item definindo a data/hora final e status 2 (fechado)
        public void FecharItem()
        {
            this.DtHrFinal = DateTime.Now;
            this.Status = 2; //fechado
        }
    }
}