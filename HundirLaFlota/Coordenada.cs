using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HundirLaFlota
{
    public sealed class Coordenada
    {
        public readonly int x;
        public readonly int y;
        public Coordenada(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public enum Estado
    {
        Agua = 0,
        Barco,
        AguaTocada,
        BarcoTocado
    }

    public enum ResultadoDisparo
    {
        Agua = 0,
        Tocado,
        Hundido
    }
}
