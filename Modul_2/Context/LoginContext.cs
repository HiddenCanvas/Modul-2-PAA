using Modul_2.Data;
using Modul_2.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Modul_2.Context
{
    public class LoginContext
    {
        private string __constr;

        public LoginContext(string pConstr)
        {
            __constr = pConstr;
        }

        public List<Login> Autentifikasi(string p_username, string p_password, IConfiguration p_config)
        {
            List<Login> list1 = new List<Login>();

            string query = string.Format(
                @"SELECT ps.id_person, ps.nama, ps.alamat, ps.email,
                         ps.username, ps.password,
                         pp.id_peran, p.nama_peran
                  FROM users.person ps
                  INNER JOIN users.peran_person pp ON ps.id_person = pp.id_person
                  INNER JOIN users.peran p ON pp.id_peran = p.id_peran
                  WHERE ps.username='{0}' AND ps.password='{1}'",
                p_username, p_password);

            SqlDBHelper db = new SqlDBHelper(__constr);
            try
            {
                var cmd = db.GetNpgsqlCommand(query);
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    // Username atau password salah
                    list1.Add(new Login()
                    {
                        pesan = "Login gagal! Username atau password salah."
                    });
                }
                else
                {
                    while (reader.Read())
                    {
                        list1.Add(new Login()
                        {
                            id_person = int.Parse(reader["id_person"].ToString()),
                            nama = reader["nama"].ToString(),
                            alamat = reader["alamat"].ToString(),
                            email = reader["email"].ToString(),
                            username = reader["username"].ToString(),
                            password = reader["password"].ToString(),
                            id_peran = int.Parse(reader["id_peran"].ToString()),
                            nama_peran = reader["nama_peran"].ToString(),
                            token = GenerateJwtToken(p_username, reader["nama_peran"].ToString(), p_config),
                            pesan = "Login berhasil!"
                        });
                    }
                }

                cmd.Dispose();
                db.CloseConnection();
            }
            catch (Exception ex)
            {
                list1.Add(new Login()
                {
                    pesan = "Error: " + ex.Message
                });
            }

            return list1;
        }

        private string GenerateJwtToken(string namaUser, string peran, IConfiguration pConfig)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(pConfig["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, namaUser),
                new Claim(ClaimTypes.Role, peran)
            };
            var token = new JwtSecurityToken(
                pConfig["Jwt:Issuer"],
                pConfig["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}