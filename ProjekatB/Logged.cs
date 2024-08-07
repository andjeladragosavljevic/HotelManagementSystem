using ProjekatB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatB
{
    public static class Logged
    {
        private static Person logged;

        public static Person GetLogged()
        {
            return logged;
        }

        public static void SetLogged(Person value)
        {
            logged = value;
        }
    }
}
