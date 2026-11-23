namespace Locadora.Dominio;

public class Locacao
{
    // Valores possíveis do status.
    public const string Ativa = "Ativa";
    public const string Finalizada = "Finalizada";

    // Multa: cada dia de atraso custa a diária com 20% de acréscimo.
    public const double FatorMulta = 1.2;

    // Quem define o Id é o repositório (Infraestrutura), por isso o set é público.
    public int IdLocacao { get; set; }

    public DateTime DataInicio { get; private set; }
    public DateTime DataFimPrevista { get; private set; }
    public DateTime? DataDevolucao { get; private set; }   // null enquanto não for devolvida
    public double ValorTotal { get; private set; }
    public string StatusLocacao { get; private set; } = Ativa;

    // Relacionamentos do UML: toda locação tem 1 cliente, 1 veículo e 1 funcionário.
    public Cliente Cliente { get; private set; }
    public Veiculo Veiculo { get; private set; }
    public Funcionario Funcionario { get; private set; }

    public Locacao(Cliente cliente, Veiculo veiculo, Funcionario funcionario,
                   DateTime dataInicio, DateTime dataFimPrevista)
    {
        if (dataInicio.Date < DateTime.Today)
            throw new RegraDeNegocioException("A data de início não pode estar no passado.");

        if (dataFimPrevista.Date <= dataInicio.Date)
            throw new RegraDeNegocioException("A data de fim prevista deve ser posterior à data de início.");

        Cliente = cliente;
        Veiculo = veiculo;
        Funcionario = funcionario;
        DataInicio = dataInicio.Date;
        DataFimPrevista = dataFimPrevista.Date;
        DataDevolucao = null;
        StatusLocacao = Ativa;

        // Valor previsto: diária x dias contratados.
        int diasContratados = (DataFimPrevista - DataInicio).Days;
        ValorTotal = veiculo.CalcularCusto(diasContratados);
    }

    public void FinalizarLocacao(DateTime dataDevolucao, int kmFinal)
    {
        if (StatusLocacao != Ativa)
            throw new RegraDeNegocioException("Esta locação já foi finalizada.");

        if (dataDevolucao.Date < DataInicio)
            throw new RegraDeNegocioException("A data de devolução não pode ser anterior à data de início.");

        // Primeiro o que pode falhar (km menor que a atual lança exceção),
        // assim nada é alterado se a quilometragem for inválida.
        Veiculo.AtualizarQuilometragem(kmFinal);

        DataDevolucao = dataDevolucao.Date;
        ValorTotal += CalcularMulta(dataDevolucao);
        StatusLocacao = Finalizada;

        Veiculo.Devolver();
        Cliente.EncerrarLocacao();
    }

    public bool Atraso(DateTime dataAtual)
    {
        // Se já foi devolvida, compara a data da devolução; senão, a data atual.
        DateTime dataReferencia = DataDevolucao ?? dataAtual;
        return dataReferencia.Date > DataFimPrevista;
    }

    public double CalcularMulta(DateTime dataAtual)
    {
        if (!Atraso(dataAtual))
            return 0;

        DateTime dataReferencia = DataDevolucao ?? dataAtual;
        int diasDeAtraso = (dataReferencia.Date - DataFimPrevista).Days;

        return diasDeAtraso * Veiculo.ValorDiaria * FatorMulta;
    }
}