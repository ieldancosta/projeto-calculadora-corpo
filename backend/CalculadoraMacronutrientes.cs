// Namespaces
using System;


// Classe
public static class CalculadoraMacronutrientes
{
    // Métodos

    /* Anotações (para talvez futuras implementações e melhorias): */
    /* Naturalmente a quantidade de gordura é atingida */
    /* Fibras são tipos de carboidratos */
    /* Padrão de fibras: A OMS sugere algo em torno de 14g de fibra a cada 1000 kcal consumidas, ou um fixo de 25g a 30g por dia para um adulto saudável. Se você não bate isso, a digestão fica travada e a fome volta muito rápido. */

    public static MacronutrientesResponse CalcularMacronutrientes(Pessoa pessoa, double caloriasAlvo, ObjetivoFisico objetivo)  /* Função pura */
    {
        double percentualGordura = 0.15; /* Valor padrão de segurança | O percentual na qual vamos pegar do GET vai ser fixo, de 15% | Calculamos a gordura pegando uma % da ingestão total do usuário */
        
        /* Talvez futuramente estudar sobre a quantidade de proteína em diferentes objetivos e cenários, talvez até analisar percentual de gordura! Para futuras melhorias */
        double proteinaGramaPorKg = objetivo switch /* Nova forma de fazer o switch */
        {
            ObjetivoFisico.Emagrecimento => 2.0, /* 2,3 até 3,1 g por Kg */
            ObjetivoFisico.GanhoDeMassa => 1.8, /* 1,6 até 2,2 g por Kg */
            _ => 1.6 /* Valor padrão de segurança | Além disso, o valor padrão para o objetivo manutenção */
        };

        // Calcular a quantidade de proteínas (g)
        double proteinaGramas = pessoa.Peso * proteinaGramaPorKg;

        // Calcular a quantidade de gordura (g)
        double caloriasGordura = caloriasAlvo * percentualGordura;
        double gorduraGramas = caloriasGordura / 9.0; /* 1 g de gordura equivalem a 9 kcal */

        // Calcular a quantidade de carboidrato (g)
        double caloriasProteina = proteinaGramas * 4.0; /* 1 g de proteína equivalem a 4 kcal */
        double caloriasGastas = caloriasProteina + caloriasGordura;
        double carboidratoGramas = (caloriasAlvo - caloriasGastas) / 4.0; /* 1 g de carboidrato equivalem a 4 kcal */
        if (carboidratoGramas < 0) carboidratoGramas = 0; /* Validação de dados */
        

        /* Para fins de informação: */
        /* 1 g de proteína = 4 calorias */
        /* 1 g de gordura = 9 calorias */
        /* 1 g de carboidrato = 4 calorias */


        // Retornar o DTO preenchido e arredondado
        return new MacronutrientesResponse
        {
            Proteina = Math.Round(proteinaGramas, 1),
            Gordura = Math.Round(gorduraGramas, 1),
            Carboidrato = Math.Round(carboidratoGramas, 1),
            CaloriasAlvo = Math.Round(caloriasAlvo, 0)
        };



        // Mais informações abaixo sobre GORDURA

        /* Recomendações - BULKING */

        /* Optar por mufas e pufas (insaturadas) */
        /* Restringir saturadas a 10% da sua ingestão (ou seja, 10% dos 10 a 20% ⬇️) */

        /* Ingestão de 10 a 20% da dieta de gordura (geralmente fecha no 15% o limite inferior, 10 pode acabar sendo muito baixo) */

        /* ------ */

        /* Recomendações - CUTTING */

        /* Optar por mufas e pufas (insaturadas) */
        /* Restringir saturadas a 10% da sua ingestão (ou seja, 10% dos 10 a 20% ⬇️) */

        /* Ingestão de 10 a 20% da dieta de gordura (geralmente fecha no 15% o limite inferior, 10 pode acabar sendo muito baixo, opte por 13 a 16%) */

        /* ------ */

        /* Tem baixo grau de saciedade */
        /* Alta densidade calórica */
        /* Rouba espaço do carboidratos, que estoca glicôgenio e tem papel primordial no treino */
    }
}
