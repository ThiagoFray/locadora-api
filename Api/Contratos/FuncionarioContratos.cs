using Locadora.Dominio;

namespace Locadora.Api.Contratos;

public record FuncionarioRequest(
    string Matricula,
    string Nome,
    string Cargo,
    string Senha);

// PUT: a matrícula vem pela URL e não pode ser alterada.
public record FuncionarioAtualizacaoRequest(
    string Nome,
    string Cargo,
    string Senha);

public record LoginRequest(
    string Matricula,
    string Senha);

// A senha NUNCA sai da API.
public record FuncionarioResponse(
    string Matricula,
    string Nome,
    string Cargo)
{
    public static FuncionarioResponse De(Funcionario funcionario)
    {
        return new FuncionarioResponse(
            funcionario.Matricula,
            funcionario.Nome,
            funcionario.Cargo);
    }
}