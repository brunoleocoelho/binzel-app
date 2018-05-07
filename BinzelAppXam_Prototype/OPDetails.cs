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

namespace BinzelApp_Prototype
{
    [Activity(Label = "OPDetails", Theme ="@style/Binzel")]
    public class OPDetails : Activity
    {
        private int OP;
        private Usuario usr = new Usuario();
        private OrdemProducao objOP = new OrdemProducao();
        private List<Tarefa> tarefas;
        private ListView listaNaTela;

        private TextView userMenu;
        private TextView numOP;
        private TextView cliente;
        private TextView desc;
        private TextView status;
        private ImageView statusImg;
        private TextView dtInicio;
        private TextView dtFinal;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetailsOP);

            //pegando variáveis passadas pelo Activity anterior            
            OP = Intent.GetIntExtra("op", 0);
            if (OP != 0)
            {
                //pegando variáveis passadas pelo Activity anterior 
                usr.User_id = Intent.GetIntExtra("usrId", 0);
                usr.Nome = Intent.GetStringExtra("usrNome");
                usr.Sobrenome = Intent.GetStringExtra("usrSobreNome");
                usr.Cargo = Intent.GetStringExtra("usrCargo");
                usr.Setor = Intent.GetStringExtra("usrSetor");
                usr.NivelAcesso = Intent.GetIntExtra("usrNivel", 0);

                //instanciando objetos do HEADER
                numOP = FindViewById<TextView>(Resource.Id.DtOpTxt_num);
                cliente = FindViewById<TextView>(Resource.Id.DtOpTxt_client);
                desc = FindViewById<TextView>(Resource.Id.DtOpTxt_desc);
                status = FindViewById<TextView>(Resource.Id.DtOpTxt_status);
                statusImg = FindViewById<ImageView>(Resource.Id.DtOpImg_status);
                dtInicio = FindViewById<TextView>(Resource.Id.DtOpTxt_inicio);
                dtFinal = FindViewById<TextView>(Resource.Id.DtOpTxt_fim);

                userMenu = FindViewById<TextView>(Resource.Id.menuProd_User);
                userMenu.Text = string.Format("{0} {1}, {2}", usr.Nome, usr.Sobrenome, usr.Setor);

                //substituindo toolbar original pela custom
                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                SetActionBar(toolbar: toolbar);
                ActionBar.Title = Resources.GetString(Resource.String.detailsOP_titulo);
                ActionBar.SetDisplayHomeAsUpEnabled(true);//define aparição do botão de voltar

                //buscando OP no banco
                DB_PCP db = new DB_PCP();
                objOP = db.GetOrdemProducao(OP);

                numOP.Text = objOP.NumOP.ToString();
                cliente.Text = objOP.Cliente;
                desc.Text = objOP.DescricaoOP;
                status.Text = objOP.StatusOP;

                int stsId;
                switch (objOP.StatusOP) {
                    case "aberto": stsId = Resource.Drawable.sign_gray_play; break;
                    case "producao": stsId = Resource.Drawable.sign_blue_in_process; break;
                    case "parado": stsId = Resource.Drawable.sign_yellow_warning; break;
                    case "completo": stsId = Resource.Drawable.sign_green_dot; break;
                    case "cancelado": stsId = Resource.Drawable.sign_red_cancel; break;
                    default: stsId = Resource.Drawable.sign_gray_play; break;
                }
                statusImg.SetImageResource(stsId);

                dtInicio.Text = objOP.DtHrInicialOP.Year == 1 ? "-" : objOP.DtHrInicialOP.ToShortDateString();
                dtFinal.Text = objOP.DtHrFinalOP.Year == 1 ? "-" : objOP.DtHrFinalOP.ToShortDateString();

                //se acesso for gerencial, muda HEADER para cinza claro (apenas p/ identificação)
                //if (usr.NivelAcesso == 2) {
                //    var header = FindViewById<RelativeLayout>(Resource.Id.DtOpRelLay_Header);                    
                //    header.Background = Resources.GetDrawable(Resource.Color.gray);
                //}

                //populando a lista de tarefas da OP             
                listaNaTela = FindViewById<ListView>(Resource.Id.DtOpListView);
                tarefas = db.GetTarefasPorOP(objOP.NumOP);
                TarefaListViewAdapter adapter = new TarefaListViewAdapter(this, tarefas);
                listaNaTela.Adapter = adapter;
                listaNaTela.ItemClick += (sender, e) => {
                    Toast.MakeText(this, 
                        "Tarefa " + tarefas[e.Position].CodApont.ToString() +" - "+ tarefas[e.Position].StatusTarefa, 
                        ToastLength.Short).Show();
                };
            }
            else
            {
                AlertDialog.Builder alerta = new AlertDialog.Builder(this);
                alerta.SetTitle(Resource.String.warning);
                alerta.SetMessage(Resources.GetString(Resource.String.detailsOP_MsgOPVazio));
                alerta.SetNeutralButton(Resource.String.btn_cancel, delegate {
                    this.Finish();
                });
            }
        }

        // Carrega o menu customizado e "infla" na tooblar da tela
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.menuProd_OpDetails_bar, menu); //menuProd_OpDetails_bar.XML    

            //Status: aberto=1, producao=2, parado=3, completo=4, cancelado=5
            if (objOP.StatusOP != "completo" || objOP.StatusOP != "cancelado")
            {
                if (usr.NivelAcesso == 1){
                    //ocultando "Ad.Colab" para produção
                    menu.FindItem(Resource.Id.menuProd_Op_AddColab).SetVisible(false); 
                }
                else if (usr.NivelAcesso == 2) {
                    //ocultando "Ini.Prod." para gerencial
                    menu.FindItem(Resource.Id.menuProd_Op_iniciar).SetVisible(false);
                }
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
                Intent intentDesenho = new Intent(this, typeof(Desenho));
                intentDesenho.PutExtra("descricao", objOP.NumOP.ToString());
                StartActivity(intentDesenho);
            }
            else if (id == Resource.Id.menuProd_Op_iniciar) {
                // acesso produção
                StartActivity(new Intent(this, typeof(OPEmProducao)));
                //Toast.MakeText(this, "Produção clicou Inciar Prod.", ToastLength.Short).Show();
            }
            else if (id == Resource.Id.menuProd_Op_AddColab) { 
                //acesso gerencial
                Toast.MakeText(this, "Gerencial clicou Adic.Colaborador", ToastLength.Short).Show();
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}