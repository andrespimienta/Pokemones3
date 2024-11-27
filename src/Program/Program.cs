// See https://aka.ms/new-console-template for more information

using Discord;
using Discord.WebSocket;
using Library.Bot.Services;
using Fachada = Library.Bot.Dominio.Fachada;

namespace Program;
public class Program
{
    static void Main()
    {
        // DemoFacade(); 
        DemoBot();
        // Ejecuta el código para enviar el mensaje

    }

    /*
    private static void DemoFacade()
    {
        Console.WriteLine(Fachada.Instance.AddTrainerToWaitingList("player"));
        Console.WriteLine(Fachada.Instance.AddTrainerToWaitingList("opponent"));
        Console.WriteLine(Fachada.Instance.GetAllTrainersWaiting());
        Console.WriteLine(Fachada.Instance.StartBattle("player", "opponent"));
        Console.WriteLine(Fachada.Instance.GetAllTrainersWaiting());
    }
*/
    private static void DemoBot()
    {
        BotLoader.LoadAsync().GetAwaiter().GetResult();
        BotLoader.OnBotReady();
    }

  
}
/*
// ANTES DE EMPEZAR RECUERDA IR A LA CLASE LEER ARCHIVO Y
        // MODIFICAR LA PRIMER STRING POR TU PATH ABSOLUTE
        // DEL ARCHIVO CATALOGOPOKEMONES.TXT

        DiccionarioTipos.GetInstancia();    // Instancia el Singleton y define el contenido de todos sus diccionarios
        Fachada fachada = Fachada.GetInstancia();
        fachada.ListaDeEspera();
        fachada.MostrarCatalogo();

        string input = "";
        bool seleccionExitosa;

        for (int j= 0; j <= 1; j++) // lo repite para los dos jugadores
        {
            for (int i = 1; i <= 1; i++)
            {
                seleccionExitosa = false;
                do
                {
                    Console.WriteLine($"{fachada.GetJugadorConTurno().GetNombre()}, seleccione su Pokemon número {i}:");
                    input = Console.ReadLine();
                    seleccionExitosa = fachada.ElegirPokemon(input.ToUpper());
                } while (!seleccionExitosa);
            }
            Console.WriteLine("======================================================================" +
                              $"\nHas completado tu selección, {fachada.GetJugadorConTurno().GetNombre()}\n" +
                              "======================================================================");
            fachada.CambiarTurno();
        }


        Console.WriteLine("----------------------------------------------------------------------" +
                          $"\n\n                ⚔ INICIA EL COMBATE ⚔                      \n\n" +
                          "----------------------------------------------------------------------");
        Entrenador entrenadorConTurno;
        entrenadorConTurno = fachada.GetJugadorConTurno();// para manejar mas facil la variable
        for (int i = 0; i <= 1; i++)
        {
            seleccionExitosa = false;
            do
            {
                Console.WriteLine($"{entrenadorConTurno.GetNombre()}, seleccione el Pokemon con el que desea combatir:");
                entrenadorConTurno.ListaDePokemones();
                input = Console.ReadLine().ToUpper();
                seleccionExitosa = fachada.CambiarPokemonPor(input);
            } while (!seleccionExitosa);

            Console.WriteLine($"{entrenadorConTurno.GetNombre()} ha seleccionado a {input} para combatir");
            fachada.CambiarTurno();
            entrenadorConTurno = fachada.GetJugadorConTurno();
        }

        fachada.ChequearQuienEmpieza();
        Console.WriteLine($"{fachada.GetJugadorConTurno().GetNombre()} tiene al Pokemon más rápido");

        do
        {
            fachada.InformeDeSituacion();
            bool operacionExitosa = false;// chequea si se realizó alguna operación con éxito
                                          // de lo contrario muestra el menu principal de nuevo
            do
            {

                if (entrenadorConTurno.GetPokemonEnUso().GetVida() > 0)
                {
                    Console.WriteLine("Elija una acción: ");
                    Console.WriteLine("(1) Para atacar, " +
                                      "(2) Para cambiar de Pokemon, " +
                                      "(3) Para usar pocion, " +
                                      "(4) Para cancelar batalla y rendirse\n ");

                    input = Console.ReadLine() ?? throw new InvalidOperationException();
                }
                else
                {
                    Console.WriteLine("Debes cambiar de pokemón");
                    input = "2";
                }

                if (input == "1") // Compara con "1" en lugar de 1
                {
                    if (entrenadorConTurno.GetPokemonEnUso().EfectoActivo != "Paralizado" &&
                        entrenadorConTurno.GetPokemonEnUso().EfectoActivo != "Dormido")
                    {
                        string respuestaUsuario;
                        do
                        {
                            seleccionExitosa = false; // cheque si se eligio un ataque con exito
                            Console.WriteLine("Elija un ataque: ");
                            fachada.ListaAtaques(); // Muestra los ataques disponibles
                            Console.WriteLine("[BACK]");
                            respuestaUsuario = Console.ReadLine().ToUpper();
                            if (respuestaUsuario != "BACK")// para que no busque en la lista de atauqes
                            {
                                seleccionExitosa = fachada.Atacar(respuestaUsuario); // Intenta realizar el ataque con lo que recibio del usuario
                            }
                        } while (!seleccionExitosa && respuestaUsuario!="BACK");// se sale por ataque exitoso o por BACK

                        if (respuestaUsuario != "BACK")// Si fue ataque exitoso se termina el turno
                                                        //sino vuelve a desplegar menu principal
                        {
                            operacionExitosa = true; // se concretó el ataque
                        }
                    }
                    else
                    {
                        Console.WriteLine("Tu pokemón se encuentra paralizado o dormido no puedes atacar");
                    }
                }
                else if (input == "2")
                {
                    Console.WriteLine($"{entrenadorConTurno.GetNombre()}, seleccione el Pokemon con el que desea combatir:");
                    entrenadorConTurno.ListaDePokemones();
                    Console.WriteLine("[BACK]");
                    input = Console.ReadLine().ToUpper();
                    if (input != "BACK")
                    {
                        fachada.CambiarPokemonPor(input);
                        operacionExitosa = true; // se concreto el cambio
                    }
                }

                else if (input == "3")
                {
                    entrenadorConTurno.GetListaDeItems();
                    Console.WriteLine("[BACK]");
                    input = Console.ReadLine() ?? throw new InvalidOperationException();
                    if (input != "BACK")
                    {
                        operacionExitosa = entrenadorConTurno.UsarItem(input);
                    }
                }
                else if (input == "4")
                {
                    Console.WriteLine("Has decidido rendirte. Fin de la batalla. /n");
                    Console.WriteLine("Estas seguro:s/n");
                    input = Console.ReadLine();
                    input = input.ToUpper();
                    if (input == "S")
                    {
                        fachada.existeGanador = true;
                        operacionExitosa = true;
                    }
                }
                else
                {
                    Console.WriteLine("Opción no válida. Inténtalo de nuevo.");
                }

            } while (!operacionExitosa); // Cabe la posibilidad de que si la operacion no se concreto
                                        // vuelva la menu de opciones para nuevamente tomar una desicion
                                        // de esta forma no se pierda el turno, ante una operacion trunca.
            fachada.CambiarTurno();
            entrenadorConTurno = fachada.GetJugadorConTurno();
        } while (!fachada.ChequeoPantallaFinal());
    }
}

*/