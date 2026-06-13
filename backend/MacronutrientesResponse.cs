// Classe
public class MacronutrientesResponse /* Classe de Transferência - DTO, Data Transfer Object | Baseado no fluxo de requisição HTTP, representa a response (resposta) de uma request (requisição) que o servidor vai devolver para o usuário */
{
    // Propriedades
    public double Proteina { get; set; } /* g */
    public double Gordura { get; set; } /* g */
    public double Carboidrato { get; set; } /* g */
    public double CaloriasAlvo { get; set; }  /* kcal */ /* GET com o déficit/superávit aplicado */ /* Ela também pertence a esta DTO pois não faz sentido exibir os macros sem as calorias alvo, fazem parte das calorias alvo */


    // Métodos
    public override string ToString() /* Método que permite imprimir a classe direto */
    {
        return $"""
            Calorias Alvo: {CaloriasAlvo} kcal
            Proteína: {Proteina} g
            Gordura: {Gordura} g
            Carboidrato: {Carboidrato} g
            """;
    }
}
