namespace eShop.Backend.Domain
{
    public class Client(int id, string nume, string prenume, string email, DateTime dataNasterii, string telefon, List<Adresa> adrese)
    {
        public int Id { get; set; } = id;
        public string Nume { get; set; } = nume;
        public string Prenume { get; set; } = prenume;
        public string Email { get; set; } = email;
        public DateTime DataNasterii { get; set; } = dataNasterii;
        public string Telefon { get; set; } = telefon;
        public List<Adresa> Adrese { get; set; } = adrese;
    }

}
