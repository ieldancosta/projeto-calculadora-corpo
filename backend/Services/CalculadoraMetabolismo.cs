// Importações / Dependências
using System.Text;
using BodyCalculator.Models; /* Permite enxergar a classe Pessoa */
using BodyCalculator.DTOs.Responses; /* Permite enxergar as Responses */
using BodyCalculator.Enums; /* Permite enxergar as Enums */


// Namespace
namespace BodyCalculator.Services;


// Classe
public class CalculadoraMetabolismo /* Antiga CalculadoraMetabolica, o início de tudo! */
{
    // Métodos

    /* Métodos públicos de cálculo direto (acesso externo) */

    // Calcular o IMC (Índice de Massa Corporal)
    public static double CalcularIMC(Pessoa pessoa)
    {
        if (pessoa.Altura <= 0 || pessoa.Peso <= 0) return 0; /* Validação de dados */

        double alturaMetros = pessoa.Altura / 100; /* Transformação de centímetros para metros */
        double IMC = pessoa.Peso / (alturaMetros * alturaMetros);
        return Math.Round(IMC, 1);
    }

    // Exibir a Classificação do IMC (Índice de Massa Corporal) pelo tipo primitivo
    public static string ClassificarIMC(double IMC)
    {
        if (IMC < 18.5) return "Abaixo do peso";
        if (IMC < 25) return "Peso normal";
        if (IMC < 30) return "Sobrepeso";
        if (IMC < 35) return "Obesidade I";
        if (IMC < 40) return "Obesidade II (Severa)";
        return "Obesidade III (Mórbida)";
    }

    // Exibir a Classificação do IMC (Índice de Massa Corporal) pela classe Pessoa | Atalho
    public static string ClassificarIMC(Pessoa pessoa)
    {
        double IMC = CalcularIMC(pessoa);
        return ClassificarIMC(IMC); /* Outra forma de fazer seria fazer direto ClassificarIMC(CalcularIMC(pessoa)), mas não é recomendado! Tem muitos pontos negativos e não muda nada em performance */
    }


    /* Método principal para carregar todos os dados no DTO (processamento Tudo-em-um para a UI) */

    public static MetabolismoResponse CalcularMetabolismo(Pessoa pessoa, ObjetivoFisico objetivo, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        double IMC = CalcularIMC(pessoa);
        string classificacaoIMC = ClassificarIMC(IMC);
        
        double GER = CalcularGER(pessoa, formula);
        double GET = CalcularGET(pessoa, GER); // Usa a sobrecarga que aceita o GER calculado
        double ETA = CalcularETA(GET); // Usa a sobrecarga que aceita o GET calculado
        double GAF = CalcularGAF(pessoa, GER, ETA); // Usa a sobrecarga que aceita GER e ETA calculados

        double caloriasAlvo = CalcularAlvoCalorico(pessoa, GET, objetivo);

        return new MetabolismoResponse
        {
            IMC = IMC, /* Já foi arredondado em seu método */
            ClassificacaoIMC = classificacaoIMC,
            FormulaUsada = formula,
            GER = Math.Round(GER, 0),
            GET = Math.Round(GET, 0),
            ETA = Math.Round(ETA, 0),
            GAF = Math.Round(GAF, 0),
            CaloriasAlvo = Math.Round(caloriasAlvo, 0),
            ObjetivoFisico = objetivo
        };
    }


    /* Métodos internos voltados (especificamente) para o DTO */ /* Mantidos estáticos para uso pela função principal */

    // Calcular o GER (Gasto Energético em Repouso), também chamado de TMB (Taxa Metabólica Basal)
    private static double CalcularGER(Pessoa pessoa, TipoFormula formula)
    {
        // O underline "_" significa "default", ou seja, se não for nenhuma das de cima, usa a Mifflin | Redundância intencional, defesa em profundidade
        double GER = formula switch /* Nova forma de fazer o switch */
        {
            TipoFormula.HarrisBenedict => CalcularHarrisBenedict(pessoa),
            TipoFormula.Cunningham => CalcularCunningham(pessoa),
            TipoFormula.TinsleyP => CalcularTinsleyP(pessoa),
            TipoFormula.TinsleyMLG => CalcularTinsleyMLG(pessoa),
            _ => CalcularMifflinStJeor(pessoa)
        };

        return GER; /* Deixamos para arrendondar no retorno para o DTO */
    }

    private static double CalcularGET(Pessoa pessoa, double GER)
    {
        if (pessoa.FatorAtividade < 1.2) throw new ArgumentException("Fator de atividade inválido. Deve conter um valor maior que 1,1."); /* Validação de dados | Fator de atividade não pode ser menor que 1 */ /* Defesa em profundidade */

        double GET = GER * pessoa.FatorAtividade; /* Fórmula universal: GET = GER + ETA + GAF + NEAT */ /* Outra forma: GET = GER + GAF */
        return GET; /* Deixamos para arrendondar no retorno para o DTO */
    }
    
    // Calcular o ETA (Efeito Térmico dos Alimentos)
    private static double CalcularETA(double GET)
    {
        /* GET inclui Atividade + NEAT + ETA */

        double ETA = GET * 0.10; /* Ou dividir por 10 | Obter 10% */ /* Outra forma: ETA = (GER + GAF) * 0.10;  | ETA = (GER * Fator) * 0.10; */
        return ETA; /* Deixamos para arrendondar no retorno para o DTO */
    }

    // Calcular o NEAT (Termogênese Não Relacionada ao Exercício Físico) | Vou deixar aqui por enquanto, mesmo que não vamos utilizar, para fins de documentação temporariamente e curiosidade
    // public double CalcularNEAT() /* Como o NEAT já está sendo levado em consideração no cálculo do GAF, não é necessário */ /* Futuramente, penso em remover este método */
    // {
        // double NEAT = 0; /* É algo muito difícil de calcular (principalmente pois depende!), e acredito que nem seja recomendado por não interferir em nada ter esse dado isolado | Por hora, não irei calcular e acho que não iremos usar */
        // return NEAT;
    // } 

    // Calcular o GAF (Gasto de Atividade Física)
    private static double CalcularGAF(Pessoa pessoa, double GER, double ETA) /* Pode ser chamado de AEE também, o que envolve, na nomenclatura, tanto o exercício (intencional/estruturado/repetitivo) quanto a atividade (não estruturado); O bloco de movimento */ /* O NEAT deve ser levado em conta também na hora de escolher o fator de atividade */
    {
        if (pessoa.FatorAtividade < 1.2) throw new ArgumentException("Fator de atividade inválido. Deve conter um valor maior que 1,1."); /* Validação de dados | Fator de atividade não pode ser menor ou igual a 1 (atualização: pelo menos 1.2) */ /* Defesa em profundidade */

        double GAF = GER * (pessoa.FatorAtividade - 1) - ETA; /* Outra forma de calcular é: pessoa.FatorAtividade * GER - GER | Outra forma: GAF = GET - GER - ETA */ /* GAF já inclui NEAT e ETA */
        return GAF; /* GAF = EAT + NEAT -> EAT (Exercise Activity Thermogenesis) é o exercício intencional/estruturado; O NEAT (Non-Exercise) é a atividade não estruturada */  /* Deixamos para arrendondar no retorno para o DTO */
    }

    // Calcular o alvo calórico com base no objetivo
    private static double CalcularAlvoCalorico(Pessoa pessoa, double GET, ObjetivoFisico objetivoFisico)
    {
        switch (objetivoFisico)
        {
            case ObjetivoFisico.Emagrecimento:
                double deficitDiario = (pessoa.Peso * 0.005 * 7700) / 7; /* O 7 no final divide o resultado ideal para cada dia */
                return GET - deficitDiario;

                /* Para fins de informação: */
                /* Cálculo usando porcentagem (mais seguro e recomendado) */
                /* Exemplo para entendermos: */
                /* 1 kg  de gordura do nosso organismo tem armazenado 7.700 kcal */

                // Nome: Plínio
                // Peso: 70 kg
                /* Vamos achar 0,5% de 70 kg */
                /* O resultado é: 0,35 kg | 350 g */
                /* O resultado encontrado equivale a quanto vamos perder por semana do nosso peso */

                /* O próximo passo é encontrar quantas calorias equivalem a essas 0,35 kg */
                /* Para isso vamos usar regra de 3 */

                /* 7700 kcal | 1 kg           */
                /* x         | 0,35 kg (0,5%) */
                /* Resultado: x = 2695 kcal   */

                /* Esse valor de calorias encontrado é o quanto precisamos diminuir do nosso gasto energético semanal */
                /* O GET semanal do Plínio é de 16.401 kcal */ /* Curiosidade: GET diário de 2343 kcal */
                /* 16.401 kcal - 2696 = 13.705 */
                /* O resultado encontrado para ser consumido na semana é obtido pelo cálculo 13.705 / 7 */
                /* Consumo diário para alcançar essa meta: 1.958 kcal */

            case ObjetivoFisico.GanhoDeMassa:
                return GET + (GET * 0.15);
                
                /* Para fins de informação: */
                /* Quanto de superávit fazer? */

                /* Magro de ruim: 15-40% */
                /* BF Baixo: 10-15% */
                /* Atleta com muita margem: ~15% */
                /* Atrela sem muita margem: ~10% */

                /* Conforme os anos de treino vão passando, menos treinável o seu corpo fica e, portanto, você ganha menos massa. Isso que significa estar com margem ou não */
                /* A margem evolutiva de alguém é a chave que dita quanto de superávit ele pode fazer */

            case ObjetivoFisico.Manutencao:
            default:
                return GET;
        }
    }


    /* Métodos específicos (privados) */

    // Calcular GER - Fórmula Mifflin-ST Jeor
    private static double CalcularMifflinStJeor(Pessoa pessoa) /* Mais segura e recomendada */ /* Padrão selecionada */
    {
        double resultadoBase = (10 * pessoa.Peso) + (6.25 * pessoa.Altura) - (5 * pessoa.Idade);
        return pessoa.Sexo == "Masculino" ? resultadoBase + 5 : resultadoBase - 161; /* Separação de sexos */
    }

    // Calcular GER - Fórmula Harris Benedict
    private static double CalcularHarrisBenedict(Pessoa pessoa) /* Recomendado para IMC < 30 */ /* Foi definido os valores não arredondados para maior precisão, de 1919 */
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
    private static double CalcularCunningham(Pessoa pessoa) /* Obesos ou metabolismo lento */
    {
        // Se estiver vazia, paramos tudo e avisamos o erro (afinal, não tem como calcular com MLG = null)
        if (pessoa.MLG == null) throw new InvalidOperationException("Não foi possível realizar a operação, pois o percentual de gordura não foi informado.");

        return (22 * pessoa.MLG.Value) + 500;
    }

    // Calcular GER - Fórmula Tinsley Peso
    private static double CalcularTinsleyP(Pessoa pessoa) /* Fisiculturistas; físico atlético */
    {
        return 24.8 * pessoa.Peso + 10;
    }

    // Calcular GER - Fórmula Tinsley MLG
    private static double CalcularTinsleyMLG(Pessoa pessoa) /* Fisiculturistas; físico atlético (mais utilizada nesse cenário) */
    {
        // Se estiver vazia, paramos tudo e avisamos o erro (afinal, não tem como calcular com MLG = null)
        if (pessoa.MLG == null) throw new InvalidOperationException("Não foi possível realizar a operação, pois o percentual de gordura não foi informado.");

        return 25.9 * pessoa.MLG.Value + 284;
    }
}

