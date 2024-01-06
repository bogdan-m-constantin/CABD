namespace eShop.Backend.Domain
{
    public class Comanda(int id, Client client, Adresa adresaLivrare, Adresa adresaFacturare, DateTime dataOra, Card? card, List<PozitieComanda> pozitii)
    {
        public int Id { get; set; } = id;
        public Client Client { get; set; } = client;
        public Adresa AdresaLivrare { get; set; } = adresaLivrare;
        public Adresa AdresaFacturare { get; set; } = adresaFacturare;
        public DateTime DataOra { get; set; } = dataOra;
        public Card? Card { get; set; } = card;
        public List<PozitieComanda> Pozitii { get; set; } = pozitii;
        public int PuncteFolosite { get; set; } = 0;   
    }


}
