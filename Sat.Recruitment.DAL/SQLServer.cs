using Sat.Recruitment.BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.DAL
{
    public class SQLServer : IStorage
    {
        string strConnection;

        public SQLServer(string pstrConnection)
        {
            strConnection = pstrConnection;
        }

        public async Task SaveUser(User user)
        {
            try
            {
                using (SqlConnection oConexionSQL = new SqlConnection(strConnection))
                {
                    string szUpdate = " INSERT INTO Users (Name, Email, Address, Phone, UserType, Money) VALUES ";
                    szUpdate += " (@Name, @Email, @Address, @Phone, @UserType, @Money) ";

                    using (SqlCommand oComandoSQL = new SqlCommand(szUpdate, oConexionSQL))
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
            catch (SqlException sqlex)
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
                using (SqlConnection oConexionSQL = new SqlConnection(strConnection))
                {
                    string szQuery = " SELECT 1 FROM Users WHERE Email = @Email OR Phone = @Phone";
                    szQuery += "UNION SELECT 1 FROM Users WHERE Name = @Address AND Phone = @Address ";

                    using (SqlCommand oComandoSQL = new SqlCommand(szQuery, oConexionSQL))
                    {
                        oComandoSQL.Parameters.AddWithValue("@Email", user.Email);
                        oComandoSQL.Parameters.AddWithValue("@Phone", user.Email);
                        oComandoSQL.Parameters.AddWithValue("@Address", user.Address);
                        oComandoSQL.Parameters.AddWithValue("@Name", user.Name);

                        await oConexionSQL.OpenAsync();
                        using (SqlDataReader oReaderSQL = oComandoSQL.ExecuteReader())
                        {
                            oData.Load(oReaderSQL);
                        }
                        await oConexionSQL.CloseAsync();
                    }
                }

                //Si no hay coincidencias, devuelvo el objeto nulo
                if (oData.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException sqlex)
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
