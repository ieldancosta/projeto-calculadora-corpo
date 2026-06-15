// Importações / Dependências
using Microsoft.AspNetCore.Mvc;
using System; // Necessário para a classe Exception
using BodyCalculator.DTOs.Requests;
using BodyCalculator.DTOs.Responses;
using BodyCalculator.Models;
using BodyCalculator.Services;


// Namespace
namespace BodyCalculator.Controllers;


[ApiController] /* Etiqueta que transforma a classe em um recebedor de requisições Web */
[Route("api/macronutrientes")] /* Define a URL base: http://localhost:5000/api/macronutrientes */
public class MacronutrientesController : ControllerBase
{
    // Define a sub-rota e o verbo HTTP: http://localhost:5000/api/macronutrientes/calcular
    [HttpPost("calcular")]
    public ActionResult<MacronutrientesResponse> Calcular([FromBody] MacronutrientesRequest request)
    {
        try
        {
            // 1. Mapeamento
            Pessoa pessoa;
            
            if (request.PercentualGordura.HasValue)
            {
                pessoa = new Pessoa(request.Nome, request.Idade, request.Sexo, request.Peso, request.Altura, request.FatorAtividade, request.PercentualGordura.Value);
            }
            else
            {
                pessoa = new Pessoa(request.Nome, request.Idade, request.Sexo, request.Peso, request.Altura, request.FatorAtividade);
            }


            // 2. Processamento

            /* Primeiro, o sistema calcula o metabolismo para descobrir a meta calórica */
            var metabolismo = CalculadoraMetabolismo.CalcularMetabolismo(pessoa, request.ObjetivoFisico);
            /* O tipo var também pode ser substituído pelo objeto MetabolismoResponse, mas é recomendado que: use o var sempre que o lado direito do igual deixar óbvio qual é o tipo de dado, como diz a palavra "inferência". Isso deixa o código muito mais limpo e elegante de ler! */
            
            /* Depois, injetamos as "CaloriasAlvo" geradas na calculadora de macros */
            MacronutrientesResponse resposta = CalculadoraMacronutrientes.CalcularMacronutrientes(pessoa, metabolismo.CaloriasAlvo, request.ObjetivoFisico);


            // 3. Resposta

            /* Retorna o código HTTP 200 (OK) embrulhando o DTO de resposta */
            return Ok(resposta);
        }
        catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}
