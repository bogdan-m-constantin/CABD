
using eShop.Backend.Domain;
using eShop.Backend.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging.EventLog;
using System.Data;

namespace eShop.Backend.Services
{
    public class ClientiService(SqlOptions sql)
    {
        private readonly SqlOptions _sql = sql;
        internal List<Client> GetClienti(int id = -1)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("SELECT * FROM CLIENTI WHERE (@CLIENTID = -1 OR @CLIENTID = CLIENTID); SELECT * FROM ADRESE WHERE (@CLIENTID = -1 OR @CLIENTID = CLIENTID)", con);
            cmd.Parameters.Add(new("@CLIENTID", id));
            using var adapter = new SqlDataAdapter(cmd);
            using var dataset = new DataSet();
            adapter.Fill(dataset);
            dataset.Tables[0].TableName = "Clienti";
            dataset.Tables[1].TableName = "Adrese";
            dataset.Relations.Add(new("AdreseClienti", dataset.Tables[0].Columns["CLIENTID"]!, dataset.Tables[1].Columns["CLIENTID"]!, false));

            return dataset.Tables[0].Rows.Cast<DataRow>().Select(cr =>
                new Client(
                        Convert.ToInt32(cr["CLIENTID"]),
                        cr["NUME"].ToString()!,
                        cr["PRENUME"].ToString()!,
                        cr["EMAIL"].ToString()!,
                        Convert.ToDateTime(cr["DATA_NASTERII"]),
                        cr["TELEFON"].ToString()!,
                        cr.GetChildRows("AdreseClienti").Cast<DataRow>().Select(ar => new Adresa(
                            Convert.ToInt32(ar["ADRESAID"]),
                            ar["DENUMIRE"].ToString()!,
                            ar["ADRESA"].ToString()!,
                            ar["JUDET"].ToString()!,
                            ar["LOCALITATE"].ToString()!,
                            ar["COD_FISCAL"].ToString()!,
                            ar["NR_REG_COM"].ToString()!

                            )).ToList()
                    )
            ).ToList();

        }
        public Client InsertClient(Client c)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("INSERT INTO CLIENTI(NUME,PRENUME,EMAIL,DATA_NASTERII,TELEFON) VALUES (@NUME,@PRENUME,@EMAIL,@DATA_NASTERII,@TELEFON); SET @CLIENTID = SCOPE_IDENTITY()", con);
            cmd.Parameters.Add(new("@CLIENTID", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@NUME", c.Nume));
            cmd.Parameters.Add(new("@PRENUME", c.Prenume));
            cmd.Parameters.Add(new("@EMAIL", c.Email));
            cmd.Parameters.Add(new("@DATA_NASTERII", c.DataNasterii));
            cmd.Parameters.Add(new("@TELEFON", c.Telefon));
            cmd.ExecuteNonQuery();
            c.Id = Convert.ToInt32(cmd.Parameters["@CLIENTID"].Value);
            return c;
        }

        public void UpdateClient(Client c)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("UPDATE CLIENTI SET NUME = @NUME,@PRENUME = @PRENUME,EMAIL = @EMAIL,DATA_NASTERII = @DATA_NASTERII,TELEFON = @TELEFON WHERE CLIENTID = @CLIENTID", con);
            cmd.Parameters.Add(new("@CLIENTID", c.Id));
            cmd.Parameters.Add(new("@NUME", c.Nume));
            cmd.Parameters.Add(new("@PRENUME", c.Prenume));
            cmd.Parameters.Add(new("@EMAIL", c.Email));
            cmd.Parameters.Add(new("@DATA_NASTERII", c.DataNasterii));
            cmd.Parameters.Add(new("@TELEFON", c.Telefon));
            cmd.ExecuteNonQuery();
        }
    }
}
