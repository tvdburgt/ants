using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ants.Model
{
    public class SquareScore
    {
        /// <summary>
        /// f(x): distance-plus-cost heuristic
        /// </summary>
        public float Total
        {
            get
            {
                return Cost + Estimate;
            }
        }

        /// <summary>
        /// g(x): distance (cost) from source
        /// </summary>
        public float Cost { get; set; }


        /// <summary>
        /// h(x): heuristic estimate to goal
        /// </summary>
        public float Estimate { get; set; }
    }
}
