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
        Pessoa pessoa2 = new Pessoa("Amigo de Testes", 30, "Feminino", 90, 167, 1.4);
        Pessoa pessoa3 = new Pessoa("Meiry", 49, "Feminino", 69, 160, 1.5);

        Calculadora calculadora = new Calculadora();
        GeradorDeRelatorio relatorio = new GeradorDeRelatorio(calculadora);


        // Execução
        Console.WriteLine(relatorio.ImprimirCompleto(pessoa3));
        Console.WriteLine();
    }
}