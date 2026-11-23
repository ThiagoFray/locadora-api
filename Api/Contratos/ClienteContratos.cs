using Locadora.Dominio;

namespace Locadora.Api.Contratos;

// O que ENTRA no cadastro (POST): o consumidor não envia Id nem PossuiLocacaoAtiva.
public record ClienteRequest(
    string Cpf,
    string Nome,
    string Cnh,
    DateTime VencimentoCnh,
    string Telefone);

// O que ENTRA na atualização (PUT): o CPF não pode ser alterado, por isso não aparece aqui.
public record ClienteAtualizacaoRequest(
    string Nome,
    string Cnh,
    DateTime VencimentoCnh,
    string Telefone);

// O que SAI da API.
public record ClienteResponse(
    int Id,
    string Cpf,
    string Nome,
    string Cnh,
    DateTime VencimentoCnh,
    string Telefone,
    bool PossuiLocacaoAtiva)
{
    // Converte a entidade do domínio para o formato do JSON.
    public static ClienteResponse De(Cliente cliente)
    {
        return new ClienteResponse(
            cliente.Id,
            cliente.Cpf,
            cliente.Nome,
            cliente.Cnh,
            cliente.VencimentoCnh,
            cliente.Telefone,
            cliente.PossuiLocacaoAtiva);
    }
}