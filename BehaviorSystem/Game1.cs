using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BehaviorSystem
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        readonly int ScreenWidth = 1920;
        readonly int ScreenHeight = 1080;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D arrowTexture = Content.Load<Texture2D>("arrow2");

            List<Actor> obsticles = new List<Actor>();
            Color obsticleColor = new Color(255, 200, 96);

            //for (int i = 0; i < 4; i++)
            //{
            //    obsticles.Add(new Actor(arrowTexture, obsticleColor));
            //    obsticles[i].Position = new Vector2(500 * i + 200, 500);
            //}

            int hCount = ScreenHeight / 150;
            int vCount = ScreenWidth / 300;

            for (int i = 0; i < hCount; i++) // top
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2((float)i / hCount * ScreenWidth + 100, 0),
                    Direction = new Vector2(0, 1)
                };
                obsticles.Add(obsticle);
            }

            for (int i = 0; i < hCount; i++) // bottom
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2((float)i / hCount * ScreenWidth + 100, ScreenHeight),
                    Direction = new Vector2(0, -1)
                };
                obsticles.Add(obsticle);
            }

            for (int i = 0; i < vCount; i++) // right
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2(ScreenWidth, (float)i / vCount * ScreenHeight + 100),
                    Direction = new Vector2(-1, 0)
                };
                obsticles.Add(obsticle);
            }

            for (int i = 0; i < vCount; i++) // left
            {
                Actor obsticle = new Actor(arrowTexture, obsticleColor)
                {
                    Position = new Vector2(0, (float)i / vCount * ScreenHeight + 100),
                    Direction = new Vector2(1, 0)
                };
                obsticles.Add(obsticle);
            }

            Actor leader = new Actor(arrowTexture, new Color(64, 255, 64))
            {
                Speed = 4,
                Direction = Actor.GetRandomDirection(),
                Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight)
            };
            //leader.BehaviorList.Add(new BehaviorConstant(0.1f, new Vector2(1, 0)));
            leader.BehaviorList.Add(new BehaviorMovement(0.5f));
            leader.BehaviorList.Add(new BehaviorWander(0.1f, 60));

            Actor enemy = new Actor(arrowTexture, new Color(255, 120, 120));
            enemy.Speed = 2;
            enemy.Position = new Vector2(200, 150);
            enemy.BehaviorList.Add(new BehaviorWander(0.1f, 30));
            enemy.BehaviorList.Add(new BehaviorSeek(0.1f, leader));

            Behavior seek = new BehaviorSeek(0.05f, leader);
            Behavior avoidEnemy = new BehaviorAvoid(0.1f, enemy, 150);

            for (int i = 0; i < 10; i++)
            {
                Actor drone = new Actor(arrowTexture, Color.White)
                {
                    Speed = 3,
                    Direction = Actor.GetRandomDirection(),
                    Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight)
                };
                drone.BehaviorList.Add(seek);
                drone.BehaviorList.Add(new BehaviorWander(0.03f, 15));
                drone.BehaviorList.Add(avoidEnemy);
                foreach (var obsticle in obsticles)
                {
                    drone.BehaviorList.Add(new BehaviorAvoid(0.2f, obsticle, 200));
                }
            }

            foreach (var obsticle in obsticles)
            {
                leader.BehaviorList.Add(new BehaviorAvoid(0.2f, obsticle, 150));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var actor in Actor.Actors)
            {
                actor.Update();

                if (actor.Position.X > ScreenWidth)
                {
                    actor.Position.X = 0;
                }
                else if (actor.Position.X < 0)
                {
                    actor.Position.X = ScreenWidth;
                }

                if (actor.Position.Y > ScreenHeight)
                {
                    actor.Position.Y = 0;
                }
                else if (actor.Position.Y < 0)
                {
                    actor.Position.Y = ScreenHeight;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(96, 96, 111));

            _spriteBatch.Begin();

            foreach (var actor in Actor.Actors)
            {
                actor.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
