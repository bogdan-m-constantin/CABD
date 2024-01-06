
using eShop.Backend.Domain;
using eShop.Backend.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eShop.Backend.Services
{
    public class ComenziService(SqlOptions sql, PozitiiComenziService pozitiiComenzi, CarduriService carduri)
    {
        private readonly SqlOptions _sql = sql;
        private readonly PozitiiComenziService _pozitiiComenzi = pozitiiComenzi;
        private readonly CarduriService _carduri = carduri;

        public List<Comanda> GetComenzi(int clientid = -1, DateTime? start = null, DateTime? end = null, int comandaId = -1)
        {

            var filters = "(@CLIENTID = -1 OR CMD.CLIENTID = @CLIENTID) AND (CMD.DATAORA BETWEEN @START AND @END) AND (@COMANDAID = -1 OR @COMANDAID = COMANDAID)";

            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand($"""
                SELECT * FROM COMENZI CMD WHERE {filters};
                SELECT CMDP.* FROM COMENZIPOS CMDP INNER JOIN COMENZI CMD ON CMDP.COMANDAID = CMD.COMANDAID WHERE {filters};
                SELECT CL.* FROM CLIENTI CL INNER JOIN COMENZI CMD ON CL.CLIENTID = CMD.CLIENTID WHERE {filters};
                SELECT P.* FROM PRODUSE P INNER JOIN COMENZIPOS CMDP ON P.PRODUSID = CMDP.PRODUSID 
                    INNER JOIN COMENZI CMD ON CMDP.COMANDAID = CMD.COMANDAID WHERE {filters};
                SELECT CL.* FROM ADRESE A INNER JOIN COMENZI CMD ON A.ADRESAID = CMD.ADRESA_LIVRARE_ID OR A.ADRESAID = CMD.ADRESA_FACTURARE_ID WHERE {filters};
                """, con);
            using var adapter = new SqlDataAdapter(cmd);
            using var dataset = new DataSet();
            adapter.Fill(dataset);

            dataset.Tables[0].TableName = "Comenzi";
            dataset.Tables[1].TableName = "ComenziPos";
            dataset.Tables[2].TableName = "Clienti";
            dataset.Tables[3].TableName = "Produse";
            dataset.Tables[4].TableName = "Adrese";

            dataset.Relations.Add(new DataRelation("Comenzi-ComenziPos", dataset.Tables["Comenzi"].Columns["COMANDAID"], dataset.Tables["ComenziPos"].Columns["COMANDAID"]));
            dataset.Relations.Add(new DataRelation("AdresaLivrareComenzi", dataset.Tables["Adrese"].Columns["ADRESAID"], dataset.Tables["Comenzi"].Columns["ADRESA_LIVRARE_ID"]));
            dataset.Relations.Add(new DataRelation("AdresaFacturareComenzi", dataset.Tables["Adrese"].Columns["ADRESAID"], dataset.Tables["Comenzi"].Columns["ADRESA_FACTURARE_ID"]));
            dataset.Relations.Add(new DataRelation("Clienti-Comenzi", dataset.Tables["Clienti"].Columns["CLIENTID"], dataset.Tables["Comenzi"].Columns["CLIENTID"]));
            dataset.Relations.Add(new DataRelation("ComenziPos-Produse", dataset.Tables["ComenziPos"].Columns["PRODUSID"], dataset.Tables["Produse"].Columns["PRODUSID"]));

            var lst = new List<Comanda>();
            foreach (DataRow cr in dataset.Tables[0].Rows)
            {
                lst.Add(new(
                    Convert.ToInt32(cr["COMANDAID"]),
                    GetClient(cr),
                    GetAdresa(cr, livrare: true),
                    GetAdresa(cr, livrare: false),
                    Convert.ToDateTime(cr["DATAROW"]),
                    GetCard(cr),
                    GetPozitii(cr)
                    ));
            }

            return lst;

        }

        private List<PozitieComanda> GetPozitii(DataRow cr)
        {
            return cr.GetChildRows("Comenzi-ComenziPos").Cast<DataRow>()
                .Select(pr => new PozitieComanda(
                    Convert.ToInt32(pr["COMANDAPOSID"]),
                    GetProdus(pr),
                    Convert.ToDouble(pr["CANTITATE"]),
                    Convert.ToDouble(pr["PRET"]),
                    Convert.ToInt32(pr["TVA"])
                    ))
                .ToList();
        }

        private Produs? GetProdus(DataRow pr)
        {
            var r = pr.GetParentRow("ComenziPos-Produse")!;
            return new Produs(
                    Convert.ToInt32(r["PRODUSID"]),
                    r["DENUMIRE"].ToString()!,
                    Convert.ToDouble(pr["PRET"]),
                    Convert.ToInt32(pr["TVA"])
                );
        }

        private Card? GetCard(DataRow cr)
        {
            var r = cr.GetParentRow("Carduri-Comenzi");
            return new(r["SERIE_CARD"].ToString()!, Convert.ToInt32(r["PUNCTE"]));
        }

        private Adresa GetAdresa(DataRow cr, bool livrare)
        {
            var relation = livrare ? "AdresaLivrareComenzi" : "AdresaFacturareComenzi";
            var arow = cr.GetParentRow(relation)!;
            return new Adresa(
                        Convert.ToInt32(arow["ADRESAID"]),
                        arow["DENUMIRE"].ToString()!,
                        arow["ADRESA"].ToString()!,
                        arow["JUDET"].ToString()!,
                        arow["LOCALITATE"].ToString()!,
                        arow["COD_FISCAL"].ToString()!,
                        arow["NR_REG_COM"].ToString()!
                    );

        }

        private Client GetClient(DataRow r)
        {
            var cr = r.GetParentRow("Carduri-Comenzi");
            return new Client(
                       Convert.ToInt32(cr["CLIENTID"]),
                       cr["NUME"].ToString()!,
                       cr["PRENUME"].ToString()!,
                       cr["EMAIL"].ToString()!,
                       Convert.ToDateTime(cr["DATA_NASTERII"]),
                       cr["TELEFON"].ToString()!,
                      new()
                   );

        }

        public Comanda? InsertComanda(Comanda c)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var transaction = con.BeginTransaction();
            using var cmd = new SqlCommand("INSERT INTO COMENZI(CLIENTID,DATAORA,ADRESA_LIVRARE_ID,ADRESA_FACTURARE_ID,SERIE_CARD) " +
                "VALUES (@CLIENTID,@DATAORA,@ADRESA_LIVRARE_ID,@ADRESA_FACTURARE_ID,NULLIF(@SERIE_CARD,'')); SET @COMANDAID = SCOPE_IDENTITY()", con, transaction);
            cmd.Parameters.Add(new("@COMANDAID", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@CLIENTID", c.Client.Id));
            cmd.Parameters.Add(new("@DATAORA", c.DataOra));
            cmd.Parameters.Add(new("@ADRESA_LIVRARE_ID", c.AdresaLivrare.Id));
            cmd.Parameters.Add(new("@ADRESA_FACTURARE_ID", c.AdresaFacturare.Id));
            cmd.Parameters.Add(new("@SERIE_CARD", (c.Card?.Serie ?? "").Trim()));
            cmd.ExecuteNonQuery();
            c.Id = Convert.ToInt32(cmd.Parameters["@COMANDAID"].Value);
            foreach (var pos in c.Pozitii)
            {
                pos.Id = _pozitiiComenzi.InsertPozitie(pos, con, transaction, c.Id);
            }

            if (c.Card != null)
            {
                var card = _carduri.GetCarduri(serie: c.Card.Serie).First();
                card.Puncte += c.Pozitii.Sum(e => e.Puncte) - c.PuncteFolosite;
                _carduri.UpdateCard(card, con, transaction);
            }
            transaction.Commit();

            return c;
        }


    }
}
