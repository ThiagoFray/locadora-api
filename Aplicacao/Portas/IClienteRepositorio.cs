using Locadora.Dominio;

namespace Locadora.Aplicacao.Portas;

public interface IClienteRepositorio
{
    void Adicionar(Cliente cliente);
    Cliente? BuscarPorId(int id);
    Cliente? BuscarPorCpf(string cpf);
    IEnumerable<Cliente> Listar();
    void Atualizar(Cliente cliente);
    bool Remover(int id);
}