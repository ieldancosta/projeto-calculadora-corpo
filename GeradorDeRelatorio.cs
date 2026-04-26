// Namespaces
using System;
using System.Text;


// Classe
public class GeradorDeRelatorio
{
    private readonly Calculadora _calculadora; /* Campo privado */


    // Construtor
    public GeradorDeRelatorio(Calculadora calculadora)
    {
        _calculadora = calculadora;
    }


    // Métodos
    public string ImprimirCompleto(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        // Coleta de todos os dados usando a calculadora injetada
        double IMC = _calculadora.CalcularIMC(pessoa);
        string classificacaoImc = _calculadora.ClassificarIMC(IMC);

        double GER = _calculadora.CalcularGER(pessoa, formula);
        double ETA = _calculadora.CalcularETA(pessoa, formula);
        double GAF = _calculadora.CalcularGAF(pessoa, formula);
        double GET = _calculadora.CalcularGET(pessoa, formula);


        // Instâncias
        StringBuilder sb = new StringBuilder();


        // Construção do Relatório
        sb.AppendLine("-----------------------------------------");
        sb.AppendLine("      RELATÓRIO NUTRICIONAL COMPLETO      ");
        sb.AppendLine("|----------------------------------------|");
        sb.AppendLine($"   Nome: {pessoa.Nome}");
        sb.AppendLine($"   Idade: {pessoa.Idade} anos  | Sexo: {pessoa.Sexo}");
        sb.AppendLine($"   Peso: {pessoa.Peso:F1} kg   | Altura: {pessoa.Altura} cm");
        sb.AppendLine("----------------------------------------");

        sb.AppendLine($"[ COMPOSIÇÃO CORPORAL ]");
        sb.AppendLine($"   IMC: {IMC:F1} ({classificacaoImc})");


        // Exibe dados de gordura apenas se existirem na pessoa
        if (pessoa.PercentualGordura.HasValue)
        {
            sb.AppendLine($"   Percentual de Gordura: {pessoa.PercentualGordura}%");
            sb.AppendLine($"   Massa Livre de Gordura (MLG): {pessoa.MLG:F2} kg");
        }


        // Exibe o gasto energético
        sb.AppendLine("|----------------------------------------|");
        sb.AppendLine($"[ GASTO ENERGÉTICO ]");
        sb.AppendLine($"   Fórmula Base: {formula}");
        sb.AppendLine($"   Fator de Atividade: {pessoa.FatorAtividade}");
        sb.AppendLine();
        sb.AppendLine($"   Gasto Energético em Repouso (GER ou TMB): {GER} kcal p/dia");
        sb.AppendLine($"   Gasto de Atividade Física (GAF): {GAF} kcal p/dia");
        sb.AppendLine($"   Efeito Térmico dos Alimentos (ETA): {ETA} kcal p/dia");
        sb.AppendLine("..........................................");
        sb.AppendLine($"   GASTO ENERGÉTICO TOTAL (GET): {GET} kcal p/dia");
        sb.AppendLine("-----------------------------------------");

        // Converte o construtor de texto numa string final
        return sb.ToString();
    }

    public string ImprimirResumido(Pessoa pessoa, TipoFormula formula = TipoFormula.MifflinStJeor)
    {
        double IMC = _calculadora.CalcularIMC(pessoa);
        string classificacaoImc = _calculadora.ClassificarIMC(IMC);

        double GER = _calculadora.CalcularGER(pessoa, formula);
        double ETA = _calculadora.CalcularETA(pessoa, formula);
        double GAF = _calculadora.CalcularGAF(pessoa, formula);
        double GET = _calculadora.CalcularGET(pessoa, formula);


        // Instâncias
        StringBuilder sb = new StringBuilder();

        // Exibe o gasto energético
        sb.AppendLine("-----------------------------------------");
        sb.AppendLine("      RELATÓRIO NUTRICIONAL RESUMIDO      ");
        sb.AppendLine("|----------------------------------------|");
        sb.AppendLine($"   Nome: {pessoa.Nome}");
        sb.AppendLine("|----------------------------------------|");
        sb.AppendLine($"   IMC: {IMC:F1} ({classificacaoImc})");

        if (pessoa.PercentualGordura.HasValue)
        {
            sb.AppendLine($"   Percentual de Gordura: {pessoa.PercentualGordura}%");
            sb.AppendLine($"   Massa Livre de Gordura (MLG): {pessoa.MLG:F2} kg");
        }

        sb.AppendLine();
        sb.AppendLine($"   Fórmula Base: {formula}");
        sb.AppendLine($"   Fator de Atividade: {pessoa.FatorAtividade}");
        sb.AppendLine();
        sb.AppendLine($"   Gasto Energético em Repouso (GER ou TMB): {GER} kcal p/dia");
        sb.AppendLine($"   Gasto de Atividade Física (GAF): {GAF} kcal p/dia");
        sb.AppendLine($"   Efeito Térmico dos Alimentos (ETA): {ETA} kcal p/dia");
        sb.AppendLine("..........................................");
        sb.AppendLine($"   GASTO ENERGÉTICO TOTAL (GET): {GET} kcal p/dia");
        sb.AppendLine("-----------------------------------------");

        // Converte o construtor de texto numa string final
        return sb.ToString();
    }

    // Mostrar o valor de calorias semanal
    // Mostrar o déficit diário e semanal
    // Mostrar quantidade de macronutrientes
    // Mostrar refeições sugeridas para atingir (e personalizado)
}