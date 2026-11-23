using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Infraestrutura.Persistencia;

public class LocacaoRepositorioMemoria : ILocacaoRepositorio
{
    private readonly List<Locacao> _locacoes = new();
    private readonly object _trava = new();

    public void Adicionar(Locacao locacao)
    {
        lock (_trava)
        {
            locacao.IdLocacao = _locacoes.Count > 0 ? _locacoes.Max(l => l.IdLocacao) + 1 : 1;
            _locacoes.Add(locacao);
        }
    }

    public Locacao? BuscarPorId(int id)
    {
        lock (_trava)
        {
            return _locacoes.FirstOrDefault(l => l.IdLocacao == id);
        }
    }

    public IEnumerable<Locacao> Listar()
    {
        lock (_trava)
        {
            return _locacoes.ToList();
        }
    }

    public IEnumerable<Locacao> ListarPorCliente(int clienteId)
    {
        lock (_trava)
        {
            return _locacoes.Where(l => l.Cliente.Id == clienteId).ToList();
        }
    }

    public IEnumerable<Locacao> ListarPorVeiculo(int veiculoId)
    {
        lock (_trava)
        {
            return _locacoes.Where(l => l.Veiculo.Id == veiculoId).ToList();
        }
    }

    public IEnumerable<Locacao> ListarPorFuncionario(string matricula)
    {
        lock (_trava)
        {
            return _locacoes.Where(l => l.Funcionario.Matricula == matricula).ToList();
        }
    }

    public void Atualizar(Locacao locacao)
    {
        // Em memória o objeto já foi alterado por referência.
    }
}