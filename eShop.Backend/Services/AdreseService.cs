
using eShop.Backend.Domain;
using eShop.Backend.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eShop.Backend.Services
{
    public class AdreseService(SqlOptions sql)
    {
        private readonly SqlOptions _sql = sql;
    
        internal List<Adresa> GetAdrese(int clientId = -1)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("SELECT * FROM ADRESE WHERE CLIENTID = @CLIENTID", con);
            cmd.Parameters.Add(new("@CLIENTID", clientId));
            using var reader = cmd.ExecuteReader();
            var lst = new List<Adresa>();
            while (reader.Read()) {
                lst.Add(
                new Adresa(
                        Convert.ToInt32(reader["ADRESAID"]),
                        reader["DENUMIRE"].ToString()!,
                        reader["ADRESA"].ToString()!,
                        reader["JUDET"].ToString()!,
                        reader["LOCALITATE"].ToString()!,
                        reader["COD_FISCAL"].ToString()!,
                        reader["NR_REG_COM"].ToString()!
                    )
                );
            }
            return lst;
        }

        internal Adresa InsertAdresa(Adresa a,int clientId)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("INSERT INTO ADRESE(CLIENTID, DENUMIRE, ADRESA, JUDET, LOCALITATE, COD_FISCAL, NR_REG_COM) " +
                "VALUES (@CLIENTID, @DENUMIRE, @ADRESA, @JUDET, @LOCALITATE, @COD_FISCAL, @NR_REG_COM); SET @ADRESAID = SCOPE_IDENTITY()", con);
            cmd.Parameters.Add(new("@ADRESAID", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@CLIENTID", clientId));
            cmd.Parameters.Add(new("@DENUMIRE", a.Denumire));
            cmd.Parameters.Add(new("@ADRESA", a.AdresaIntreaga));
            cmd.Parameters.Add(new("@JUDET", a.Judet));
            cmd.Parameters.Add(new("@LOCALITATE", a.Localitate));
            cmd.Parameters.Add(new("@COD_FISCAL", a.CodFiscal)); 
            cmd.Parameters.Add(new("@NR_REG_COM", a.NrRegCom));

            cmd.ExecuteNonQuery();
            a.Id = Convert.ToInt32(cmd.Parameters["@ADRESAID"].Value);
            return a;
        }

        internal void UpdateAdresa(Adresa a)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("UPDATE ADRESE SET  DENUMIRE = @DENUMIRE, ADRESA = @ADRESA, JUDET = @JUDET, LOCALITATE = @LOCALITATE, " +
                "COD_FISCAL = @COD_FISCAL, NR_REG_COM = @NR_REG_COM WHERE ADRESAID = @ADRESAID", con);
            cmd.Parameters.Add(new("@ADRESAID", a.Id));
            cmd.Parameters.Add(new("@DENUMIRE", a.Denumire));
            cmd.Parameters.Add(new("@ADRESA", a.AdresaIntreaga));
            cmd.Parameters.Add(new("@JUDET", a.Judet));
            cmd.Parameters.Add(new("@LOCALITATE", a.Localitate));
            cmd.Parameters.Add(new("@COD_FISCAL", a.CodFiscal));
            cmd.Parameters.Add(new("@NR_REG_COM", a.NrRegCom));

            cmd.ExecuteNonQuery();
        }
    }
}
