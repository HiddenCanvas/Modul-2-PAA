using Npgsql;

namespace Modul_2.Data
{
    public class SqlDBHelper
    {
        private string __constr;

        public SqlDBHelper(string pConstr)
        {
            __constr = pConstr;
            Console.WriteLine(">>> SqlDBHelper dibuat, constr: " + __constr);
        }

        public NpgsqlCommand GetNpgsqlCommand(string query)
        {
            Console.WriteLine(">>> Mau buka koneksi, constr: " + __constr);

            NpgsqlConnection connection = new NpgsqlConnection(__constr);
            connection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
            cmd.CommandType = System.Data.CommandType.Text;
            return cmd;
        }

        public void CloseConnection()
        {
        }
    }
}