using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedStock
{
    public enum EnumCategoria
    {
        Referência,
        Similar,
        Genérico
    }

    public class Medicamento
    {
        

        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataValidade { get; set; }
        public EnumCategoria Categoria { get; set; }

        public Medicamento(string descricao, int quantidade,
                            DateTime dataValidade,
                            EnumCategoria categoria)
        {
            this.Codigo = 0;
            this.Descricao = descricao;
            this.Quantidade = quantidade;
            this.DataValidade = dataValidade;
            this.Categoria = categoria;
        }

        public Medicamento()
        {
            this.Codigo = 0;
        }

        public static List<Medicamento> Listagem { get; set; }
        static Medicamento()
        {
            Medicamento.Listagem = new List<Medicamento>();
        }

        public static Medicamento Inserir(Medicamento medicamento)
        {
            int codigo = Medicamento.Listagem.Count > 0 ? Medicamento.Listagem.Max(m => m.Codigo) + 1 : 1;

            medicamento.Codigo = codigo;
            Medicamento.Listagem.Add(medicamento);
            return medicamento;
        }
    }
}
