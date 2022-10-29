using Npgsql;


public class Tipo_Quarto
{
    public int Id_tipo { get; set; }
    public string Descricao { get; set; }
    public double Valor { get; set; }

    Conexao conn = new Conexao();
    public void inserir_tipo_quarto()
    {
        System.Console.Write("ID tipo do quarto: ");
        this.Id_tipo = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Descrição do tipo do quarto: ");
        this.Descricao = Console.ReadLine();
        System.Console.Write("Valor: R$ ");
        this.Valor = Convert.ToDouble(Console.ReadLine());
        
        try
        {
            conn.conectar();
            var cmd = new NpgsqlCommand(@"INSERT INTO tipo_quarto(id_tipo, descricao, valor) VALUES (@p1, @p2, @p3)", conn.conectar());
            cmd.Parameters.Add(new NpgsqlParameter("p1", Id_tipo));
            cmd.Parameters.Add(new NpgsqlParameter("p2", Descricao));
            cmd.Parameters.Add(new NpgsqlParameter("p3", Valor));
            cmd.ExecuteNonQuery();

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("REGISTRO INSERIDO COM SUCESSO. ");
            Console.ResetColor();

            //fechar conexao
            conn.desconectar();
            Thread.Sleep(2000);
            Menu menu = new Menu();
            menu.MenuPrincipal();
        }

        catch (Exception e)
        {

            System.Console.WriteLine("Ocorreu um erro, não foi possível cadastrar um novo cliente no Banco de Dados");
            System.Console.WriteLine(e.Message);

        }


        
    }

}