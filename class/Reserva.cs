using System.Text;
using Npgsql;

public class Reserva
{

    //CLASSE PARA REALIZAR RESERVAS
    public int rg { get; set; }
    public int num_quarto { get; set; }
    public int dias { get; set; }
    public DateTime data_entrada { get; set; }







    NpgsqlCommand cmd = new NpgsqlCommand();
    Conexao conn = new Conexao();
    Quarto quarto = new Quarto();
    public void adicionaReserva()
    {
        System.Console.Write("RG do cliente: ");
        rg = Convert.ToInt32(Console.ReadLine());
        quarto.quartosDisponiveis();
        System.Console.Write("ID do quarto: ");
        num_quarto = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Numero de diárias: ");
        dias = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Data para check-in(YYYY-MM-DD): ");
        data_entrada = Convert.ToDateTime(Console.ReadLine());


        StringBuilder sb = new StringBuilder();
        sb.Append("SELECT adicionaReserva(@rg,@num_quarto,@dias,@data_entrada)");
        //sb.Append("SELECT adicionaReserva(@rg,@num_quarto,@dias,'2022-10-10')");
        NpgsqlCommand cmd = new NpgsqlCommand(sb.ToString(), conn.conectar());
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.Parameters.AddWithValue("@rg", rg);
        cmd.Parameters.AddWithValue("@num_quarto", num_quarto);
        cmd.Parameters.AddWithValue("@dias", dias);
        cmd.Parameters.AddWithValue("@data_entrada", data_entrada);


        try
        {
            cmd.Connection = conn.conectar();
            cmd.ExecuteNonQuery();

            conn.desconectar();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            System.Console.WriteLine("\nRESERVA REALIZADA COM SUCESSO!\n");
            Console.ResetColor();
            Thread.Sleep(2000);
            Menu menu = new Menu();
            menu.MenuPrincipal();

        }
        catch (Exception e)
        {

            System.Console.WriteLine(e);
        }


    }

}
