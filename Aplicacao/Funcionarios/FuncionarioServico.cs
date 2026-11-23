using Locadora.Aplicacao.Excecoes;
using Locadora.Aplicacao.Portas;
using Locadora.Dominio;

namespace Locadora.Aplicacao.Funcionarios;

public class FuncionarioServico
{
    private readonly IFuncionarioRepositorio _funcionarios;
    private readonly ILocacaoRepositorio _locacoes;

    public FuncionarioServico(IFuncionarioRepositorio funcionarios, ILocacaoRepositorio locacoes)
    {
        _funcionarios = funcionarios;
        _locacoes = locacoes;
    }

    public IEnumerable<Funcionario> Listar()
    {
        return _funcionarios.Listar();
    }

    public Funcionario BuscarPorMatricula(string matricula)
    {
        return _funcionarios.BuscarPorMatricula(matricula.Trim())
            ?? throw new NaoEncontradoException($"Funcionário com matrícula {matricula} não encontrado.");
    }

    public Funcionario Cadastrar(string matricula, string nome, string cargo, string senha)
    {
        var funcionario = new Funcionario(matricula, nome, cargo, senha);

        if (_funcionarios.BuscarPorMatricula(funcionario.Matricula) is not null)
            throw new ConflitoException($"Já existe um funcionário com a matrícula {funcionario.Matricula}.");

        _funcionarios.Adicionar(funcionario);
        return funcionario;
    }

    public Funcionario Atualizar(string matricula, string nome, string cargo, string senha)
    {
        var funcionario = BuscarPorMatricula(matricula);
        funcionario.AlterarDados(nome, cargo, senha);
        _funcionarios.Atualizar(funcionario);
        return funcionario;
    }

    public void Remover(string matricula)
    {
        var funcionario = BuscarPorMatricula(matricula);

        // Não excluir funcionário que possua locações.
        if (_locacoes.ListarPorFuncionario(funcionario.Matricula).Any())
            throw new ConflitoException("Não é possível excluir um funcionário que possui locações registradas.");

        _funcionarios.Remover(funcionario.Matricula);
    }

        public bool RealizarLogin(string matricula, string senha)
    {
        // JSON sem matrícula ou senha chega como null: trata como login inválido.
        if (string.IsNullOrWhiteSpace(matricula) || string.IsNullOrWhiteSpace(senha))
            return false;

        var funcionario = _funcionarios.BuscarPorMatricula(matricula.Trim());

        // Matrícula inexistente ou senha errada: a resposta é a mesma (false).
        if (funcionario is null)
            return false;

        return funcionario.RealizarLogin(senha);
    }
}