using Npgsql;

public class Conexao

{
    //CLASSE EXCLUSIVA PARA CONEXAO COM BANCO DE DADOS
    NpgsqlConnection conn = new NpgsqlConnection();

    public Conexao()
    {
        
        conn = new NpgsqlConnection($"server=127.0.0.1;Port=5432;user id=postgres;Password=;Database= DB_Hotel;");
    }

    public NpgsqlConnection conectar()
    {
        if (conn.State == System.Data.ConnectionState.Closed)
        {
            conn.Open();
        }
        return conn;
    }

    public void desconectar()
    {
        if (conn.State == System.Data.ConnectionState.Open)
        {
            conn.Close();
        }
    }
}