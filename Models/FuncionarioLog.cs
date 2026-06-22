using Azure;
using Azure.Data.Tables;

namespace trilha_net_azure_desafio.Models
{
    public class FuncionarioLog : Funcionario, ITableEntity
    {
        public FuncionarioLog() { }

        public FuncionarioLog(Funcionario funcionario, TipoAcao acao)
        {
            Id = funcionario.Id;
            Nome = funcionario.Nome;
            Endereco = funcionario.Endereco;
            Ramal = funcionario.Ramal;
            EmailProfissional = funcionario.EmailProfissional;
            Departamento = funcionario.Departamento;
            Salario = funcionario.Salario;
            DataAdmissao = funcionario.DataAdmissao;

            TipoAcao = acao.ToString();
            Acao = $"{acao} {DateTime.Now}";
            DataAlteracao = DateTime.Now;

            PartitionKey = "Funcionario";
            RowKey = Guid.NewGuid().ToString();
        }

        public string TipoAcao { get; set; }
        public string Acao { get; set; }
        public DateTime DataAlteracao { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

    public enum TipoAcao
    {
        Inclusao,
        Atualizacao,
        Remocao
    }
}
