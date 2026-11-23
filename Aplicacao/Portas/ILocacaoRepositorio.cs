using Locadora.Dominio;

namespace Locadora.Aplicacao.Portas;

public interface ILocacaoRepositorio
{
    void Adicionar(Locacao locacao);
    Locacao? BuscarPorId(int id);
    IEnumerable<Locacao> Listar();
    IEnumerable<Locacao> ListarPorCliente(int clienteId);
    IEnumerable<Locacao> ListarPorVeiculo(int veiculoId);
    IEnumerable<Locacao> ListarPorFuncionario(string matricula);
    void Atualizar(Locacao locacao);
}