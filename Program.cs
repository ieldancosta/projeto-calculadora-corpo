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
        Pessoa pessoa1 = new Pessoa("Daniel", 20, "Masculino", 70, 176);
        Pessoa pessoa2 = new Pessoa("Matheus", 23, "Masculino", 40, 150);
        Pessoa pessoa3 = new Pessoa("Stanley", 20, "Masculino", 80, 170);

        Calculadora calculadora = new Calculadora();

        
        // Execução
        Console.WriteLine(pessoa1);
        Console.WriteLine();
        Console.WriteLine(pessoa2);
        Console.WriteLine();
        Console.WriteLine(pessoa3);
        Console.WriteLine();
        
        double resultado = calculadora.CalcularIMC(pessoa1);
        string status = calculadora.ClassificarIMC(resultado);
        Console.WriteLine(status);
    }
}