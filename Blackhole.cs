using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace OrbtNN
{
    class Blackhole : CircularObject
    {
        HashSet<Planet> affected = new HashSet<Planet>();
        public Blackhole(GameController controller) : base(controller) { }
        public override void Update(GameTime game_time)
        {
            foreach (Sprite animator in Extra)
            {
                animator.Update(game_time);
            }
            foreach (Planet planet in new HashSet<Planet>(affected))
            {
                float before = Distance(this, planet);
                planet.Update(game_time);
                float after = Distance(this, planet);
                planet.Opacity = 1f + after / radius;
                if (after > before) affected.Remove(planet);
            }
        }
        public override void Draw()
        {
            base.Draw();
            foreach (Sprite animator in Extra)
                controller.DrawTexture(animator.Current, Position);
            foreach (Planet planet in affected)
                planet.Draw();
        }
        public void Initialize(Sprite sprite, Vector2 position, float radius)
        {
            this.radius = radius;
            Initialize(sprite, position);
        }
        public void Affect(Planet planet)
        {
            affected.Add(planet);
            planet.Velocity = 60f;
            planet.Mass = 150f;
        }
        public HashSet<Sprite> Extra { get; } = new HashSet<Sprite>();
    }
}
