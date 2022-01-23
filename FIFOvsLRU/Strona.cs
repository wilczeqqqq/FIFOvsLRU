using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIFOalpha
{
    internal class Strona
    {
        private int _numer; // Numer strony

        public Strona(int numer)
        {
            _numer = numer;
        }

        public int Numer { get => _numer; set => _numer = value; }
    }
}
