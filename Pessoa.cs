// Namespaces
using System;


// Classe
public class Pessoa
{
    // Propriedades
    public string Nome { get; set; } 
    public int Idade { get; set; } /* Anos */
    public string Sexo { get; set; }
    public double Peso { get; set; } /* Kg */
    public double Altura { get; set; } /* cm */
    public double FatorAtividade { get; set; } /* Não só o GAF, mas também o NEAT deve ser levado em conta na hora de escolher o fator de atividade */
    public double? PercentualGordura { get; set; } /* % */
    public double? MLG /* Kg | Massa Livre de Gordura */
    {
        get 
        {
            if (PercentualGordura == null) return null; /* Se não informou o percentual de gordura, impossível calcular a MLG (Massa Livre de Gordura) */
            return Peso * (1 - (PercentualGordura.Value / 100));
        }
    }


    // Construtores

    // Construtor 1 | Sem informar o percentual de gordura
    public Pessoa(string nome, int idade, string sexo, double peso, double altura, double fatorAtividade)
    {
        this.Nome = nome;
        this.Idade = idade;
        this.Sexo = sexo;
        this.Peso = peso;
        this.Altura = altura;
        this.FatorAtividade = fatorAtividade;
    }

    // Construtor 2 | Informando o percentual de gordura
    public Pessoa(string nome, int idade, string sexo, double peso, double altura, double fatorAtividade, double percentualGordura)
    {
        this.Nome = nome;
        this.Idade = idade;
        this.Sexo = sexo;
        this.Peso = peso;
        this.Altura = altura;
        this.FatorAtividade = fatorAtividade;
        this.PercentualGordura = percentualGordura;
    }


    // Métodos
    public override string ToString() /* Método que permite imprimir a classe direto */
    {
        // Caso a porcentagem de gordura tenha sido informada, será exibida junto com a massa livre de gordura
        if (PercentualGordura.HasValue) return $"Nome: {Nome}\nIdade: {Idade} anos\nSexo: {Sexo}\nPeso: {Peso:F1} Kg\nAltura: {Altura} cm\nFator de Atividade: {FatorAtividade}\nPercentual de gordura: {PercentualGordura}\nMassa livre de gordura: {MLG:F2} Kg";

        // Informações básicas que sempre existem
        return $"Nome: {Nome}\nIdade: {Idade} anos\nSexo: {Sexo}\nPeso: {Peso} Kg\nAltura: {Altura} cm\nFator de Atividade: {FatorAtividade}";
    }
}