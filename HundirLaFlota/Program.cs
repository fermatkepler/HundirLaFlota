using System.Runtime.InteropServices.JavaScript;

namespace HundirLaFlota
{
    internal class Program
    {
        private const int NUM_BARCOS_A_HUNDIR = 2;
        private static Tablero tableroJ1 = new(10, 10);  //No está preparada la app para otros valores
        private static Tablero tableroJ2 = new Tablero(10, 10);  //No está preparada la app para otros valores
        private static int barcosHundidosPorJ1 = 0;
        private static int barcosHundidosPorJ2 = 0;
        static void Main(string[] args)
        {
            bool opcionValida = false;
            string opcion=string.Empty;
            Console.WriteLine("HUNDIR LA FLOTA");
            Console.WriteLine("---------------");

            Console.WriteLine("1 -> Jugar contra otro jugador");
            Console.WriteLine("2 -> Jugar contra el ordenador");
            Console.WriteLine("Elige una opción:");

            while (!opcionValida)
            {
                opcion = Console.ReadLine();

                if (opcion == "1" || opcion == "2")
                {
                    opcionValida = true;
                }
            }

            if (opcion=="1")
            {
                VersusPlayer();
            }
            else
            {
                VersusIA();
            }
        }

        private static void VersusIA()
        {
            // FASE 1 - Colocación de barcos

            // Empieza a colocar el player 1

            Console.WriteLine("Empieza a colocar el player 1");
            if (!CrearBarcosEnTablero(tableroJ1))
            {
                //Habría que repetir la intro de todos los barcos => Mejorable pero no en esta version, que asumimos que la entrada es correcta
            }

            // Empieza a colocar la IA
            Console.WriteLine("Empieza a colocar la IA");
            CrearBarcosEnTablero(tableroJ2, true);

            // FASE 2 - La guerra
            Guerra(true);

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

        private static void VersusPlayer()
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

        private static void Guerra(bool soyIA = false)
        {
            Coordenada shoot;
            Coordenada buenDisparo = null;

            while (barcosHundidosPorJ1 != NUM_BARCOS_A_HUNDIR && barcosHundidosPorJ2 != NUM_BARCOS_A_HUNDIR) // se podría hacer con tableroJ1.QuedanBarcosAFlote()
            {
                Console.WriteLine("Jugador 1... ");
                var resultados = EleccionArmaContra(tableroJ2);
                foreach (var resultado in resultados)
                {
                    if (resultado == ResultadoDisparo.Hundido)
                    {
                        barcosHundidosPorJ1++;
                    }
                }

                //Incluso si el primer jugador ha hundido todo, el segundo jugador tiene opción a su último disparo para poder empatar
                Console.WriteLine("Jugador 2... ");
                //El segundo jugador puede disparar como IA si se ha requerido así
                if (soyIA)
                {
                    Console.WriteLine("Dispara!");
                    // A la IA no se le implementan las acciones de elegir arma. Siempre usará un disparo normal. 
                    shoot = CapturaCoordenadaDeLaIA(buenDisparo);
                    Console.WriteLine($"Ensayando disparo a ({(char)(shoot.x + 65)}{shoot.y})");
                    ResultadoDisparo result = tableroJ1.ResultadoDelDisparo(shoot);
                    ImprimeResultado(result);

                    if (result == ResultadoDisparo.Tocado)
                    {
                        // Guardamos el tiro para que la IA se aproxime en el siguiente turno
                        buenDisparo = shoot;
                    }
                    else
                    {
                        buenDisparo = null;
                    }
                }
                else
                {
                    resultados = EleccionArmaContra(tableroJ1);
                    foreach (var resultado in resultados)
                    {
                        if (resultado == ResultadoDisparo.Hundido)
                        {
                            barcosHundidosPorJ2++;
                        }
                    }
                }
            }
        }

        private static List<ResultadoDisparo> EleccionArmaContra(Tablero tablero)
        {
            bool opcionValida = false;
            string opcion = string.Empty;

            Console.WriteLine("N -> Disparo Normal");
            Console.WriteLine("S -> Super-Disparo");
            Console.WriteLine("D -> Disparo Doble");
            Console.WriteLine("R -> Radar");
            Console.WriteLine("Elige una opción:");

            while (!opcionValida)
            {
                opcion = Console.ReadLine().ToUpper();

                if (opcion == "N" || opcion == "S" || opcion == "D" || opcion == "R")
                {
                    opcionValida = true;
                }
            }
            switch (opcion)
            {
                case "N":
                    {
                        return DisparoNormal(tablero);
                        break;
                    }
                case "S":
                    {
                        return SuperDisparo(tablero);
                        break;
                    }
                case "D":
                    {
                        break;
                    }
                case "R":
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return new List<ResultadoDisparo>();
        }

        private static List<ResultadoDisparo> SuperDisparo(Tablero tablero)
        {
            throw new NotImplementedException();
        }

        private static List<ResultadoDisparo> DisparoNormal(Tablero tablero)
        {
            Coordenada shoot;
            var resultados = new List<ResultadoDisparo>();

            Console.WriteLine("Dispara!");

            shoot = CapturaCoordenadaDeLaConsola();
            Console.WriteLine($"Ensayando disparo a ({(char)(shoot.x + 65)}{shoot.y})");

            ResultadoDisparo result = tablero.ResultadoDelDisparo(shoot);
            ImprimeResultado(result);

            resultados.Add(result);

            return resultados;
        }

        private static bool CrearBarcosEnTablero(Tablero tablero, bool soyIA = false)
        {
            tablero.PrintTablero(true);

            // Generamos el barco de Cinco fichas
            Console.WriteLine("Vamos a crear el barco de longitud CINCO! ");
            Barco barcoDeCinco = new Barco(5);
            PosicionaBarcoEnTablero(tablero, barcoDeCinco, soyIA);

            // Generamos el barco de Cuatro fichas
            Console.WriteLine("Vamos a crear el barco de longitud CUATRO! ");
            Barco barcoDeCuatro = new Barco(4);
            PosicionaBarcoEnTablero(tablero, barcoDeCuatro, soyIA);

            //// Generamos DOS barcos de Tres fichas
            //Console.WriteLine("Vamos a crear el primer barco de longitud TRES! ");
            //Barco barcoDeTres1 = new Barco(3);
            //PosicionaBarcoEnTablero(tablero, barcoDeTres1, soyIA);
            //Console.WriteLine("Vamos a crear el segundo barco de longitud TRES! ");
            //Barco barcoDeTres2 = new Barco(3);
            //PosicionaBarcoEnTablero(tablero, barcoDeTres2, soyIA);

            //// Generamos el barco de Dos fichas
            //Console.WriteLine("Vamos a crear el barco de longitud DOS! ");
            //Barco barcoDeDos = new Barco(2);
            //PosicionaBarcoEnTablero(tablero, barcoDeDos, soyIA);

            if (!soyIA)
            {
                tablero.PrintTablero();
            }

            return true;
        }

        private static void PosicionaBarcoEnTablero(Tablero tablero, Barco barco, bool soyIA = false)
        {
            bool posicionFactible = false;
            Coordenada inicio = null;
            Coordenada fin = null;

            while (!posicionFactible)
            {
                if (soyIA)
                {
                    (inicio, fin) = CapturaCoordenadasAleatorias(barco);
                }
                else
                {
                    (inicio, fin) = CapturaCoordenadasDelUsuario(barco);
                }

                // Establecemos la posición del barco en función de sus coordenadas
                if (inicio.x == fin.x)
                {
                    barco.Posicion = Posicion.Horizontal;
                }
                else
                {
                    barco.Posicion = Posicion.Vertical;
                }

                posicionFactible = tablero.IntentaColocar(barco, inicio, fin);
            }
            tablero.Coloca(barco, inicio, fin);
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

        private static (Coordenada, Coordenada) CapturaCoordenadasAleatorias(Barco barco)
        {
            Coordenada inicio = null;
            Coordenada fin = null;

            Random random = new Random();
            bool esVertical = random.Next(2) > 1;
            int comienza = random.Next(10 - barco.Length);
            int filaColumna = random.Next(10);
            if (esVertical)
            {
                inicio = new Coordenada(comienza, filaColumna);
                fin = new Coordenada(comienza + barco.Length - 1, filaColumna);
            }
            else
            {
                inicio = new Coordenada(filaColumna, comienza);
                fin = new Coordenada(filaColumna, comienza + barco.Length - 1);
            }

            return (inicio, fin);
        }

        private static (Coordenada inicio, Coordenada fin) CapturaCoordenadasDelUsuario(Barco barco)
        {
            // Sería más sencillo tal vez si se diese a elegir al usuario la posición del barco (horizontal / vertical) y las coordenadas de un extremo del mismo
            Coordenada inicio = null;
            Coordenada fin = null;

            // Pedimos coordenadas iniciales del barco
            Console.WriteLine("En qué coordenadas empieza? ");
            inicio = CapturaCoordenadaDeLaConsola();
            // Pedimos coordenadas finales del barco
            Console.WriteLine("En qué coordenadas termina? ");
            fin = CapturaCoordenadaDeLaConsola();

            return (inicio, fin);
        }

        private static Coordenada CapturaCoordenadaDeLaIA(Coordenada buenDisparo)
        {
            int nuevaX = 0;
            int nuevaY = 0;
            if (buenDisparo != null) //con esta coordenada se tocó un barco enemigó
            {
                //Aleatorizamos sumando o restando uno a la coordenada vertical o a la horizontal (a la vez no!)
                Random random = new Random();
                bool esVertical = random.Next(2) > 1;
                if (esVertical)
                {
                    nuevaX = buenDisparo.x;
                    switch (buenDisparo.y)
                    {
                        case 0:
                            {
                                nuevaY = buenDisparo.y + 1;
                                break;
                            }
                        case 9:
                            {
                                nuevaY = buenDisparo.y - 1;
                                break;
                            }
                        default:
                            {
                                int incremento = random.Next(0, 1) == 0 ? -1 : 1;
                                nuevaY = buenDisparo.y + incremento;
                                break;
                            }
                    }
                }
                else
                {
                    nuevaY = buenDisparo.y;
                    switch (buenDisparo.x)
                    {
                        case 0:
                            {
                                nuevaX = buenDisparo.x + 1;
                                break;
                            }
                        case 9:
                            {
                                nuevaX = buenDisparo.x - 1;
                                break;
                            }
                        default:
                            {
                                int incremento = random.Next(0, 1) == 0 ? -1 : 1;
                                nuevaX = buenDisparo.x + incremento;
                                break;
                            }
                    }
                }

                return new Coordenada(nuevaX, nuevaY);
            }
            else
            {
                Random random = new Random();
                nuevaX = random.Next(10);
                nuevaY = random.Next(10);
                return new Coordenada(nuevaX, nuevaY);
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
