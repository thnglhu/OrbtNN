using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN
{
    class GameManager
    {
        static float SCALE = .5f;
        static int player_vel = 600;
        static int player_mas = 300;
        GameController controller;
        // HashSet<Planet> planets = new HashSet<Planet>();
        Planet[] planets = new Planet[360];
        PlayerPlus player;
        Blackhole blackhole;
        float next_spawn, spawn_time;
        int width, height;
        float threshhold;
        public GameManager(GameController controller, int width, int height)
        {
            this.controller = controller;
            this.width = width;
            this.height = height;
            threshhold = (float)Math.Atan((float)height / width);
            blackhole = new Blackhole(controller);
            blackhole.Initialize(SpriteFactory.GetSprite("Blackhole"), new Vector2(width / 2, height / 2), 10);
            blackhole.Extra.Add(SpriteFactory.GetSprite("BlackHoleCover"));
            player = new PlayerPlus(controller);
            player.Maximum = 200;
            player.Initialize(blackhole, SpriteFactory.GetSprite("Earth"), 0, 250f, 10, player_mas * SCALE, player_vel * SCALE);
            next_spawn = 0;
            spawn_time = .1f;
        }
        public void Update(GameTime game_time)
        {
            player.Update(game_time);
            blackhole.Update(game_time);
            Random random = new Random();
            next_spawn -= (float)game_time.ElapsedGameTime.TotalSeconds;
            if (next_spawn <= 0)
            {
                next_spawn += spawn_time / SCALE;
                spawn_time *= 0.95f;
                if (spawn_time < 0.1f) spawn_time = 0.1f;
                Planet planet = new Planet(controller);
                float radius = 10;
                float mass = 100 + (float)(random.NextDouble() * 200);
                int int_angle = random.Next(0, 360);
                if (planets[int_angle] == null)
                {
                    float angle = (float)(int_angle * Math.PI / 180 - Math.PI);
                    float distance;
                    float abs_angle = Math.Abs(angle);
                    if (abs_angle > threshhold && abs_angle < Math.PI - threshhold)
                    {
                        distance = Math.Abs(height / (float)(2 * Math.Sin(angle)));
                    }
                    else
                    {
                        distance = Math.Abs(width / (float)(2 * Math.Cos(angle)));
                    }
                    distance -= planet.Radius + blackhole.Radius;
                    planet.Initialize(blackhole, SpriteFactory.GetSprite("Circle"), angle, distance, radius, 1.5f * mass * SCALE);
                    planets[int_angle] = planet;
                }
            }
            if (player.Check(blackhole))
            {
                Reset(); return;
            }
            for (int index = 0; index < 360; index++)
            {
                Planet planet = planets[index];
                if (planet != null)
                {
                    if (CircularObject.Collide(blackhole, planet))
                    {
                        blackhole.Affect(planet);
                        planets[index] = null;
                    }
                    else
                    {
                        planet.Update(game_time);
                        if (player.Check(planet) == false)
                        {
                            Reset(); return;
                        }
                    }
                }
            }
            player.Test(game_time);
        }
        public void Draw()
        {
            blackhole.Draw();
            for (int int_angle = 0; int_angle < planets.Length; int_angle++)
            {
                Planet planet = planets[int_angle];
                if (planet == null)
                {
                    //float angle = (float)(int_angle * Math.PI / 180 - Math.PI);
                    //float distance;
                    //float abs_angle = Math.Abs(angle);
                    //if (abs_angle > threshhold && abs_angle < Math.PI - threshhold)
                    //{
                    //    distance = Math.Abs(height / (float)(2 * Math.Sin(angle)));
                    //}
                    //else
                    //{
                    //    distance = Math.Abs(width / (float)(2 * Math.Cos(angle)));
                    //}
                    //Vector2 end = blackhole.Position + Vector2.Multiply(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), distance);
                    //controller.DrawLine(blackhole.Position, end, Color.Blue);
                    continue;
                }
                // controller.DrawLine(blackhole.Position, planet.Position, new Color(0.25f + planet.Mass / 500f, 0f, 0f));
                planet.Draw();
            }
            //controller.DrawVector(player.Position, blackhole.Position, Color.Green);;
            player.Draw();
            //controller.DrawString(blackhole.Position, round.ToString());
        }
        public void Reset()
        {
            next_spawn = 0;
            spawn_time = .1f;
            for (int index = 0; index < 360; index++) planets[index] = null;
            player.Initialize(blackhole, SpriteFactory.GetSprite("Earth"), 0, 250f, 10, player_mas * SCALE, player_vel * SCALE);
        }
        public void Press()
        {
            player.Press();
        }
        public void Release()
        {
            player.Release();
        }
    }
}
