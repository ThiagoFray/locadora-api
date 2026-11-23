using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Infraestrutura.Persistencia;

// Dados mocados: carregados na memória sempre que a aplicação inicia.
public static class DadosIniciais
{
    public static void Popular(IClienteRepositorio clientes, IVeiculoRepositorio veiculos,
                               IFuncionarioRepositorio funcionarios, ILocacaoRepositorio locacoes)
    {
        // Clientes
        var ana = new Cliente("12345678901", "Ana Souza", "11122233344", DateTime.Today.AddYears(2), "49999990001");
        var bruno = new Cliente("98765432100", "Bruno Lima", "55566677788", DateTime.Today.AddYears(1), "49999990002");
        var carla = new Cliente("11122233355", "Carla Mendes", "99988877766", DateTime.Today.AddDays(-30), "49999990003"); // CNH vencida

        clientes.Adicionar(ana);
        clientes.Adicionar(bruno);
        clientes.Adicionar(carla);

        // Veículos
        var mobi = new Veiculo("ABC1D23", "Fiat Mobi", 2022, 120.00, 35000);
        var onix = new Veiculo("XYZ9K87", "Chevrolet Onix", 2023, 150.00, 18000);
        var corolla = new Veiculo("JKL4M56", "Toyota Corolla", 2021, 250.00, 52000);
        corolla.EnviarManutencao(); // em manutenção

        veiculos.Adicionar(mobi);
        veiculos.Adicionar(onix);
        veiculos.Adicionar(corolla);

        // Funcionários
        var pedro = new Funcionario("F001", "Pedro Alves", "Atendente", "1234");
        var mariana = new Funcionario("F002", "Mariana Costa", "Gerente", "admin");

        funcionarios.Adicionar(pedro);
        funcionarios.Adicionar(mariana);

        // Uma locação ativa: Bruno alugou o Onix por 3 dias, atendido por Pedro.
        var locacao = new Locacao(bruno, onix, pedro, DateTime.Today, DateTime.Today.AddDays(3));
        bruno.AlugarNovoVeiculo();
        onix.Alugar();
        locacoes.Adicionar(locacao);
    }
}