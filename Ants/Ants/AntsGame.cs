using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Ants.Model;
using Ants.PathFinding;
using Ants.States;

namespace Ants
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AntsGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int TeamCount = 2;

        private readonly TimeSpan tickRate;
        private readonly TimeSpan spawnRate;

        public Map Map { get; private set; }
        private TimeSpan previousTick;
        private TimeSpan previousSpawn;
        private Texture2D blank;
        private MouseState mouseState;
        private MouseState previousMouseState;

        public List<Ant>[] Ants { get; private set; }
        public Hill[] Hills { get; private set; }
        public Color[] AntColors { get; private set; }

        public AntsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            tickRate = TimeSpan.FromSeconds(0.3f);
            spawnRate = TimeSpan.FromSeconds(tickRate.TotalSeconds * 10);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CreateMap("Content/Maps/map1.txt");

            Ants = new List<Ant>[TeamCount];

            for (int i = 0; i < TeamCount; i++)
			{
			    Ants[i] = new List<Ant>();
			}

            AntColors = new Color[]
            {
                Color.Red,
                Color.Blue,
                Color.Green
            };

            for (int i = 0; i < 4; i++)
            {
                SpawnAnts();    
            }
           

            blank = new Texture2D(GraphicsDevice, 1, 1);
            blank.SetData(new[] { Color.White });
        }

        private void CreateMap(string path)
        {
            var importer = new MapImporter();
            Square[,] squares;
            Hill[] hills;

            importer.Import(path, this, out squares, out hills);
            Map = new Map(this, squares);

            Hills = hills;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            bool tick = false;

            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            TimeSpan timeSinceTick = gameTime.TotalGameTime - previousTick;
            float progress = (float)(timeSinceTick.TotalMilliseconds / tickRate.TotalMilliseconds);

            if (timeSinceTick > tickRate)
            {
                previousTick = gameTime.TotalGameTime;
                tick = true;
                AttackAnts();
            }

            if (gameTime.TotalGameTime - previousSpawn > spawnRate)
            {
                previousSpawn = gameTime.TotalGameTime;
                SpawnAnts();
            }

            foreach (List<Ant> ants in Ants)
            {
                foreach (Ant ant in ants)
                {
                    ant.Update(gameTime, tick, progress);
                }
            }

            //if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            //{
            //    int x = (int)(mouseState.X / Square.Width);
            //    int y = (int)(mouseState.Y / Square.Height);

            //    if (x < 0 || x >= Map.Columns || y < 0 || y > Map.Rows)
            //    {
            //        // Outside window
            //    }

            //    else
            //    {

            //        var ant = Ants[0][0];

            //        ant.Path = AStar.Search(Map, ant.Square, Map.Squares[y, x], Map.Distance);

            //        Console.WriteLine("{0}, {1}", x, y);
            //    }
            //}

            base.Update(gameTime);
        }

        private void AttackAnts()
        {
            var deadAnts = new List<Ant>();

            for (int i = 0; i < Ants.Length; i++)
            {
                foreach (Ant ant in Ants[i])
                {
                    foreach (Square neighbor in Map.GetNeighbors(ant.Square))
                    {
                        if (neighbor.Ant != null && neighbor.Ant.Team != ant.Team)
                        {
                            bool isDead = ant.Attack(neighbor.Ant);

                            // Remove dead ants after all ants are processed
                            if (isDead)
                                deadAnts.Add(neighbor.Ant);

                            // Allow only one attack for every ant
                            break;
                        }
                    }
                }
            }

            foreach (Ant ant in deadAnts)
            {
                Ants[ant.Team].Remove(ant);
                ant.Square.Ant = null;
            }
        }

        private void SpawnAnts()
        {
            for (int i = 0; i < Hills.Length; i++)
            {
                foreach (Square square in Map.GetNeighbors(Hills[i].Square))
                {
                    if (square.IsPassable)
                    {
                        Ant ant = new Ant(this, square, i);
                        ant.ChangeState(new AttackState(this, ant));
                        Ants[i].Add(ant);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            Map.Draw(spriteBatch);

            for (int i = 0; i < Ants.Length; i++)
            {
                foreach (var ant in Ants[i])
                {
                    ant.Draw(spriteBatch);
                }
            }

            for (int i = 0; i < Hills.Length; i++)
            {
                Hills[i].Draw(spriteBatch, AntColors[i]);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawLine(Color color, float x1, float y1, float x2, float y2)
        {
            var point1 = new Vector2(x1, y1);
            var point2 = new Vector2(x2, y2);

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, 1),
                       SpriteEffects.None, 0);
        }

        public void FillSquare(Vector2 position, Color color)
        {
            Vector2 scale = new Vector2(Square.Width, Square.Height);
            spriteBatch.Draw(blank, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public Texture2D CreateCircle(int radius, Color color)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(GraphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = color;
            }

            texture.SetData(data);

            return texture;
        }
    }
}
