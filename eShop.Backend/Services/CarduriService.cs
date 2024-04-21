using eShop.Backend.Domain;
using eShop.Backend.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace eShop.Backend.Services
{
    public class CarduriService(SqlOptions sql)
    {
        private readonly SqlOptions _sql = sql;

        public List<Card> GetCarduri(int clientId = -1, string? serie = null)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("""
                SELECT 
                    * 
                FROM CARDURI 
                WHERE 
                    (@SERIE IS NULL OR @SERIE = SERIE_CARD) AND (@CLIENTID = -1 OR @CLIENTID = CLIENTID)
               """, con);
            cmd.Parameters.Add(new("@SERIE", serie ?? (object)DBNull.Value));
            cmd.Parameters.Add(new("@CLIENTID", clientId));
            using var reader = cmd.ExecuteReader();
            var lst = new List<Card>();
            while (reader.Read())
            {
                lst.Add(
                new Card(
                    reader["SERIE_CARD"].ToString()!,
                    Convert.ToInt32(reader["PUNCTE"])

                    )
                );
            }
            return lst;
        }

        public Card InsertCard(Card c, int clientId)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("INSERT INTO CARDURI(SERIE_CARD,CLIENTID,PUNCTE) VALUES (@SERIE_CARD,@CLIENTID,@PUNCTE)", con);
            cmd.Parameters.Add(new("@SERIE_CARD", c.Serie));
            cmd.Parameters.Add(new("@CLIENTID", clientId));
            cmd.Parameters.Add(new("@PUNCTE", c.Puncte));
            cmd.ExecuteNonQuery();
            return c;
        }
        public void UpdateCard(Card c)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            UpdateCard(c, con, null);
        }
        public void UpdateCard(Card c, SqlConnection con, SqlTransaction? transaction)
        {
            using var cmd = new SqlCommand("UPDATE CARDURI SET PUNCTE = @PUNCTE WHERE SERIE_CARD = @SERIE_CARD", con,transaction);
            cmd.Parameters.Add(new("@SERIE_CARD", c.Serie));
            cmd.Parameters.Add(new("@PUNCTE", c.Puncte));
            cmd.ExecuteNonQuery();
        }
        public List<IstoricCard> GetIstoricCard(string? serie = null)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("SELECT * FROM CARDURI_DATA WHERE (@SERIE IS NULL OR @SERIE = SERIE_CARD)", con);
            cmd.Parameters.Add(new("@SERIE", serie ?? (object)DBNull.Value));
            using var reader = cmd.ExecuteReader();
            var lst = new List<IstoricCard>();
            while (reader.Read())
            {
                lst.Add(
                new IstoricCard(
                    reader["SERIE_CARD"].ToString()!,
                    Convert.ToInt32(reader["PUNCTE"]),
                    Convert.ToDateTime(reader["TIMESTAMP_START"]),
                    reader["TIMESTAMP_END"] == DBNull.Value ? null : Convert.ToDateTime(reader["TIMESTAMP_END"])
                    )
                );
            }
            return lst;
        }

        public IstoricCard? GetIntervalMaximCard(string? serie = null)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand(
               """
                SELECT 
                    SERIE_CARD,PUNCTE,TIMESTAMP_START,TIMESTAMP_END 
                FROM ( 
                    SELECT 
                        CD.SERIE_CARD, TIMESTAMP_START, TIMESTAMP_END, PUNCTE,  
                        ROW_NUMBER() OVER (ORDER BY DATEDIFF(SECOND,TIMESTAMP_START,TIMESTAMP_END) DESC) RNK 
                    FROM CARDURI_DATA CD 
                    WHERE  
                    SERIE_CARD = @SERIE_CARD  
                    AND PUNCTE = (SELECT MAX(PUNCTE) PUNCTE FROM CARDURI_DATA WHERE SERIE_CARD = @SERIE_CARD)
                ) T  WHERE RNK = 1 
               """, con);
            cmd.Parameters.Add(new("@SERIE_CARD", serie ?? (object)DBNull.Value));
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return
                 new IstoricCard(
                     reader["SERIE_CARD"].ToString()!,
                     Convert.ToInt32(reader["PUNCTE"]),
                     Convert.ToDateTime(reader["TIMESTAMP_START"]),
                     reader["TIMESTAMP_END"] == DBNull.Value ? null : Convert.ToDateTime(reader["TIMESTAMP_END"])
                     );

            }
            return null;
        }

        public List<EvolutieCard> GetEvolutieCard(string? serie = null)
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("""
                    SELECT 
                        TIMESTAMP_START, PUNCTE,PUNCTE - ISNULL(LAG(PUNCTE,1) OVER (ORDER BY TIMESTAMP_START),0) DIFERENTA 
                    FROM CARDURI_DATA 
                    WHERE SERIE_CARD = @SERIE_CARD 
                    ORDER BY TIMESTAMP_START
                    """, con);
            cmd.Parameters.Add(new("@SERIE_CARD", serie ?? (object)DBNull.Value));
            using var reader = cmd.ExecuteReader();
            var lst = new List<EvolutieCard>();
            while (reader.Read())
            {
                lst.Add(
            new EvolutieCard(
                     Convert.ToDateTime(reader["TIMESTAMP_START"]),
                     Convert.ToInt32(reader["PUNCTE"]),
                     Convert.ToInt32(reader["DIFERENTA"])
                     ));

            }
            return lst;
        }

        public Card? GetCardLaMoment(string serie , DateTime timestamp )
        {
            using var con = new SqlConnection(_sql.ConnectionString);
            con.Open();
            using var cmd = new SqlCommand("""
                    SELECT 
                        SERIE_CARD,PUNCTE 
                    FROM CARDURI_DATA 
                    WHERE SERIE_CARD = @SERIE_CARD AND  @TIMESTAMP BETWEEN TIMESTAMP_START AND ISNULL(TIMESTAMP_END,GETDATE())
                """, con);
            cmd.Parameters.Add(new("@SERIE_CARD", serie));
            cmd.Parameters.Add(new("@TIMESTAMP", timestamp.ToLocalTime()));
            using var reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                return
           new Card(
                    reader["SERIE_CARD"].ToString()!,
                    Convert.ToInt32(reader["PUNCTE"])

                    );

            }
            return null;
        }




    }
}
