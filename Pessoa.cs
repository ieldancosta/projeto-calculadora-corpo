// Namespaces
using System;


// Classe
public class Pessoa
{
    // Propriedades
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Sexo { get; set; }
    public double Peso { get; set; }
    public double Altura { get; set; }


    // Construtor
    public Pessoa(string nome, int idade, string sexo, double peso, double altura)
    {
        this.Nome = nome;
        this.Idade = idade;
        this.Sexo = sexo;
        this.Peso = peso;
        this.Altura = altura;
    }


    // Métodos
    public override string ToString() /* Método que permite imprimir a classe direto */
    {
        return $"Nome: {Nome}\nIdade: {Idade}\nSexo: {Sexo}\nPeso: {Peso}\nAltura: {Altura}";
    }
}