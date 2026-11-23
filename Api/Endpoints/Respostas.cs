using Locadora.Aplicacao.Excecoes;
using Locadora.Dominio;

namespace Locadora.Api.Endpoints;

// Converte as exceções de negócio em respostas HTTP, num lugar só.
public static class Respostas
{
    public static IResult Executar(Func<IResult> acao)
    {
        try
        {
            return acao();
        }
        catch (NaoEncontradoException ex)
        {
            return Results.NotFound(new { mensagem = ex.Message });
        }
        catch (ConflitoException ex)
        {
            return Results.Conflict(new { mensagem = ex.Message });
        }
        catch (RegraDeNegocioException ex)
        {
            return Results.BadRequest(new { mensagem = ex.Message });
        }
    }
}