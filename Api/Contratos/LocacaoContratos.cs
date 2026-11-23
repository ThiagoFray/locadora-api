using Locadora.Dominio;

namespace Locadora.Api.Contratos;

// POST /locacoes: o consumidor informa QUEM e QUANDO.
// Não envia Id, valor, status nem data de devolução: tudo isso é controlado pelo sistema.
public record LocacaoRequest(
    int ClienteId,
    int VeiculoId,
    string MatriculaFuncionario,
    DateTime DataInicio,
    DateTime DataFimPrevista);

// PUT /locacoes/{id}/finalizar
public record FinalizarLocacaoRequest(
    DateTime DataDevolucao,
    int KmFinal);

public record LocacaoResponse(
    int IdLocacao,
    int ClienteId,
    string NomeCliente,
    int VeiculoId,
    string PlacaVeiculo,
    string MatriculaFuncionario,
    string NomeFuncionario,
    DateTime DataInicio,
    DateTime DataFimPrevista,
    DateTime? DataDevolucao,
    double ValorTotal,
    string StatusLocacao,
    double ComissaoFuncionario)
{
    public static LocacaoResponse De(Locacao locacao)
    {
        return new LocacaoResponse(
            locacao.IdLocacao,
            locacao.Cliente.Id,
            locacao.Cliente.Nome,
            locacao.Veiculo.Id,
            locacao.Veiculo.Placa,
            locacao.Funcionario.Matricula,
            locacao.Funcionario.Nome,
            locacao.DataInicio,
            locacao.DataFimPrevista,
            locacao.DataDevolucao,
            locacao.ValorTotal,
            locacao.StatusLocacao,
            locacao.Funcionario.CalcularComissao(locacao.ValorTotal));
    }
}

// Situação de atraso e multa de uma locação numa data (normalmente hoje).
public record AtrasoResponse(
    int IdLocacao,
    string NomeCliente,
    string PlacaVeiculo,
    DateTime DataFimPrevista,
    bool EmAtraso,
    double Multa)
{
    public static AtrasoResponse De(Locacao locacao, DateTime dataAtual)
    {
        return new AtrasoResponse(
            locacao.IdLocacao,
            locacao.Cliente.Nome,
            locacao.Veiculo.Placa,
            locacao.DataFimPrevista,
            locacao.Atraso(dataAtual),
            locacao.CalcularMulta(dataAtual));
    }
}