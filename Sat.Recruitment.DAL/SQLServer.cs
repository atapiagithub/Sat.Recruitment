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

        public async Task<User> GetUserByEmail(User user)
        {
            User oResultado = null;
            DataTable oData = new DataTable();
            try
            {
                using (SqlConnection oConexionSQL = new SqlConnection(strConnection))
                {
                    string szUpdate = " SELECT Name, Email, Address, Phone, UserType, Money FROM Users ";
                    szUpdate += " WHERE Email = @Email ";

                    using (SqlCommand oComandoSQL = new SqlCommand(szUpdate, oConexionSQL))
                    {
                        oComandoSQL.Parameters.AddWithValue("@Email", user.Email);

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
