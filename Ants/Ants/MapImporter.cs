using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ants.Model;
using Microsoft.Xna.Framework;

namespace Ants
{
    class MapImporter
    {
        public void Import(string path, AntsGame game, out Square[,] squares, out Hill[] hills)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                int colCount = int.Parse(sr.ReadLine());
                int rowCount = int.Parse(sr.ReadLine());

                var viewport = game.GraphicsDevice.Viewport;
                squares = new Square[rowCount, colCount];
                hills = new Hill[2];
                int x = 0;
                int y = 0;

                Square.Width = (float)viewport.Width / colCount;
                Square.Height = (float)viewport.Height / rowCount;
                Vector2 origin = new Vector2(Square.Width / 2f, Square.Height / 2f);

                sr.ReadLine();

                try
                {
                    
                    for (y = 0; y < rowCount; y++)
                    {
                        string line = sr.ReadLine();

                        for (x = 0; x < colCount; x++)
                        {
                            //Vector2 position = new Vector2(x * Square.Width, y * Square.Height) + origin;
                            Vector2 position = new Vector2(x * Square.Width, y * Square.Height);
                            squares[y, x] = new Square(x, y, position);

                            switch (line[x])
                            {
                                // Passable square
                                case '.':
                                    break;

                                // Impassable square
                                case '#':
                                    squares[y, x].IsObstacle = true;
                                    break;

                                // Ant hill team 0
                                case 'a':
                                    hills[0] = new Hill(game, squares[y, x], 0);
                                    break;

                                // Ant hill team 1
                                case 'b':
                                    hills[1] = new Hill(game, squares[y, x], 1);
                                    break;

                                default:
                                    throw new FormatException("Invalid token '" + line[x] + "' at row " + (y + 1) + ", column " + (x + 1));
                            }
                        }
                    }
                }

                catch (Exception)
                {
                    throw new FormatException("Map error at row " + (y + 1) + ", column " + (x + 1));
                }
            }
        }
    }
}
