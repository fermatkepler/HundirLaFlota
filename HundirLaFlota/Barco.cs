

namespace HundirLaFlota
{
    public class Barco
    {
        public int Length { get; }

        // La posición la sacaremos de las coordenadas iniciales y finales
        public Posicion Posicion { get; set; }
        public EstadoBarco Estado { get; set; }
        public int ImpactosPendientes { get; set; }

        private Coordenada[] _disparos; 
        public Barco(int length)
        {
            this.Length = length;
            this.ImpactosPendientes = length;
            this.Estado = EstadoBarco.Indemne;

            _disparos = new Coordenada[length];
        }

        public ResultadoDisparo Tocado(Coordenada shoot)
        {
            // No debe estar ya hundido. Tal vez se debería contar como fallo un disparo a un barco hundido... no lo haremos
            if (this.EstaHundido())
            {
                return ResultadoDisparo.NoValido;
            }
            // Verificamos que no ha repetido disparo en esa coordenada. Tal vez se debería contar como fallo un disparo repetido... no lo haremos
            if (YaDisparado(shoot))
            {
                return ResultadoDisparo.NoValido;
            }

            // Almacenamos la coordenada que le dio, para que no la pueda repetir
            _disparos[ImpactosPendientes - 1] = shoot;

            // le queda un impacto menos para hundirse
            ImpactosPendientes--;

            //Comprobamos si solo está tocado o está totalmente hundido
            if (this.EstaHundido())
            {
                // Cambiamos el estado
                this.Estado = EstadoBarco.Hundido;
                return ResultadoDisparo.Hundido;
            }
            else
            {
                // Cambiamos el estado
                this.Estado = EstadoBarco.Tocado;
                return ResultadoDisparo.Tocado;
            }
        }

        private bool YaDisparado(Coordenada shoot)
        {
            for (int i = Length-1; i > 0; i--)
            {
                if (_disparos[i]!= null)
                {
                    // Comparamos los valores de las propiedades de los objetos a comparar
                    if (_disparos[i].x == shoot.x && _disparos[i].y == shoot.y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool EstaHundido()
        {
            // Devuelve si está hundido
            return ImpactosPendientes < 1;
        }
    }

}