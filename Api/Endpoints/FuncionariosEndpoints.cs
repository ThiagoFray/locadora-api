using Locadora.Api.Contratos;
using Locadora.Aplicacao.Funcionarios;

namespace Locadora.Api.Endpoints;

public static class FuncionariosEndpoints
{
    public static void MapFuncionariosEndpoints(this WebApplication app)
    {
        var grupo = app.MapGroup("/funcionarios");

        grupo.MapGet("/", (FuncionarioServico servico) =>
            Results.Ok(servico.Listar().Select(f => FuncionarioResponse.De(f))));

        grupo.MapGet("/{matricula}", (string matricula, FuncionarioServico servico) =>
            Respostas.Executar(() =>
                Results.Ok(FuncionarioResponse.De(servico.BuscarPorMatricula(matricula)))));

        grupo.MapPost("/", (FuncionarioRequest req, FuncionarioServico servico) =>
            Respostas.Executar(() =>
            {
                var funcionario = servico.Cadastrar(req.Matricula, req.Nome, req.Cargo, req.Senha);
                return Results.Created($"/funcionarios/{funcionario.Matricula}", FuncionarioResponse.De(funcionario));
            }));

        grupo.MapPut("/{matricula}", (string matricula, FuncionarioAtualizacaoRequest req, FuncionarioServico servico) =>
            Respostas.Executar(() =>
            {
                var funcionario = servico.Atualizar(matricula, req.Nome, req.Cargo, req.Senha);
                return Results.Ok(FuncionarioResponse.De(funcionario));
            }));

        grupo.MapDelete("/{matricula}", (string matricula, FuncionarioServico servico) =>
            Respostas.Executar(() =>
            {
                servico.Remover(matricula);
                return Results.NoContent();
            }));

        // POST /funcionarios/login
        grupo.MapPost("/login", (LoginRequest req, FuncionarioServico servico) =>
        {
            if (servico.RealizarLogin(req.Matricula, req.Senha))
                return Results.Ok(new { mensagem = "Login realizado com sucesso." });

            return Results.Json(new { mensagem = "Matrícula ou senha inválidas." }, statusCode: 401);
        });
    }
}