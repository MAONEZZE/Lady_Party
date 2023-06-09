﻿using LadyParty.Infra.ModuloCliente;

namespace LadyParty.WinForms.ModuloCliente
{
    public partial class TelaClienteForm : Form
    {
        private Cliente cliente;

        public RepositorioArquivoCliente repCliente;
        public TelaClienteForm(RepositorioArquivoCliente repCliente)
        {
            InitializeComponent();

            this.ConfigurarTelas();
            txt_id.ReadOnly = true;
            this.repCliente = repCliente;
        }
        public Cliente Cliente
        {
            set
            {
                txt_nomeCliente.Text = value.nomeCliente;
                txt_telefone.Text = value.telefoneCliente;
                txt_id.Text = value.id.ToString();
            }
            get
            {
                return cliente;
            }
        }

        public void DefinirID(int id = 0)
        {
            txt_id.Text = id.ToString();
        }
        private void btn_gravar_Click(object sender, EventArgs e)
        {
            TelaPrincipalForm tlPrinc = TelaPrincipalForm.TelaPrincipal;

            string nome = txt_nomeCliente.Text;

            string telefone = txt_telefone.Text;

            int id = Convert.ToInt32(txt_id.Text);

            cliente = new Cliente(nome, telefone);

            cliente.id = id;

            if (repCliente.EhRepetido(cliente))
            {
                TelaPrincipalForm.TelaPrincipal.AtualizarRodape("Nome de cliente já existente, por facor insira um novo nome!");

                DialogResult = DialogResult.None;
            }
            else
            {
                string[] erros = cliente.Validar();

                if (erros.Length > 0)
                {
                    tlPrinc.AtualizarRodape(erros[0]);
                    DialogResult = DialogResult.None;
                }
                else
                {
                    tlPrinc.AtualizarRodape("");

                }
            }

        }
    }
}
