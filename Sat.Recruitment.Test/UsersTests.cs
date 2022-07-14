using System;
using System.Dynamic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.DAL;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UsersTests
    {
        [Fact]
        public async void CreateUserTest()
        {
            AppSettings appsettings = new AppSettings();
            //obtengo el directorio actual donde esta corriendo el test para obtener la base de SQLite
            appsettings.ConnectionString = "Data Source=" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "storage.db");
            SQLite storage = new SQLite(appsettings.ConnectionString);
            UsersController userController = new UsersController(appsettings, storage);
            var apiResult = await userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");
            var objectResult = apiResult as ObjectResult;

            //valido resultado no nulo
            Assert.NotNull(objectResult);
            //validacion de tipo de objeto
            Assert.True(objectResult is ObjectResult);           
            //validacion de codigo de resultado HTTP
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async void ExistingUserTest()
        {
            AppSettings appsettings = new AppSettings();
            //obtengo el directorio actual donde esta corriendo el test para obtener la base de SQLite
            appsettings.ConnectionString = "Data Source=" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "storage.db");
            SQLite storage = new SQLite(appsettings.ConnectionString);
            UsersController userController = new UsersController(appsettings, storage);
            var apiResult = await userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");
            
            var objectResult = apiResult as ObjectResult;

            //valido resultado no nulo
            Assert.NotNull(objectResult);
            //validacion de tipo de objeto
            Assert.True(objectResult is ObjectResult);
            //validacion de tipo de objeto de respuesta (si viene un mensaje de error y es string)
            Assert.IsType<string>(objectResult.Value);

            string errorMessage = (string)objectResult.Value;

            //validacion de mensaje de error
            Assert.Equal("The user is duplicated", errorMessage);
            //validacion de codigo de resultado HTTP
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
