﻿namespace LadyParty.WinForms.ModuloCliente
{
    public partial class ClienteUserControl : UserControl
    {
        public ClienteUserControl()
        {
            InitializeComponent();

            ConfigurarColunas();
            grid.ConfigurarGrid();
            grid.ConfigurarGridZebrado();
        }

        private void ConfigurarColunas()
        {
            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.Name = "ID";
            id.HeaderText = "ID";

            DataGridViewTextBoxColumn nomeCliente = new DataGridViewTextBoxColumn();
            nomeCliente.Name = "nome";
            nomeCliente.HeaderText = "Nome do Cliente";

            DataGridViewTextBoxColumn telefoneCliente = new DataGridViewTextBoxColumn();
            telefoneCliente.Name = "listaClientes";
            telefoneCliente.HeaderText = "Telefone";

            grid.Columns.AddRange(id);
            grid.Columns.AddRange(nomeCliente);
            grid.Columns.AddRange(telefoneCliente);
        }

        public void AtualizarRegistros(List<Cliente> listaClientes)
        {
            grid.Rows.Clear();

            foreach (Cliente cliente in listaClientes)
            {
                grid.Rows.Add(cliente.id, cliente.nomeCliente, cliente.telefoneCliente);
            }
        }

        public int ObterIdSelecionado()
        {
            if (grid.SelectedRows.Count == 0)
                return -1;

            int id = Convert.ToInt32(grid.SelectedRows[0].Cells["ID"].Value);

            return id;
        }
    }
}
