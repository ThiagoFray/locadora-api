using Locadora.Aplicacao.Excecoes;
using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Aplicacao.Clientes;

public class ClienteServico
{
    private readonly IClienteRepositorio _clientes;
    private readonly ILocacaoRepositorio _locacoes;

    public ClienteServico(IClienteRepositorio clientes, ILocacaoRepositorio locacoes)
    {
        _clientes = clientes;
        _locacoes = locacoes;
    }

    public IEnumerable<Cliente> Listar()
    {
        return _clientes.Listar();
    }

    public Cliente BuscarPorId(int id)
    {
        return _clientes.BuscarPorId(id)
            ?? throw new NaoEncontradoException($"Cliente {id} não encontrado.");
    }

    public Cliente BuscarPorCpf(string cpf)
    {
        return _clientes.BuscarPorCpf(cpf.Trim())
            ?? throw new NaoEncontradoException($"Cliente com CPF {cpf} não encontrado.");
    }

    public Cliente Cadastrar(string cpf, string nome, string cnh, DateTime vencimentoCnh, string telefone)
    {
        // O construtor valida os dados (lança RegraDeNegocioException se algo estiver errado).
        var cliente = new Cliente(cpf, nome, cnh, vencimentoCnh, telefone);

        if (_clientes.BuscarPorCpf(cliente.Cpf) is not null)
            throw new ConflitoException($"Já existe um cliente com o CPF {cliente.Cpf}.");

        _clientes.Adicionar(cliente);
        return cliente;
    }

    public Cliente Atualizar(int id, string nome, string cnh, DateTime vencimentoCnh, string telefone)
    {
        var cliente = BuscarPorId(id);

        cliente.AlterarDados(nome, cnh, vencimentoCnh);
        cliente.AtualizarContato(telefone);

        _clientes.Atualizar(cliente);
        return cliente;
    }

    public void Remover(int id)
    {
        BuscarPorId(id);   // garante que existe (senão lança NaoEncontradoException)

        // Não excluir cliente que possua locações (preserva o histórico).
        if (_locacoes.ListarPorCliente(id).Any())
            throw new ConflitoException("Não é possível excluir um cliente que possui locações registradas.");

        _clientes.Remover(id);
    }
}