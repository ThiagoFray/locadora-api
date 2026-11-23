namespace Locadora.Dominio;

public class Funcionario
{
    // Percentual de comissão aprovado: 5% do valor da locação.
    public const double PercentualComissao = 0.05;

    public string Matricula { get; private set; } = "";
    public string Nome { get; private set; } = "";
    public string Cargo { get; private set; } = "";

    // Campo privado: ninguém fora da classe consegue ler a senha.
    private string senha = "";

    public Funcionario(string matricula, string nome, string cargo, string senha)
    {
        if (string.IsNullOrWhiteSpace(matricula))
            throw new RegraDeNegocioException("A matrícula é obrigatória.");

        Matricula = matricula.Trim();

        AlterarDados(nome, cargo, senha);
    }

    public void AlterarDados(string nome, string cargo, string novaSenha)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new RegraDeNegocioException("O nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(cargo))
            throw new RegraDeNegocioException("O cargo é obrigatório.");

        if (string.IsNullOrWhiteSpace(novaSenha))
            throw new RegraDeNegocioException("A senha é obrigatória.");

        Nome = nome.Trim();
        Cargo = cargo.Trim();
        senha = novaSenha;
    }

    public bool RealizarLogin(string senha)
    {
        return this.senha == senha;
    }

    public double CalcularComissao(double valorLocacao)
    {
        if (valorLocacao < 0)
            throw new RegraDeNegocioException("O valor da locação não pode ser negativo.");

        return valorLocacao * PercentualComissao;
    }
}