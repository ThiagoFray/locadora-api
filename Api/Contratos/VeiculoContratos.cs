using Locadora.Dominio;

namespace Locadora.Api.Contratos;

// POST: o consumidor não envia Id nem StatusVeiculo (todo veículo novo começa Disponível).
public record VeiculoRequest(
    string Placa,
    string Modelo,
    int Ano,
    double ValorDiaria,
    int QuilometragemAtual);

// PUT: sem placa (não pode mudar), sem status e sem km.
public record VeiculoAtualizacaoRequest(
    string Modelo,
    int Ano,
    double ValorDiaria);

// PUT /veiculos/{id}/quilometragem
public record QuilometragemRequest(int NovaKm);

public record VeiculoResponse(
    int Id,
    string Placa,
    string Modelo,
    int Ano,
    double ValorDiaria,
    int QuilometragemAtual,
    string StatusVeiculo)
{
    public static VeiculoResponse De(Veiculo veiculo)
    {
        return new VeiculoResponse(
            veiculo.Id,
            veiculo.Placa,
            veiculo.Modelo,
            veiculo.Ano,
            veiculo.ValorDiaria,
            veiculo.QuilometragemAtual,
            veiculo.StatusVeiculo);
    }
}