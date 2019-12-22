using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN.Drawable
{
    class SquareParticle : DrawableObject
    {
        Vector2 direction = new Vector2();
        GameController controller;

        public Vector2 Direction { get => direction; set => direction = value; }

        public SquareParticle(GameController controller)
        {
            this.controller = controller;
        }
        public void Initialize(int index, Vector2 position)
        {
            base.Initialize(SpriteFactory.GetSprite($"Particle{index}"), position);
        }
        public override void Update(GameTime game_time)
        {
            Position += Direction * (float)game_time.ElapsedGameTime.TotalSeconds;
        }
        public override void Draw()
        {
            controller.DrawSprite(sprite, Position, sprite.Opacity);
        }
    }
}
