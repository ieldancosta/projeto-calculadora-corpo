// Importações / Dependências
using BodyCalculator.Services; /* Permite enxergar os Services */
using BodyCalculator.Enums; /* Permite enxergar as Enums */


// Namespace
namespace BodyCalculator.DTOs.Responses;


// Classe
public class MetabolismoResponse /* Classe de Transferência - DTO, Data Transfer Object | Baseado no fluxo de requisição HTTP, representa a response (resposta) de uma request (requisição) que o servidor vai devolver para o usuário */
{
    public double IMC { get; set; } /* Valor independente */
    public required string ClassificacaoIMC { get; set; } /* Texto/String */ /* O required obriga o preenchimento na hora de instanciar a classe | Ao adicionar o required, você diz ao compilador: "Eu não vou dar um valor inicial aqui na classe, mas eu proíbo que qualquer pessoa crie este DTO sem preencher essa propriedade" */
    public TipoFormula FormulaUsada { get; set; } 
    public double GER { get; set; } /* kcal */
    public double ETA { get; set; } /* kcal */
    public double GAF { get; set; } /* kcal */
    public double GET { get; set; } /* kcal */
    public double CaloriasAlvo { get; set; } /* kcal */
    public ObjetivoFisico ObjetivoFisico { get; set; } /* string */
    

    // Métodos
    public override string ToString() /* Método que permite imprimir a classe direto */
    {
        return $"""
            Objetivo Físico: {ObjetivoFisico}
            Calorias Alvo: {CaloriasAlvo} kcal
            Fórmula Usada: {FormulaUsada}
            GET: {GET} kcal
            GAF: {GAF} kcal
            ETA: {ETA} kcal
            GER: {GER} kcal
            IMC: {IMC}
            Classificação IMC: {ClassificacaoIMC}
            """;
    }
}
