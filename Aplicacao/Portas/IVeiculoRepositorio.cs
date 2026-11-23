using Locadora.Dominio;

namespace Locadora.Aplicacao.Portas;

public interface IVeiculoRepositorio
{
    void Adicionar(Veiculo veiculo);
    Veiculo? BuscarPorId(int id);
    Veiculo? BuscarPorPlaca(string placa);
    IEnumerable<Veiculo> Listar();
    void Atualizar(Veiculo veiculo);
    bool Remover(int id);
}