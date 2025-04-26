namespace HundirLaFlota
{
    internal class Program
    {
        private static Tablero tableroJ1 = new(10, 10);  //No está preparada la app para otros valores
        private static Tablero tableroJ2 = new Tablero(10, 10);  //No está preparada la app para otros valores
        static void Main(string[] args)
        {
            // FASE 1 - Colocación de barcos

            // Empieza a colocar el player 1
            if (!CrearBarcosEnTablero(tableroJ1))
            {
                //Habría que repetir la intro de todos los barcos => Mejorable pero no en esta version, que asumimos que la entrada es correcta
            }

            // Empieza a colocar el player 2
            if (!CrearBarcosEnTablero(tableroJ2))
            {
                //Habría que repetir la intro de todos los barcos => Mejorable pero no en esta version, que asumimos que la entrada es correcta
            }

            // FASE 2 - La guerra
            if (!War())
            {
                // Ha acabado la guerra por algún motivo
            }
            // Dispara player-1
            // se mira en el tablero-2 el valor que hay (agua/tocado)
            // se cambia el valor del tablero-2 al estado (aguatocado / barcotocado)
        }

        private static bool War()
        {
            while(true)
            {
                Console.WriteLine("Jugador 1... Dispara!");
                Coordenada shoot = GetPointFromConsole();
                ResultadoDisparo result = tableroJ2.GetShootResult(shoot);

            }
        }

        private static bool CrearBarcosEnTablero(Tablero tablero)
        {
            // Generamos el barco de Cinco fichas
            Console.WriteLine("Vamos a crear el barco de longitud CINCO! ");
            // Pedimos coordenadas iniciales del barco
            Console.WriteLine("En qué coordenadas empieza? ");
            Coordenada inicio = GetPointFromConsole();
            // Pedimos coordenadas finales del barco
            Console.WriteLine("En qué coordenadas termina? ");
            Coordenada fin = GetPointFromConsole();

            Barco BarcoDeCinco = new Barco(5);
            
            if (!tablero.IntentaColocar(BarcoDeCinco, inicio, fin))
            {
                Console.WriteLine("Algo falló"); //ERROR
                return false;
            }

            // Generamos el barco de Cuatro fichas
            Console.WriteLine("Vamos a crear el barco de longitud CUATRO! ");
            // Pedimos coordenadas iniciales del barco
            Console.WriteLine("En qué coordenadas empieza? ");
            inicio = GetPointFromConsole();
            // Pedimos coordenadas finales del barco
            Console.WriteLine("En qué coordenadas termina? ");
            fin = GetPointFromConsole();

            Barco BarcoDeCuatro = new Barco(4);
            if (!tablero.IntentaColocar(BarcoDeCuatro, inicio, fin))
            {
                Console.WriteLine("Algo falló"); //ERROR
                return false;
            }

            // Generamos DOS barcos de Tres fichas
            Console.WriteLine("Vamos a crear el primer barco de longitud TRES! ");
            // Pedimos coordenadas iniciales del barco
            Console.WriteLine("En qué coordenadas empieza? ");
            inicio = GetPointFromConsole();
            // Pedimos coordenadas finales del barco
            Console.WriteLine("En qué coordenadas termina? ");
            fin = GetPointFromConsole();

            Barco BarcoDeTres1 = new Barco(3);
            if (!tablero.IntentaColocar(BarcoDeTres1, inicio, fin))
            {
                Console.WriteLine("Algo falló"); //ERROR
                return false;
            }

            Console.WriteLine("Vamos a crear el segundo barco de longitud TRES! ");
            // Pedimos coordenadas iniciales del barco
            Console.WriteLine("En qué coordenadas empieza? ");
            inicio = GetPointFromConsole();
            // Pedimos coordenadas finales del barco
            Console.WriteLine("En qué coordenadas termina? ");
            fin = GetPointFromConsole();

            Barco BarcoDeTres2 = new Barco(3);
            if (!tablero.IntentaColocar(BarcoDeTres2, inicio, fin))
            {
                Console.WriteLine("Algo falló"); //ERROR
                return false;
            }

            // Generamos el barco de Dos fichas
            Console.WriteLine("Vamos a crear el barco de longitud DOS! ");
            // Pedimos coordenadas iniciales del barco
            Console.WriteLine("En qué coordenadas empieza? ");
            inicio = GetPointFromConsole();
            // Pedimos coordenadas finales del barco
            Console.WriteLine("En qué coordenadas termina? ");
            fin = GetPointFromConsole();

            Barco BarcoDeDos = new Barco(2);
            if (!tablero.IntentaColocar(BarcoDeDos, inicio, fin))
            {
                Console.WriteLine("Algo falló"); //ERROR
                return false;
            }

            PrintTablero(tablero.GetTablero());
            return true;
        }

        private static void PrintTablero(Estado[,] tablero)
        {
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    Console.WriteLine(tablero[i, j] + "    ");
                }
                Console.Write("");
            }
            Console.Read();
        }

        private static Coordenada GetPointFromConsole()
        {
            string? coordenadaFromConsole = Console.ReadLine();

            // Debe ser dos caracteres
            // El primero debe estar entre la A y la J
            // Se permiten minúsculas (no es case-sensitive)
            if (coordenadaFromConsole?.Length != 2)
            {
                return new Coordenada(-1, -1); // ERROR
            }

            char PrimerCaracter = coordenadaFromConsole.ToUpper()[0]; // Estamos asumiendo que es mayúscula
            char SegundoCaracter = coordenadaFromConsole[1];

            // Verificamos que está en [A-J]
            if (PrimerCaracter < 65 || PrimerCaracter > 75)
            {
                return new Coordenada(-1, -1); // ERROR
            }
            // Verificamos que está en [0-9]
            if (SegundoCaracter < 48 || SegundoCaracter > 57)
            {
                return new Coordenada(-1, -1); // ERROR
            }

            return new Coordenada(Convert.ToInt32(PrimerCaracter) - 65, Convert.ToInt32(SegundoCaracter) - 48);
        }
    }
}
