public class Menu
{

    //CLASSE EXCLUSIVA PARA OS MENU'S
    
    Cliente c = new Cliente();
    Tipo_Quarto tipo_quarto = new Tipo_Quarto();
    Quarto quarto = new Quarto();
    Reserva reserva = new Reserva();

    int opcao = 1;
    public void MenuPrincipal()
    {
        Console.Clear();
        Cliente c = new Cliente();
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.Black;
        System.Console.WriteLine("=================================");
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        System.Console.WriteLine("    BEM-VINDO AO SYSTEM HOTEL    ");
        System.Console.WriteLine("                                 ");
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("[1] CADASTRAR CLIENTE            ");
        System.Console.WriteLine("[2] LISTAR CLIENTES              ");
        System.Console.WriteLine("[3] CADASTRAR TIPO DE QUARTO     ");
        System.Console.WriteLine("[4] CADASTRAR QUARTO             ");
        System.Console.WriteLine("[5] CHECKOUT QUARTO              ");
        System.Console.WriteLine("[6] RESERVAR QUARTO              ");
        System.Console.WriteLine("[0] SAIR DO SISTEMA              ");
        Console.BackgroundColor = ConsoleColor.Blue;
        System.Console.WriteLine("=================================");
        System.Console.Write(" > OPÇÃO: ");
        opcao = Convert.ToInt32(Console.ReadLine());
        
        Console.ResetColor();

        switch (opcao)
        {
            case 0:
                System.Environment.Exit(0);
                break;

            case 1:
                Console.Clear();
                c.inserirCliente();
                break;

            case 2:
                Console.Clear();
                c.listaClientes();
                break;

            case 3:
                Console.Clear();
                tipo_quarto.inserir_tipo_quarto();
                break;
            case 4:
                Console.Clear();
                quarto.inserir_quarto();
                break;

            case 5:
                Console.Clear();
                quarto.checkoutQuarto();
                break;
            case 6:
                Console.Clear();
                reserva.adicionaReserva();
                break;
                



        }
    }

    public void subMenu()
    {
        Cliente c = new Cliente();
        System.Console.WriteLine("[1] VOLTAR AO MENU PRINCIPAL");
        System.Console.WriteLine("[2] ENCERRAR PROGRAMA       ");
        Console.ResetColor();
        opcao = Convert.ToInt32(Console.ReadLine());
        switch (opcao)
        {
            
            case 1:
                Console.Clear();
                MenuPrincipal();
                break;
            case 2:
                System.Environment.Exit(0);
                break;
        }
    }

}