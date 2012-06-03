using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ants.Model;
using Microsoft.Xna.Framework;

namespace Ants.PathFinding
{
    public static class AStar
    {
        
        public static Stack<Square> Search(Map map, Square source, Square target, Func<Square, Square, float> heuristic)
        {
            Dictionary<Square, Square> parents = new Dictionary<Square, Square>();
            HashSet<Square> closed = new HashSet<Square>();
            SortedSet<Square> open = new SortedSet<Square>();

            source.Score.Cost = 0;
            source.Score.Estimate = heuristic(source, target);
            open.Add(source);

            while (open.Count > 0)
            {

                // Get node with lowest f(x) score
                Square current = open.First();

                if (current == target)
                    break;

                // Transfer node to closed set
                open.Remove(current);
                closed.Add(current);

                // Examine neighbors
                foreach (Square neighbor in map.GetNeighbors(current))
                {
                    // Check if node is already processed or not passable
                    if (closed.Contains(neighbor) || !neighbor.IsPassable)
                        continue;

                    // Tentative g score
                    float g = current.Score.Cost + 1;

                    // Add (new) node to open
                    if (!open.Contains(neighbor))
                    {
                        parents[neighbor] = current;
                        neighbor.Score.Cost = g;
                        neighbor.Score.Estimate = heuristic(neighbor, target);
                        open.Add(neighbor);
                    }

                    // Updating existing node in open
                    else if (g < neighbor.Score.Cost)
                    {
                        open.Remove(neighbor);
                        parents[neighbor] = current;
                        neighbor.Score.Cost = g;
                        open.Add(neighbor);
                    }
                }
            }

            return ReconstructPath(parents, target);
        }
        
        private static Stack<Square> ReconstructPath(Dictionary<Square, Square> parents, Square target)
        {
            Stack<Square> path = new Stack<Square>();

            Square parent;
            Square child = target;

            while (parents.TryGetValue(child, out parent))
            {
                path.Push(child);
                child = parent;
            }

            return path;
        }

        public static float ManhattanHeuristic(Square square, Square target)
        {
            return Math.Abs(square.X - target.X) + Math.Abs(square.Y - target.Y);
        }

        public static float EuclideanHeuristic(Square square, Square target)
        {
            return Vector2.Distance(square.ScreenPosition, target.ScreenPosition);
        }

        public static float DijkstraHeuristic(Square square, Square target)
        {
            return 0f;
        }
    }
}
