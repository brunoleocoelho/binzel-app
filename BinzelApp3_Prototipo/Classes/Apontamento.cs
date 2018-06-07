using System;
using SQLite;

namespace BinzelApp3_Prototipo
{
    [Table("Apontamento")]
    public class Apontamento
    {
        [PrimaryKey, Column("CodApont")]
        public int CodApont { get; set; }
        public string Descricao { get; set; }
        public string AreaAtua { get; set; }
        public string StatusPadrao { get; set; } 
        //produtivo, parado, atividade

        /**CONSTRUTORES*/
        public Apontamento() { }

        public Apontamento(int cod, string desc, string area, string stsPadrao)
        {
            CodApont = cod;
            Descricao = desc;
            AreaAtua = area;
            StatusPadrao = stsPadrao;
        }
    }
}