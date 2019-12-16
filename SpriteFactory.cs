using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN
{
    static class SpriteFactory
    {
        public static ContentManager Content = null;
        public static Sprite GetSprite(string name)
        {
            if (Content != null)
            {
                switch (name)
                {
                    case "BlackHoleCover":
                        return new Sprite(
                            Content.Load<Texture2D>("test/test1"),
                            Content.Load<Texture2D>("test/test2"),
                            Content.Load<Texture2D>("test/test3"),
                            Content.Load<Texture2D>("test/test4"),
                            Content.Load<Texture2D>("test/test5")
                            ).SetDelay(0.3f);
                    default:
                        return new Sprite(Content.Load<Texture2D>(name));
                }
            }
            return null;
        }
    }
}
