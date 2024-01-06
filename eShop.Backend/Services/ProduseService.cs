
using eShop.Backend.Domain;
using eShop.Backend.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eShop.Backend.Services
{
    public class ProduseService(SqlOptions sql)
    {
        private readonly SqlOptions _sql = sql;
    
        internal List<Produs> GetProduse(int id = -1)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("SELECT * FROM PRODUSE WHERE (@PRODUSID = -1 OR @PRODUSID = PRODUSID)", con);
            cmd.Parameters.Add(new("@PRODUSID", id));
            using var reader = cmd.ExecuteReader();
            var lst = new List<Produs>();
            while (reader.Read()) {
                lst.Add(
                new Produs(
                    Convert.ToInt32(reader["PRODUSID"]),
                    reader["DENUMIRE"].ToString()!,
                    Convert.ToDouble(reader["PRET"]),
                    Convert.ToDouble(reader["TVA"])

                    )
                );
            }
            return lst;
        }

        internal Produs InsertProdus(Produs p)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("INSERT INTO PRODUSE(DENUMIRE,PRET,TVA) VALUES (@DENUMIRE,@PRET,@TVA); SET @PRODUSID = SCOPE_IDENTITY()", con);
            cmd.Parameters.Add(new("@PRODUSID", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@DENUMIRE", p.Denumire));
            cmd.Parameters.Add(new("@PRET", p.Pret));
            cmd.Parameters.Add(new("@TVA", p.Tva));
            cmd.ExecuteNonQuery();
            p.Id = Convert.ToInt32(cmd.Parameters["@CLIENTID"].Value);
            return p;
        }

        internal void UpdateProdus(Produs p)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("UPDATE PRODUSE SET  DENUMIRE = @DENUMIRE,PRET = @PRET,TVA = @TVA WHERE PRODUSID = @PRODUSID", con);
            cmd.Parameters.Add(new("@PRODUSID", p.Id));
            cmd.Parameters.Add(new("@DENUMIRE", p.Denumire));
            cmd.Parameters.Add(new("@PRET", p.Pret));
            cmd.Parameters.Add(new("@TVA", p.Tva));
            cmd.ExecuteNonQuery();
        }
    }
}
