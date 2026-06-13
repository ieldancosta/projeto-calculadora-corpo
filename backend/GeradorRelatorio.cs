// Namespaces
using System.Text;


// Classe
public static class GeradorRelatorio
{
    // Métodos
    
    public static string ImprimirCompleto(Pessoa pessoa, MetabolismoResponse? metabolismo = null, MacronutrientesResponse? macronutrientes = null, IngestaoAguaResponse? ingestaoAgua = null)
    {
        // Instâncias
        StringBuilder sb = new StringBuilder();
        

        // Geração de relatório, você pode omitir campos e utilizar a "Named Syntax" no C# para passar o paramêtro específico e retornar apenas ele; Ou passar null no lugar dos campos que não quiser

        // Bloco 1 - Identificação
        sb.AppendLine($"""
        ==================================================
                  RELATÓRIO NUTRICIONAL COMPLETO      
        ==================================================
           Nome:  {pessoa.Nome}
           Idade: {pessoa.Idade} anos   | Sexo:    {pessoa.Sexo}
           Peso:  {pessoa.Peso:F1} kg   | Altura:  {pessoa.Altura} cm
        --------------------------------------------------
        """);


        // Bloco 2 - Metabolismo
        if (metabolismo != null)
        {   
            // Bloco 2 - Composição Corporal
            sb.AppendLine($"""
                [ COMPOSIÇÃO CORPORAL ]
                   IMC: {metabolismo.IMC:F1} ({metabolismo.ClassificacaoIMC})
                """);

            /* Exibe dados de gordura apenas se existirem na pessoa */
            if (pessoa.PercentualGordura.HasValue)
            {
                sb.AppendLine($"""
                   Percentual de Gordura:        {pessoa.PercentualGordura}%
                   Massa Livre de Gordura (MLG): {pessoa.MLG:F2} kg
                """);
            }


            // Bloco 3 - Gasto Energético (Metabolismo)
            sb.AppendLine($"""
                --------------------------------------------------
                [ GASTO ENERGÉTICO ]
                   Fórmula Base Utilizada: {metabolismo.FormulaUsada}
                   Fator de Atividade:     {pessoa.FatorAtividade}
                   
                   Gasto Energético de Repouso (GER ou TMB): {metabolismo.GER} kcal p/dia
                   Gasto com Atividade Física (GAF):   {metabolismo.GAF} kcal p/dia
                   Efeito Térmico dos Alimentos (ETA): {metabolismo.ETA} kcal p/dia
                   ..................................................
                   Gasto Energético Total (GET):       {metabolismo.GET} kcal p/dia
                --------------------------------------------------
                """);
        }
        

        // Bloco 4 - Planejamento de Macronutrientes (segurança contra nulos)
        if (macronutrientes != null)
        {
            sb.AppendLine($"""
                [ PLANEJAMENTO DIETÉTICO ]
                   Objetivo Físico:      {metabolismo?.ObjetivoFisico.ToString() ?? "Não informado"}
                   Alvo Calórico Diário: {macronutrientes.CaloriasAlvo} kcal p/dia                  
                   
                   Distribuição de Macronutrientes:
                   • Proteína:    {macronutrientes.Proteina:F1} g  ({macronutrientes.Proteina * 4:F0} kcal)
                   • Gordura:     {macronutrientes.Gordura:F1} g  ({macronutrientes.Gordura * 9:F0} kcal)
                   • Carboidrato: {macronutrientes.Carboidrato:F1} g  ({macronutrientes.Carboidrato * 4:F0} kcal)
                --------------------------------------------------
                """);
        }


        // Bloco 5 - Planejamento Hídrico (Segurança contra nulos)
        if (ingestaoAgua != null)
        {
            sb.AppendLine($"""
                [ METAS DE HIDRATAÇÃO ]
                   Multiplicador Aplicado: {ingestaoAgua.MultiplicadorUsado} mL por kg
                   
                   Recomendação Diária:    {ingestaoAgua.TotalLitros} L  ({ingestaoAgua.TotalMililitros} mL)
                """);
        }


        // Bloco 6 - Rodapé do relatório
        sb.AppendLine($"""
            ==================================================
                             FIM DO RELATÓRIO               
            ==================================================
            """);


        // Converte o construtor de texto numa string final
        return sb.ToString();
    }


    // Para fins de melhoria:
    // Mostrar o valor de calorias semanal também
    // Mostrar o déficit diário e semanal
    // Mostrar quantidade de macronutrientes
    // Mostrar refeições sugeridas para atingir (de forma personalizado)
}
