// Importações / Dependências
using System.Text;
using BodyCalculator.Models; /* Permite enxergar a classe Pessoa */
using BodyCalculator.DTOs.Responses; /* Permite enxergar as Responses */


// Namespace
namespace BodyCalculator.Services;


// Classe
public static class CalculadoraIngestaoAgua
{
    // Métodos

    // Calcular a quantidade de água para ingestão no dia
    public static IngestaoAguaResponse CalcularIngestaoAgua(Pessoa pessoa) /* Função pura */
    {
        // O underline "_" significa "default", ou seja, se não for nenhuma das de cima, usa o padrão de 35 ml por */ / O switch avalia a idade e retorna a quantidade de ml por kg */
        int mlPorKg = pessoa.Idade switch /* Nova forma de fazer o switch */
        {
            < 0             => throw new ArgumentException("Idade inválida."), /* Tolerância a falhas */
            <= 17           => 40, /* 17 anos ou menos */
            >= 56 and <= 65 => 30, /* 56 a 65 anos */
            >= 66           => 25, /* 66 anos ou mais */
            _               => 35 /* Padrão: 18 a 55 anos (e qualquer valor não previsto) */
        };


        /* Para fins de informação */
        /* ≤17 anos → 40ml/kg */
        /* 18–55 anos → 35ml/kg */
        /* 56–65 anos → 30ml/kg */
        /* 66+ anos → 25ml/kg */

        /* Para fins de melhoria (futuramente, inicialmente pode começar como um balão informativo): */
        /* Atividade física intensa: adicionar 500 a 1.000 mL */
        /* Dias muito quentes: adicionar 300 a 700 mL */
        /* Gestantes: adicionar cerca de 300 mL */
        /* Lactantes: adicionar cerca de 700 mL */

        /* Existe alguma forma de refinar esse cálculo com base no percentualGordura? 
           Mais informações no comentário de IngestaoAguaRequest. Talvez tenha, precisa ser estudado */


        // Calcular quantidade de água em ml e L
        double totalMililitros = pessoa.Peso * mlPorKg;
        double totalLitros = totalMililitros / 1000.0;

        // Retorna o DTO estruturado com os resultados
        return new IngestaoAguaResponse
        {
            Idade = pessoa.Idade,
            Peso = pessoa.Peso,
            MultiplicadorUsado = mlPorKg,
            TotalMililitros = Math.Round(totalMililitros, 0),
            TotalLitros = Math.Round(totalLitros, 1)
        };
    }
}
