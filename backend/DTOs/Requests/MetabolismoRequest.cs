// Importações / Dependências
using BodyCalculator.Services; /* Permite enxergar os Services */
using System.ComponentModel.DataAnnotations; /* Permite utilizar Data Annotations (etiquetas) */
using BodyCalculator.Enums; /* Permite enxergar as Enums */


// Namespace
namespace BodyCalculator.DTOs.Requests;


public class MetabolismoRequest /* Classe de Transferência - DTO, Data Transfer Object | Baseado no fluxo de requisição HTTP, representa o agrupamento dos dados para uma request (requisição) de um primeiro contato pelo usuário na aplicação web */
{
    // Dados necessários para construir a entidade Pessoa posteriormente
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
    public double? PercentualGordura { get; set; } /* % */

    // Parâmetros exigidos diretamente pelo método da CalculadoraMetabolismo
    public ObjetivoFisico ObjetivoFisico { get; set; } = ObjetivoFisico.Manutencao;
    public TipoFormula FormulaUsada { get; set; } = TipoFormula.MifflinStJeor;
}
