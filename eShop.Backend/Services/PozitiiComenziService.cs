
using eShop.Backend.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eShop.Backend.Services
{
    public class PozitiiComenziService()
    {
        public int InsertPozitie(PozitieComanda pos, SqlConnection con, SqlTransaction transaction, int id)
        {
            using var cmd = new SqlCommand("INSERT INTO COMENZIPOS(COMANDAID,PRODUSID,CANTITATE,PRET,TVA) " +
                "VALUES (@COMANDAID,@PRODUSID,@CANTITATE,@PRET,@TVA); SET @COMANDAPOSID = SCOPE_IDENTITY()", con, transaction);
            cmd.Parameters.Add(new("@COMANDAPOSID", 0) { Direction = ParameterDirection.Output });
            cmd.Parameters.Add(new("@COMANDAID", id));
            cmd.Parameters.Add(new("@PRODUSID", pos.Produs.Id));
            cmd.Parameters.Add(new("@CANTITATE", pos.Cantitate));
            cmd.Parameters.Add(new("@PRET", pos.Pret));
            cmd.Parameters.Add(new("@TVA", pos.Tva));
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(cmd.Parameters["@COMANDAID"].Value);
        }
    }
}
