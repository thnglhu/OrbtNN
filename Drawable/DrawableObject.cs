using Microsoft.Xna.Framework;

namespace OrbtNN.Drawable
{
    public abstract class DrawableObject
    {
        Vector2 position;
        protected Sprite sprite;
        protected virtual void Initialize(Sprite sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
        }
        public abstract void Update(GameTime game_time);
        public abstract void Draw();
        internal Vector2 Position
        {
            get => position;
            set => position = value;
        }
        public float Opacity
        {
            get => sprite.Opacity;
            set => sprite.Opacity = value;
        }
    }
}
