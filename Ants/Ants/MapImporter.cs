using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ants.Model;

namespace Ants
{
    class MapImporter
    {
        public Square[,] Import(string path)
        {
            Square[,] map;
            var hills = new Hill[2];

            using (StreamReader sr = new StreamReader(path))
            {
                int width = int.Parse(sr.ReadLine());
                int height = int.Parse(sr.ReadLine());
                map = new Square[height, width];

                sr.ReadLine();

                int x = 0;
                int y = 0;

                try
                {
                    
                    for (y = 0; y < height; y++)
                    {
                        string line = sr.ReadLine();

                        for (x = 0; x < width; x++)
                        {
                            var square = new Square(x, y);
                            map[y, x] = square;

                            switch (line[x])
                            {
                                // Passable square
                                case '.':
                                    break;

                                // Impassable square
                                case '#':
                                    square.IsPassable = false;
                                    break;

                                case 'a':
                                    hills[0] = new Hill(square);
                                    break;

                                case 'b':
                                    hills[1] = new Hill(square);
                                    break;

                                default:
                                    throw new FormatException("Invalid token '" + line[x] + "' at row " + (y + 1) + ", column " + (x + 1));
                            }
                        }
                    }
                }

                catch (IndexOutOfRangeException)
                {
                    throw new FormatException("Map error at row " + (y + 1) + ", column " + (x + 1));
                }
            }

            return map;
        }
    }
}
