using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BehaviorSystem
{
    public class Actor
    {
        public static List<Actor> Actors = new List<Actor>();
        private static Random random = new Random();

        public Color Color;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public Vector2 Origin;
        public List<Behavior> BehaviorList = new List<Behavior>();

        public static Vector2 GetRandomPosition(int rangeX, int rangeY)
        {
            return new Vector2(random.Next(rangeX), random.Next(rangeY));
        }

        public static Vector2 GetRandomDirection()
        {
            double rotation = random.NextDouble() * MathHelper.TwoPi;
            
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public Actor(Texture2D texture, Color color)
        {
            Actors.Add(this);
            Color = color;
            Texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Update()
        {
            foreach (var behavior in BehaviorList)
            {
                behavior.Update(this);
            }

            if (Direction.Length() > 0f)
            {
                Direction.Normalize();
            }

            Position += Direction * Speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float rotation = (float)Math.Atan2(Direction.Y, Direction.X) + MathHelper.Pi / 2;

            spriteBatch.Draw(Texture, Position, null, Color, rotation, Origin, 0.5f, SpriteEffects.None, 0f);
        }
    }
}
