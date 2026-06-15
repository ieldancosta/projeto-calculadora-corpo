// Importações / Dependências
using Microsoft.AspNetCore.Mvc;
using System; // Necessário para ArgumentException
using BodyCalculator.DTOs.Requests;
using BodyCalculator.DTOs.Responses;
using BodyCalculator.Models;
using BodyCalculator.Services;


// Namespace
namespace BodyCalculator.Controllers;


[ApiController] /* Etiqueta que transforma a classe em um recebedor de requisições Web */
[Route("api/agua")] /* Define a URL base: http://localhost:5000/api/agua */
public class IngestaoAguaController : ControllerBase
{
    // Define a sub-rota e o verbo HTTP: http://localhost:5000/api/agua/calcular
    [HttpPost("calcular")]
    public ActionResult<IngestaoAguaResponse> Calcular([FromBody] IngestaoAguaRequest request)
    {
        try
        {
            // 1. Mapeamento

            /* Construímos o nosso Model seguro com base no DTO Request validado */
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
            
            /* Injetamos a Pessoa na Cozinha (Service) usando a inferência de tipo (var) */
            /* Pessoa é necessário para os cálculos de ingestão de água em: */
            /* Idade | Peso */
            var resposta = CalculadoraIngestaoAgua.CalcularIngestaoAgua(pessoa);
            /* O tipo var também pode ser substituído pelo objeto IngestaoAguaResponse, mas é recomendado que: */
            /* use o var sempre que o lado direito do igual deixar óbvio qual é o tipo de dado, como diz a palavra "inferência" (dedução) */
            /* Isso deixa o código muito mais limpo e elegante de ler! */


            // 3. Resposta

            // Retornamos o DTO pronto com Status 200 OK
            return Ok(resposta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}