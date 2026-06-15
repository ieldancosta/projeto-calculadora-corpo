// Namespaces
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;


// 1. O "Builder" é o construtor do seu restaurante (Servidor)
var builder = WebApplication.CreateBuilder(args);

// 2. Adicionando os serviços (Contratando os garçons e o cardápio visual)
builder.Services.AddControllers().AddJsonOptions(options => /* Habilita o uso de Controllers */
    {
        // Ensina a API a ler e devolver Enums como Strings em vez de números
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(); /* Habilita o Swagger (Interface gráfica para testar a API)  */

// Configuração de CORS (A Lista VIP)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontendReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // A origem exata do seu React
              .AllowAnyHeader()  // Permite qualquer cabeçalho (como Content-Type)
              .AllowAnyMethod(); // Permite POST, GET, OPTIONS, etc.
    });
});

// 3. Constrói a aplicação de fato
var app = builder.Build();

// 4. Configura o "Salão" (Pipeline HTTP)
app.UseSwagger();
app.UseSwaggerUI();

// Redireciona tudo para HTTPS por segurança
app.UseHttpsRedirection();

// Ativar o CORS (deve vir ANTES do MapControllers e do UseAuthorization)
app.UseCors("PermitirFrontendReact");

// Mapeia os seus Controllers para que a internet consiga achá-los
app.MapControllers();


// 5. Abre as portas do restaurante e fica escutando eternamente!
app.Run(); /* Caso queira mudar a porta, pode passar diretamente em: app.Run("http://localhost:8080"); */
