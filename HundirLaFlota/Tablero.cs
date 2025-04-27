using System.Runtime.InteropServices.JavaScript;

namespace HundirLaFlota
{
    internal class Tablero
    {
        private readonly Barco[,] _tablero;

        public Tablero(int v1, int v2)
        {
            _tablero = new Barco [v1, v2];
        }

        public bool IntentaColocar(Barco barco, Coordenada inicio, Coordenada fin)
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
                    if (_tablero[i, fin.y] != null)
                    {
                        Console.WriteLine($"Hay ya otro barco ocupando la posición ({i},{fin.y})");
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
                    if (_tablero[fin.x, i] != null)
                    {
                        Console.WriteLine($"Hay ya otro barco ocupando la posición ({fin.x},{i})");
                        return false;
                    }
                }
            }

            return true;
        }

        internal void Coloca(Barco barco, Coordenada inicio, Coordenada fin)
        {
            int rangoLetras = Math.Abs(inicio.x - fin.x);
            // Marcamos el barco en el tablero en horizontal o vertical
            if (rangoLetras > 0)
            {
                // usamos min y max por si el usuario ha introducido los rangos al revés de lo esperable
                int min = Math.Min(inicio.x, fin.x);
                int max = Math.Max(inicio.x, fin.x);
                for (int i = min; i <= max; i++)
                {
                    _tablero[i, fin.y] = barco;
                }
            }
            else
            {
                int min = Math.Min(inicio.y, fin.y);
                int max = Math.Max(inicio.y, fin.y);
                for (int i = min; i <= max; i++)
                {
                    _tablero[fin.x, i] = barco;
                }
            }
        }

        internal ResultadoDisparo ResultadoDelDisparo(Coordenada shoot)
        {
            // recuperamos el barco que está en esa posición (si hay alguno)
            Barco barco = _tablero[shoot.x, shoot.y];

            if (barco != null)
            {
                //Anotamos el disparo y devolvemos
                barco.Tocado(shoot);

                //Comprobamos si solo está tocado o está totalmente hundido
                if (barco.EstaHundido())
                {
                    return ResultadoDisparo.Hundido;
                }
                else
                {
                    return ResultadoDisparo.Tocado;
                }
            }

            return ResultadoDisparo.Agua;
        }
        public bool QuedanBarcosAFlote()
        {
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    Barco barco = _tablero[i, j];
                    if (barco != null)
                    {
                        if (!barco.EstaHundido())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void PrintTablero(bool imprimeVacio = false)
        {
            Console.WriteLine("     0    1    2    3    4    5    6    7    8    9");
            for (int i = 0; i < 10; i++)
            {
                Console.Write((char)(65 + i) + "  ");
                for (int j = 0; j < 10; j++)
                {
                    Barco barco = _tablero[i, j];
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

            if (!imprimeVacio)
            {
                Console.WriteLine("Esta es tu configuración. Pulsa una tecla para borrrarla de la pantalla");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}