// Namespaces
using System;
using System.Collections.Generic;
using System.Linq;


// Classe
class Program
{
    static void Main(string[] args)
    {
        // Instâncias
        Pessoa pessoa1 = new Pessoa("Daniel", 20, "Masculino", 71, 176, 1.5, 8.83); /* Nome | Idade | Sexo | Peso | Altura | Fator de Atividade | Percentual de Gordura */
        Pessoa pessoa2 = new Pessoa("Amigo de Testes", 30, "Feminino", 69, 160, 1.4);

        Calculadora calculadora = new Calculadora();


        // Execução
        Console.WriteLine(pessoa1);
        Console.WriteLine();

        Console.WriteLine($"Seu GET (Gasto Energético Total): {calculadora.CalcularGET(pessoa1)}");
        Console.WriteLine();

        Console.WriteLine(pessoa2);
        Console.WriteLine();

        Console.WriteLine($"Seu GET (Gasto Energético Total): {calculadora.CalcularGET(pessoa2)}");
    }
}