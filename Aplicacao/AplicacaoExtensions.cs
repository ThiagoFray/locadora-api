using Locadora.Aplicacao.Clientes;
using Locadora.Aplicacao.Funcionarios;
using Locadora.Aplicacao.Locacoes;
using Locadora.Aplicacao.Veiculos;
using Microsoft.Extensions.DependencyInjection;

namespace Locadora.Aplicacao;

public static class AplicacaoExtensions
{
    public static IServiceCollection AddAplicacao(this IServiceCollection services)
    {
        services.AddScoped<ClienteServico>();
        services.AddScoped<VeiculoServico>();
        services.AddScoped<FuncionarioServico>();
        services.AddScoped<LocacaoServico>();
        return services;
    }
}