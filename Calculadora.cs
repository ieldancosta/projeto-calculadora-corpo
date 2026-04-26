// Namespaces
using System;


// Classe
public class Calculadora
{
    // Métodos

    // Calcular o IMC (Índice de Massa Corporal)
    public double CalcularIMC(Pessoa pessoa)
    {
        if (pessoa.Altura <= 0 || pessoa.Peso <= 0) return 0; /* Validação de dados */

        double alturaMetros = pessoa.Altura / 100; /* Transformação de centímetros para metros */
        double IMC = pessoa.Peso / (alturaMetros * alturaMetros);
        return Math.Round(IMC, 1);
    }

    // Exibir a Classificação do IMC (Índice de Massa Corporal) pelo tipo primitivo
    public string ClassificarIMC(double IMC)
    {
        if (IMC < 18.5) return "Abaixo do peso";
        if (IMC < 25) return "Peso normal";
        if (IMC < 30) return "Sobrepeso";
        if (IMC < 35) return "Obesidade I";
        if (IMC < 40) return "Obesidade II (Severa)";
        return "Obesidade III (Mórbida)";
    }

    // Exibir a Classificação do IMC (Índice de Massa Corporal) pela classe Pessoa | Atalho
    public string ClassificarIMC(Pessoa pessoa)
    {
        double IMC = CalcularIMC(pessoa); 
        return ClassificarIMC(IMC);
    }

    // Calcular o GER (Gasto Energético em Repouso), também chamado de TMB (Taxa Metabólica Basal)
    public double CalcularGER(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        // O underline "_" significa "default", ou seja, se não for nenhuma das de cima, usa a Mifflin
        double GER = formula switch /* Nova forma de fazer o switch */
        {
            TipoFormula.HarrisBenedict => CalcularHarrisBenedict(pessoa),
            TipoFormula.Cunningham => CalcularCunningham(pessoa),
            TipoFormula.TinsleyP => CalcularTinsleyP(pessoa),
            TipoFormula.TinsleyMLG => CalcularTinsleyMLG(pessoa),
            _ => CalcularMifflinStJeor(pessoa)
        };

        return Math.Round(GER, 0);
    }

    // Calcular GER - Fórmula Mifflin-ST Jeor
    private double CalcularMifflinStJeor(Pessoa pessoa) /* Mais segura e recomendada */
    {
        double resultadoBase = (10 * pessoa.Peso) + (6.25 * pessoa.Altura) - (5 * pessoa.Idade);
        return pessoa.Sexo == "Masculino" ? resultadoBase + 5 : resultadoBase - 161; /* Separação de sexos */
    }

    // Calcular GER - Fórmula Harris Benedict
    private double CalcularHarrisBenedict(Pessoa pessoa) /* Recomendado para IMC < 30 */
    {
        if (pessoa.Sexo == "Masculino") /* Se for do sexo masculino */
        {
            return 66 + (13.8 * pessoa.Peso) + (5.0 * pessoa.Altura) - (6.8 * pessoa.Idade);
        }
        else /* Se for do sexo feminino */
        {
            return 655 + (9.6 * pessoa.Peso) + (1.9 * pessoa.Altura) - (4.7 * pessoa.Idade);
        }
    }

    // Calcular GER - Fórmula Cunningham
    private double CalcularCunningham(Pessoa pessoa) /* Obesos ou metabolismo lento */
    {
        if (pessoa.MLG == null)
        {
            // Se estiver vazia, paramos tudo e avisamos o erro (afinal, não tem como calcular com MLG = null)
            throw new InvalidOperationException("Não foi possível realizar a operação, pois o percentual de gordura não foi informado.");
        }

        return (22 * pessoa.MLG.Value) + 500;
    }

    // Calcular GER - Fórmula Tinsley Peso
    private double CalcularTinsleyP(Pessoa pessoa) /* Fisiculturistas; físico atlético */
    {
        return 24.8 * pessoa.Peso + 10;
    }

    // Calcular GER - Fórmula Tinsley MLG
    private double CalcularTinsleyMLG(Pessoa pessoa) /* Fisiculturistas; físico atlético (mais utilizada) */
    {
        if (pessoa.MLG == null)
        {
            // Se estiver vazia, paramos tudo e avisamos o erro (afinal, não tem como calcular com MLG = null)
            throw new InvalidOperationException("Não foi possível realizar a operação, pois o percentual de gordura não foi informado.");
        }

        return 25.9 * pessoa.MLG.Value + 284;
    }

    // Calcular o ETA (Efeito Térmico dos Alimentos)
    public double CalcularETA(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        double GER = CalcularGER(pessoa, formula); /* Repouso */
        double GAF = CalcularGAF(pessoa, formula); /* Atividade + NEAT */

        double ETA = (GER + GAF) * 0.10; /* Ou dividir por 10 | Obter 10% */
        return Math.Round(ETA, 0);
    }

    // Calcular o GAF (Gasto de Atividade Física)
    public double CalcularGAF(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor) /* O NEAT deve ser levado em conta também na hora de escolher o fator de atividade */
    {
        if (pessoa.FatorAtividade < 1) return 0; /* Validação de dados | Fator de atividade não pode ser menor que 1 */ /* Deixei como anotação, deve ser removido e feito a validação na aplicação Web */

        double GER = CalcularGER(pessoa, formula);

        double GAF = GER * (pessoa.FatorAtividade - 1); /* Outra forma de calcular é: pessoa.FatorAtividade * GER - GER */
        return Math.Round(GAF, 0);
    }

    // Calcular o NEAT (Termogênese Não Relacionada ao Exercício Físico)
    public double CalcularNEAT() /* Como o NEAT já está sendo levado em consideração no cálculo do GAF, não é necessário */ /* Futuramente, penso em remover este método */
    {
        double NEAT = 0;
        return NEAT;
    }

    // Calcular o GET (Gasto Energético Total)
    public double CalcularGET(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        double GER = CalcularGER(pessoa, formula);
        double ETA = CalcularETA(pessoa, formula);
        double GAF = CalcularGAF(pessoa, formula);
        double NEAT = CalcularNEAT();

        double GET = GER + ETA + GAF + NEAT;
        return Math.Round(GET, 0);
    }

    // Solucionar Double Counting
}


// Enumeração para os Tipos de Fórmula - GER
public enum TipoFormula
{
    MifflinStJeor,
    HarrisBenedict,
    Cunningham,
    TinsleyP,
    TinsleyMLG
}