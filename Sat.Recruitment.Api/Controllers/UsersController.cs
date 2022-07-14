﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.BE;
using Sat.Recruitment.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        IStorage _storage;
        AppSettings _appSettings;       
       
        public UsersController(AppSettings appSettings, IStorage storage)
        {
            _appSettings = appSettings;
            _storage = storage;
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

            try
            {
                //Validacion de usuario existente (por email, que es dato univoco)
                User user = await _storage.GetUserByEmail(newUser);
                if (user != null)
                {
                    return new ResultModel(false, "The user is duplicated");
                }
                await _storage.SaveUser(newUser);
            }
            catch (Exception ex)
            {
                return new ResultModel(false, ex.Message);
            }

            return new ResultModel(true, "User Created");

        }
    }
}
