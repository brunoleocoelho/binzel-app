using System;
using SQLite;

namespace BinzelApp_Prototype
{
    /**Classe usada contendo usuários no DB*/
    [Table("Usuario")]
    public class Usuario
    {
        private int user_id;
        private string nome;
        private string sobrenome;
        private string senha;
        private string cargo;
        private string setor;
        private string userName;
        private int nivelAcesso; //1-Produção, 2-Gerencial

        [PrimaryKey, AutoIncrement, Column("user_id")]
        public int User_id { get => user_id; set => user_id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Sobrenome { get => sobrenome; set => sobrenome = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Cargo { get => cargo; set => cargo = value; }
        public string Setor { get => setor; set => setor = value; }
        public string UserName { get => userName; set => userName = value; }
        public int NivelAcesso { get => nivelAcesso; set => nivelAcesso = value; }

        public IntPtr Handle => throw new NotImplementedException();

        public Usuario(string name, string lastname, string pwd, string function, string dpto, int access)
        {
            this.nome = name.Trim();
            this.Sobrenome = lastname.Trim();
            this.senha = pwd;
            this.cargo = function;
            this.setor = dpto;
            //cria username a partir do nome e sobrenome ex: "Bruno Coelho" -> "bruno.coelho"            
            this.userName = string.Format("{0}", string.Join(".", new[] { this.nome.ToLower(), this.Sobrenome.ToLower() }));
            this.nivelAcesso = access;
        }

        public Usuario(string uName, string pwd)
        {
            this.nome = uName;
            this.senha = pwd;
        }

        public Usuario() { }
       
    }
}