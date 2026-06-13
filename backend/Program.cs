// Namespaces
using System;


// Classe
class Program
{
    static void Main(string[] args)
    {
        // Instâncias

        // 1. O "Front-end" envia os dados da Pessoa
        /* Nome (nome de usuário) | Idade | Sexo | Peso | Altura | Fator de Atividade | Percentual de Gordura */
        Pessoa daniel = new Pessoa("Daniel", 20, "Masculino", 71, 176, 1.5, 8.83); 
        Pessoa amigao = new Pessoa("Amigo de Testes", 30, "Feminino", 90, 167, 1.4);
        Pessoa maclopes = new Pessoa("Meiry", 49, "Feminino", 68, 160, 1.5);

        // 2. O Backend chama as calculadoras estáticas e gera as DTOs (Caixas de dados)
        MetabolismoResponse respostaMetabolismo = CalculadoraMetabolismo.CalcularMetabolismo(maclopes, ObjetivoFisico.Emagrecimento);
        MacronutrientesResponse respostaMacronutrientes = CalculadoraMacronutrientes.CalcularMacronutrientes(maclopes, respostaMetabolismo.CaloriasAlvo, respostaMetabolismo.ObjetivoFisico);
        IngestaoAguaResponse respostaAgua = CalculadoraIngestaoAgua.CalcularIngestaoDiaria(maclopes);

        // 3. O Gerador de Relatório recebe as caixas (DTOs) prontas e formata a interface de texto
        string relatorioFinal = GeradorRelatorio.ImprimirCompleto(maclopes, respostaMetabolismo, respostaMacronutrientes, respostaAgua);
        Console.WriteLine(relatorioFinal);

        // Exibição das DTO utilizando do método ToString
        // Console.WriteLine(daniel);
        // System.Console.WriteLine();
        // Console.WriteLine(respostaMetabolismo);
        // System.Console.WriteLine();
        // Console.WriteLine(respostaMacronutrientes);
        // System.Console.WriteLine();
        // Console.WriteLine(respostaAgua);   
    }
}
