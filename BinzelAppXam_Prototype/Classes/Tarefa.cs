using System;
using SQLite;

namespace BinzelApp_Prototype
{
    [Table("Tarefa")]
    public class Tarefa
    {
        [PrimaryKey, AutoIncrement, Column("IdTarefa")]
        public int IdTarefa { get; set; }        
        public int CodApont { get; set; }
        public int NumOP { get; set; }        
        public DateTime DtHrInicioTarefa { get; set; }
        public DateTime DtHrFinalTarefa { get; set; }
        public string StatusTarefa { get; set; }
        //aberto=1, producao=2, parado=3, completo=4, cancelado=5

        /**CONSTRUTORES */
        public Tarefa() { }

        public Tarefa(int cod_apont, int num_op)
        {
            this.CodApont = cod_apont;
            this.NumOP = num_op;
        }

        /**METODOS PUBLICOS*/
        public void IniciarTarefa(DateTime inicio)
        {
            this.DtHrInicioTarefa = inicio;
            this.StatusTarefa = "producao";
        }

        /// <summary>
        /// Finaliza a tarefa. Motivo: completo=4, cancelado=5
        /// </summary>
        /// <param name="fim">Data e Hora do fim da tarefa</param>
        /// <param name="motivo">completo=4, cancelado=5</param>
        public void FinalizarTarefa(DateTime fim, int motivo)
        {
            this.DtHrFinalTarefa = fim;
            switch (motivo) {
                case 4: this.StatusTarefa = "completo"; break;
                case 5: this.StatusTarefa = "cancelado"; break;
                default: this.StatusTarefa = "cancelado"; break;
            }
        }

        public void PausarTarefa()
        {
            this.StatusTarefa = "parado";
        }

    }
}