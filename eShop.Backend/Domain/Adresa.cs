namespace eShop.Backend.Domain
{
    public class Adresa(int id, string denumire, string adresaIntreaga, string judet, string localitate, string codFiscal, string nrRegCom)
    {
        public int Id { get; set; } = id;
        public string Denumire { get; set; } = denumire;
        public string AdresaIntreaga { get; set; } = adresaIntreaga;
        public string Judet { get; set; } = judet;
        public string Localitate { get; set; } = localitate;
        public string CodFiscal { get; set; } = codFiscal;
        public string NrRegCom { get; set; } = nrRegCom;
    }

}
