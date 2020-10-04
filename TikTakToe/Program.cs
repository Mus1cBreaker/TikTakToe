using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TikTakToe
{
    class Program
    {
        static void Main(string[] args)
        {
            TikTakToe game = new TikTakToe("_________");
            game.Game_Menu();
        }
    }
}
