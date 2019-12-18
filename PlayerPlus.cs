using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace OrbtNN
{
    class PlayerPlus : Player
    {
        public static float SCALE = 0.25f;
        public static readonly int RAY = 30;
        public static int MAX_DISTANCE = 300;
        public float[] distances;
        public Color[] colors;
        Vector2 ToCenter = new Vector2();
        float start_time = float.PositiveInfinity, last_time = 0f;
        public PlayerPlus(GameController controller) : base(controller)
        {
            distances = new float[RAY + 1];
            colors = new Color[RAY + 1];
        }
        public override void Initialize(Blackhole origin, Sprite sprite, float angle, float distance, float radius, float mass, float velocity)
        {
            base.Initialize(origin, sprite, angle, distance, radius, mass, velocity);
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = 1;
                colors[i] = Color.Green;
            }
        }
        public override void Draw()
        {
            base.Draw();
            for (int index = 1; index < RAY; index++)
            {
                Vector2 end = GameController.Rotate(ToCenter, -index * Math.PI / RAY) * distances[index] * MAX_DISTANCE;
                controller.DrawVector(Position, Position + end, colors[index]);
            }
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = 1;
                colors[i] = Color.Green;
            }

        }
        float next_input = 0, next_time = 0.005f;
        public override void Update(GameTime game_time = null)
        {
            if (game_time != null)
            {
                last_time = (float)game_time.TotalGameTime.TotalSeconds;
                if (start_time > last_time) start_time = last_time;
            }
            base.Update(game_time);
            ToCenter = Vector2.Normalize(Pivot.Position - Position);
        }
        static int Orientation(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float val = (p2.Y - p1.Y) * (p3.X - p2.X) - (p2.X - p1.X) * (p3.Y - p2.Y);

            if (Math.Abs(val - 0) <= 0.00001f) return 0;  // colinear 

            return (val > 0) ? 1 : 2; // clock or counterclock wise 
        }
        public override bool Check(Planet planet)
        {
            Detect(planet);
            return base.Check(planet);
        }
        public override bool Check(Blackhole blackhole)
        {
            Detect(blackhole);
            return base.Check(blackhole);
        }
        public void Detect(CircularObject circle)
        {
            float l = Distance(this, circle) + Radius;
            if (l > MAX_DISTANCE) return;
            l += circle.Radius;
            if (Orientation(Pivot.Position, Position, circle.Position) == 1) return;
            for (int index = 1; index < RAY; index++)
            {
                Vector2 end = Position + GameController.Rotate(ToCenter, -index * Math.PI / RAY) * distances[index];
                Vector2 A, B;
                A = end - Position;
                B = circle.Position - Position;
                double angle = Math.Acos(Vector2.Dot(A, B) / A.Length() / B.Length());
                if (angle > Math.PI / 2) continue;
                double x1 = Position.X;
                double y1 = Position.Y;
                double x2 = end.X;
                double y2 = end.Y;
                double x0 = circle.Position.X;
                double y0 = circle.Position.Y;
                double d = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
                d /= Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1));
                double r = circle.Radius;
                if (d <= r)
                {
                    colors[index] = Color.Red;
                    distances[index] = Math.Min(distances[index], (float)(Math.Sqrt(l * l - d * d) - Math.Sqrt(r * r - d * d)) / MAX_DISTANCE);
                }
            }
        }
    }
}
