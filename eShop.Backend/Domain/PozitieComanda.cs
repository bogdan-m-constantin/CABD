namespace eShop.Backend.Domain
{
    public class PozitieComanda(int id, Produs? produs, double cantitate, double pret, double tva)
    {
        public int Id { get; set; } = id;
        public Produs? Produs { get; set; } = produs;
        public double Cantitate { get; set; } = cantitate;
        public double Pret { get; set; } = pret;
        public double Tva { get; set; } = tva;
        public int Puncte => (int)(Pret*Cantitate);
    }
}
