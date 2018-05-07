using System;
using SQLite;

namespace BinzelApp2_Prototipo
{
    /// <summary>
    /// Tarefa é uma OrdemProducao associada ao um Colaborador de produção (nivel 1)
    /// delegada por um Colaborador de supervisão/gerência (niveis 2 ou 3)
    /// </summary>
    [Table("Tarefa")]
    public class Tarefa
    {
        [PrimaryKey, AutoIncrement, Column("IdTarefa")]
        public int IdTarefa { get; set; }        
        public int NumOP { get; set; }        
        public int MatriculaColab { get; set; }
        public DateTime DtHrCriacao { get; set; }
        public int DelegadoPor { get; set; }
        /// <summary> 0=NÃO_iniciado, 1=iniciado, 2=fechado, 3=cancelado </summary>
        public int Status { get; set; } 
        //0=NÃO_iniciado, 1=iniciado, 2=fechado, 3=cancelado

        /**CONSTRUTORES */
        public Tarefa() { }

        public Tarefa(int num_op, int colabMatr, int porUsr)
        {
            this.NumOP = num_op;
            this.MatriculaColab = colabMatr;
            this.DtHrCriacao = DateTime.Now;
            this.DelegadoPor = porUsr;
            this.Status = 0; //não_iniciado
        }

        /**METODOS PUBLICOS*/       
    }
}