using Locadora.Api.Contratos;
using Locadora.Aplicacao.Locacoes;

namespace Locadora.Api.Endpoints;

public static class LocacoesEndpoints
{
    public static void MapLocacoesEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/locacoes");

        grupo.MapGet("/", (LocacaoServico servico) =>
            Results.Ok(servico.Listar().Select(l => LocacaoResponse.De(l))));

        grupo.MapGet("/{id:int}", (int id, LocacaoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(LocacaoResponse.De(servico.BuscarPorId(id)))));

        grupo.MapGet("/cliente/{clienteId:int}", (int clienteId, LocacaoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(servico.ListarPorCliente(clienteId).Select(l => LocacaoResponse.De(l)))));

        grupo.MapGet("/veiculo/{veiculoId:int}", (int veiculoId, LocacaoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(servico.ListarPorVeiculo(veiculoId).Select(l => LocacaoResponse.De(l)))));

        // Consultar atrasos e multas
        grupo.MapGet("/atrasadas", (LocacaoServico servico) =>
            Results.Ok(servico.ListarAtrasadas().Select(l => AtrasoResponse.De(l, DateTime.Today))));

        grupo.MapGet("/{id:int}/multa", (int id, LocacaoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(AtrasoResponse.De(servico.BuscarPorId(id), DateTime.Today))));

        // Registrar nova locação
        grupo.MapPost("/", (LocacaoRequest req, LocacaoServico servico) =>
            Respostas.Executar(() =>
            {
                var locacao = servico.Registrar(req.ClienteId, req.VeiculoId, req.MatriculaFuncionario,
                                                req.DataInicio, req.DataFimPrevista);
                return Results.Created($"/locacoes/{locacao.IdLocacao}", LocacaoResponse.De(locacao));
            }));

        // Finalizar locação
        grupo.MapPut("/{id:int}/finalizar", (int id, FinalizarLocacaoRequest req, LocacaoServico servico) =>
            Respostas.Executar(() =>
            {
                var locacao = servico.Finalizar(id, req.DataDevolucao, req.KmFinal);
                return Results.Ok(LocacaoResponse.De(locacao));
            }));
    }
}