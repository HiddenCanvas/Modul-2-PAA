using Modul_2.Data;
using Modul_2.Model;

namespace Modul_2.Context
{
    public class PersonContext
    {
        private readonly SqlDBHelper _dbHelper;

        public PersonContext(SqlDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // GET ALL
        public List<Person> GetAll()
        {
            var list = new List<Person>();
            var cmd = _dbHelper.GetNpgsqlCommand("SELECT * FROM users.person");
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Person
                {
                    id_person = reader.GetInt32(0),
                    nama = reader.GetString(1),
                    alamat = reader.GetString(2),
                    email = reader.GetString(3)
                });
            }
            _dbHelper.CloseConnection();
            return list;
        }

        // GET BY ID
        public Person? GetById(int id)
        {
            var cmd = _dbHelper.GetNpgsqlCommand(
                $"SELECT * FROM users.person WHERE id_person = {id}");
            using var reader = cmd.ExecuteReader();
            Person? p = null;
            if (reader.Read())
            {
                p = new Person
                {
                    id_person = reader.GetInt32(0),
                    nama = reader.GetString(1),
                    alamat = reader.GetString(2),
                    email = reader.GetString(3)
                };
            }
            _dbHelper.CloseConnection();
            return p;
        }

        // CREATE
        public void Create(Person p)
        {
            var cmd = _dbHelper.GetNpgsqlCommand(
                $"INSERT INTO users.person (id_person, nama, alamat, email) " +
                $"VALUES ({p.id_person}, '{p.nama}', '{p.alamat}', '{p.email}')");
            cmd.ExecuteNonQuery();
            _dbHelper.CloseConnection();
        }

        // UPDATE
        public void Update(int id, Person p)
        {
            var cmd = _dbHelper.GetNpgsqlCommand(
                $"UPDATE users.person SET nama='{p.nama}', alamat='{p.alamat}', " +
                $"email='{p.email}' WHERE id_person={id}");
            cmd.ExecuteNonQuery();
            _dbHelper.CloseConnection();
        }

        // DELETE
        public void Delete(int id)
        {
            var cmd = _dbHelper.GetNpgsqlCommand(
                $"DELETE FROM users.person WHERE id_person = {id}");
            cmd.ExecuteNonQuery();
            _dbHelper.CloseConnection();
        }
    }
}