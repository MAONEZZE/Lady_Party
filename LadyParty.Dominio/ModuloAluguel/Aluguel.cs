﻿using Microsoft.Data.SqlClient;

namespace LadyParty.Dominio.ModuloAluguel
{
    public class Aluguel : EntidadeBase<Aluguel>
    {

        public int idCliente;

        public int idTema;

        public StatusAluguelEnum statusAluguel;

        public Festa festa;

        public decimal valorAluguel;

        public decimal valorDaEntrada;

        public decimal totalJaPago;

        public decimal valorDevidoPago;

        public DateTime dataQuitacao;



        public Aluguel(int id, int idCliente, int idTema, Festa festa, decimal valorAluguel, decimal valorDaEntrada, decimal valorRestantePago)
        {
            this.id = id;
            this.idCliente = idCliente;
            this.idTema = idTema;
            this.festa = festa;

            this.valorDaEntrada = valorDaEntrada;

            this.valorAluguel = valorAluguel;

            this.valorDevidoPago = valorRestantePago;

            this.dataQuitacao = default(DateTime);

            ObterValorDevido();
        }

        public Aluguel()
        {
            this.id = -1;

            this.idCliente = -1;

            this.idTema = -1;

            this.festa = new Festa();

            this.festa.data = default(DateTime);

            this.festa.horaInicio = default(TimeSpan);

            this.festa.horaTermino = default(TimeSpan);

            this.valorDaEntrada = 0;

            this.valorAluguel = 0;

            this.valorDevidoPago = 0;

            this.dataQuitacao = default(DateTime);

            ObterValorDevido();
        }

        public decimal ObterTotalPago()
        {
            if (valorDaEntrada + valorDevidoPago == valorAluguel)
            {
                return valorAluguel;
            }
            return valorDaEntrada + valorDevidoPago;
        }

        public decimal ObterValorDevido()
        {
            decimal valorDevido = valorAluguel - (valorDaEntrada + valorDevidoPago);
            if (valorDevido == 0)
            {
                dataQuitacao = DateTime.Now;
            }
            else
            {
                dataQuitacao = default(DateTime);
            }
            return valorDevido;
        }

        public StatusAluguelEnum StatusAluguel()
        {
            if (ObterValorDevido() == 0 && DateTime.Now.Date >= festa.data.Date && DateTime.Now.TimeOfDay > festa.horaTermino)
            {
                return StatusAluguelEnum.Concluido;
            }
            else if (ObterValorDevido() > 0 && DateTime.Now.Date >= festa.data.Date && DateTime.Now.TimeOfDay > festa.horaTermino)
            {
                return StatusAluguelEnum.PagamentoPendente;
            }

            return StatusAluguelEnum.Ativo;
        }
        public override void AtualizarInformacoes(Aluguel entidade)
        {
            this.idTema = entidade.idTema;

            this.festa = entidade.festa;

            this.valorAluguel = entidade.valorAluguel;

            this.valorDaEntrada = entidade.valorDaEntrada;

            this.valorDevidoPago = entidade.valorDevidoPago;

            ObterValorDevido();
        }

        public override string[] Validar()
        {
            List<string> erros = new List<string>();
            if (idCliente < 0)
            {
                erros.Add("Cliente não selecionado");
            }
            if (idTema < 0)
            {
                erros.Add("Tema não selecionado");
            }
            if (valorAluguel < 1)
            {
                erros.Add("Valor com desconto não informado");
            }
            if (valorDaEntrada < 1)
            {
                erros.Add("Valor da entrada não informado");
            }
            if (String.IsNullOrEmpty(festa.endereco))
            {
                erros.Add("Endereço não informado");
            }
            if (festa.data == default(DateTime))
            {
                erros.Add("Data não selecionada");
            }
            if (festa.horaInicio == default(TimeSpan))
            {
                erros.Add("Hora de início não selecionada");
            }
            if (festa.horaTermino == default(TimeSpan))
            {
                erros.Add("Hora de término não selecionada");
            }
            return erros.ToArray();
        }

        public override bool VerificarRepeticao(Aluguel obj)
        {
            return this.id != obj.id && this.idCliente == obj.idCliente && this.idTema == obj.idTema; // && festa.Equals(aluguel.festa);
        }

        public override string ObterCampoSQL(bool ehParametro = false)
        {
            String sufixo = "[";
            String prefixo = "]";
            String campo = "";

            if (ehParametro)
            {
                sufixo = "@";
                prefixo = "";
            }

            campo = sufixo + "idCliente" + prefixo + ", ";
            campo += sufixo + "idTema" + prefixo + ", ";
            campo += sufixo + "idFesta" + prefixo + ", ";
            campo += sufixo + "statusAluguel" + prefixo + ", ";
            campo += sufixo + "valorAluguel" + prefixo + ", ";
            campo += sufixo + "valorDaEntrada" + prefixo + ", ";
            campo += sufixo + "totalJaPago" + prefixo + ", ";
            campo += sufixo + "valorDevidoPago" + prefixo + ", ";
            campo += sufixo + "dataQuitacao" + prefixo + ", ";

            return campo;

        }

        public override SqlParameter[] ObterParametroSQL()
        {
            return new SqlParameter[]
            {
                new SqlParameter("@idCliente", idCliente),
                new SqlParameter("@idTema", idTema),
                //new SqlParameter("@idFesta", festa.id),
                new SqlParameter("@statusAluguel", statusAluguel),
                new SqlParameter("@valorAluguel", valorAluguel),
                new SqlParameter("@valorDaEntrada", valorDaEntrada),
                new SqlParameter("@totalJaPago", ObterTotalPago()),
                new SqlParameter("@valorDevidoPago", valorDevidoPago),
                new SqlParameter("@dataQuitacao", dataQuitacao)
            };
        }
    }
}
