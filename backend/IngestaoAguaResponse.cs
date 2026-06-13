// Classe
public class IngestaoAguaResponse /* Classe de Transferência - DTO, Data Transfer Object | Baseado no fluxo de requisição HTTP, representa a response (resposta) de uma request (requisição) que o servidor vai devolver para o usuário */
{
    // Propriedades
    public int Idade { get; set; } /* anos */
    public double Peso { get; set; } /* kg */
    public int MultiplicadorUsado { get; set; } /* mL por Kg */
    public double TotalMililitros { get; set; } /* mL */
    public double TotalLitros { get; set; } /* litros (L) */


    // Métodos
    public override string ToString() /* Método que permite imprimir a classe direto */
    {
        return $"""
            Total em litros: {TotalLitros} L
            Total em mL: {TotalMililitros} mL
            Multiplicador usado: {MultiplicadorUsado} mL por Kg
            Idade: {Idade} anos
            Peso: {Peso} kg
            """;
    }
}
