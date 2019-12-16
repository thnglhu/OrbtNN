using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN
{
    public class Sprite
    {
        Texture2D[] spritesheet;
        int index;
        float delay = 1f;
        float next = 0;
        public Sprite(params Texture2D[] spritesheet)
        {
            this.spritesheet = spritesheet;
        }
        public void Update(GameTime game_time)
        {
            next -= (float)game_time.ElapsedGameTime.TotalSeconds;
            if (next <= 0)
            {
                index = (index + 1) % spritesheet.Length;
                next += delay;
            }
        }
        public Texture2D Current
        {
            get => spritesheet[index];
        }
        public Sprite SetDelay(float value)
        {
            delay = value;
            return this;
        }
        public Sprite Clone()
        {
            return new Sprite(spritesheet);
        }
    }
}
