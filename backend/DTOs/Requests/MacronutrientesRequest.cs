// Importações / Dependências
using BodyCalculator.Services; /* Permite enxergar */
using System.ComponentModel.DataAnnotations; /* Permite utilizar Data Annotations (etiquetas) */
using BodyCalculator.Enums; /* Permite enxergar as Enums */


// Namespace
namespace BodyCalculator.DTOs.Requests;


public class MacronutrientesRequest
{
    // Dados da Pessoa
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
    
    /* Mantido para futuras atualizações no algoritmo de macros 
       Planejamos usar o percentual de gordura (MLG) para refinar a distribuição de proteínas 
       Mais informações no documento "Informações para Conhecimento" */
    public double? PercentualGordura { get; set; } /* % */

    // Regra de Negócio específica para os macros
    public ObjetivoFisico ObjetivoFisico { get; set; }
}