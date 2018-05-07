using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BinzelApp2_Prototipo
{
    [Activity(Label = "OPDetails", Theme ="@style/Binzel")]
    public class OPDetails : Activity
    {
        private int OP;
        private DB_PCP db = new DB_PCP();
        private Bundle bld = new Bundle();
        private Colaborador usr = new Colaborador();
        private OrdemProducao objOP = new OrdemProducao();
        private Produto prd = new Produto();
        private List<Tarefa> tarefas = new List<Tarefa>();
        //private List<Producao> regProducao = new List<Producao>();
        private ListView listaNaTela;

        private TextView userMenu;
        private TextView numOP;
        private TextView cliente;
        private TextView prod;
        private TextView status;
        private ImageView statusImg;
        private TextView dtInicio;
        private TextView dtFinal;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetailsOP);

            //pegando numero da OP passada pelo Activity anterior                        
            OP = Intent.GetIntExtra("op", 0);
            if (OP != 0)
            {
                //recebendo as variaveis passadas da Activity anterior
                usr.Matricula = Intent.GetIntExtra("usrMatr", 0);
                usr.Nome = Intent.GetStringExtra("usrNome");
                usr.Sobrenome = Intent.GetStringExtra("usrSobreNome");
                usr.Cargo = Intent.GetStringExtra("usrCargo");
                usr.Setor = Intent.GetStringExtra("usrSetor");
                usr.NivelAcesso = Intent.GetIntExtra("usrNivel", 0);

                //instanciando objetos do HEADER da OP
                numOP = FindViewById<TextView>(Resource.Id.DtOpTxt_num);
                cliente = FindViewById<TextView>(Resource.Id.DtOpTxt_client);
                prod = FindViewById<TextView>(Resource.Id.DtOpTxt_produto);
                status = FindViewById<TextView>(Resource.Id.DtOpTxt_status);
                statusImg = FindViewById<ImageView>(Resource.Id.DtOpImg_status);
                dtInicio = FindViewById<TextView>(Resource.Id.DtOpTxt_inicio);
                dtFinal = FindViewById<TextView>(Resource.Id.DtOpTxt_fim);

                //label contendo usuario e setor logado
                userMenu = FindViewById<TextView>(Resource.Id.menuProd_User);
                userMenu.Text = string.Format("{0} {1}, {2}", usr.Nome, usr.Sobrenome, usr.Setor);

                //substituindo toolbar original pela custom
                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                SetActionBar(toolbar: toolbar);
                ActionBar.Title = Resources.GetString(Resource.String.detailsOP_titulo);
                ActionBar.SetDisplayHomeAsUpEnabled(true);//define aparição do botão de voltar

                //lista de com registos de produção da OP             
                listaNaTela = FindViewById<ListView>(Resource.Id.DtOpListView);

                this.SetHeaderOPDetails();
                this.SetListaNaTela();

            }
            else
            {
                AlertDialog.Builder alerta = new AlertDialog.Builder(this);
                alerta.SetTitle(Resource.String.warning);
                alerta.SetMessage(Resources.GetString(Resource.String.detailsOP_MsgOPVazio));
                alerta.SetNeutralButton(Resource.String.btn_cancel, delegate {
                    this.Finish();
                });
                alerta.Show();
            }

            //criando variaveis de passagem de Activities
            bld.PutInt("usrMatr", usr.Matricula);
            bld.PutString("usrNome", usr.Nome);
            bld.PutString("usrSobreNome", usr.Sobrenome);
            bld.PutString("usrCargo", usr.Cargo);
            bld.PutString("usrSetor", usr.Setor);
            bld.PutInt("usrNivel", usr.NivelAcesso);
            bld.PutInt("OP", OP);
            bld.PutInt("QtdOP", objOP.QtdTotal);
            bld.PutString("Cliente", objOP.Cliente);
            bld.PutString("Produto", string.Format("{0} - {1}", objOP.CodProduto, prd.Descricao));
        }


        //chamado quando inicia ou volta para Activity
        //protected override void OnStart()
        //{
        //    this.SetHeaderOPDetails();
        //    this.SetListaNaTela();
        //    //base.OnStart();
        //}


        //protected override void OnResume()
        //{
        //    this.SetHeaderOPDetails();
        //    this.SetListaNaTela();
        //    //base.OnRestart();
        //}


        // Carrega o menu customizado e "infla" na tooblar da tela
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menuProd_OpDetails_bar, menu); //menuProd_OpDetails_bar.XML    
           
            switch (usr.NivelAcesso)
            {
                case 1: //ocultar "Ad.Colab" para produção
                    menu.FindItem(Resource.Id.menuProd_Op_AddColab).SetVisible(false);
                    break;
                case 2:
                case 3: //ocultar "Ini.Prod." para gerencial/pcp
                    menu.FindItem(Resource.Id.menuProd_Op_iniciar).SetVisible(false);
                    break;
            }
            return base.OnCreateOptionsMenu(menu);
        }
        

        //manipula os itens selecionados do menu da toolbar
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Android.Resource.Id.Home) { //back button
                this.OnBackPressed();
            }
            else if (id == Resource.Id.menuProd_Op_desenho) {
                //Toast.MakeText(this, "Marcou VER DESENHO", ToastLength.Short).Show();
                Intent intentDesenho = new Intent(this, typeof(Desenho));
                intentDesenho.PutExtra("descricao", objOP.NumOP.ToString());
                StartActivity(intentDesenho);
            }
            else if (id == Resource.Id.menuProd_Op_iniciar) {
                // acesso produção
                //StartActivity(new Intent(this, typeof(OPEmProducao)));
                //Toast.MakeText(this, "Marcou INICIAR PROD.", ToastLength.Short).Show();
                ApontarProducaoNova();
            }
            else if (id == Resource.Id.menuProd_Op_AddColab) { 
                //acesso gerencial
                Toast.MakeText(this, "Marcou ADIC.COLABORADOR", ToastLength.Short).Show();
            }
            return base.OnOptionsItemSelected(item);
        }


        /// <summary>
        /// Atualiza o cabeçalho da activity
        /// </summary>
        private void SetHeaderOPDetails()
        {
            //buscando OP no banco
            objOP = db.GetOrdemProducao(OP);
            prd = db.GetProdutoPorCodigo(objOP.CodProduto);

            numOP.Text = objOP.NumOP.ToString();
            cliente.Text = objOP.Cliente;
            prod.Text = string.Format("{0} - {1}", objOP.CodProduto, prd.Descricao);
            //status.Text = objOP.StatusOP;

            int stsId;
            //0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado
            switch (objOP.StatusOP)
            {
                case 1:
                    stsId = Resource.Drawable.sign_blue_in_process;
                    status.Text = "Iniciado";
                    break;
                case 2:
                    stsId = Resource.Drawable.sign_green_dot;
                    status.Text = "Completo";
                    break;
                case 3:
                    stsId = Resource.Drawable.sign_red_cancel;
                    status.Text = "Cancelado";
                    break;
                default: //case: 0
                    stsId = Resource.Drawable.sign_gray_not_started;
                    status.Text = "Não Inciado";
                    break;
            }
            statusImg.SetImageResource(stsId);

            dtInicio.Text = objOP.DtHrCriacao.Year == 1 ? "--/--/----" : objOP.DtHrCriacao.ToShortDateString();
            dtFinal.Text = objOP.DtHrFinalOP.Year == 1 ? "--/--/----" : objOP.DtHrFinalOP.ToShortDateString();

            //se acesso for gerencial, muda HEADER para cinza claro (apenas p/ identificação)
            //if (usr.NivelAcesso == 2) {
            //    var header = FindViewById<RelativeLayout>(Resource.Id.DtOpRelLay_Header);                    
            //    header.Background = Resources.GetDrawable(Resource.Color.gray);
            //}
        }


        /// <summary>
        /// Traz todas as tarefas associadas a OP em questão e
        /// Popula a lista que será carregada na tela
        /// </summary>
        private void SetListaNaTela()
        {
            //producao
            if (usr.NivelAcesso == 1)
                tarefas = db.GetTarefasPorOp(objOP.NumOP, usr.Matricula);
            //outros
            else
                tarefas = db.GetTarefasPorOp(objOP.NumOP);

            if (tarefas != null)
            {
                List<Producao> lista = new List<Producao>();
                foreach (var trf in tarefas)
                {
                    var trfList = db.GetProducaoPorIdTarefa(trf.IdTarefa);
                    if (trfList.Count() > 0)
                    {
                        foreach (var item in trfList)
                        {
                            lista.Add(item);
                        }
                    }
                }

                if (lista != null)
                {
                    if (lista.Count() > 0)
                    {
                        //se lista de registros de produção não é vazia, adicionar na tela
                        Producao_ListViewAdapter adapter = new Producao_ListViewAdapter(this, lista);
                        listaNaTela.Adapter = adapter;
                        listaNaTela.ItemClick += (sender, e) => {
                            //se iniciado (1)
                            if (lista[e.Position].Status == 1)
                            {

                                if (usr.NivelAcesso == 1)
                                    this.ProduzirApontamento(lista[e.Position]);
                                else
                                    Toast.MakeText(this, "Item ainda em produção", ToastLength.Short).Show();
                            }
                            //se fechado (2)
                            else
                            {
                                var lblCodApt = Resources.GetString(Resource.String.lbl_ApontCod);
                                var lblQtdPrd = Resources.GetString(Resource.String.lbl_QtdProd);
                                var lblIni = Resources.GetString(Resource.String.lbl_Ini_list);
                                var lblFim = Resources.GetString(Resource.String.lbl_Fim_list);

                                Toast.MakeText(this,
                                    string.Format("{0}: {1} -> {2}: {3}\n{4}: {5}\n{6}: {7}",
                                        lblCodApt, lista[e.Position].CodApont.ToString(),
                                        lblQtdPrd, lista[e.Position].QtdProduzida.ToString(),
                                        lblIni, lista[e.Position].DtHrInicial.ToString(),
                                        lblFim, lista[e.Position].DtHrFinal.ToString()
                                    ),
                                    ToastLength.Long).Show();
                            }
                        };
                    }
                }
            }


        }


        /// <summary>
        /// Cria um novo apontamento para a produção da OP
        /// </summary>
        /// <returns></returns>
        private bool ApontarProducaoNova()
        {
            try
            {
                DB_PCP db = new DB_PCP();
                List<Apontamento> lstAp = new List<Apontamento>();
                lstAp.AddRange( db.GetApontamentosPorTipo(3) );
                string[] aux = new string[lstAp.Count];

                foreach (var item in lstAp)
                {
                    var index = lstAp.IndexOf(item);
                    var str = item.CodApont.ToString() +": "+ item.Descricao;
                    aux[index] = str;
                }

                AlertDialog.Builder alerta = new AlertDialog.Builder(this);
                alerta.SetTitle("Qual o Código de Apontamento?");
                alerta.SetItems(aux, (sender, e) => {
                    var trfApt = tarefas.FindLast(x => x.NumOP == OP);
                    if (trfApt != null) {
                        int x = db.InsertProducaoApont(lstAp[e.Which].CodApont, trfApt);
                        if (x > 0) {
                            var prdcao = db.GetProducao(x);
                            this.ProduzirApontamento(prdcao);
                        }
                    }
                    this.SetHeaderOPDetails(); //atualizando header
                    this.SetListaNaTela(); //atualiza itens na tela                    
                });
                alerta.SetNeutralButton(Resource.String.btn_cancel, (sender, e) => {
                    alerta.Dispose();
                });
                alerta.Show();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ApontarProducao(): " + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Atualiza um item de apontamento que já foi iniciado
        /// iniciando a Producao
        /// </summary>
        /// <param name="registro"></param>
        private void ProduzirApontamento(Producao registro)
        {
            Intent intent = new Intent(this, typeof(OPEmProducao));
            string[] regProd =
            {
                registro.RegProducao.ToString(),
                registro.CodApont.ToString(),
                registro.DtHrFinal.ToString(),
                registro.DtHrInicial.ToString(),
                registro.IdTarefa.ToString(),
                registro.QtdProduzida.ToString(),
                registro.Status.ToString()
            };
            //trazendo a descrição do apontamento
            var apt = db.GetApontamentos().Where(x => x.CodApont == registro.CodApont).First();

            bld.PutStringArray("regProd", regProd);
            bld.PutString("ApontDesc", apt.Descricao);
            bld.PutString("ApontCod", apt.CodApont.ToString());
            
            intent.PutExtras(bld);
            StartActivity(intent);
        }

        
        /// <summary>
        /// Atualiza um item de apontamento que já foi iniciado
        /// </summary>
        /// <param name="regProdTrf"></param>
        /// <returns></returns>
        //private void AtualizaRegProducao(Producao registro)
        //{
        //    try
        //    {
        //        Intent intent = new Intent(this, typeof(OPEmProducao));
        //        string []regProd =
        //        {
        //            registro.RegProducao.ToString(),
        //            registro.CodApont.ToString(),
        //            registro.DtHrFinal.ToString(),
        //            registro.DtHrInicial.ToString(),
        //            registro.IdTarefa.ToString(),
        //            registro.QtdProduzida.ToString(),
        //            registro.Status.ToString()
        //        };
        //        bld.PutStringArray("regProd", regProd);
        //        intent.PutExtras(bld);
        //        StartActivity(intent);

        //        //AlertDialog.Builder alerta = new AlertDialog.Builder(this);
        //        //alerta.SetTitle("Apontamento");                
        //        //alerta.SetPositiveButton("Completo", (s, e) => {
        //        //    //DB_PCP db = new DB_PCP();
        //        //    Toast.MakeText(this, "Marcou COMPLETO RegProd: "+ regProdTrf.RegProducao.ToString(), ToastLength.Short).Show();
        //        //});
        //        //alerta.SetNegativeButton("Finalizar", (s, e) => {
        //        //    //DB_PCP db = new DB_PCP();
        //        //    Toast.MakeText(this, "Marcou FINALIZAR RegProd: "+ regProdTrf.RegProducao.ToString(), ToastLength.Short).Show();
        //        //});
        //        //alerta.SetNeutralButton("Cancelar", (sender, e) => {
        //        //    alerta.Dispose();
        //        //});
        //        //alerta.Show();
        //        //return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Erro AtualizarRegProducao("+ regProdTrf.RegProducao +"):" + ex.Message);
        //        //return false;
        //    }
        //}
    }
}