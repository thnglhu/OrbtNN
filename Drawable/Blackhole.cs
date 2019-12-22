using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace OrbtNN.Drawable
{
    public class Blackhole : CircularObject
    {
        HashSet<Asteroid> affected = new HashSet<Asteroid>();
        List<SquareParticle> list = new List<SquareParticle>();
        int width, height;
        public Blackhole(GameController controller, int width, int height) : base(controller) {
            this.width = width;
            this.height = height;
        }
        public override void Update(GameTime game_time)
        {
            foreach (SquareParticle particle in list)
            {
                particle.Update(game_time);
                Vector2 position = particle.Position;
                if (position.X < -10 || position.X > width + 10
                    || position.Y < -10 || position.Y > height + 10) particle.Direction *= -1;
            }
            foreach (Sprite animator in Extra)
            {
                animator.Update(game_time);
            }
            foreach (Asteroid asteroid in new HashSet<Asteroid>(affected))
            {
                float before = Distance(this, asteroid);
                asteroid.Update(game_time);
                float after = Distance(this, asteroid);
                asteroid.Opacity = 1f + after / radius;
                if (after > before) affected.Remove(asteroid);
            }
        }
        public override void Draw()
        {
            foreach (SquareParticle particle in list) particle.Draw();
            base.Draw();
            foreach (Sprite sprite in Extra)
                controller.DrawTexture(sprite.Current, Position, sprite.Opacity);
            foreach (Asteroid planet in affected)
                planet.Draw();
        }
        public void Initialize(Sprite sprite, Vector2 position, float radius)
        {
            this.radius = radius;
            Initialize(sprite, position);
            Random random = new Random();
            for (int index = 0; index < 300; index++)
            {
                SquareParticle particle = new SquareParticle(controller);
                particle.Initialize(random.Next(1, 5), new Vector2(random.Next(width), random.Next(height)));
                particle.Direction = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
                particle.Opacity = (float)random.NextDouble() / 2 + 0.1f;
                list.Add(particle);
            }
        }
        public void Affect(Asteroid planet)
        {
            affected.Add(planet);
            planet.Velocity += 20;
            // planet.Mass = 150f;
        }
        public HashSet<Sprite> Extra { get; } = new HashSet<Sprite>();
    }
}
