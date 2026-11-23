using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Infraestrutura.Persistencia;

public class FuncionarioRepositorioMemoria : IFuncionarioRepositorio
{
    private readonly List<Funcionario> _funcionarios = new();
    private readonly object _trava = new();

    // Funcionário não tem Id: a chave é a matrícula, que já vem preenchida.
    public void Adicionar(Funcionario funcionario)
    {
        lock (_trava)
        {
            _funcionarios.Add(funcionario);
        }
    }

    public Funcionario? BuscarPorMatricula(string matricula)
    {
        lock (_trava)
        {
            return _funcionarios.FirstOrDefault(f => f.Matricula == matricula);
        }
    }

    public IEnumerable<Funcionario> Listar()
    {
        lock (_trava)
        {
            return _funcionarios.ToList();
        }
    }

    public void Atualizar(Funcionario funcionario)
    {
        // Em memória o objeto já foi alterado por referência.
    }

    public bool Remover(string matricula)
    {
        lock (_trava)
        {
            return _funcionarios.RemoveAll(f => f.Matricula == matricula) > 0;
        }
    }
}