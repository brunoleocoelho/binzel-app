using System;
using SQLite;

namespace BinzelApp2_Prototipo
{
    /// <summary>
    /// Um colaborador pode ser de Produção, Supervisão ou Gerência. Níveis:
    /// <param> 1-Producao: aponta codApont numa tarefa dentro de uma OP direcionada a ele;</param>
    /// <param> 2-Supervisao: direciona OPs para usuários, cria tarefas dentro das OPs;</param>
    /// <param> 3-Gerencial/PCP: direciona OP para usuários, cria tarefas dentro das OPs, visualiza desempenhos; </param>
    /// </summary>
    [Table("Colaborador")]
    public class Colaborador
    {
        private int matricula;

        [PrimaryKey, AutoIncrement, Column("Matricula")]
        public int Matricula { get => matricula; set => matricula = value; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Cargo { get; set; }
        public string Setor { get; set; }
        public string UserName { get; set; }
        public string Senha { get; set; }
        public int NivelAcesso { get; set; } 

        public Colaborador() { }

        public Colaborador(string name, string lastname, string function, string dpto, string pwd, int access)
        {
            this.Matricula = 0;
            this.Nome = name.Trim();
            this.Sobrenome = lastname.Trim();
            this.Cargo = function;
            this.Setor = dpto;
            this.UserName = this.GerarUserName(name, lastname);
            this.Senha = pwd;
            this.NivelAcesso = access;
        }

        //cria username a partir do nome e sobrenome ex: "Bruno Coelho" -> "bruno.coelho"            
        public string GerarUserName(string name, string lastName)
        {
            return string.Format("{0}", string.Join(".", new[] { name.ToLower(), lastName.ToLower() }));
        }
       
    }
}