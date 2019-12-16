using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrbtNN
{
    public abstract class CircularObject
    {
        Vector2 position = new Vector2();
        protected float radius, opacity;
        protected Sprite sprite;
        protected bool ready = false;
        protected GameController controller;
        public CircularObject(GameController controller)
        {
            this.controller = controller;
        }
        protected void Initialize(Sprite sprite, Vector2 position)
        {
            Position = position;
            ready = true;
            this.sprite = sprite;
            radius = sprite.Current.Width / 2;
            opacity = 1f;
            Update(null);
        }
        public abstract void Update(GameTime game_time);
        public virtual void Draw()
        {
            if (ready)
            {
                controller.DrawSprite(sprite, Position, opacity);
            }
        }

        internal Vector2 Position
        {
            get => position;
            set => position = value;
        }
        internal float Radius
        {
            get => radius;
        }
        internal float Opacity
        {
            get => opacity;
            set => opacity = value;
        }
        public static float Distance(CircularObject A, CircularObject B)
        {
            return Vector2.Distance(A.Position, B.Position) - A.Radius - B.Radius;
        }
        public static bool Collide(CircularObject A, CircularObject B)
        {
            return Distance(A, B) <= 0f;
        }
    }
}
