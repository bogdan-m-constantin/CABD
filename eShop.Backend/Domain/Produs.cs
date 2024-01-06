namespace eShop.Backend.Domain
{
    public class Produs(int id, string denumire, double pret, double tva)
    {
        public int Id { get; set; } = id;
        public string Denumire { get; set; } = denumire;
        public double Pret { get; set; } = pret;
        public double Tva { get; set; } = tva;
    }

}
