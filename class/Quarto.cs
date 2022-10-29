using System.Text;
using Npgsql;

public class Quarto
{
    public int Numero_quarto { get; set; }
    public string Andar { get; set; }
    public int Id_tipo { get; set; }
    public char Status { get; set; }


    Conexao conn = new Conexao();
    public void inserir_quarto()
    {
        System.Console.Write("Informe o Numero do Quarto: ");
        this.Numero_quarto = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Informe o Andar: ");
        this.Andar = Console.ReadLine();
        System.Console.Write("Informe o código do Tipo do Quarto: ");
        this.Id_tipo = Convert.ToInt32(Console.ReadLine());


        try
        {
            conn.conectar();
            var cmd = new NpgsqlCommand(@"INSERT INTO quarto(num_quarto, andar, id_tipo, status) VALUES (@p1, @p2, @p3, default)", conn.conectar());
            cmd.Parameters.Add(new NpgsqlParameter("p1", Numero_quarto));
            cmd.Parameters.Add(new NpgsqlParameter("p2", Andar));
            cmd.Parameters.Add(new NpgsqlParameter("p3", Id_tipo));
            cmd.ExecuteNonQuery();

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("QUARTO INSERIDO COM SUCESSO. ");
            Console.ResetColor();

            //fechar conexao
            conn.desconectar();
            Thread.Sleep(2000);
            Menu menu = new Menu();
            menu.MenuPrincipal();
        }

        catch (Exception e)
        {

            System.Console.WriteLine("Ocorreu um erro, não foi possível cadastrar um novo QUARTO no Banco de Dados");
            System.Console.WriteLine(e.Message);

        }


    }

    public void quartosDisponiveis()
    {
        int i = 0;

        NpgsqlCommand cmd = new NpgsqlCommand("select * from quarto WHERE status = 'D' ORDER BY num_quarto", conn.conectar());

        NpgsqlDataReader rd = cmd.ExecuteReader();
        
        System.Console.WriteLine("Quartos disponíveis: \n");
        System.Console.WriteLine("ID|ANDAR      |TIPO|STATUS|");
        while (rd.Read())
        {

            for (i = 0; i < rd.FieldCount; i++)
            {
                Console.Write("{0}", rd[i]);
                System.Console.Write(" | ");
                
            }
            System.Console.WriteLine();
            
        }
        conn.desconectar();

    }

    public void checkoutQuarto()
    {
        int i = 0;
        try
        {
            NpgsqlCommand cmd = new NpgsqlCommand("select * from quarto ORDER BY num_quarto", conn.conectar());

            NpgsqlDataReader rd = cmd.ExecuteReader();

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            System.Console.WriteLine("ID|ANDAR      |TIPO|STATUS|");
            Console.ResetColor();
            while (rd.Read())
            {

                for (i = 0; i < rd.FieldCount; i++)
                {
                    Console.Write("{0}", rd[i]);
                    System.Console.Write(" | ");
                }
                Console.WriteLine();
            }
        }
        catch
        {

        }
        // Fecha conexão com o Banco.
        conn.desconectar();
        System.Console.WriteLine("\nInforme o ID do quarto que deseje LIBERAR: ");
        int id = Convert.ToInt32(Console.ReadLine());

        StringBuilder sb = new StringBuilder();
        sb.Append("UPDATE quarto SET status = 'D' WHERE num_quarto = @id;");
        NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), conn.conectar());
        comando.Parameters.AddWithValue("id", id);

        comando.Connection = conn.conectar();
        comando.ExecuteNonQuery();
        conn.desconectar();

        System.Console.WriteLine("\nSTATUS ALTERADO PARA 'DISPONÍVEL' COM SUCESSO!\n");

        Menu menu = new Menu();
        menu.subMenu();




    }


}