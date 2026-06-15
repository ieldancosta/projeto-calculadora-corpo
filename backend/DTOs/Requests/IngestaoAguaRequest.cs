// Importações / Dependências
using System.ComponentModel.DataAnnotations; /* Permite utilizar Data Annotations (etiquetas) */


// Namespace
namespace BodyCalculator.DTOs.Requests;


public class IngestaoAguaRequest
{
    public required string Nome { get; set; } /* Texto/String */

    [Range(1, 120, ErrorMessage = "A idade deve ser entre 1 e 120 anos.")]
    public int Idade { get; set; } /* anos */

    public required string Sexo { get; set; } /* Texto/String */

    [Range(1.0, 400.0, ErrorMessage = "O peso deve ser maior que zero.")]
    public double Peso { get; set; } /* kg */

    [Range(50.0, 300.0, ErrorMessage = "A altura deve ser informada em centímetros e ser maior que zero.")]
    public double Altura { get; set; } /* cm */

    [Range(1.2, 3.0, ErrorMessage = "O fator de atividade deve ser de no mínimo 1.2.")]
    public double FatorAtividade { get; set; } /* Valor independente */

    /* De forma semelhante ao que aconteceu em MacronutrientesRequest, aqui
       vou manter também pois podemos expandir o cálculo de ingestão de água com esse dado.
       No momento ele não é necessário para o cálculo (nem pra criar o objeto de Pessoa) */
    public double? PercentualGordura { get; set; } /* % */
}

