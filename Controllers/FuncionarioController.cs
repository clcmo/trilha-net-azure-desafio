using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using trilha_net_azure_desafio.Context;
using trilha_net_azure_desafio.Models;

namespace trilha_net_azure_desafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly RHContext _context;
        private readonly string connectionString;
        private readonly string tableName = "FuncionarioLog";

        public FuncionarioController(RHContext context, IConfiguration configuration)
        {
            _context = context;
            connectionString = configuration.GetValue<string>("ConnectionStrings:SAConnectionString");
        }

        [HttpGet("{id}")]
        public IActionResult GetFuncionario(int id)
        {
            var funcionario = _context.Funcionarios.Find(id);

            if (funcionario == null)
                return NotFound();

            return Ok(funcionario);
        }

        [HttpPost]
        public IActionResult CreateFuncionario(Funcionario funcionario)
        {
            _context.Funcionarios.Add(funcionario);
            _context.SaveChanges();

            SalvaLogTabela(new FuncionarioLog(funcionario, TipoAcao.Inclusao));

            return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, funcionario);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFuncionario(int id, Funcionario funcionario)
        {
            var funcionarioBanco = _context.Funcionarios.Find(id);

            if (funcionarioBanco == null)
                return NotFound();

            funcionarioBanco.Nome = funcionario.Nome;
            funcionarioBanco.Endereco = funcionario.Endereco;
            funcionarioBanco.Ramal = funcionario.Ramal;
            funcionarioBanco.EmailProfissional = funcionario.EmailProfissional;
            funcionarioBanco.Departamento = funcionario.Departamento;
            funcionarioBanco.Salario = funcionario.Salario;
            funcionarioBanco.DataAdmissao = funcionario.DataAdmissao;

            _context.Funcionarios.Update(funcionarioBanco);
            _context.SaveChanges();

            SalvaLogTabela(new FuncionarioLog(funcionarioBanco, TipoAcao.Atualizacao));

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFuncionario(int id)
        {
            var funcionarioBanco = _context.Funcionarios.Find(id);

            if (funcionarioBanco == null)
                return NotFound();

            _context.Funcionarios.Remove(funcionarioBanco);
            _context.SaveChanges();

            SalvaLogTabela(new FuncionarioLog(funcionarioBanco, TipoAcao.Remocao));

            return NoContent();
        }

        private void SalvaLogTabela(FuncionarioLog log)
        {
            TableClient tableClient = new TableClient(connectionString, tableName);
            tableClient.CreateIfNotExists();
            tableClient.AddEntity(log);
        }
    }
}
