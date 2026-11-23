using Locadora.Api.Contratos;
using Locadora.Aplicacao.Clientes;

namespace Locadora.Api.Endpoints;

public static class ClientesEndpoints
{
    public static void MapClientesEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/clientes");

        // GET /clientes -> lista todos
        grupo.MapGet("/", (ClienteServico servico) =>
            Results.Ok(servico.Listar().Select(c => ClienteResponse.De(c))));

        // GET /clientes/1 -> busca por Id
        grupo.MapGet("/{id:int}", (int id, ClienteServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(ClienteResponse.De(servico.BuscarPorId(id)))));

        // GET /clientes/cpf/12345678901 -> busca por CPF
        grupo.MapGet("/cpf/{cpf}", (string cpf, ClienteServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(ClienteResponse.De(servico.BuscarPorCpf(cpf)))));

        // POST /clientes -> cadastra
        grupo.MapPost("/", (ClienteRequest req, ClienteServico servico) =>
            Respostas.Executar(() =>
            {
                var cliente = servico.Cadastrar(req.Cpf, req.Nome, req.Cnh, req.VencimentoCnh, req.Telefone);
                return Results.Created($"/clientes/{cliente.Id}", ClienteResponse.De(cliente));
            }));

        // PUT /clientes/1 -> atualiza
        grupo.MapPut("/{id:int}", (int id, ClienteAtualizacaoRequest req, ClienteServico servico) =>
            Respostas.Executar(() =>
            {
                var cliente = servico.Atualizar(id, req.Nome, req.Cnh, req.VencimentoCnh, req.Telefone);
                return Results.Ok(ClienteResponse.De(cliente));
            }));

        // DELETE /clientes/1 -> exclui
        grupo.MapDelete("/{id:int}", (int id, ClienteServico servico) =>
            Respostas.Executar(() =>
            {
                servico.Remover(id);
                return Results.NoContent();
            }));
    }
}