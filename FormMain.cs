using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedStock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CriarCategoria();
            DesabilitarCampos();
        }

        //métodos de perguntas para interação com usuário
        private void Informar(string mensagem)
        {
            MessageBox.Show(mensagem, "Informação", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        
        private bool Confirmar(string pergunta)
        {
            return MessageBox.Show(pergunta, "Confirmação", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }

        //atualização do cbx de Medicamentos e aparência
        private void AtualizarcbxMedicamentos()
        {
            cbxMedicamento.DataSource = Medicamento.Listagem;
            cbxMedicamento.DisplayMember = "";
            cbxMedicamento.DisplayMember = "Descricao";
            cbxMedicamento.ValueMember = "Codigo";
        }

        //correção para quando apertar tab passar pelas opções de categoria
        private void CorrigirTabStop(object sender, EventArgs e)
        {
            ((RadioButton)sender).TabStop = true;
        }

        //criar categoria dentro do gbxCategoria
        private void CriarCategoria()
        {
            int i = 0;
            var TipoCategoria = Enum.GetValues(typeof(EnumCategoria));
            foreach (var tc in TipoCategoria)
            {
                RadioButton rb = new RadioButton()
                {
                    Text = tc.ToString(),
                    Location = new Point(10, (i + 1) * 24),
                    Width = 85,
                    TabStop = true,
                    TabIndex = i,
                    Tag = tc
                };
                rb.TabStopChanged += new EventHandler(CorrigirTabStop);
                gbxCategoria.Controls.Add(rb);
                i++;


            }

        }
        //LerCategoria e guardar info
        private EnumCategoria? LerCategoria()
        {
            foreach (var control in gbxCategoria.Controls)
            {
                RadioButton rb = control as RadioButton;
                if (rb.Checked)
                {
                    return (EnumCategoria)(rb.Tag);
                }
            }
            return null;
        }

        //marcar cbx a partir de uma categoria passada

        private void MarcarCategoria(EnumCategoria categoria)
        {
            foreach (var control in gbxCategoria.Controls)
            {
                RadioButton rb = control as RadioButton;
                if ((EnumCategoria)(rb.Tag) == categoria)
                {
                    rb.Checked = true;
                    return;
                }
            }
        }

        //método para limpar os campos do formulário
        private void LimparCampos()
        {
            txtCodigo.Clear();
            txtDescricao.Clear();
            txtQuantidade.Clear();
            dtpDataValidade.Value = DateTime.Now.Date;
            foreach (var control in gbxCategoria.Controls)
            {
                (control as RadioButton).Checked = false;
            }
        }

        //método para quando selecionar o medicamento pesquise cada campo do mesmo
        private void PesquisaMedicamento(Medicamento medicamento)
        {
            txtCodigo.Text = medicamento.Codigo.ToString();
            txtDescricao.Text = medicamento.Descricao;
            txtQuantidade.Text = medicamento.Quantidade.ToString();
            dtpDataValidade.Value = medicamento.DataValidade;
            MarcarCategoria(medicamento.Categoria);
        }

        //Salvar os dados no Objeto Medicamento
        private void SalvarMedicamento(Medicamento medicamento)
        {
            medicamento.Descricao = txtDescricao.Text;
            medicamento.Quantidade = Convert.ToInt32(txtQuantidade.Text);
            medicamento.DataValidade = dtpDataValidade.Value.Date;
            medicamento.Categoria = LerCategoria().Value;
        }

        //Checar se o usuario preencheu tds os campos do registro

        private bool ChecarPreenchimento()
        {
            if (String.IsNullOrWhiteSpace(txtDescricao.Text)) return false;
            if (String.IsNullOrWhiteSpace(txtQuantidade.Text)) return false;
            if (dtpDataValidade.Value.Date == DateTime.Now.Date) return false;
            if (LerCategoria() == null) return false;

            return true;
        }

        //Método para verificação de valores não salvos

        private bool VerificaValoresNaoSalvos()
        {
            if (cbxMedicamento.SelectedIndex < 0)
            {
                if (!String.IsNullOrWhiteSpace(txtDescricao.Text)) return true;
                if (!String.IsNullOrWhiteSpace(txtQuantidade.Text)) return true;
                if (dtpDataValidade.Value.Date != DateTime.Now.Date) return true;
                if (LerCategoria() != null) return true;
            }
            else
            {
                Medicamento medicamento = cbxMedicamento.SelectedItem as Medicamento;
                if (txtDescricao.Text.Trim() != medicamento.Descricao) return true;
                if (txtQuantidade.Text != medicamento.Quantidade.ToString()) return true;
                if (dtpDataValidade.Value.Date != medicamento.DataValidade) return true;
                if (LerCategoria() != medicamento.Categoria) return true;
            }
            return false;
        }

        //alteração do status dos campos (editável ou não)

        private void AlterarEstadoDeEdicao(bool estado)
        {
            txtDescricao.Enabled = estado;
            txtQuantidade.Enabled = estado;
            dtpDataValidade.Enabled = estado;
            gbxCategoria.Enabled = estado;
            btnRegistrar.Enabled = estado;
            btnCancelar.Enabled = estado;
        }

        private void HabilitarCampos()
        {
            AlterarEstadoDeEdicao(true);
        }
        private void DesabilitarCampos()
        {
            AlterarEstadoDeEdicao(false);
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            cbxMedicamento.SelectedIndex = -1; //tira seleção de algum medicamento que esteja selecionado
            LimparCampos();
            HabilitarCampos();
            txtDescricao.Select(); // foca no txt Descrição
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (VerificaValoresNaoSalvos())
            {
                if (Confirmar("Há alterações não salvas. Deseja realmente cancelar?"))
                {
                    if (cbxMedicamento.SelectedIndex == -1)
                        LimparCampos();
                    else
                        PesquisaMedicamento(cbxMedicamento.SelectedItem as Medicamento);
                    cbxMedicamento.Select();
                    DesabilitarCampos();
                }
            }
            else
            {
                if (cbxMedicamento.SelectedIndex == -1)
                    LimparCampos();
                else
                    PesquisaMedicamento(cbxMedicamento.SelectedItem as Medicamento);
                cbxMedicamento.Select();
                DesabilitarCampos();
            }


        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (VerificaValoresNaoSalvos())
                e.Cancel = !Confirmar("Há alterações não salvas. Deseja realmente Sair?");
            else
                e.Cancel = !Confirmar("Deseja realmente sair?");
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            HabilitarCampos();
            PesquisaMedicamento(cbxMedicamento.SelectedItem as Medicamento);
            txtDescricao.Select();
        }

        private void cbxMedicamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMedicamento.SelectedIndex < 0)
            {
                btnConsultar.Enabled = false;
            }
            else
            {
                PesquisaMedicamento(cbxMedicamento.SelectedItem as Medicamento);
                btnConsultar.Enabled = true;
            }
        }

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {
            if (ChecarPreenchimento())
            {
                Medicamento medicamento = cbxMedicamento.SelectedIndex < 0 ?
                    new Medicamento() : cbxMedicamento.SelectedItem as Medicamento;

                SalvarMedicamento(medicamento);
                DesabilitarCampos();

                if (cbxMedicamento.SelectedIndex < 0)
                {
                    medicamento = Medicamento.Inserir(medicamento);
                    AtualizarcbxMedicamentos();
                    Informar("Medicamento Registrado com Sucesso!");
                }
                else
                {
                    AtualizarcbxMedicamentos();
                    Informar("Medicamento Alterado com Sucesso");
                }
            }
            else
            {
                Informar("Só é possível salvar se todos os campos estiverem preenchidos. Registro não realizado.");
            }

        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
