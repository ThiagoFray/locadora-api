using Locadora.Aplicacao.Excecoes;
using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Aplicacao.Veiculos;

public class VeiculoServico
{
    private readonly IVeiculoRepositorio _veiculos;
    private readonly ILocacaoRepositorio _locacoes;

    public VeiculoServico(IVeiculoRepositorio veiculos, ILocacaoRepositorio locacoes)
    {
        _veiculos = veiculos;
        _locacoes = locacoes;
    }

    public IEnumerable<Veiculo> Listar()
    {
        return _veiculos.Listar();
    }

    public Veiculo BuscarPorId(int id)
    {
        return _veiculos.BuscarPorId(id)
            ?? throw new NaoEncontradoException($"Veículo {id} não encontrado.");
    }

    public Veiculo BuscarPorPlaca(string placa)
    {
        // A placa é guardada em maiúsculas, então a busca também usa maiúsculas.
        return _veiculos.BuscarPorPlaca(placa.Trim().ToUpper())
            ?? throw new NaoEncontradoException($"Veículo com placa {placa} não encontrado.");
    }

    public Veiculo Cadastrar(string placa, string modelo, int ano, double valorDiaria, int quilometragemAtual)
    {
        var veiculo = new Veiculo(placa, modelo, ano, valorDiaria, quilometragemAtual);

        if (_veiculos.BuscarPorPlaca(veiculo.Placa) is not null)
            throw new ConflitoException($"Já existe um veículo com a placa {veiculo.Placa}.");

        _veiculos.Adicionar(veiculo);
        return veiculo;
    }

    public Veiculo Atualizar(int id, string modelo, int ano, double valorDiaria)
    {
        var veiculo = BuscarPorId(id);
        veiculo.AlterarDados(modelo, ano, valorDiaria);
        _veiculos.Atualizar(veiculo);
        return veiculo;
    }

    public Veiculo AtualizarQuilometragem(int id, int novaKm)
    {
        var veiculo = BuscarPorId(id);
        veiculo.AtualizarQuilometragem(novaKm);
        _veiculos.Atualizar(veiculo);
        return veiculo;
    }

    public Veiculo EnviarManutencao(int id)
    {
        var veiculo = BuscarPorId(id);
        veiculo.EnviarManutencao();
        _veiculos.Atualizar(veiculo);
        return veiculo;
    }

    public Veiculo RetornarDaManutencao(int id)
    {
        var veiculo = BuscarPorId(id);
        veiculo.RetornarDaManutencao();
        _veiculos.Atualizar(veiculo);
        return veiculo;
    }

    public void Remover(int id)
    {
        BuscarPorId(id);

        // Não excluir veículo que possua locações.
        if (_locacoes.ListarPorVeiculo(id).Any())
            throw new ConflitoException("Não é possível excluir um veículo que possui locações registradas.");

        _veiculos.Remover(id);
    }
}