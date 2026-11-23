using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Infraestrutura.Persistencia;

public class VeiculoRepositorioMemoria : IVeiculoRepositorio
{
    private readonly List<Veiculo> _veiculos = new();
    private readonly object _trava = new();

    public void Adicionar(Veiculo veiculo)
    {
        lock (_trava)
        {
            veiculo.Id = _veiculos.Count > 0 ? _veiculos.Max(v => v.Id) + 1 : 1;
            _veiculos.Add(veiculo);
        }
    }

    public Veiculo? BuscarPorId(int id)
    {
        lock (_trava)
        {
            return _veiculos.FirstOrDefault(v => v.Id == id);
        }
    }

    public Veiculo? BuscarPorPlaca(string placa)
    {
        lock (_trava)
        {
            return _veiculos.FirstOrDefault(v => v.Placa == placa);
        }
    }

    public IEnumerable<Veiculo> Listar()
    {
        lock (_trava)
        {
            return _veiculos.ToList();
        }
    }

    public void Atualizar(Veiculo veiculo)
    {
        // Em memória o objeto já foi alterado por referência.
    }

    public bool Remover(int id)
    {
        lock (_trava)
        {
            return _veiculos.RemoveAll(v => v.Id == id) > 0;
        }
    }
}