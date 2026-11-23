namespace Locadora.Dominio;

public class Cliente
{
    // Quem define o Id é o repositório (Infraestrutura), por isso o set é público.
    public int Id { get; set; }

    public string Cpf { get; private set; } = "";
    public string Nome { get; private set; } = "";
    public string Cnh { get; private set; } = "";
    public DateTime VencimentoCnh { get; private set; }
    public bool PossuiLocacaoAtiva { get; private set; }
    public string Telefone { get; private set; } = "";

    public Cliente(string cpf, string nome, string cnh, DateTime vencimentoCnh, string telefone)
    {
        if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
            throw new RegraDeNegocioException("O CPF deve conter exatamente 11 números.");

        Cpf = cpf;
        PossuiLocacaoAtiva = false;

        // Reaproveita as validações dos métodos abaixo (evita duplicar código).
        AlterarDados(nome, cnh, vencimentoCnh);
        AtualizarContato(telefone);
    }

    public void AlterarDados(string nome, string cnh, DateTime vencimentoCnh)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new RegraDeNegocioException("O nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(cnh))
            throw new RegraDeNegocioException("A CNH é obrigatória.");

        if (vencimentoCnh == default)
            throw new RegraDeNegocioException("A data de vencimento da CNH é obrigatória.");

        Nome = nome.Trim();
        Cnh = cnh.Trim();
        VencimentoCnh = vencimentoCnh;
    }

    public void AtualizarContato(string novoTelefone)
    {
        if (string.IsNullOrWhiteSpace(novoTelefone))
            throw new RegraDeNegocioException("O telefone é obrigatório.");

        Telefone = novoTelefone.Trim();
    }

    public bool CnhValida(DateTime dataAtual)
    {
        return VencimentoCnh.Date >= dataAtual.Date;
    }

    public bool AlugarNovoVeiculo()
    {
        if (PossuiLocacaoAtiva)
            return false;

        PossuiLocacaoAtiva = true;
        return true;
    }

    public void EncerrarLocacao()
    {
        PossuiLocacaoAtiva = false;
    }
}