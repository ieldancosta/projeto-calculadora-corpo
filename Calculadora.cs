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
    private double CalcularMifflinStJeor(Pessoa pessoa) /* Mais segura e recomendada */ /* Padrão selecionada */
    {
        double resultadoBase = (10 * pessoa.Peso) + (6.25 * pessoa.Altura) - (5 * pessoa.Idade);
        return pessoa.Sexo == "Masculino" ? resultadoBase + 5 : resultadoBase - 161; /* Separação de sexos */
    }

    // Calcular GER - Fórmula Harris Benedict
    private double CalcularHarrisBenedict(Pessoa pessoa) /* Recomendado para IMC < 30 */ /* Foi definido os valores não arredondados para maior precisão, de 1919 */
    {
        if (pessoa.Sexo == "Masculino") /* Se for do sexo masculino */
        {
            return 66.5 /* 1919 | 1994: 66 */ + (13.75 /* 1919 | 1994: 13.8 */ * pessoa.Peso) + (5.003 /* 1919 | 1994: 5.0 */ * pessoa.Altura) - (6.75 /* 1919 | 1994 - Arredondamento: 6.8 */ * pessoa.Idade);
        }
        else /* Se for do sexo feminino */
        {
            return 655.1 /* 1919 | 1994: 655 */ + (9.563 /* 1919 | 1994: 9.6 */ * pessoa.Peso) + (1.850 /* 1919 | 1994: 1.8 */ * pessoa.Altura) - (4.676 /* 1919 | 1994 - Arredondamento: 5.0 */ * pessoa.Idade);
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
    private double CalcularTinsleyMLG(Pessoa pessoa) /* Fisiculturistas; físico atlético (mais utilizada nesse cenário) */
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
        double GET = CalcularGET(pessoa, formula); /* Atividade + NEAT + ETA */

        double ETA = GET * 0.10; /* Ou dividir por 10 | Obter 10% */ /* Outra forma: ETA = (GER + GAF) * 0.10;  | ETA = (GER * Fator) * 0.10; */
        return Math.Round(ETA, 0);
    }

    // Calcular o GAF (Gasto de Atividade Física)
    public double CalcularGAF(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor) /* Pode ser chamado de AEE também, o que envolve, na nomenclatura, tanto o exercício (intencional/estruturado/repetitivo) quanto a atividade (não estruturado); O bloco de movimento */ /* O NEAT deve ser levado em conta também na hora de escolher o fator de atividade */
    {
        if (pessoa.FatorAtividade < 1.2) return 0; /* Validação de dados | Fator de atividade não pode ser menor que 1 */ /* Deixei como anotação, deve ser removido e feito a validação na aplicação Web */

        double GER = CalcularGER(pessoa, formula);
        double ETA = CalcularETA(pessoa, formula);

        double GAF = GER * (pessoa.FatorAtividade - 1) - ETA; /* Outra forma de calcular é: pessoa.FatorAtividade * GER - GER | Outra forma: GAF = GET - GER - ETA */ /* GAF já inclui NEAT e ETA */
        return Math.Round(GAF, 0); /* GAF = EAT + NEAT -> EAT (Exercise Activity Thermogenesis) é o exercício intencional/estruturado; O NEAT (Non-Exercise) é a atividade não estruturada  */
    }

    // Calcular o NEAT (Termogênese Não Relacionada ao Exercício Físico)
    public double CalcularNEAT() /* Como o NEAT já está sendo levado em consideração no cálculo do GAF, não é necessário */ /* Futuramente, penso em remover este método */
    {
        double NEAT = 0; /* É algo muito difícil de calcular (principalmente pois depende!), e acredito que nem seja recomendado por não interferir em nada ter esse dado isolado | Por hora, não irei calcular e acho que não iremos usar */
        return NEAT;
    }

    // Calcular o GET (Gasto Energético Total)
    public double CalcularGET(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        if (pessoa.FatorAtividade < 1.2) return 0; /* Validação de dados | Fator de atividade não pode ser menor que 1 */ /* Deixei como anotação, deve ser removido e feito a validação na aplicação Web */

        double GER = CalcularGER(pessoa, formula);

        double GET = GER * pessoa.FatorAtividade; /* Fórmula universal: GET = GER + ETA + GAF + NEAT */ /* Outra forma: GET = GER + GAF */
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