namespace HundirLaFlota
{
    public class Program
    {
        private const int NUM_BARCOS_A_HUNDIR = 5;
        private static Tablero tableroJ1;
        private static Tablero tableroJ2;

        static void Main(string[] args)
        {
            bool opcionValida = false;
            string opcion=string.Empty;
            Console.WriteLine("HUNDIR LA FLOTA");
            Console.WriteLine("---------------");

            while (!opcionValida)
            {
                Console.WriteLine("1 -> Jugar contra otro jugador");
                Console.WriteLine("2 -> Jugar contra el ordenador");
                Console.WriteLine("Elige una opción:");

                opcion = Console.ReadLine();

                if (opcion == "1" || opcion == "2")
                {
                    Console.Clear();
                    if (opcion == "1")
                    {
                        ComienzoJuego(false);
                    }
                    else
                    {
                        ComienzoJuego(true);
                    }
                }
            }
        }

        private static void ComienzoJuego(bool soyIA = false)
        {
            Console.WriteLine("Eres el primer jugador. ¿Cómo te llamas?");
            string? j1 = Console.ReadLine();
            Console.WriteLine($"Empieza a colocar {j1}");

            // FASE 1 - Colocación de barcos

            // Empieza a colocar el player 1
            tableroJ1 = new Tablero(j1);
            CrearBarcosEnTablero(tableroJ1);

            // Empieza a colocar el player 2 / la IA
            string? j2;
            if (soyIA)
            {
                j2 = "Elon";
            }
            else
            {
                Console.WriteLine("Eres el segundo jugador. ¿Cómo te llamas?");
                j2 = Console.ReadLine();
            }
            Console.WriteLine($"Empieza a colocar {j2}.");
            tableroJ2 = new Tablero(j2);
            CrearBarcosEnTablero(tableroJ2, soyIA);

            // FASE 2 - La guerra
            Guerra(soyIA);

            // FASE 3 - Resultado
            if (tableroJ1.BarcosHundidosAlRival == NUM_BARCOS_A_HUNDIR)
            {
                PintaEnCuadrado($"¡Ganó {j1}!");
            }
            if (tableroJ2.BarcosHundidosAlRival == NUM_BARCOS_A_HUNDIR)
            {
                PintaEnCuadrado($"¡Ganó {j2}!");
            }
        }

        private static void CrearBarcosEnTablero(Tablero tablero, bool soyIA = false)
        {
            if (!soyIA)
            {
                tablero.PrintTablero(true);
            }

            // Generamos el barco de Cinco fichas
            Console.WriteLine("Creación del barco de longitud CINCO! ");
            Barco barcoDeCinco = new Barco(5);
            PosicionaBarcoEnTablero(tablero, barcoDeCinco, soyIA);

            // Generamos el barco de Cuatro fichas
            Console.WriteLine("Creación del barco de longitud CUATRO! ");
            Barco barcoDeCuatro = new Barco(4);
            PosicionaBarcoEnTablero(tablero, barcoDeCuatro, soyIA);

            // Generamos DOS barcos de Tres fichas
            Console.WriteLine("Creación del primer barco de longitud TRES! ");
            Barco barcoDeTres1 = new Barco(3);
            PosicionaBarcoEnTablero(tablero, barcoDeTres1, soyIA);

            Console.WriteLine("Creación del segundo barco de longitud TRES! ");
            Barco barcoDeTres2 = new Barco(3);
            PosicionaBarcoEnTablero(tablero, barcoDeTres2, soyIA);

            // Generamos el barco de Dos fichas
            Console.WriteLine("Creación del barco de longitud DOS! ");
            Barco barcoDeDos = new Barco(2);
            PosicionaBarcoEnTablero(tablero, barcoDeDos, soyIA);

            if (!soyIA)
            {
                Console.WriteLine("Esta es tu configuración. Pulsa <ENTER> para borrrarla de la pantalla");
                Console.ReadLine();
                Console.Clear();
            }
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
                    (inicio, fin) = CapturaRangoCoordenadasAleatorias(barco);
                }
                else
                {
                    (inicio, fin) = CapturaRangoCoordenadasDelUsuario(barco);
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

                posicionFactible = tablero.IntentaColocar(barco, inicio, fin, soyIA);
            }

            tablero.Coloca(barco, inicio, fin);

            if (!soyIA)
            {
                tablero.PrintTablero();
            }
        }

        private static void Guerra(bool soyIA = false)
        {
            Coordenada shoot;
            Coordenada? buenDisparo = null;
            int ronda = 0;

            while (tableroJ1.BarcosHundidosAlRival != NUM_BARCOS_A_HUNDIR && 
                tableroJ2.BarcosHundidosAlRival != NUM_BARCOS_A_HUNDIR) // se podría hacer con tableroJ1.QuedanBarcosAFlote()
            {
                ronda++;

                PintaEnCuadrado("Ronda " + ronda);
                Console.WriteLine($"Turno de {tableroJ1.Nombre}... ");
                Console.WriteLine();

                var resultados = EleccionArmaContra(tableroJ2, ronda);

                foreach (var resultado in resultados)
                {
                    if (resultado == ResultadoDisparo.Hundido)
                    {
                        tableroJ1.BarcosHundidosAlRival++;
                    }
                }

                //Incluso si el primer jugador ha hundido todo, el segundo jugador tiene opción a su último disparo para poder empatar
                Console.WriteLine("- CAMBIO DE JUGADOR -");
                Console.WriteLine();
                Console.WriteLine($"Turno de {tableroJ2.Nombre}... ");
                Console.WriteLine();
                //El segundo jugador puede disparar como IA si se ha requerido así
                if (soyIA)
                {
                    // A la IA no se le implementan las acciones de elegir arma. Siempre usará un disparo normal. 
                    buenDisparo = DisparoIA(tableroJ1, buenDisparo);
                }
                else
                {
                    resultados = EleccionArmaContra(tableroJ1, ronda);

                    foreach (var resultado in resultados)
                    {
                        if (resultado == ResultadoDisparo.Hundido)
                        {
                            tableroJ2.BarcosHundidosAlRival++;
                        }
                    }
                }
            }
        }

        private static List<ResultadoDisparo> EleccionArmaContra(Tablero tablero, int ronda)
        {
            bool opcionValida = false;
            string opcion = string.Empty;

            Console.WriteLine("N -> Disparo Normal");
            Console.WriteLine("S -> Super-Disparo");
            Console.WriteLine("D -> Disparo Doble");
            Console.WriteLine("R -> Radar");
            Console.WriteLine("Elige una opción:");

            while(true)
            {
                while (!opcionValida)
                {
                    opcion = Console.ReadLine()!.ToUpper();

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
                        }
                    case "S":
                        {
                            if (tablero.EstaHundidoAlgunBarcoDe(5))
                            {
                                Console.WriteLine($"Tiene tu rival el barco de tamaño 5 hundido, no puedes usar SuperDisparo");
                                opcionValida = false;
                                break;
                            }
                            // Al comenzar el turno no ha habido un último superdisparo, pero aún así no lo permitimos hasta la tercera ronda
                            if (ronda - tablero.UltimoSuperDisparo - 1 < 3)
                            {
                                Console.WriteLine($"Todavía debes esperar {4 - (ronda - tablero.UltimoSuperDisparo)} rondas para usar el SuperDisparo");
                                opcionValida = false;
                                break;
                            }

                            tablero.UltimoSuperDisparo = ronda;
                            return SuperDisparo(tablero);
                        }
                    case "D":
                        {
                            if (tablero.EstaHundidoAlgunBarcoDe(4))
                            {
                                Console.WriteLine($"Tiene tu rival el barco de tamaño 4 hundido, no puedes usar Disparo Doble");
                                opcionValida = false;
                                break;
                            }
                            // Al comenzar el turno no ha habido un último disparo doble, pero aún así no lo permitimos hasta la segunda ronda
                            if (ronda - tablero.UltimoDisparoDoble - 1 < 2)
                            {
                                Console.WriteLine($"Todavía debes esperar {3 - (ronda - tablero.UltimoDisparoDoble)} rondas para usar el Disparo Doble");
                                opcionValida = false;
                                break;
                            }

                            tablero.UltimoDisparoDoble = ronda;
                            return DisparoDoble(tablero);
                        }
                    case "R":
                        {
                            if (tablero.EstaHundidoAlgunBarcoDe(2))
                            {
                                Console.WriteLine($"Tiene tu rival el barco de tamaño 2 hundido, no puedes usar Radar");
                                opcionValida = false;
                                break;
                            }
                            if (tablero.FallosConsecutivos < 5)
                            {
                                Console.WriteLine($"Debes fallar {5 - tablero.FallosConsecutivos} rondas adicionales para usar el Radar");
                                opcionValida = false;
                                break;
                            }

                            tablero.UltimoRadar = ronda; //Actualmente no se usa, pero lo pongo por simetría con lo anterior
                            return Radar(tablero);
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private static List<ResultadoDisparo> Radar(Tablero tablero)
        {
            var resultados = new List<ResultadoDisparo>();

            Console.WriteLine("Mira!");
            tablero.PrintTablero();
            Thread.Sleep(1000);
            Console.Clear();

            return resultados;
        }

        private static List<ResultadoDisparo> DisparoDoble(Tablero tablero)
        {
            var resultados = DisparoNormal(tablero);
            var result1 = DisparoNormal(tablero);

            foreach (var result in result1)
            {
                resultados.Add(result);
            }

            return resultados;
        }

        private static List<ResultadoDisparo> SuperDisparo(Tablero tablero)
        {
            Coordenada shoot;
            var resultados = new List<ResultadoDisparo>();

            Console.WriteLine("Dispara!");

            // Las coordenadas son de la casilla central de la cruz del super-disparo. Se pierden los disparos fuera de rango
            shoot = CapturaCoordenadaDeLaConsola();
            Console.WriteLine($"SUPER-DISPARO con centro de cruz ({(char)(shoot.x + 65)}{shoot.y})");

            ResultadoDisparo result = DisparoBase(tablero, shoot);
            resultados.Add(result);

            result = DisparoBase(tablero, new Coordenada(shoot.x + 1, shoot.y));
            resultados.Add(result);

            result = DisparoBase(tablero, new Coordenada(shoot.x - 1, shoot.y));
            resultados.Add(result);

            result = DisparoBase(tablero, new Coordenada(shoot.x, shoot.y + 1));
            resultados.Add(result);

            result = DisparoBase(tablero, new Coordenada(shoot.x, shoot.y - 1));
            resultados.Add(result);

            return resultados;
        }

        private static Coordenada? DisparoIA(Tablero tablero, Coordenada? buenDisparo)
        {
            Console.WriteLine("Dispara!");

            Coordenada shoot = CapturaCoordenadaDeLaIA(buenDisparo);

            ResultadoDisparo result = DisparoBase(tablero, shoot);

            if (result == ResultadoDisparo.Hundido)
            {
                tableroJ2.BarcosHundidosAlRival++;
            }

            if (result == ResultadoDisparo.Tocado)
            {
                // Guardamos el tiro para que la IA se aproxime en el siguiente turno
                return shoot;
            }

            // La IA solo recuerda el disparo si ha habido un acierto en el intento inmediatamente anterior
            return null;
        }

        private static List<ResultadoDisparo> DisparoNormal(Tablero tablero)
        {
            Coordenada shoot;
            var resultados = new List<ResultadoDisparo>();

            Console.WriteLine("Dispara!");

            shoot = CapturaCoordenadaDeLaConsola();

            ResultadoDisparo result = DisparoBase(tablero, shoot);
            resultados.Add(result);

            return resultados;
        }

        private static ResultadoDisparo DisparoBase(Tablero tablero, Coordenada shoot)
        {
            Console.Write($"Ensayando disparo a ({(char)(shoot.x + 65)}{shoot.y})...   ");
            var result = tablero.ResultadoDelDisparo(shoot);
            ImprimeResultado(result);

            return result;
        }

        private static (Coordenada, Coordenada) CapturaRangoCoordenadasAleatorias(Barco barco)
        {
            Coordenada inicio = null;
            Coordenada fin = null;

            Random random = new Random(DateTime.Now.Microsecond);
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

        private static (Coordenada inicio, Coordenada fin) CapturaRangoCoordenadasDelUsuario(Barco barco)
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
                Random random = new Random(DateTime.Now.Microsecond);
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
                Random random = new Random(DateTime.Now.Microsecond);
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
            Console.WriteLine();
        }

        private static void PintaEnCuadrado(string texto)
        {
            for (int i = 0; i < texto.Length + 4; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
            Console.WriteLine("* " + texto + " *");
            for (int i = 0; i < texto.Length + 4; i++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
