using Locadora.Aplicacao.Portas;
using Locadora.Infraestrutura.Persistencia;
using Microsoft.Extensions.DependencyInjection;

namespace Locadora.Infraestrutura;

public static class InfraestruturaExtensions
{
    // Aqui se escolhe o ADAPTADOR de cada PORTA.
    // Trocar memória por banco = criar novos adaptadores e mudar só estas linhas.
    public static IServiceCollection AddInfraestrutura(this IServiceCollection services)
    {
        var clientes = new ClienteRepositorioMemoria();
        var veiculos = new VeiculoRepositorioMemoria();
        var funcionarios = new FuncionarioRepositorioMemoria();
        var locacoes = new LocacaoRepositorioMemoria();

        DadosIniciais.Popular(clientes, veiculos, funcionarios, locacoes);

        services.AddSingleton<IClienteRepositorio>(clientes);
        services.AddSingleton<IVeiculoRepositorio>(veiculos);
        services.AddSingleton<IFuncionarioRepositorio>(funcionarios);
        services.AddSingleton<ILocacaoRepositorio>(locacoes);

        return services;
    }
}