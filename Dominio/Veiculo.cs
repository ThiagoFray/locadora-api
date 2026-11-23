namespace Locadora.Dominio;

public class Veiculo
{
    // Valores possíveis do status (evita erros de digitação espalhados pelo código).
    public const string Disponivel = "Disponivel";
    public const string Alugado = "Alugado";
    public const string EmManutencao = "EmManutencao";

    // Quem define o Id é o repositório (Infraestrutura), por isso o set é público.
    public int Id { get; set; }

    public string Placa { get; private set; } = "";
    public string Modelo { get; private set; } = "";
    public int Ano { get; private set; }
    public double ValorDiaria { get; private set; }
    public int QuilometragemAtual { get; private set; }
    public string StatusVeiculo { get; private set; } = Disponivel;

    public Veiculo(string placa, string modelo, int ano, double valorDiaria, int quilometragemAtual)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new RegraDeNegocioException("A placa é obrigatória.");

        if (quilometragemAtual < 0)
            throw new RegraDeNegocioException("A quilometragem não pode ser negativa.");

        Placa = placa.Trim().ToUpper();
        QuilometragemAtual = quilometragemAtual;
        StatusVeiculo = Disponivel;

        AlterarDados(modelo, ano, valorDiaria);
    }

    // Não altera status nem quilometragem: essas mudanças têm métodos próprios (regra R7).
    public void AlterarDados(string modelo, int ano, double valorDiaria)
    {
        if (string.IsNullOrWhiteSpace(modelo))
            throw new RegraDeNegocioException("O modelo é obrigatório.");

        int anoMaximo = DateTime.Now.Year + 1;
        if (ano < 1950 || ano > anoMaximo)
            throw new RegraDeNegocioException($"O ano deve estar entre 1950 e {anoMaximo}.");

        if (valorDiaria <= 0)
            throw new RegraDeNegocioException("O valor da diária deve ser maior que zero.");

        Modelo = modelo.Trim();
        Ano = ano;
        ValorDiaria = valorDiaria;
    }

    public void AtualizarQuilometragem(int novaKm)
    {
        if (novaKm < QuilometragemAtual)
            throw new RegraDeNegocioException(
                $"A nova quilometragem ({novaKm}) não pode ser menor que a atual ({QuilometragemAtual}).");

        QuilometragemAtual = novaKm;
    }

    public void EnviarManutencao()
    {
        if (StatusVeiculo != Disponivel)
            throw new RegraDeNegocioException("Somente veículos disponíveis podem ser enviados para manutenção.");

        StatusVeiculo = EmManutencao;
    }

    public void RetornarDaManutencao()
    {
        if (StatusVeiculo != EmManutencao)
            throw new RegraDeNegocioException("O veículo não está em manutenção.");

        StatusVeiculo = Disponivel;
    }

    public double CalcularCusto(int quantidadeDias)
    {
        if (quantidadeDias < 1)
            throw new RegraDeNegocioException("A quantidade de dias deve ser de no mínimo 1.");

        return quantidadeDias * ValorDiaria;
    }

    public void Alugar()
    {
        if (StatusVeiculo != Disponivel)
            throw new RegraDeNegocioException("O veículo não está disponível para locação.");

        StatusVeiculo = Alugado;
    }

    public void Devolver()
    {
        StatusVeiculo = Disponivel;
    }
}