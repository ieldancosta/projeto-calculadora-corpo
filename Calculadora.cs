// Namespaces
using System;


// Classe
public class Calculadora
{
    // Métodos

    // Calcular o IMC (Índice de Massa Corporal)
    public double CalcularIMC(Pessoa pessoa)
    {
        if (pessoa.Altura <= 0 || pessoa.Peso <= 0)
        {
            return 0;  
        }

        double alturaEmMetros = pessoa.Altura / 100; /* Transformação de centímetros para metros */
        double IMC = pessoa.Peso / (alturaEmMetros * alturaEmMetros);

        return Math.Round(IMC, 2);
    }

    // Exibir a Classificação do IMC (Índice de Massa Corporal)
    public string ClassificarIMC(double IMC)
    {
        if (IMC < 18.5) return "Abaixo do peso";
        if (IMC < 25) return "Peso normal";
        if (IMC < 30) return "Sobrepeso";
        return "Obesidade";
    }
    


    // Calcular o GER (Gasto Energético em Repouso), também chamado de TMB (Taxa Metabólica Basal)
    public double CalcularGER()
    {
        double GER = 0;
        return GER;
    }

    // Calcular o ETA (Efeito Térmico dos Alimentos)
    public double CalcularETA()
    {
        double ETA = 0;
        return ETA;
    }

    // Calcular o GAF (Gasto de Atividade Física)
    public double CalcularGAF(double fatorAtividade, double GER)
    {
        double GAF = fatorAtividade * GER;
        return GAF;
    }

    // Calcular o NEAT (Termogênese Não Relacionada ao Exercício Físico)
    public double CalcularNEAT()
    {
        double NEAT = 0;
        return NEAT;
    }

    // Calcular o GET (Gasto Energético Total)
    public double CalcularGET(double GER, double ETA, double GAF, double NEAT)
    {
        double GET = GER + ETA + GAF + NEAT;
        return GET;
    }
}