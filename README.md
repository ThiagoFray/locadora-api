# API – Locadora de Veículos

API REST para gerenciamento de locação de veículos, desenvolvida em **C# / ASP.NET Core Minimal API (.NET 10)** como projeto acadêmico de Ciência da Computação (Unoesc).

**Autores:** Thiago Frey, Adriano Engster e Guilherme Yuri Bach Bordin

## Arquitetura

Projeto único organizado em camadas (as dependências apontam para dentro):

| Pasta | Responsabilidade |
|---|---|
| `Dominio/` | Entidades e regras de negócio (Cliente, Veiculo, Funcionario, Locacao) |
| `Aplicacao/` | Serviços (casos de uso) e interfaces dos repositórios |
| `Infraestrutura/` | Repositórios em memória e dados mocados |
| `Api/` | Contratos JSON (Request/Response) e endpoints |
| `Program.cs` | Raiz de composição (injeção de dependência e rotas) |

Os dados ficam em memória: ao reiniciar a aplicação, voltam aos dados iniciais.

## Como executar

```bash
dotnet run
```

A API sobe em `http://localhost:5000`.

## Endpoints

| Recurso | Rotas |
|---|---|
| Clientes | `GET/POST /clientes` · `GET/PUT/DELETE /clientes/{id}` · `GET /clientes/cpf/{cpf}` |
| Veículos | `GET/POST /veiculos` · `GET/PUT/DELETE /veiculos/{id}` · `GET /veiculos/placa/{placa}` · `PUT /veiculos/{id}/quilometragem` · `PUT /veiculos/{id}/manutencao` · `PUT /veiculos/{id}/retorno-manutencao` |
| Funcionários | `GET/POST /funcionarios` · `GET/PUT/DELETE /funcionarios/{matricula}` · `POST /funcionarios/login` |
| Locações | `GET/POST /locacoes` · `GET /locacoes/{id}` · `GET /locacoes/cliente/{clienteId}` · `GET /locacoes/veiculo/{veiculoId}` · `PUT /locacoes/{id}/finalizar` · `GET /locacoes/atrasadas` · `GET /locacoes/{id}/multa` |

## Testes

O arquivo `Locadora.Api.http` contém todas as requisições de teste (sucesso e erro). Para usá-lo no VS Code, instale a extensão **REST Client**, deixe a API rodando e clique em **Send Request** acima de cada requisição, na ordem.
