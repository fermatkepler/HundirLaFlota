namespace HundirLaFlota
{
    internal class Tablero
    {
        private readonly Estado[,] _tablero;

        public Tablero(int v1, int v2)
        {
            _tablero = new Estado [v1, v2];
        }

        public bool IntentaColocar(Barco barco, Coordenada inicio, Coordenada fin)
        {
            int horizontalSize = Math.Abs(inicio.x - fin.x);
            int verticalSize = Math.Abs(inicio.y - fin.y);

            // uno y solo uno de los dos valores debe ser cero
            if (horizontalSize * verticalSize != 0)
            {
                return false;
            }
            if (horizontalSize + verticalSize == 0)
            {
                return false;
            }

            // Debe coincidir el tamaño del barco con las coordenadas 
            if (barco.Length -1 != horizontalSize + verticalSize)
            {
                return false;
            }

            // Marcamos el barco en el tablero en horizontal o vertical
            if (horizontalSize > 0)
            {
                //Si hay ya algún barco en ese rango, no se puede superponer el nuevo, dicho de otra forma: todo el rango debe ser AGUA

                for (int i = inicio.x; i <= fin.x; i++)
                {
                    if (_tablero[i, fin.y] == Estado.Agua)
                    {
                        _tablero[i, fin.y] = Estado.Barco;
                    }
                    else 
                    { 
                        return false; 
                    }
                }
            }
            else
            {
                for (int i = inicio.y; i <= fin.y; i++)
                {
                    if (_tablero[fin.x, i] == Estado.Agua)
                    {
                        _tablero[fin.x, i] = Estado.Barco;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Estado[,] GetTablero()
        {
            return _tablero;
        }

        internal ResultadoDisparo GetShootResult(Coordenada shoot)
        {
            if (_tablero[shoot.x, shoot.y].Equals(Estado.Agua))
            {
                //Anotamos el disparo y devolvemos
                _tablero[shoot.x, shoot.y] = Estado.AguaTocada;
                return ResultadoDisparo.Agua;
            }

            if (_tablero[shoot.x, shoot.y].Equals(Estado.Barco))
            {
                //Anotamos el disparo y devolvemos
                _tablero[shoot.x, shoot.y] = Estado.BarcoTocado;
                // recuperamos el barco que está en esa posición
                Barco barco = _tablero[shoot.x, shoot.y];
                //Comprobamos si solo está tocado o está totalmente hundido
                if (barco.hundido)
                {
                    return ResultadoDisparo.Hundido;
                }
                else
                {
                    return ResultadoDisparo.Tocado;
                }
            }
            //Si todo ha ido bien, aquí no debe llegar
            return ResultadoDisparo.Agua;
        }
    }
}