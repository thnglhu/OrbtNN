using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN.Drawable
{
    internal class Computer : Player
    {
        class Packed
        {
            private PlayerPlus player;
            private NeuralNetwork network;
            private Tag tag;
            internal decimal Time { get; set; } = 0.1M;
            internal decimal Fitness { get; set; } = 0.1M;
            internal Tag Tag { get => tag; set => tag = value; }
            internal int Score { get; set; } = 0;
            public string Name;

            internal PlayerPlus Player { get => player; set => player = value; }
            internal NeuralNetwork Network { get => network; set => network = value; }

            public Packed(GameController controller)
            {
                Player = new PlayerPlus(controller);
                Network = new NeuralNetwork(PlayerPlus.RAY, 5);
                Tag = new Tag(controller);
            }
            public void Update(GameTime game_time)
            {
                player.Update(game_time);
                if (game_time != null)
                {
                    Time += (decimal)game_time.ElapsedGameTime.TotalMilliseconds;
                    decimal total = Player.Total();
                    Score = (int) (total / 2 / (decimal)Math.PI);
                    if (Time != 0) Fitness = total + total / Time;
                    else Fitness = total;
                }
            }
            public float Activate()
            {
                return network.Activate(player.distances);
            }
        }
        class Graph
        {
            GameController controller;
            List<decimal> records = new List<decimal>();
            decimal max = 0;
            public Graph(GameController controller)
            {
                this.controller = controller;
            }
            public void Add(decimal value)
            {
                if (value > max) max = value;
                records.Add(value);
            }
            public void EditLast(decimal value)
            {
                if (value > max) max = value;
                records[records.Count - 1] = value;
            }
            public void Draw(Vector2 position, int width, int height)
            {
                int count = records.Count;
                for (int index = 0; max != 0 && index < count - 1 && count > 1; index++)
                {
                    Vector2 begin = new Vector2(
                        position.X + (float)width * index / (count - 1),
                        position.Y + height - (float)(height * records[index] / max));

                    Vector2 end = new Vector2(
                        position.X + (float)width * (index + 1) / (count - 1),
                        position.Y + height - (float)(height * records[index + 1] / max));
                    controller.DrawLine(begin, end, Color.Red, 2);
                }

            }
        }
        Packed[] players;
        int number, remain;
        int generation = 0;
        bool fresh = true;
        Sprite window;
        static Vector2 delta = new Vector2(1080, 0);
        Graph graph;
        public bool Display
        {
            get => display;
            set
            {
                display = value;
                Array.ForEach(players, player => player.Player.Display = value);
            }
        }
        public Computer(GameController controller, int number = 1) : base(controller)
        {
            this.number = number;
            players = new Packed[number];
            for (int index = 0; index < number; index++)
            {
                players[index] = new Packed(controller);
                players[index].Player.Display = Display;
                players[index].Name = $"Earth-G1-{index + 1}";
            }
            window = SpriteFactory.GetSprite("Window");
            graph = new Graph(controller);
        }
        public override void Initialize(Blackhole origin, Sprite sprite, float angle, float distance, float radius, float mass, float velocity)
        {
            generation++;
            remain = number;
            int start = 1;
            graph.Add(players[0].Fitness);
            if (!fresh)
            {
                for (int index = number / 3; index < 2 * number / 3; index++)
                {
                    players[index].Network = NeuralNetwork.Mutate(players[0].Network);
                    players[index].Name = $"Earth-G{generation}-{start++}";
                }
                for (int index = 2 * number / 3; index < number; index++)
                {
                    players[index].Network = NeuralNetwork.CrossOver(players[index - 2 * number / 3].Network, players[index - 2 * number / 3 + 1].Network);
                    players[index].Name = $"Earth-G{generation}-{start++}";
                }
            }
            else fresh = false;
            for (int index = 0; index < number; index++)
            {
                players[index].Player.Initialize(origin, sprite.Clone(), 0/*angle + index * 2 * (float)Math.PI / number*/, distance, radius, mass, velocity);
                players[index].Time = 1;
                players[index].Tag.Avatar = SpriteFactory.GetSprite("Earth");
            }
            Array.ForEach(players, player => player.Time = 0);
        }
        public override void Draw()
        {
            bool found = false;
            for (int index = 0; index < number; index++)
            {
                Packed player = players[index];
                if (!found && player.Player.Alive)
                {
                    player.Player.Opacity = 1f;
                    found = true;
                }
                else player.Player.Opacity = 0.2f;
                if (player.Player.Alive) player.Player.Draw();
            }
            Vector2 offset = new Vector2(10, 10);
            controller.DrawSprite(window, delta , 1f, Align.CORNER);
            for (int index = 0; index < Math.Min(number, 10); index++)
            {
                Tag tag = players[index].Tag;
                tag.Text = 
                    $"Name: {players[index].Name}\n" +
                    $"Fitness: {players[index].Fitness.ToString("N3")}\n" +
                    $"Score: {players[index].Score}\n";
                if (players[index].Player.Alive) tag.Text += "Status: Alive";
                else tag.Text += $"Status: {players[index].Player.Cause}";
                tag.Draw(delta + offset);
                offset.Y += 55;
            }
            graph.Draw(new Vector2(1085, 570), 190, 100);
            players[0].Network.Draw(controller, new Vector2(400, 400));
        }
        float time = 0;
        private bool display = false;

        public override void Update(GameTime game_time = null)
        {
            bool trigger = false;
            if (game_time != null)
            {
                time += (float)game_time.ElapsedGameTime.TotalSeconds;
                if (time > 0.05f)
                {
                    time -= 0.05f;
                    trigger = true;
                }
            }
            Array.ForEach(players, player =>
            {
                if (player.Player.Alive)
                {
                    if (trigger)
                    {
                        float value = player.Activate();
                        if (value <= 0) player.Player.Press();
                        else player.Player.Release();
                    }
                    player.Update(game_time);
                }
            });
            if (trigger)
            {
                Array.Sort(players, (A, B) => decimal.Compare(
                    B.Fitness,
                    A.Fitness
                    ));
            }
            graph.EditLast(players[0].Fitness);
        }
        public override bool Check(Asteroid planet)
        {
            Array.ForEach(players, player =>
            {
                if (player.Player.Alive)
                {
                    if (player.Player.Check(planet))
                    {
                        remain--;
                        player.Player.Alive = false;
                        player.Tag.Avatar = SpriteFactory.GetSprite("Earth2");
                    }
                }
            });
            return remain == 0;
        }
        public override bool Check(Blackhole blackhole)
        {
            Array.ForEach(players, player =>
            {
                if (player.Player.Alive)
                {
                    if (player.Player.Check(blackhole))
                    {
                        remain--;
                        player.Player.Alive = false;
                        player.Tag.Avatar = SpriteFactory.GetSprite("Earth2");
                        blackhole.Affect(player.Player);
                    }
                }
            });
            return remain == 0;
        }
        public override void Press()
        {
            // Array.ForEach(players, player => player.Player.Press());
        }
        public override void Release()
        {
            // Array.ForEach(players, player => player.Player.Release());
        }
        public override float Maximum
        {
            set => Array.ForEach(players, player => player.Player.Maximum = value);
        }
    }
}
