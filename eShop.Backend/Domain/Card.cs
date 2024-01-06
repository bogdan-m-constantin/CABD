namespace eShop.Backend.Domain
{
    public class Card(string serie, int puncte)
    {
        public string Serie { get; set; } = serie;
        public int Puncte { get; set; } = puncte;
    }
    public class IstoricCard(string serie, int puncte, DateTime start, DateTime? end) : Card(serie, puncte)
    {
        public DateTime Start { get; set; } = start;
        public DateTime? End { get; set; } = end;

    }
    public record EvolutieCard( DateTime Timestamp, int Puncte, int Diferenta);

}
