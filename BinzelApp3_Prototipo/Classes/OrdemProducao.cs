using System;
using System.Collections.Generic;
using SQLite;

namespace BinzelApp3_Prototipo
{
    //[Table("OrdemProducao")]
    public class OrdemProducao
    {
        public string ARMAZEM { get; set; }
        public string CODPRODUTO { get; set; }
        public string EMISSAO { get; set; }
        public string ITEM { get; set; }
        public string NUMOP { get; set; }
        public string OBSERVACAO { get; set; }
        public string PREVISAOENTREGA { get; set; }
        public string PREVISAOINICIO { get; set; }
        public float QUANTIDADE { get; set; }
        public string SEQUENCIA { get; set; }
        public string STATUS { get; set; }
        public string TIPOOP { get; set; }
        public string TIPOPRODUCAO { get; set; }
        public string UNIDADE { get; set; }

        /**CONSTRUTORES*/
        public OrdemProducao() { }

        //[PrimaryKey]
        //public int NumOP { get; set; }
        //public string CodProduto { get; set; }
        //public int QtdTotal { get; set; }
        //public string Cliente { get; set; }
        //public string DescricaoOP { get; set; }
        //public DateTime DtHrCriacao { get; set; }
        //public DateTime DtHrFinalOP { get; set; }
        //public string DesenhoTecnico { get; set; }
        ///// <summary> 0=Não_iniciado, 1=Iniciado, 2=Completo, 3=Cancelado </summary>
        //public int StatusOP { get; set; }

        //public OrdemProducao(int num, string codProduto, int qtd, string cliente, string desc)
        //{
        //    this.NumOP = num;
        //    this.CodProduto = codProduto;
        //    this.QtdTotal = qtd;
        //    this.Cliente = cliente;
        //    this.DescricaoOP = desc;
        //    this.DtHrCriacao = DateTime.Now;
        //    this.StatusOP = 0; //não_iniciado
        //}

    }


    public class RootOP
    {
        public int Status { get; set; }
        public string Descricao { get; set; }
        public int Registros { get; set; }
        public List<OrdemProducao> OrdemProducao { get; set; }
    }
}

/**
 
public class Rootobject
{
public int Status { get; set; }
public string Descricao { get; set; }
public int Registros { get; set; }
public Ordemproducao[] OrdemProducao { get; set; }
}

public class Ordemproducao
{
public string ARMAZEM { get; set; }
public string CODPRODUTO { get; set; }
public string EMISSAO { get; set; }
public string ITEM { get; set; }
public string NUMOP { get; set; }
public string OBSERVACAO { get; set; }
public string PREVISAOENTREGA { get; set; }
public string PREVISAOINICIO { get; set; }
public float QUANTIDADE { get; set; }
public string SEQUENCIA { get; set; }
public string STATUS { get; set; }
public string TIPOOP { get; set; }
public string TIPOPRODUCAO { get; set; }
public string UNIDADE { get; set; }
}

     */
