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

        public async Task<bool> ValidateUserExists(User user)
        {
            DataTable oData = new DataTable();
            try
            {
                using (SqliteConnection oConexionSQL = new SqliteConnection(strConnection))
                {
                    string szQuery = " SELECT 1 FROM Users WHERE Email = @Email OR Phone = @Phone;";
                    szQuery += "SELECT 1 FROM Users WHERE Name = @Name AND Address = @Address;";

                    using (SqliteCommand oComandoSQL = new SqliteCommand(szQuery, oConexionSQL))
                    {
                        oComandoSQL.Parameters.AddWithValue("@Email", user.Email);
                        oComandoSQL.Parameters.AddWithValue("@Phone", user.Phone);
                        oComandoSQL.Parameters.AddWithValue("@Address", user.Address);
                        oComandoSQL.Parameters.AddWithValue("@Name", user.Name);

                        await oConexionSQL.OpenAsync();
                        using (SqliteDataReader oReaderSQL = oComandoSQL.ExecuteReader())
                        {
                            oData.Load(oReaderSQL);
                        }
                        await oConexionSQL.CloseAsync();
                    }
                }

                //Si no se cumple ninguna de las validaciones, no se devuelven filas.
                //Si se cumple alguna, se devolvera al menos una fila con un 1
                if (oData.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }               
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
