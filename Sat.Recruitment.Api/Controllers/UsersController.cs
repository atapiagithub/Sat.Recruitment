using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.BE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {

        private readonly List<User> _users = new List<User>();
        public UsersController()
        {
        }

        [HttpPost("create-user")]      
        public async Task<ResultModel> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            string errors = "";

            //Creacion del usuario en base a los parametros recibidos
            //En el constructor del usuario existen las validaciones pertinentes sobre los parametros
            //Tambien se incluye en el constructor la logica de asignacion de monto segun tipo de usuario
            User newUser = new User(name, email, address, phone, userType, money);

            //Valido los datos minimos que requiere el usuario, si no se cumple, retorno los errores y no continuo
            if (!newUser.IsValid(out errors))
            {
                return new ResultModel(true, errors);
            }            

            var reader = ReadUsersFromFile();

            //Normalize email
            var aux = newUser.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            newUser.Email = string.Join("@", new string[] { aux[0], aux[1] });

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User(line.Split(',')[0].ToString(),
                    line.Split(',')[1].ToString(),
                    line.Split(',')[3].ToString(),
                    line.Split(',')[2].ToString(),
                    line.Split(',')[4].ToString(),
                    line.Split(',')[5].ToString());
               
                _users.Add(user);
            }
            reader.Close();
            try
            {
                var isDuplicated = false;
                foreach (var user in _users)
                {
                    if (user.Email == newUser.Email
                        ||
                        user.Phone == newUser.Phone)
                    {
                        isDuplicated = true;
                    }
                    else if (user.Name == newUser.Name)
                    {
                        if (user.Address == newUser.Address)
                        {
                            isDuplicated = true;
                            throw new Exception("User is duplicated");
                        }

                    }
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");
                    return new ResultModel(true, "User Created");
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");
                    return new ResultModel(false, "The user is duplicated");
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                return new ResultModel(false, "The user is duplicated");
            }
        }
    }
}
