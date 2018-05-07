using System;
using System.IO;
using System.Data;
using SQLite;
using System.Collections.Generic;

namespace BinzelApp_Prototype
{
    /** 
     * Classe cria e manipula database
     * São 4 metodos Insert, Update, InsertAll, e UpdateAll     
    */
    public class DB_PCP
    {
        //bdPath é o caminho onde está armazenado o banco de dados
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "binzelDB.db3");
        //private SQLiteConnection db;

    /**Construtor cria database e tabelas se não existir, ou se conecta com ele se existir*/
        public DB_PCP()
        {
            try
            {
                //string dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "ormextras.db3");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                this.SetTables();
                this.SetOPs();
                this.SetTarefas();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("ErroDB_PCP.SQLiteConection: " + ex.Message);
            }
        }

    
    /** METODOS PRIVADOS*/
        private bool SetTables()
        {
            try
            {
                //criando a tabela de Usuario, caso não exista
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateTable<Usuario>();

                //adicionando usuarios iniciais no database, caso não exista
                List<Usuario> usuarios = new List<Usuario>();
                usuarios.Add(new Usuario("Joao", "Silva", "123456", "Gerente", "Gerência",2));
                usuarios.Add(new Usuario("Marcos", "Pereira", "abc123", "Mecânico", "Produção",1));
                usuarios.Add(new Usuario("Mario", "Coutinho", "xyz789", "Mecânico", "Produção",1));
                usuarios.Add(new Usuario("Vitor", "Lima", "abcd1234", "Mecânico", "Produção",1));
                foreach (Usuario usr in usuarios)
                {
                    this.SetUsers(usr);
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("ErroSetTables: " + ex.Message);
                return false;
            }
        }

        //Verifica no incio se item já existe usuarios no database
        //se não tiver inclui, se tiver, faz update
        private bool SetUsers(Usuario usr)
        {
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                if (db.Insert(usr) != 0)
                    db.Update(usr);
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("ErroSetUsers: " + ex.Message);
                return false;
            }
        }

        //simulando ordens no database
        private bool SetOPs()
        {
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<OrdemProducao>();

            //criando registros ficticios. Status: aberto=1, producao=2, parado=3, completo=4, cancelado=5
            List<OrdemProducao> ls = new List<OrdemProducao>();
            ls.Add(new OrdemProducao(1, "Fabricação Equipamento 001", "Cliente A", new DateTime(2018, 04, 10, 08, 30, 01), new DateTime(2018, 04, 11, 10, 30, 15), "completo"));
            ls.Add(new OrdemProducao(2, "Fabricação Equipamento 002", "Cliente B", new DateTime(2018, 04, 11, 09, 15, 25), new DateTime(2018, 04, 12, 08, 25, 03), "cancelado"));
            ls.Add(new OrdemProducao(3, "Fabricação Equipamento 001", "Cliente B", new DateTime(2018, 04, 11, 10, 0, 10), new DateTime(), "parado"));
            ls.Add(new OrdemProducao(4, "Fabricação Equipamento 002", "Cliente A", new DateTime(2018, 04, 12, 14, 15, 40), new DateTime(), "producao"));
            ls.Add(new OrdemProducao(5, "Fabricação Equipamento 001", "Cliente A", new DateTime(2018, 04, 15, 10, 35, 40), new DateTime(), "producao"));
            ls.Add(new OrdemProducao(6, "Fabricação Equipamento 003", "Cliente C", new DateTime(2018, 04, 15, 11, 10, 30), new DateTime(), "aberto"));
            ls.Add(new OrdemProducao(7, "Fabricação Equipamento 001", "Cliente C", new DateTime(2018, 04, 15, 11, 10, 30), new DateTime(), "aberto"));
            ls.Add(new OrdemProducao(8, "Fabricação Equipamento 002", "Cliente B", new DateTime(2018, 04, 15, 12, 00, 00), new DateTime(), "aberto"));
            ls.Add(new OrdemProducao(9, "Fabricação Equipamento 001", "Cliente C", new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(), "aberto"));

            //adicionando ordens producao se não existem
            int x = db.InsertAll(ls);
            if (x != 0){
                db.UpdateAll(ls);//atualizando
                return false;
            }
            else return true;
        }

        //criação da tabela Tarefa
        private bool SetTarefas()
        {
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<Tarefa>();            

            //criando registros ficticios para tarefas
            List<Tarefa> tarefas = new List<Tarefa>();
                Tarefa t1 = new Tarefa(1, 1);
                t1.DtHrInicioTarefa = new DateTime(2018, 04, 10, 08, 30, 01);
                t1.DtHrFinalTarefa = new DateTime(2018, 04, 10, 09, 45, 10);
                t1.StatusTarefa = "completo";
                tarefas.Add(t1);

                Tarefa t2 = new Tarefa(4, 1);
                t2.DtHrInicioTarefa = new DateTime(2018, 04, 10, 09, 50, 10);
                t2.DtHrFinalTarefa = new DateTime(2018, 04, 11, 10, 30, 15);
                t2.StatusTarefa = "completo";
                tarefas.Add(t2);

                Tarefa t3 = new Tarefa(1, 2);
                t3.DtHrInicioTarefa = new DateTime(2018, 04, 11, 09, 15, 25);
                t3.DtHrFinalTarefa = new DateTime(2018, 04, 11, 10, 30, 15);
                t3.StatusTarefa = "cancelado";
                tarefas.Add(t3);

                Tarefa t4 = new Tarefa(4, 3);
                t4.DtHrInicioTarefa = new DateTime(2018, 04, 12, 09, 00, 25);
                t4.DtHrFinalTarefa = new DateTime(2018, 04, 12, 10, 30, 15);
                t4.StatusTarefa = "completo";
                tarefas.Add(t4);

                Tarefa t5 = new Tarefa(6, 3);
                t5.DtHrInicioTarefa = new DateTime(2018, 04, 12, 14, 15, 25);
                t5.DtHrFinalTarefa = new DateTime(2018, 04, 12, 15, 00, 15);
                t5.StatusTarefa = "completo";
                tarefas.Add(t5);

                Tarefa t6 = new Tarefa(7, 3);
                t6.DtHrInicioTarefa = new DateTime(2018, 04, 12, 15, 05, 25);
                t6.DtHrFinalTarefa = new DateTime(2018, 04, 12, 15, 40, 15);
                t6.StatusTarefa = "parado";
                tarefas.Add(t6);

                Tarefa t7 = new Tarefa(1, 4);
                t7.DtHrInicioTarefa = new DateTime(2018, 04, 12, 13, 05, 25);
                t7.DtHrFinalTarefa = new DateTime();
                t7.StatusTarefa = "producao";
                tarefas.Add(t7);

                Tarefa t8 = new Tarefa(1, 5);
                t8.DtHrInicioTarefa = new DateTime(2018, 04, 13, 08, 35, 15);
                t8.DtHrFinalTarefa = new DateTime();
                t8.StatusTarefa = "producao";
                tarefas.Add(t8);

            int x = db.InsertAll(tarefas);
            if (x != 0) { 
                db.UpdateAll(tarefas);
                return false;
            }
            else return true;
        }


    /**METODOS PUBLICOS DE CHAMADA AO DATABASE*/
        public Usuario GetUsuario(string usr, string pwd)
        {
            usr = usr.Trim();
            pwd = pwd.Trim();
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                var user = db.Table<Usuario>().Where(u => u.UserName == usr && u.Senha == pwd).FirstOrDefault();
                return user != null ? user : null;               
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("ErroGetUsuario: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Traz as ordens de producao do database, filtrando "status":
        /// todos=0, aberto=1, producao=2, parado=3, completo=4, cancelado=5
        /// </summary>
        /// <param name="sts">Status da OP: todos=0, aberto=1, producao=2, parado=3, completo=4, cancelado=5</param>
        /// <returns>Retorna a lista de OPs, ou nulo se não encontrar nada</returns>
        public List<OrdemProducao> GetOrdensProducao(int sts)
        {
            try  {
                List<OrdemProducao> listOP = new List<OrdemProducao>();
                SQLiteConnection db = new SQLiteConnection(dbPath);
                TableQuery<OrdemProducao> orders;

                if (sts == 0)
                {
                    orders = db.Table<OrdemProducao>();
                }
                else
                {
                    string status;
                    switch (sts)  {
                        case 1:     status = "aberto";       break;
                        case 2:     status = "producao";    break;
                        case 3:     status = "parado";      break;
                        case 4:     status = "completo";  break;
                        case 5:     status = "cancelado";   break;
                        default:    status = "aberto";       break;
                    }
                    orders = db.Table<OrdemProducao>()
                        .Where(o => o.StatusOP == status)
                        .OrderBy(or => or.DtHrInicialOP);
                }

                foreach (var op in orders) {
                    listOP.Add(op);
                }
                return listOP;
            }
            catch (SQLiteException ex)  {
                Console.WriteLine("ErroGetOrdensProducao: " + ex.Message);
                return null;
            }
        }

        public OrdemProducao GetOrdemProducao(int numOP)
        {
            OrdemProducao op = new OrdemProducao();
            SQLiteConnection db = new SQLiteConnection(dbPath);

            op = db.Table<OrdemProducao>().Where(o => o.NumOP == numOP).FirstOrDefault();
            if (op != null)
                return op;
            else
                return null;
            
        }

        //traz as tarefas associadas ao numero de OP
        public List<Tarefa> GetTarefasPorOP(int numOP)
        {
            List<Tarefa> listaTarefas = new List<Tarefa>();
            SQLiteConnection con = new SQLiteConnection(dbPath);
            var aux = con.Table<Tarefa>().Where(x => x.NumOP == numOP);

            foreach (Tarefa item in aux)  {
                listaTarefas.Add(item);
            }

            return listaTarefas;
        }
    }


}