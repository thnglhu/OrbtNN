using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN
{
    class Tag
    {
        Sprite background, avatar = null;
        string text;
        GameController controller;
        static Vector2 delta = new Vector2(25, 25);

        public Sprite Avatar { get => avatar; set => avatar = value; }
        public string Text { get => text; set => text = value; }
        public Tag(GameController controller)
        {
            this.controller = controller;
            background = SpriteFactory.GetSprite("Tag");
            avatar = SpriteFactory.GetSprite("Earth");
        }
        public void Draw(Vector2 position)
        {
            controller.DrawSprite(background, position, 1f, Align.CORNER);
            controller.DrawSprite(avatar, position + delta);
            controller.DrawString(position + delta + new Vector2(23, -21), text, Color.Black, Align.CORNER);
        }
    }
}
