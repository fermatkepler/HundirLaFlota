

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

        public void Tocado(Coordenada shoot)
        {
            // No debe estar ya hundido
            if( this.EstaHundido())
            {
                return;
            }
            // Verificamos que no ha repetido disparo en esa coordenada
            if (YaDisparado(shoot))
            {
                return;
            }

            // Almacenamos la coordenada que le dio, para que no la pueda repetir
            _disparos[ImpactosPendientes-1] = shoot;

            // le queda un impacto menos para hundirse
            ImpactosPendientes--;
        }

        private bool YaDisparado(Coordenada shoot)
        {
            for (int i = Length-1; i > 0; i--)
            {
                if (_disparos[i]!= null && _disparos[i] == shoot)
                {
                    return true;
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