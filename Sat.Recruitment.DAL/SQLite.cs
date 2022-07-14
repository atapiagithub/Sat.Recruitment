using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Sat.Recruitment.BE;

namespace Sat.Recruitment.DAL
{
    public class SQLite : IStorage
    {
        string strConnection;

        public SQLite(string pstrConnection)
        {
            strConnection = pstrConnection;
        }

        public async Task SaveUser(User user)
        {
            string szQuery = "";
            try
            {
                using (SqliteConnection oConexionSQL = new SqliteConnection(strConnection))
                {
                    szQuery = " INSERT INTO Users (Name, Email, Address, Phone, UserType, Money) VALUES ";
                    szQuery += " (@Name, @Email, @Address, @Phone, @UserType, @Money); ";                    

                    using (SqliteCommand oComandoSQL = new SqliteCommand(szQuery, oConexionSQL))
                    {
                        oComandoSQL.Parameters.AddWithValue("@Name", user.Name);
                        oComandoSQL.Parameters.AddWithValue("@Email", user.Email);
                        oComandoSQL.Parameters.AddWithValue("@Address", user.Address);
                        oComandoSQL.Parameters.AddWithValue("@Phone", user.Phone);
                        oComandoSQL.Parameters.AddWithValue("@UserType", user.UserType);
                        oComandoSQL.Parameters.AddWithValue("@Money", user.Money);

                        await oConexionSQL.OpenAsync();
                        await oComandoSQL.ExecuteNonQueryAsync();
                        await oConexionSQL.CloseAsync();
                    }
                }
                return;
            }
            catch (SqliteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<User> GetUserByEmail(User user)
        {
            User oResultado = null;
            DataTable oData = new DataTable();
            try
            {
                using (SqliteConnection oConexionSQL = new SqliteConnection(strConnection))
                {
                    string szUpdate = " SELECT Name, Email, Address, Phone, UserType, Money FROM Users ";
                    szUpdate += " WHERE Email = @Email ";

                    using (SqliteCommand oComandoSQL = new SqliteCommand(szUpdate, oConexionSQL))
                    {
                        oComandoSQL.Parameters.AddWithValue("@Email", user.Email);

                        await oConexionSQL.OpenAsync();
                        using (SqliteDataReader oReaderSQL = oComandoSQL.ExecuteReader())
                        {
                            oData.Load(oReaderSQL);
                        }
                        await oConexionSQL.CloseAsync();
                    }
                }

                //Si no hay coincidencias, devuelvo el objeto nulo
                if (oData.Rows.Count > 0)
                {
                    //de lo contrario, completo los datos
                    oResultado = new User(oData.Rows[0]["Name"].ToString(),
                    oData.Rows[0]["Email"].ToString(),
                    oData.Rows[0]["Address"].ToString(),
                    oData.Rows[0]["Phone"].ToString(),
                    oData.Rows[0]["UserType"].ToString(),
                    oData.Rows[0]["Money"].ToString());
                }                

                return oResultado;
            }
            catch (SqliteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
