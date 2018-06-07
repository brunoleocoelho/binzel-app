using System;
using SQLite;

namespace BinzelApp3_Prototipo
{
    public class RootUsuario
    {
        public int Status { get; set; }
        public string Descricao { get; set; }
        public Object Usuario { get; set; }
    }

    /// <summary>
    /// Um colaborador pode ser de Produção, Supervisão ou Gerência. Níveis:
    /// <param> 1-Producao: aponta codApont numa tarefa dentro de uma OP direcionada a ele;</param>
    /// <param> 2-Supervisao: direciona OPs para usuários, cria tarefas dentro das OPs;</param>
    /// <param> 3-Gerencial/PCP: direciona OP para usuários, cria tarefas dentro das OPs, visualiza desempenhos; </param>
    /// </summary>
    [Table("Usuario")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement, Column("Matricula")]
        public string IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string Setor { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Bloqueado { get; set; }
        //public int NivelAcesso { get; set; }

        public Usuario() { }

        public Usuario(string userName, string name, string cargo, string dpto)
        {
            this.IdUsuario = "";
            this.Nome = name.Trim();
            this.Cargo = cargo;
            this.Setor = dpto;
            this.UserName = userName;
            //this.NivelAcesso = access;
        }       
    }
}


