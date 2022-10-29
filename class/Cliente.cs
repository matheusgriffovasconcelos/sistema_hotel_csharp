using System.Text;
using Npgsql;



public class Cliente
{
    public int rg { get; set; }
    public string nome { get; set; }
    public char sexo { get; set; }
    public int telefone { get; set; }

    int i;

    Conexao conn = new Conexao();



#region INSERT CLIENTES
    //MÉTODO PARA INSERIR CLIENTES
    public void inserirCliente()
    {
        System.Console.WriteLine("CADASTRAR NOVO CLIENTE\n");
        System.Console.Write("RG(apenas numeros): ");
        rg = Convert.ToInt32(Console.ReadLine());

        System.Console.Write("NOME:  ");
        nome = Console.ReadLine();

        System.Console.Write("SEXO(M/F): ");
        sexo = Convert.ToChar(Console.ReadLine().ToUpper());

        System.Console.Write("TELEFONE: ");
         telefone = Convert.ToInt32(Console.ReadLine());

        try
        {
            conn.conectar();
            var cmd = new NpgsqlCommand(@"INSERT INTO cliente(rg, nome, sexo, telefone) VALUES (@p1, @p2, @p3, @p4)", conn.conectar());
            cmd.Parameters.Add(new NpgsqlParameter("p1", rg));
            cmd.Parameters.Add(new NpgsqlParameter("p2", nome));
            cmd.Parameters.Add(new NpgsqlParameter("p3", sexo));
            cmd.Parameters.Add(new NpgsqlParameter("p4", telefone));
    
            cmd.ExecuteNonQuery();

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;
            System.Console.WriteLine("CLIENTE CADASTRADO COM SUCESSO! ");
            Console.ResetColor();

            
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

#endregion


#region DELETE CLIENTES
    //MÉTODO PARA EXCLUIR CLIENTES
    public void excluirCliente()
    {

        System.Console.WriteLine("\nInforme o RG do Cliente que deseja excluir: ");
        int excluiRG = Convert.ToInt32(Console.ReadLine());
        StringBuilder sb = new StringBuilder();
        sb.Append("DELETE FROM cliente WHERE rg = @rg;");
        NpgsqlCommand comando = new NpgsqlCommand(sb.ToString(), conn.conectar());
        comando.Parameters.AddWithValue("rg", excluiRG);

        comando.Connection = conn.conectar();
        comando.ExecuteNonQuery();
        conn.desconectar();

        Console.Clear();
        System.Console.WriteLine("\n*CLIENTE EXCLUÍDO COM SUCESSO!*\n");

        Menu menu = new Menu();
        menu.subMenu();
    }

#endregion



#region SELECT/LISTA CLIENTES
    // MÉTODO PARA LISTAR CLIENTES E MENU PARA CHAMAR MÉTODO DE EXCLUSAO
    public void listaClientes()
    {
        
        try
        {
            //consulta para listar os clientes
            NpgsqlCommand cmd = new NpgsqlCommand("select * from cliente ORDER BY nome", conn.conectar());
            // Executa a consulta
            NpgsqlDataReader rd = cmd.ExecuteReader();
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
            System.Console.WriteLine("\nRG \tNOME \tSEXO \tTELEFONE");
            Console.ResetColor();
            while (rd.Read())
            {
                
                for (i = 0; i < rd.FieldCount; i++)
                {
                    Console.Write("{0}", rd[i]);
                    
                    System.Console.Write("\t");
                }
                Console.WriteLine();
            }
        }
        catch
        {

        }
        
        conn.desconectar();
        
        System.Console.Write("\nDeseja EXCLUIR um Cliente? (S/N): ");
        string exclui = Console.ReadLine().ToUpper();
        if (exclui == "N")
        {
            System.Console.WriteLine();
            Menu menu = new Menu();
            menu.subMenu();
        }
        else
        {
            excluirCliente();
        }

    }
#endregion


}
