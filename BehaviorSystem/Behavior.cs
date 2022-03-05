using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BehaviorSystem
{
    public class Behavior
    {
        protected float Weight;

        public Behavior(float weight)
        {
            Weight = weight;
        }

        public virtual void Update(Actor actor) { }
    }

    public class BehaviorConstant : Behavior
    {
        private Vector2 direction;

        public BehaviorConstant(float weight, Vector2 direct) : base(weight)
        {
            direction = direct;
        }

        public override void Update(Actor actor)
        {
            actor.Direction += direction * Weight;
        }
    }

    public class BehaviorMovement : Behavior
    {
        public BehaviorMovement(float weight) : base(weight) { }

        public override void Update(Actor actor)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Vector2 direction = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                actor.Direction.Y--;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                actor.Direction.Y++;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                actor.Direction.X--;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                actor.Direction.X++;
            }

            if (direction.Length() > 0.0f)
                direction.Normalize();
        }
    }

    public class BehaviorWander : Behavior
    {
        private static Random random = new Random();

        int changeInterval;
        int tick;
        Vector2 direction;

        public BehaviorWander(float weight, int changeInt) : base(weight) 
        {
            changeInterval = changeInt;
        }

        public override void Update(Actor actor)
        {
            if (tick == 0)
            {
                direction = Actor.GetRandomDirection();
            }

            tick++;
            tick %= changeInterval;

            actor.Direction += direction * Weight;
        }
    }

    public class BehaviorSeek : Behavior
    {
        private Actor target;

        public BehaviorSeek(float weight, Actor tar) : base(weight)
        {
            target = tar;
        }

        public override void Update(Actor actor)
        {
            Vector2 targetDirection = target.Position - actor.Position;
            targetDirection.Normalize();
            actor.Direction += targetDirection * Weight;
        }
    }

    public class BehaviorAvoid : Behavior
    {
        Actor target;
        float radius;

        public BehaviorAvoid(float weight, Actor tar, float rad) : base(weight)
        {
            target = tar;
            radius = rad;
        }

        public override void Update(Actor actor)
        {
            Vector2 targetDirection = actor.Position - target.Position;
            if (targetDirection.Length() < radius)
            {
                targetDirection.Normalize();
                actor.Direction += targetDirection * Weight;
            }
        }
    }
}
