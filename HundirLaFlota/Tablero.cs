using System.Runtime.InteropServices.JavaScript;

namespace HundirLaFlota
{
    public class Tablero
    {
        private readonly Barco[,] _tableroBarcos;
        private readonly PintarDisparo[,] _tableroDisparos;
        private readonly List<Barco> _barcos;

        public string Nombre {get; }

        public int UltimoSuperDisparo { get; set; }

        public int UltimoDisparoDoble { get; set; }

        public int UltimoRadar { get; set; }

        public int FallosConsecutivos { get; set; }

        public int BarcosHundidosAlRival { get; set; }

        public Tablero(string nombre)
        {
            Nombre = nombre;
            _tableroBarcos = new Barco [10, 10];
            _tableroDisparos = new PintarDisparo[10, 10];
            _barcos = new List<Barco>();
        }

        public bool IntentaColocar(Barco barco, Coordenada inicio, Coordenada fin, bool soyIA = false)
        {
            int rangoLetras = Math.Abs(inicio.x - fin.x);
            int rangoNumeros = Math.Abs(inicio.y - fin.y);

            // uno y solo uno de los dos valores debe ser cero
            if (rangoNumeros * rangoLetras != 0)
            {
                Console.WriteLine("Los barcos solo pueden colocarse horizontalmente o verticalmente");
                return false;
            }
            if (rangoNumeros + rangoLetras == 0)
            {
                Console.WriteLine("El rango no coincide con las dimensiones del barco");
                return false;
            }

            // Debe coincidir el tamaño del barco con las coordenadas 
            if (barco.Length -1 != rangoNumeros + rangoLetras)
            {
                Console.WriteLine("El rango no coincide con las dimensiones del barco");
                return false;
            }

            // Marcamos el barco en el tablero en horizontal o vertical
            if (rangoLetras > 0)
            {
                // usamos min y max por si el usuario ha introducido los rangos al revés de lo esperable
                int min = Math.Min(inicio.x, fin.x);
                int max = Math.Max(inicio.x, fin.x);
                for (int i = min; i <= max; i++)
                {
                    //Si hay ya algún barco en ese rango, no se puede superponer el nuevo
                    if (_tableroBarcos[i, fin.y] != null)
                    {
                        if (!soyIA)
                        {
                            Console.WriteLine($"Hay ya otro barco ocupando la posición ({(char)(65 + i)},{fin.y})");
                        }
                        return false; 
                    }
                }
            }
            else
            {
                int min = Math.Min(inicio.y, fin.y);
                int max = Math.Max(inicio.y, fin.y);
                for (int i = min; i <= max; i++)
                {
                    if (_tableroBarcos[fin.x, i] != null)
                    {
                        if (!soyIA)
                        {
                            Console.WriteLine($"Hay ya otro barco ocupando la posición ({(char)(65 + fin.x)},{i})");
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        public void Coloca(Barco barco, Coordenada inicio, Coordenada fin)
        {
            int rangoLetras = Math.Abs(inicio.x - fin.x);
            // Marcamos el barco en el tablero en horizontal o vertical
            if (rangoLetras > 0) // Horizontal
            {
                // usamos min y max por si el usuario ha introducido los rangos al revés de lo esperable
                int min = Math.Min(inicio.x, fin.x);
                int max = Math.Max(inicio.x, fin.x);
                for (int i = min; i <= max; i++)
                {
                    _tableroBarcos[i, fin.y] = barco;
                }
            }
            else
            {
                int min = Math.Min(inicio.y, fin.y);
                int max = Math.Max(inicio.y, fin.y);
                for (int i = min; i <= max; i++)
                {
                    _tableroBarcos[fin.x, i] = barco;
                }
            }
            // Guardamos el barco en una lista de barcos propios para ver rápidamente cuáles están hundidos
            _barcos.Add(barco);
        }

        public ResultadoDisparo ResultadoDelDisparo(Coordenada shoot)
        {
            // validamos las coordenadas pedidas
            if (shoot.x < 0 || shoot.x > 9 || shoot.y < 0 || shoot.y > 9)
            {
                return ResultadoDisparo.NoValido;
            }
            // recuperamos el barco que está en esa posición (si hay alguno)
            Barco barco = _tableroBarcos[shoot.x, shoot.y];

            if (barco != null)
            {
                //Anotamos el disparo, con lo que si es válido (no se ha repetido el tiro ni estaba hundido), reiniciamos los fallos consecutivos
                var resultado = barco.Tocado(shoot);
                if (resultado == ResultadoDisparo.Tocado || resultado==ResultadoDisparo.Hundido)
                {
                    //Inicializamos los fallos consecutivos
                    FallosConsecutivos = 0;
                    //Se marca tocado aunque sea hundido, es solo para visualizar el disparo 
                    _tableroDisparos[shoot.x, shoot.y] = PintarDisparo.Tocado;
                    return resultado;
                }
            }

            FallosConsecutivos++;
            // Si es AGUA, no validamos que el tiro sea erróneo aunque esté repetido. Devolvemos agua de nuevo
            _tableroDisparos[shoot.x, shoot.y] = PintarDisparo.Agua;
            return ResultadoDisparo.Agua;
        }

        public bool QuedanBarcosAFlote()
        {
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    Barco barco = _tableroBarcos[i, j];
                    if (barco != null && !barco.EstaHundido())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool EstaHundidoAlgunBarcoDe(int longitud)
        {
            foreach (Barco barco in _barcos)
            {
                if (barco.Length == longitud)
                {
                    return barco.EstaHundido();
                }
            }
            return true;
        }

        public void MuestraTableroBarcos(bool imprimeVacio = false)
        {
            Console.WriteLine("     0    1    2    3    4    5    6    7    8    9");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)(65 + i) + "  ");
                for (int j = 0; j < 10; j++)
                {
                    Barco barco = _tableroBarcos[i, j];
                    if (barco != null && !imprimeVacio)
                    {
                        Console.Write(" |X| ");
                    }
                    else
                    {
                        Console.Write(" | | ");
                    }
                }
                Console.WriteLine("");
            }
        }

        public void MuestraTableroDisparos()
        {
            Console.WriteLine("     0    1    2    3    4    5    6    7    8    9");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)(65 + i) + "  ");
                for (int j = 0; j < 10; j++)
                {
                    var resultado = _tableroDisparos[i, j];
                    switch(resultado)
                    {
                        case PintarDisparo.Tocado:
                            {
                                Console.Write(" |O| ");
                                break;
                            }
                        case PintarDisparo.Agua:
                            {
                                Console.Write(" |-| ");
                                break;
                            }
                        default:
                            {
                                Console.Write(" | | ");
                                break;
                            }
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}