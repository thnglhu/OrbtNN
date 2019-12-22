using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrbtNN.Drawable
{
    public abstract class CircularObject : DrawableObject
    {
        protected float radius;
        protected GameController controller;
        public CircularObject(GameController controller)
        {
            this.controller = controller;
        }
        protected override void Initialize(Sprite sprite, Vector2 position)
        {
            base.Initialize(sprite, position);
            radius = sprite.Current.Height / 2;
            Update(null);
        }
        public override void Draw()
        {
            controller.DrawSprite(sprite, Position, sprite.Opacity);
        }
        internal float Radius
        {
            get => radius;
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
