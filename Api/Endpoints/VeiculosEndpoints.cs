using Locadora.Api.Contratos;
using Locadora.Aplicacao.Veiculos;

namespace Locadora.Api.Endpoints;

public static class VeiculosEndpoints
{
    public static void MapVeiculosEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/veiculos");

        grupo.MapGet("/", (VeiculoServico servico) =>
            Results.Ok(servico.Listar().Select(v => VeiculoResponse.De(v))));

        grupo.MapGet("/{id:int}", (int id, VeiculoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(VeiculoResponse.De(servico.BuscarPorId(id)))));

        grupo.MapGet("/placa/{placa}", (string placa, VeiculoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(VeiculoResponse.De(servico.BuscarPorPlaca(placa)))));

        grupo.MapPost("/", (VeiculoRequest req, VeiculoServico servico) =>
            Respostas.Executar(() =>
            {
                var veiculo = servico.Cadastrar(req.Placa, req.Modelo, req.Ano, req.ValorDiaria, req.QuilometragemAtual);
                return Results.Created($"/veiculos/{veiculo.Id}", VeiculoResponse.De(veiculo));
            }));

        grupo.MapPut("/{id:int}", (int id, VeiculoAtualizacaoRequest req, VeiculoServico servico) =>
            Respostas.Executar(() =>
            {
                var veiculo = servico.Atualizar(id, req.Modelo, req.Ano, req.ValorDiaria);
                return Results.Ok(VeiculoResponse.De(veiculo));
            }));

        grupo.MapDelete("/{id:int}", (int id, VeiculoServico servico) =>
            Respostas.Executar(() =>
            {
                servico.Remover(id);
                return Results.NoContent();
            }));

        // Operações específicas do veículo
        grupo.MapPut("/{id:int}/quilometragem", (int id, QuilometragemRequest req, VeiculoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(VeiculoResponse.De(servico.AtualizarQuilometragem(id, req.NovaKm)))));

        grupo.MapPut("/{id:int}/manutencao", (int id, VeiculoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(VeiculoResponse.De(servico.EnviarManutencao(id)))));

        grupo.MapPut("/{id:int}/retorno-manutencao", (int id, VeiculoServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(VeiculoResponse.De(servico.RetornarDaManutencao(id)))));
    }
}