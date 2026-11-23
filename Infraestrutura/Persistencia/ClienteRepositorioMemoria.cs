using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Infraestrutura.Persistencia;

// ADAPTADOR: implementa a porta IClienteRepositorio guardando tudo numa lista.
public class ClienteRepositorioMemoria : IClienteRepositorio
{
    private readonly List<Cliente> _clientes = new();
    private readonly object _trava = new();

    public void Adicionar(Cliente cliente)
    {
        lock (_trava)
        {
            // Gera o próximo Id: maior Id existente + 1 (ou 1, se a lista estiver vazia).
            cliente.Id = _clientes.Count > 0 ? _clientes.Max(c => c.Id) + 1 : 1;
            _clientes.Add(cliente);
        }
    }

    public Cliente? BuscarPorId(int id)
    {
        lock (_trava)
        {
            return _clientes.FirstOrDefault(c => c.Id == id);
        }
    }

    public Cliente? BuscarPorCpf(string cpf)
    {
        lock (_trava)
        {
            return _clientes.FirstOrDefault(c => c.Cpf == cpf);
        }
    }

    public IEnumerable<Cliente> Listar()
    {
        lock (_trava)
        {
            return _clientes.ToList();
        }
    }

    public void Atualizar(Cliente cliente)
    {
        // Em memória não há nada a fazer: o objeto já foi alterado por referência.
        // Com banco de dados, aqui ficaria o UPDATE.
    }

    public bool Remover(int id)
    {
        lock (_trava)
        {
            return _clientes.RemoveAll(c => c.Id == id) > 0;
        }
    }
}