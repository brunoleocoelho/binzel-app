using System;
using System.IO;
using System.Data;
using SQLite;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace BinzelApp3_Prototipo
{
    /** 
     * Classe cria e manipula database
     * São 4 metodos Insert, Update, InsertAll, e UpdateAll     
    */
    public class DB_PCP
    {
        //bdPath é o caminho onde está armazenado o banco de dados
        //Environment.SpecialFolder.Personal -> /data/data/[your.package.name]/files
        private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "BinzelProt2DB.db3");
        //private SQLiteConnection db;

        /**Construtor cria database e tabelas se não existir, ou se conecta com ele se existir*/
        public DB_PCP()
        {
            //if (File.Exists(dbPath))    //Se Existes
            if (!File.Exists(dbPath))     //Se NÃO existe
            {
                try
                {                
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    //var info = db.GetTableInfo("Colaborador");
                    //this.SetColaborador();
                    this.SetApontamento();
                    this.SetProduto();
                    //this.SetOrdemProducao();

                    this.SetTarefa();
                    this.SetProducao();
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine("Erro DB_PCP().SQLiteConection: " + ex.Message);
                }                
            }
            else
            {
                var dbCrtd = File.GetCreationTime(dbPath);
                Console.WriteLine("DB_PCP criado em " + dbCrtd.ToLongDateString() + "\nArmazenado: " + dbPath);
            }
        }

    
    /** METODOS PRIVADOS CRIACAO TABELAS NO DATABASE*/
        //private bool SetColaborador()
        //{
        //    try
        //    {
        //        //criando a tabela de Usuario, caso não exista
        //        SQLiteConnection db = new SQLiteConnection(dbPath);
                
        //        db.DropTable<Colaborador>();
        //        db.CreateTable<Colaborador>();

        //        //adicionando usuarios iniciais no database, caso não exista
        //        //Colaborador(string name, string lastname, string function, string dpto, string pwd, int access)
        //        List<Colaborador> usuarios = new List<Colaborador>
        //        {
        //            new Colaborador("Joao", "Silva", "Gerente", "Gerência", "123456", 3),
        //            new Colaborador("Marcos", "Pereira", "Mecânico", "Produção", "abc123", 1),
        //            new Colaborador("Mario", "Coutinho", "Mecânico", "Produção", "abc123", 1),
        //            new Colaborador("Vitor", "Lima", "Mecânico", "Produção", "abc123", 1),
        //            new Colaborador("Roberto", "Santos", "Mecânico", "Produção", "abc123", 1),
        //            new Colaborador("Jose", "Costa", "Mecânico", "Produção", "abc123", 1),
        //            new Colaborador("Paulo", "Gomes", "Supervisor", "Produção", "brasil123", 2)
        //        };

        //        foreach (var user in usuarios)  {
        //            if (db.Insert(user) != 0)
        //            {
        //                db.Update(user);
        //            }                    
        //        }

        //        return true;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.SetColaborador(): " + ex.Message);
        //        return false;
        //    }
        //}

        //criando apontamentos ficticios para testes
        //baseadas no plano de implamtação da Abicor Binzel
        private bool SetApontamento()
        {
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateTable<Apontamento>();

                List<Apontamento> listaApont = new List<Apontamento>
                {
                    new Apontamento(1, "Setup (Preparação da Máquina)", "Usinagem", "atividade"),
                    new Apontamento(2, "PRODUÇÃO", "Todas", "produção"),
                    new Apontamento(3, "Falta Ordem de Produção", "Todas", "parado"),
                    new Apontamento(4, "Limpeza da Máquina entre O.P.s", "Usinagem", "atividade"),
                    new Apontamento(5, "Problemas / falta de Ferramentas", "Todas", "parado"),
                    new Apontamento(6, "Problemas com Materia Prima / Componete", "Todas", "parado"),
                    new Apontamento(7, "Teste ou Produção - Qualidade / Engenharia / Marketing", "Todas", "atividade"),
                    new Apontamento(8, "Reparo ou Manutenção da Máquina ou Estação de Trabalho", "Todas", "parado"),
                    new Apontamento(9, "Falta / Atraso / Saída antecipada", "Todas", "parado"),
                    new Apontamento(10, "Falta Materiais / Suprimentos / Insumos", "Todas", "parado"),
                    new Apontamento(11, "Reuniões / Confraternização / Exames Periodicos", "Todas", "parado"),
                    new Apontamento(12, "À espera de reparos", "Todas", "parado"),
                    new Apontamento(13, "Força Maior (falta de energia, ar comprimido, etc)", "Todas", "parado"),
                    new Apontamento(16, "Intervalo almoço", "Todas", "parado"),
                    new Apontamento(17, "Higiene Pessoal / Pausa para café", "Todas", "parado"),
                    new Apontamento(18, "Desmonte de Material", "Todas", "atividade"),
                    new Apontamento(19, "Auxiliando Almoxarifado", "Todas", "atividade"),
                    new Apontamento(20, "Limpeza / Organização - Setor ou Predial", "Todas", "atividade"),
                    new Apontamento(21, "Manutenção Predial", "Todas", "atividade"),
                    new Apontamento(22, "Inventário na Produção", "Todas", "atividade"),
                    new Apontamento(23, "Atraso no Transporte (onibus)", "Todas", "parado"),
                    new Apontamento(24, "Retrabalho", "Todas", "atividade"),
                    new Apontamento(25, "Atividade de supervisão", "Todas", "ativiade"),
                    new Apontamento(26, "Atividade de Reforma", "Todas", "atividade"),
                    new Apontamento(27, "Atividade CNC", "Todas", "atividade"),
                    new Apontamento(28, "Aguardando Qualidade / Engenharia", "Todas", "parado"),
                    new Apontamento(29, "Seviços externos", "Todas", "atividade")
                };

                foreach (var apt in listaApont)
                {
                    if (db.Insert(apt) != 0)  {
                        db.Update(apt);
                    }
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.SetApontamento(): " + ex.Message);
                return false;
            }
 
        }

        //criando produtos ficticios para testes
        private bool SetProduto()
        {
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateTable<Produto>();

                List<Produto> prodList = new List<Produto>
                {
                    new Produto("014.0143-10", "TOCHA MIG/MAG MB 36KD 3M ERGO", "pc"),
                    new Produto("002.0451-10", "TOCHA MIG/MAG MB 15AK 5M ERGO", "pc"),
                    new Produto("Z00.2570-10", "TOCHA MIG/MAG KHBR 400 3M S", "pc"),
                    new Produto("Z00.0549-10", "TOCHA MIG/MAG MB 300 5M S", "pc"),
                    new Produto("Z00.1966-10", "TOCHA MIG/MAG TMN501 3M", "pc"),
                    new Produto("Z00.2122-07", "PpcHO ERGONOMICO AZUL COMPLETO - FURO 19,5MM", "pc"),
                    new Produto("160.0237-09", "MONOCABO BIKOX R6 2M", "pc"),
                    new Produto("Z00.4026-09", "MONOCABO BIKOX V18 2,35M (L)", "pc"),
                    new Produto("10N24-10", "PINCA D2,4X50MM", "pc"),
                    new Produto("10N31-10", "PORTA PINCA 1,6MM", "pc"),
                    new Produto("831.0352-10", "JpcTA 5/2 PARA VALVULA", "pc"),
                    new Produto("831.0023-10", "FRESA BOCAL NW 15,5MM (F455) 39MM", "pc"),
                    new Produto("002.0009-10", "PESCOCO MB 15AK 50GR", "pc"),
                    new Produto("173.0010-10", "ABRACADEIRA D 15,0MM C/COMPLEMENTO", "pc"),
                    new Produto("Z00.0185-10", "CONEXAO TUBO P/ ALIMENTADOR PACK (BELGO)", "pc"),
                    new Produto("155.B006-10", "TUBO AZUL  ALIMENTADOR PACK 6M", "pc"),
                    new Produto("501.0114-10", "PLUG DE ACOPLADOR RAPIDO", "pc"),
                    new Produto("155.B005-10", "TUBO AZUL  ALIMENTADOR PACK 5M", "un"),
                    new Produto("780.3157-10", "CILINDRO ICAT - WH", "pc"),
                    new Produto("850.0292-10", "FILTRO P/ VALVULA DE SEGURANCA", "pc"),
                    new Produto("13N22-10", "PINCA 1,6X25,4MM", "pc")
                };

                foreach (var prd in prodList)
                {
                    if (db.Insert(prd) != 0) { 
                        db.Update(prd);
                    }
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.SetProduto(): " + ex.Message);
                return false;
            }
        }

        //criação da tabela OrdemProducao, simulando OPs no database
        //private bool SetOrdemProducao()
        //{
        //    try
        //    {
        //        SQLiteConnection db = new SQLiteConnection(dbPath);
        //        db.DropTable<OrdemProducao>();
        //        db.CreateTable<OrdemProducao>();

        //        //criando registros ficticios. 
        //        List<OrdemProducao> ls = new List<OrdemProducao>
        //        {
        //            new OrdemProducao(630405, "850.0292-10", 8, "Cliente A", ""),    // new DateTime(2018, 04, 10, 08, 30, 01), new DateTime(2018, 04, 11, 10, 30, 15), "completo"
        //            new OrdemProducao(789455, "780.3157-10", 2, "Cliente B", ""),    // new DateTime(2018, 04, 11, 09, 15, 25), new DateTime(2018, 04, 12, 08, 25, 03), "cancelado"
        //            new OrdemProducao(789456, "831.0023-10", 27, "Cliente B", ""),    // new DateTime(2018, 04, 11, 10, 0, 10), new DateTime(), "parado"
        //            new OrdemProducao(789717, "831.0352-10", 1, "Cliente A", ""),    // new DateTime(2018, 04, 12, 14, 15, 40), new DateTime(),"producao"
        //            new OrdemProducao(789724, "780.3157-10", 2, "Cliente A", ""),    // new DateTime(2018, 04, 15, 10, 35, 40), new DateTime(),"producao"
        //            new OrdemProducao(790593, "002.0009-10", 15, "Cliente C", ""),    // new DateTime(2018, 04, 15, 11, 10, 30), new DateTime(),"aberto"
        //            new OrdemProducao(790596, "014.0143-10", 7, "Cliente C", ""),    // new DateTime(2018, 04, 15, 11, 10, 30), new DateTime(),"aberto"
        //            new OrdemProducao(790600, "10N24-10", 90, "Cliente C", ""),    // new DateTime(2018, 04, 15, 11, 10, 30), new DateTime(),"aberto"
        //            new OrdemProducao(790601, "10N31-10", 20, "Cliente B", ""),    // new DateTime(2018, 04, 15, 12, 00, 00), new DateTime(),"aberto"
        //            new OrdemProducao(790660, "Z00.1966-10", 4, "Cliente B", ""),    // new DateTime(2018, 04, 15, 12, 00, 00), new DateTime(),"aberto"
        //            new OrdemProducao(790672, "Z00.2122-07", 33, "Cliente C", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790692, "13N22-10", 10, "Cliente A", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790702, "002.451-10", 10, "Cliente A", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790706, "10N24-10", 90, "Cliente B", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790726, "155.8005-10", 5, "Cliente B", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790727, "155.8006-10", 5, "Cliente B", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790728, "173.0010-10", 20, "Cliente D", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790730, "501.0114-10", 20, "Cliente D", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790748, "Z00.0185-10", 2, "Cliente E", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790750, "Z00.0549-10", 9, "Cliente E", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790753, "Z00.2570-10", 2, "Cliente C", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790761, "160.0237-09", 5, "Cliente D", ""),    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //            new OrdemProducao(790762, "Z00.4026-09", 3, "Cliente D", "")    // new DateTime(2018, 04, 15, 13, 15, 45), new DateTime(),"aberto"
        //        };

        //        //adicionando ordens producao se não existem
        //        foreach (var op in ls)
        //        {
        //            if (db.Insert(op) != 0){
        //                db.Update(op);//atualizando
        //            }
        //        }
        //        return true;
        //    }
        //    catch(SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.SetOrdemProducao(): " + ex.Message);
        //        return false;
        //    }
        //}

        //criação da tabela Tarefa
        private bool SetTarefa()
        {
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.DropTable<Tarefa>();
                db.CreateTable<Tarefa>();

                //criando registros ficticios para tarefas            
                List<Tarefa> tarefas = new List<Tarefa> {
                    new Tarefa(630405, 2, 1),
                    new Tarefa(630405, 3, 1),
                    new Tarefa(789455, 2, 2),
                    new Tarefa(789456, 3, 2),
                    new Tarefa(789456, 2, 1),
                    new Tarefa(789717, 2, 2),
                    new Tarefa(789724, 3, 1),
                    new Tarefa(789717, 3, 2)                
                };

                foreach (var trf in tarefas)
                {
                    int x = db.Insert(trf);
                    if (x != 0) { 
                        db.Update(trf);
                    }
                }                
                return true;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.SetTarefa(): " + ex.Message);
                return false;
            }
        }

        //criando tabela de produção
        private bool SetProducao()
        {
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateTable<Producao>();
                return true;
            }
            catch(SQLiteException ex)
            {
                Console.WriteLine("Erro SetProducao(): " + ex.Message);
                return false;
            }
        }




    /**METODOS PUBLICOS DE CHAMADA AO DATABASE*/
        public Usuario GetColaborador(string usr, string pwd)
        {
            Usuario usuario = new Usuario();
            string url = "http://10.0.0.108:8624/rest/loginuser?";
            url += "usr=" + usr.Trim();
            url += "&pwd=" + pwd;

            var uri = new Uri(url);
            var wClient = new WebClient();
            wClient.DownloadDataAsync(uri);
            wClient.DownloadDataCompleted += (s, e) =>
            {
                try
                {
                    //var jsonArray = e.Result.Clone();
                    var strJson = Encoding.UTF8.GetString(e.Result);
                    if (!string.IsNullOrEmpty(strJson))
                        usuario = JsonConvert.DeserializeObject<Usuario>(strJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO WebClient: " + ex.Message);
                    usuario = null;
                }
            };

            return usuario;

            // trecho removido 22-MAI-2018 Bruno Coelho
            //usr = usr.Trim();
            //pwd = pwd.Trim();
            //try
            //{
            //    SQLiteConnection db = new SQLiteConnection(dbPath);
            //    var user = db.Table<Colaborador>().Where(u => u.UserName == usr && u.Senha == pwd).FirstOrDefault();
            //    return user ?? null;
            //}
            //catch (SQLiteException ex)
            //{
            //    Console.WriteLine("Erro DB_PCP.GetColaborador(): " + ex.Message);
            //    return null;
            //}
        }

        /// <summary>
        /// Traz as ordens de producao do database, filtrando "status":
        /// 0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado
        /// </summary>
        /// <param name="sts">Status da OP: todos=0, aberto=1, producao=2, parado=3, completo=4, cancelado=5</param>
        /// <returns>Retorna a lista de OPs, ou nulo se não encontrar nada</returns>
        //public List<OrdemProducao> GetOrdensProducaoPorStatus(int sts)
        //{
        //    try
        //    {
        //        List<OrdemProducao> listOP = new List<OrdemProducao>();
        //        SQLiteConnection db = new SQLiteConnection(dbPath);
        //        TableQuery<OrdemProducao> orders;

        //        orders = db.Table<OrdemProducao>()
        //            .Where(o => o.StatusOP == sts)
        //            .OrderBy(or => or.NumOP);          

        //        foreach (var op in orders) {
        //            listOP.Add(op);
        //        }
        //        return listOP;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.GetOrdensProducao(): " + ex.Message);
        //        return null;
        //    }
        //}

        //Traz o registro da tabela OrdemProducao pelo numero da OP
        //public OrdemProducao GetOrdemProducao(int numOP)
        //{
        //    try
        //    {
        //        OrdemProducao op = new OrdemProducao();
        //        SQLiteConnection db = new SQLiteConnection(dbPath);
        //        op = db.Table<OrdemProducao>().Where(o => o.NumOP == numOP).FirstOrDefault();
        //        return op ?? null; //if ok return op, else null
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.GetOrdemProducao(): " + ex.Message);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Traz as tarefas associadas ao numero de OP e colaborador
        /// </summary>
        //public List<Tarefa> GetTarefasPorOp(int numOP, int matColab)
        //{
        //    try
        //    {
        //        List<Tarefa> listaTarefas = new List<Tarefa>();
        //        SQLiteConnection con = new SQLiteConnection(dbPath);
        //        TableQuery<Tarefa> trfs;

        //        trfs = con.Table<Tarefa>()
        //            .Where(x => x.NumOP == numOP && x.MatriculaColab == matColab)
        //            .OrderBy(x => x.DtHrCriacao);

        //        foreach (Tarefa item in trfs)   {
        //            listaTarefas.Add(item);
        //        }
        //        return listaTarefas;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.GetTarefasPorOp(): " + ex.Message);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Retorna as tarefas associadas a numero de OP passado
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        //public List<Tarefa> GetTarefasPorOp(int op)
        //{
        //    try
        //    {
        //        List<Tarefa> listaTarefas = new List<Tarefa>();
        //        SQLiteConnection con = new SQLiteConnection(dbPath);
        //        TableQuery<Tarefa> trfs;

        //        trfs = con.Table<Tarefa>()
        //            .Where(x => x.NumOP == op)
        //            .OrderBy(x => x.DtHrCriacao);

        //        if (trfs.Count() > 0)
        //        {
        //            foreach (var item in trfs)
        //            {
        //                listaTarefas.Add(item);
        //            }
        //        }
        //        return listaTarefas;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.GetTarefasPorOp(): " + ex.Message);
        //        return null; ;
        //    }
        //}

        /// <summary>        
        /// Traz as tarefas associadas a uma matricula de colaborador
        /// com um status especificado: 0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado
        /// </summary>
        /// <param name="matrColab"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        //public List<Tarefa> GetTarefasPorColaborador(int matrColab, int status)
        //{
        //    try
        //    {
        //        SQLiteConnection con = new SQLiteConnection(dbPath);
        //        List<Tarefa> listaTarefas = new List<Tarefa>();
        //        var aux = con.Table<Tarefa>()
        //            .Where(x => x.MatriculaColab == matrColab && x.Status == status)
        //            .OrderBy(x => x.NumOP);
        //        if (aux.Count() > 0)
        //        {
        //            foreach (var item in aux)
        //            {
        //                listaTarefas.Add(item);
        //            }
        //        }
        //        return listaTarefas;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.GetTarefasPorColaborador(): " + ex.Message);
        //        return null; ;
        //    }
        //}


        /// <summary>
        /// Busca um produto pelo seu código, retornando tipo classe Produto
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Produto GetProdutoPorCodigo (string codigo)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(dbPath);
                Produto pd = new Produto();
                pd = con.Get<Produto>(codigo);
                return pd;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.GetProdutoPorCodigo(): " + ex.Message);
                return null; ;
            }
        }

        /// <summary>
        /// Retorna a lista de registros de produção referente ao id da tarefa
        /// </summary>
        /// <returns></returns>
        public List<Producao> GetProducaoPorIdTarefa(int idTarefa)
        {
            try
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                List<Producao> listaProd = new List<Producao>();
                var ls = from p in db.Table<Producao>()
                         where p.IdTarefa == idTarefa
                         select p;
                if (ls != null)
                {
                    foreach (var item in ls)   {
                        listaProd.Add(item);
                    }
                }
                return listaProd;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.GetProducaoPorIdTarefa(): " + ex.Message);
                return null; 
            }
        }

        /// <summary>
        /// Insere um registro de Producao baseado numa tarefa e apontamento
        /// </summary>
        /// <param name="cod_apont"></param>
        /// <param name="id_tarefa"></param>
        /// <returns></returns>
        //public int InsertProducaoApont(int cod_apont, Tarefa trf)
        //{
        //    try
        //    {
        //        SQLiteConnection con = new SQLiteConnection(dbPath);
                
        //        //Status de Tarefa como iniciado
        //        trf.Status = 1;
        //        int x = con.Update(trf);
                
        //        //Status de OP  como iniciado
        //        var ordProd = con.Table<OrdemProducao>().Where(a => a.NumOP == trf.NumOP).FirstOrDefault();
        //        ordProd.StatusOP = 1;
        //        int z = con.Update(ordProd);
                
        //        if ( x>0 && z>0 ){
        //            Producao aptProd = new Producao();
        //            aptProd.CodApont = cod_apont;
        //            aptProd.IdTarefa = trf.IdTarefa;
        //            aptProd.QtdProduzida = 0;
        //            aptProd.DtHrInicial = DateTime.Now;
        //            aptProd.Status = 1; //1-iniciado, 2-fechado

        //            int y = con.Insert(aptProd);
        //            return y;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Erro Update (Tarefa): " + trf.IdTarefa);
        //            return 0;
        //        }
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.InsertApontProducao(): " + ex.Message);
        //        return 0;
        //    }            
        //}

        /// <summary>
        /// Atualiza o registro de producao passado por parametro
        /// </summary>
        /// <param name="prd"></param>
        /// <returns></returns>
        public bool UpdateProducaoApont(Producao prd)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(dbPath);
                prd.DtHrFinal = DateTime.Now;
                prd.Status = 2; //fechando
                //prd.QtdProduzida
                int x = con.Update(prd);
                return x !=0 ? true : false;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.UpdateProducaoApont(): " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Atualiza o ultimo registro de Produção de um Colaborador numa OP
        /// </summary>
        /// <param name="matrColb"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        //public bool UpdateProducaoApont(int matrColab, int idTarefa, int op, int qtdProduzida)
        //{
        //    try
        //    {
        //        SQLiteConnection db = new SQLiteConnection(dbPath);

        //        //verifica se existem tarefas do id especificado na tabela de producao, e atualiza a última
        //        var apt = db.Table<Producao>().Where(x => x.IdTarefa == idTarefa).OrderByDescending(x => x.RegProducao).First();
        //        if (apt != null)
        //        {
        //            apt.DtHrFinal = DateTime.Now;
        //            apt.QtdProduzida = qtdProduzida;
        //            apt.Status = 2; //registro fechado                    
        //            db.Update(apt);
        //        }
        //        return true;
        //    }
        //    catch (SQLiteException ex)
        //    {
        //        Console.WriteLine("Erro DB_PCP.UpdateApontProducaoPorColab(): " + ex.Message);
        //        return false;
        //    }
        //}

        /// <summary>
        /// Traz o registro de Producao
        /// </summary>
        /// <param name="regProd"></param>
        /// <returns></returns>
        public Producao GetProducao(int regProd)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(dbPath);
                Producao prd = new Producao();
                prd = con.Get<Producao>(regProd);
                return prd;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.GetProducao(): " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retorna a lista de TODOS os apontamentos armazenados
        /// </summary>
        /// <returns></returns>
        public List<Apontamento> GetApontamentos()
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(dbPath);
                List<Apontamento> ap = new List<Apontamento>();
                var ls = con.Table<Apontamento>().OrderBy(x => x.CodApont);
                if (ls.Count() > 0)
                {
                    foreach (var item in ls)
                    {
                        ap.Add(item);
                    }
                }
                return ap;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.GetApontamentos(): " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Retorna a lista de apontamentos armazenados filtrando pelo status passado: 1-produtivo, 2-parado, 3-atividade
        /// </summary>
        /// <param name="status">1-produtivo, 2-parado, 3-atividade</param>
        /// <returns></returns>
        public List<Apontamento> GetApontamentosPorTipo(int status)
        {
            try
            {
                SQLiteConnection con = new SQLiteConnection(dbPath);
                List<Apontamento> apt = new List<Apontamento>();

                string strStatus;
                //1-produtivo, 2-parado, 3-atividade
                switch (status)
                {
                    case 1: strStatus = "produtivo"; break;
                    case 2: strStatus = "parado"; break;
                    case 3:
                    default: strStatus = "atividade"; break;
                }

                var ls = con.Table<Apontamento>()
                    .Where(x => x.StatusPadrao == strStatus)
                    .OrderBy(x => x.CodApont);

                if (ls.Count() > 0)
                {
                    foreach (var item in ls)
                    {
                        apt.Add(item);
                    }
                }
                return apt;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Erro DB_PCP.GetApontamentosPorTipo(): " + ex.Message);
                return null;
            }
        }
        
    }


}