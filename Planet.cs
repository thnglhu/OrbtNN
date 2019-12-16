using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN
{
    class Planet : CircularObject
    {
        #region Properties
        protected float distance, mass, angle, velocity;
        Blackhole origin;
        public Planet(GameController controller) : base(controller) { }
        public virtual void Initialize(Blackhole origin, Sprite sprite, float angle, float distance, float radius, float mass)
        {
            this.angle = angle;
            this.distance = distance;
            this.radius = radius;
            this.mass = mass;
            this.origin = origin;
            velocity = 0;
            Update();
            Initialize(sprite, Position);
        }
        #endregion
        public override void Update(GameTime game_time = null)
        {
            if (ready)
            {
                if (game_time != null)
                {
                    Move(game_time);
                    angle += (float)Math.Atan(velocity * (float)game_time.ElapsedGameTime.TotalSeconds / (distance +origin.Radius));
                    Modular();
                }
                Position = origin.Position + Vector2.Multiply(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), distance + radius + origin.Radius);
            }
        }
        protected virtual void Move(GameTime game_time)
        {
            distance -= mass * (float)game_time.ElapsedGameTime.TotalSeconds;
        }
        protected virtual void Modular()
        {
            if (angle > 2 * (float)Math.PI) angle -= 2 * (float)Math.PI;
        }
        public float Mass { set => mass = value; get => mass; }
        public float Dist { get => distance; }
        public float Velocity { set => velocity = value; }
        public float Angle { get => angle; }
    }
}
