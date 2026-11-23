using Locadora.Aplicacao.Excecoes;
using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Aplicacao.Locacoes;

public class LocacaoServico
{
    private readonly ILocacaoRepositorio _locacoes;
    private readonly IClienteRepositorio _clientes;
    private readonly IVeiculoRepositorio _veiculos;
    private readonly IFuncionarioRepositorio _funcionarios;

    public LocacaoServico(ILocacaoRepositorio locacoes, IClienteRepositorio clientes,
                          IVeiculoRepositorio veiculos, IFuncionarioRepositorio funcionarios)
    {
        _locacoes = locacoes;
        _clientes = clientes;
        _veiculos = veiculos;
        _funcionarios = funcionarios;
    }

    public IEnumerable<Locacao> Listar()
    {
        return _locacoes.Listar();
    }

    public Locacao BuscarPorId(int id)
    {
        return _locacoes.BuscarPorId(id)
            ?? throw new NaoEncontradoException($"Locação {id} não encontrada.");
    }

    public IEnumerable<Locacao> ListarPorCliente(int clienteId)
    {
        BuscarCliente(clienteId);   // 404 se o cliente não existir
        return _locacoes.ListarPorCliente(clienteId);
    }

    public IEnumerable<Locacao> ListarPorVeiculo(int veiculoId)
    {
        BuscarVeiculo(veiculoId);   // 404 se o veículo não existir
        return _locacoes.ListarPorVeiculo(veiculoId);
    }

    // Locações ativas que já passaram da data de fim prevista.
    public IEnumerable<Locacao> ListarAtrasadas()
    {
        return _locacoes.Listar()
            .Where(l => l.StatusLocacao == Locacao.Ativa && l.Atraso(DateTime.Today));
    }

    public Locacao Registrar(int clienteId, int veiculoId, string matriculaFuncionario,
                             DateTime dataInicio, DateTime dataFimPrevista)
    {
        // 1) Os três envolvidos precisam existir (integridade das referências).
        var cliente = BuscarCliente(clienteId);
        var veiculo = BuscarVeiculo(veiculoId);

        // JSON sem a matrícula chega como null: responde 400 em vez de erro 500.
        if (string.IsNullOrWhiteSpace(matriculaFuncionario))
            throw new RegraDeNegocioException("A matrícula do funcionário é obrigatória.");

        var funcionario = _funcionarios.BuscarPorMatricula(matriculaFuncionario.Trim())
            ?? throw new NaoEncontradoException($"Funcionário com matrícula {matriculaFuncionario} não encontrado.");

        // 2) Regras que envolvem mais de uma entidade (regra R9).
        if (!cliente.CnhValida(dataInicio))
            throw new RegraDeNegocioException("A CNH do cliente estará vencida na data de início da locação.");

        if (veiculo.StatusVeiculo != Veiculo.Disponivel)
            throw new ConflitoException($"O veículo {veiculo.Placa} não está disponível (status: {veiculo.StatusVeiculo}).");

        // 3) Cria a locação: o construtor valida as datas e calcula o valor.
        var locacao = new Locacao(cliente, veiculo, funcionario, dataInicio, dataFimPrevista);

        // 4) Só agora altera o estado do cliente e do veículo.
        if (!cliente.AlugarNovoVeiculo())
            throw new ConflitoException("O cliente já possui uma locação ativa.");

        veiculo.Alugar();

        // 5) Salva tudo.
        _locacoes.Adicionar(locacao);
        _clientes.Atualizar(cliente);
        _veiculos.Atualizar(veiculo);

        return locacao;
    }

    public Locacao Finalizar(int id, DateTime dataDevolucao, int kmFinal)
    {
        var locacao = BuscarPorId(id);

        // A própria locação valida, calcula a multa e atualiza veículo e cliente.
        locacao.FinalizarLocacao(dataDevolucao, kmFinal);

        _locacoes.Atualizar(locacao);
        _veiculos.Atualizar(locacao.Veiculo);
        _clientes.Atualizar(locacao.Cliente);

        return locacao;
    }

    // Métodos auxiliares privados: evitam repetir a mesma busca.
    private Cliente BuscarCliente(int clienteId)
    {
        return _clientes.BuscarPorId(clienteId)
            ?? throw new NaoEncontradoException($"Cliente {clienteId} não encontrado.");
    }

    private Veiculo BuscarVeiculo(int veiculoId)
    {
        return _veiculos.BuscarPorId(veiculoId)
            ?? throw new NaoEncontradoException($"Veículo {veiculoId} não encontrado.");
    }
}