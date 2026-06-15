// Importações / Dependências
using Microsoft.AspNetCore.Mvc;
using BodyCalculator.DTOs.Requests;
using BodyCalculator.DTOs.Responses;
using BodyCalculator.Models; /* Permite enxergar os Models */
using BodyCalculator.Services;  /* Permite enxergar os Services */


// Namespace
namespace BodyCalculator.Controllers;


[ApiController] /* Etiqueta que transforma a classe em um recebedor de requisições Web */
[Route("api/metabolismo")] /* Define a URL base: http://localhost:5000/api/metabolismo  */
public class MetabolismoController : ControllerBase
{
    // Define a sub-rota e o verbo HTTP: http://localhost:5000/api/metabolismo/calcular
    [HttpPost("calcular")] 
    public ActionResult<MetabolismoResponse> Calcular([FromBody] MetabolismoRequest request) /* O FromBody diz para procurar os dados dentro do corpo da requisição HTTP */
    {
        // 1. Mapeamento (De DTO para Model)

        /* O cliente mandou o pedido (request). Agora montamos a nossa entidade oficial do sistema. */
        Pessoa pessoa;
        
        /* Assim como em Program.cs, vamos carregar a classe Pessoa com os dados recebidos do Frontend */
        if (request.PercentualGordura.HasValue)
        {
            // Usa o construtor 2 (com gordura)
            pessoa = new Pessoa(request.Nome, request.Idade, request.Sexo, request.Peso, request.Altura, request.FatorAtividade, request.PercentualGordura.Value);
        }
        else
        {
            // Usa o construtor 1 (sem gordura)
            pessoa = new Pessoa(request.Nome, request.Idade, request.Sexo, request.Peso, request.Altura, request.FatorAtividade);
        }


        // 2. Processamento (A Cozinha)

        /* Passamos a entidade limpa e as regras para o seu motor matemático */
        MetabolismoResponse resposta = CalculadoraMetabolismo.CalcularMetabolismo(
            pessoa, 
            request.ObjetivoFisico, 
            request.FormulaUsada
        );


        // 3. Resposta (O Prato Pronto)

        /* Retorna o código HTTP 200 (OK) embrulhando o DTO de resposta */
        return Ok(resposta);
    }
}