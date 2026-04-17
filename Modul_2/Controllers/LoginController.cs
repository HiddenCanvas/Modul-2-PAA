using Microsoft.AspNetCore.Mvc;
using Modul_2.Context;
using Modul_2.Model;

namespace Modul_2.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration __config;
        private string __constr;

        public LoginController(IConfiguration configuration)
        {
            __config = configuration;
            __constr = "Host=localhost;Port=5432;Database=modul3_db;Username=postgres;Password=ashar";
        }

        [HttpPost("api/Login")]
        public IEnumerable<Login> LoginUser(string namaUser, string password)
        {
            LoginContext context = new LoginContext(__constr);
            List<Login> listLogin = context.Autentifikasi(namaUser, password, __config);
            return listLogin.ToArray();
        }
    }
}