namespace HundirLaFlota
{
    internal class Program
    {
        private const int NUM_BARCOS_A_HUNDIR = 5;
        private static Tablero tableroJ1 = new(10, 10);  //No está preparada la app para otros valores
        private static Tablero tableroJ2 = new Tablero(10, 10);  //No está preparada la app para otros valores
        private static int barcosHundidosPorJ1 = 0;
        private static int barcosHundidosPorJ2 = 0;
        static void Main(string[] args)
        {
            // FASE 1 - Colocación de barcos

            // Empieza a colocar el player 1

            Console.WriteLine("Empieza a colocar el player 1");
            if (!CrearBarcosEnTablero(tableroJ1))
            {
                //Habría que repetir la intro de todos los barcos => Mejorable pero no en esta version, que asumimos que la entrada es correcta
            }

            // Empieza a colocar el player 2
            Console.WriteLine("Empieza a colocar el player 2");
            if (!CrearBarcosEnTablero(tableroJ2))
            {
                //Habría que repetir la intro de todos los barcos => Mejorable pero no en esta version, que asumimos que la entrada es correcta
            }

            // FASE 2 - La guerra
            Guerra();

            // FASE 3 - Resultado
            if (barcosHundidosPorJ1 == NUM_BARCOS_A_HUNDIR)
            {
                Console.WriteLine("¡Ganó J1!");
            }
            if (barcosHundidosPorJ2 == NUM_BARCOS_A_HUNDIR)
            {
                Console.WriteLine("¡Ganó J2!");
            }
        }
        private static bool CrearBarcosEnTablero(Tablero tablero)
        {
            tablero.PrintTablero(true);

            // Generamos el barco de Cinco fichas
            Console.WriteLine("Vamos a crear el barco de longitud CINCO! ");
            Barco barcoDeCinco = new Barco(5);
            PosicionaBarcoEnTablero(tablero, barcoDeCinco);

            // Generamos el barco de Cuatro fichas
            Console.WriteLine("Vamos a crear el barco de longitud CUATRO! ");
            Barco barcoDeCuatro = new Barco(4);
            PosicionaBarcoEnTablero(tablero, barcoDeCuatro);

            // Generamos DOS barcos de Tres fichas
            Console.WriteLine("Vamos a crear el primer barco de longitud TRES! ");
            Barco barcoDeTres1 = new Barco(3);
            PosicionaBarcoEnTablero(tablero, barcoDeTres1);
            Console.WriteLine("Vamos a crear el segundo barco de longitud TRES! ");
            Barco barcoDeTres2 = new Barco(3);
            PosicionaBarcoEnTablero(tablero, barcoDeTres2);

            // Generamos el barco de Dos fichas
            Console.WriteLine("Vamos a crear el barco de longitud DOS! ");
            Barco barcoDeDos = new Barco(2);
            PosicionaBarcoEnTablero(tablero, barcoDeDos);

            tablero.PrintTablero();
            return true;
        }

        private static void PosicionaBarcoEnTablero(Tablero tablero, Barco barco)
        {
            bool posicionFactible = false;
            Coordenada inicio = null;
            Coordenada fin = null;

            while (!posicionFactible)
            {
                // Pedimos coordenadas iniciales del barco
                Console.WriteLine("En qué coordenadas empieza? ");
                inicio = CapturaCoordenadaDeLaConsola();
                // Pedimos coordenadas finales del barco
                Console.WriteLine("En qué coordenadas termina? ");
                fin = CapturaCoordenadaDeLaConsola();

                posicionFactible = tablero.IntentaColocar(barco, inicio, fin);
            }
            tablero.Coloca(barco, inicio, fin);
        }

        private static void Guerra()
        {
            while(barcosHundidosPorJ1!= NUM_BARCOS_A_HUNDIR && barcosHundidosPorJ2 != NUM_BARCOS_A_HUNDIR) // se podría hacer con tableroJ1.QuedanBarcosAFlote()
            {
                Console.WriteLine("Jugador 1... Dispara!");
                Coordenada shoot = CapturaCoordenadaDeLaConsola();
                ResultadoDisparo result = tableroJ2.ResultadoDelDisparo(shoot);
                ImprimeResultado(result);
                if (result == ResultadoDisparo.Hundido)
                {
                    barcosHundidosPorJ1++;
                }

                //Incluso si el primer jugador ha hundido todo, el segundo jugador tiene opción a su último disparo para poder empatar
                Console.WriteLine("Jugador 2... Dispara!");
                shoot = CapturaCoordenadaDeLaConsola();
                result = tableroJ1.ResultadoDelDisparo(shoot);
                ImprimeResultado(result);
                if (result == ResultadoDisparo.Hundido)
                {
                    barcosHundidosPorJ2++;
                }
            }
        }

        private static void ImprimeResultado(ResultadoDisparo result)
        {
            switch (result)
            {
                case ResultadoDisparo.Agua:
                    {
                        Console.WriteLine("¡AGUA!");
                        break;
                    }
                case ResultadoDisparo.Tocado:
                    {
                        Console.WriteLine("¡TOCADO!");
                        break;
                    }
                case ResultadoDisparo.Hundido:
                    {
                        Console.WriteLine("¡HUNDIDO!");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("ERROR");
                        break;
                    }
            }
        }
        private static Coordenada CapturaCoordenadaDeLaConsola()
        {
            bool rangoXOk = false;
            bool rangoYOk = false;
            char PrimerCaracter ='\0';
            char SegundoCaracter = '\0';

            while (!rangoXOk || !rangoYOk)
            {
                string? coordenadaFromConsole = Console.ReadLine();

                // Debe ser dos caracteres
                if (coordenadaFromConsole?.Length == 2)
                {
                    PrimerCaracter = coordenadaFromConsole.ToUpper()[0];
                    SegundoCaracter = coordenadaFromConsole[1];
                }

                // Verificamos que está en [A-J]
                rangoXOk = PrimerCaracter > 64 && PrimerCaracter < 76;

                // Verificamos que está en [0-9]
                rangoYOk = SegundoCaracter > 47 && SegundoCaracter < 58;
            }

            return new Coordenada(Convert.ToInt32(PrimerCaracter) - 65, Convert.ToInt32(SegundoCaracter) - 48);
        }
    }
}
