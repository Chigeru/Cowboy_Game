using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy_game
{
    class Cowboy
    {
        public string name { get; set; }
        public bool wounded { get; set; }
        int points { get; set; }

        public Cowboy(string name)
        {
            this.name = name;
            this.wounded = false;
            this.points = 0;
        }

        public void Fire()
        {
            // animation
            // spawn image on location

        }

    }
}
