using Locadora.Dominio;

namespace Locadora.Aplicacao.Portas;

public interface IFuncionarioRepositorio
{
    void Adicionar(Funcionario funcionario);
    Funcionario? BuscarPorMatricula(string matricula);
    IEnumerable<Funcionario> Listar();
    void Atualizar(Funcionario funcionario);
    bool Remover(string matricula);
}