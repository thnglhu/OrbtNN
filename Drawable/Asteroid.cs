using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN.Drawable
{
    public class Asteroid : CircularObject
    {
        #region Properties
        protected float distance, mass, angle, velocity;
        private Blackhole blackhole;
        public Asteroid(GameController controller) : base(controller) { }
        public virtual void Initialize(Blackhole origin, Sprite sprite, float angle, float distance, float radius, float mass)
        {
            this.angle = angle;
            this.distance = distance;
            this.radius = radius;
            this.mass = mass;
            this.blackhole = origin;
            velocity = 0;
            Update();
            Initialize(sprite, Position);
        }
        #endregion
        public override void Update(GameTime game_time = null)
        {
            if (game_time != null)
            {
                Move(game_time);
                Move((float)Math.Atan(velocity * (float)game_time.ElapsedGameTime.TotalSeconds / (distance + blackhole.Radius)));
                if (angle > 2 * (float)Math.PI) angle -= 2 * (float)Math.PI;
            }
            Position = blackhole.Position + Vector2.Multiply(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), distance + radius + blackhole.Radius);
        }
        protected virtual void Move(GameTime game_time)
        {
            distance -= mass * (float)game_time.ElapsedGameTime.TotalSeconds;
        }
        protected virtual void Move(float angle)
        {
            this.angle += angle;
        }
        public Blackhole Blackhole { get => blackhole; }
        public float Mass { set => mass = value; get => mass; }
        public float Dist { get => distance; }
        public float Velocity { set => velocity = value; get => velocity; }
        public float Angle { get => angle; }
    }
}
