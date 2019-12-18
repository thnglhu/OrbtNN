using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrbtNN
{
    class Player : Planet
    {
        int round;
        int risk;
        float backup;
        public bool alive = true;
        protected float max = float.PositiveInfinity;
        HashSet<Planet> bonus = new HashSet<Planet>();
        public Player(GameController controller) : base(controller) { }
        public virtual void Initialize(Blackhole origin, Sprite sprite, float angle, float distance, float radius, float mass, float velocity)
        {
            if (distance > max) distance = max;
            Initialize(origin, sprite, angle, distance, radius, mass);
            this.velocity = velocity;
            round = 0;
            risk = 0;
            bonus.Clear();
            backup = mass;
            alive = true;
        }
        protected override void Move(GameTime game_time)
        {
            base.Move(game_time);
            if (distance > max) distance = max;
        }
        public int Total()
        {
            return round;
        }
        protected override void Modular()
        {
            if (angle > 2 * (float)Math.PI)
            {
                round++;
                bonus.Clear();
            }
            base.Modular();
        }
        public virtual bool Check(Planet planet)
        {
            float dist = Distance(this, planet);
            if (dist < 0) return false;
            else if (dist < 20 && !bonus.Contains(planet))
            {
                risk++;
                bonus.Add(planet);
            }
            return true;
        }
        public virtual bool Check(Blackhole blackhole)
        {
            return Collide(this, blackhole);
        }
        public float Resist
        {
            set {
                mass = -value;
                if (mass == 0) mass = backup;
            }
        }
        bool pressed = false;
        public void Press()
        {
            if (!pressed)
            {
                pressed = true;
                Resist = mass * 3 / 4;
            }
        }
        public void Release()
        {
            if (pressed)
            {
                pressed = false;
                Resist = 0;
            }
        }
        public virtual float Maximum
        {
            set => max = value;
        }
        public virtual void Test(GameTime time)
        {

        }
    }
}
