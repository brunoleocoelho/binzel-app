/**
 * Descricao de parmaetros de autenticação de USUARIO:
 * http://tdn.totvs.com/pages/releaseview.action?pageId=267792734
 * https://www.codigofonte.com.br/codigos/funcoes-para-exibir-informacoes-dos-usuarios-protheus
 * 
 * Tabelas possivelmente a serem usadas
 * http://tdn.totvs.com/pages/releaseview.action?pageId=286729861
 * http://tdn.totvs.com/display/public/PROT/Fluxo+Operacional+-+SIGAPCP
 * SH6 - Movimentação de Produção
 * CVL - Controle Diário;
 * SB1 - Cadastro de produtos;
 * SB2 - Saldos físicos e financeiros;
 * SB3 - Demandas;
 * SB9 - Saldos iniciais;
 * SC2 - Ordens de produção;
 * SD3 - Movimentações internas;
 * SD4 - Requisições empenhadas;
 * SF5 - Tipos de movimentação;
 * SI1 - Plano de contas;
 * SI2 - Lançamentos Contábeis;
 * SI5 - Lançamentos Padronizados;
 * SI6 - Totais de lote;
 * SI7 - Plano de contas em outras moedas;
 * SHD - Operações alocadas e sacramentadas.
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BinzelApp3_Prototipo
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/binzel_icon", Theme = "@style/Binzel", NoHistory = true)]
    public class MainActivity : Activity
    {
        ////variaveis de objetos da tela
        private EditText nameLogin;
        private EditText password;
        private Button btnLoginOk;
        private DB_PCP con = new DB_PCP();
        //private WebClient wClient;
        private Usuario usr;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            //Variavel criada recebe a toolbar criada, e é setada pelo SetActionBar
            //após isso, pode ser referenciada chamando "ActionBar"
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = Resources.GetString(Resource.String.app_name); //"Binzel App (Prototype)"
            //ActionBar.SetLogo(Resource.Drawable.binzel_app_icon_light);

            //instanciando objetos da tela
            nameLogin = FindViewById<EditText>(Resource.Id.txtName);
            password = FindViewById<EditText>(Resource.Id.txtPwd);
            btnLoginOk = FindViewById<Button>(Resource.Id.btnLoginOk);
            btnLoginOk.Click += async delegate
            {
                Toast mensagem;
                usr = new Usuario();
                try
                {
                    // Faz a verificação de login e senha de usuário e encaminha para menu inicial
                    using (var client = new HttpClient())
                    {
                        string url = "http://10.0.0.108:8624/rest/loginuser?";
                        url += "usr=" + this.nameLogin.Text.Trim();
                        url += "&pwd=" + this.password.Text;
                        
                        // GET request da URL                         
                        var result = await client.GetStringAsync(url);
                        //resposta  
                        JObject jObj = JObject.Parse(result);
                        usr = JsonConvert.DeserializeObject<Usuario>( jObj.SelectToken("Usuario").ToString() );

                        //opção de array
                        //JArray jArr = JArray.Parse(jObj.SelectToken("Usuario").ToString());//jObj.SelectToken("Usuario").ToString()
                        //var root = JsonConvert.DeserializeObject<RootUsuario>(result);
                        //usr = root.Usuario as Usuario;
                    }

                    if (usr != null)
                    {
                        mensagem = Toast.MakeText(this, "Bem-Vindo " + usr.Nome, ToastLength.Long);

                        Bundle bld = new Bundle();
                        bld.PutString("usrId", usr.IdUsuario);
                        bld.PutString("usrNome", usr.Nome);
                        bld.PutString("usrCargo", usr.Cargo);
                        bld.PutString("usrSetor", usr.Setor);
                        //bld.PutInt("usrNivel", usr.NivelAcesso);

                        switch (usr.Setor)
                        {
                            case "Produção":
                                Intent intentProd = new Intent(Application.Context, typeof(MenuProducao));
                                intentProd.PutExtras(bld);
                                StartActivity(intentProd);
                                break;
                            //case 2: //pcp
                            case "Gerência":
                                Intent intentGer = new Intent(Application.Context, typeof(MenuGerencial));
                                intentGer.PutExtras(bld);
                                StartActivity(intentGer);
                                break;
                        }
                    }
                    else
                    {
                        mensagem = Toast.MakeText(this, "USUARIO/SENHA INCORRETOS", ToastLength.Long);
                        this.nameLogin.Text = "";
                        this.password.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERRO WebClient: " + ex.Message);
                    mensagem = Toast.MakeText(this, "ERRO de Conexão: " + ex.Message, ToastLength.Long);
                }

                mensagem.Show();
            };

        }


        //private async Task<Usuario> GetUsuarioAsync(string url)
        //{
        //    // Create an HTTP web request using the URL:
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
        //    request.ContentType = "application/json";
        //    request.Method = "GET";

        //    // Send the request to the server and wait for the response:
        //    using (WebResponse response = await request.GetResponseAsync())
        //    {
        //        // Get a stream representation of the HTTP web response:
        //        using (Stream stream = response.GetResponseStream())
        //        {
        //            // Use this stream to build a JSON document object:
        //            Usuario jsonDoc = await Task.Run(() => JsonObject.Load(stream));
        //            Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

        //            // Return the JSON document:
        //            return jsonDoc;
        //        }
        //    }
        //}

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void LoginSystem(object sender, System.EventArgs e)
        //{
        //    //var usr = con.GetColaborador(this.nameLogin.Text, this.password.Text);                 
        //    Toast mensagem;
        //    string url = "http://10.0.0.108:8624/rest/loginuser?";
        //    url += "usr=" + this.nameLogin.Text.Trim();
        //    url += "&pwd=" + this.password.Text;

        //    var uri = new Uri(url);
        //    var wClient = new WebClient();
        //    wClient.DownloadDataAsync(uri);
        //    wClient.DownloadDataCompleted += WClient_DownloadDataCompleted;
            

        //    if (usr != null)
        //    {
        //        mensagem = Toast.MakeText(this, "Bem-Vindo " + usr.Nome, ToastLength.Long);

        //        Bundle bld = new Bundle();
        //        bld.PutString("usrId", usr.IdUsuario);
        //        bld.PutString("usrNome", usr.Nome);                
        //        bld.PutString("usrCargo", usr.Cargo);
        //        bld.PutString("usrSetor", usr.Setor);                
        //        //bld.PutInt("usrNivel", usr.NivelAcesso);

        //        switch (usr.Setor)
        //        {
        //            case "Produção":
        //                Intent intentProd = new Intent(Application.Context, typeof(MenuProducao));
        //                intentProd.PutExtras(bld);
        //                StartActivity(intentProd);
        //                break;
        //            //case 2: //pcp
        //            case "Gerência":
        //                Intent intentGer = new Intent(Application.Context, typeof(MenuGerencial));
        //                intentGer.PutExtras(bld);
        //                StartActivity(intentGer);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        mensagem = Toast.MakeText(this, "USUARIO/SENHA INCORRETOS", ToastLength.Long);
        //        this.nameLogin.Text = "";
        //        this.password.Text = "";
        //    }

        //    mensagem.Show();
        //}

        //private void WClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        //{            
        //    try
        //    {
        //        usr = new Usuario();
        //        //var jsonArray = e.Result.Clone();
        //        var strJson = Encoding.UTF8.GetString(e.Result);
        //        if (!string.IsNullOrEmpty(strJson))
        //            usr = JsonConvert.DeserializeObject<Usuario>(strJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("ERRO WebClient: " + ex.Message);
        //        usr = null;
        //    }
            
        //}
    }
}
