using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ants
{
    class MapImporter
    {
        public Square[,] Import(string path)
        {
            Square[,] map;

            using (StreamReader sr = new StreamReader(path))
            {

                int width = int.Parse(sr.ReadLine());
                int height = int.Parse(sr.ReadLine());
                map = new Square[width, height];

                sr.ReadLine();

                int x = 0;
                int y = 0;
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    x = 0;

                    foreach (char c in line)
                    {
                        switch (c)
                        {
                            case '.':
                                map[y, x] = new Square(x, y, true);
                                break;
                            case '#':
                                map[y, x] = new Square(x, y, false);
                                break;

                            default:
                                break;
                        }

                        x++;
                    }

                    y++;
                }
            }

            return map;
        }
    }
}
