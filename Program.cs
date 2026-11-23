using Locadora.Api.Endpoints;
using Locadora.Aplicacao;
using Locadora.Infraestrutura;

// RAIZ DE COMPOSIÇÃO: liga as peças da aplicação, e mais nada.

var builder = WebApplication.CreateBuilder(args);

// Registro das dependências: cada camada registra as próprias peças.
builder.Services.AddAplicacao();
builder.Services.AddInfraestrutura();

var app = builder.Build();

app.MapGet("/", () => "API Locadora de Veículos no ar!");

app.MapClientesEndpoints();
app.MapVeiculosEndpoints();
app.MapFuncionariosEndpoints();
app.MapLocacoesEndpoints();

app.Run("http://localhost:5000");