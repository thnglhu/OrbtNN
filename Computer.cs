using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace OrbtNN
{
    class Computer : Player
    {
        static int number = 50;
        int remain;
        public Planet[] planets;
        public float width, height, threshhold;
        Player[] players = null;
        NeuralNetwork[] networks;
        float[] highest = null;
        float[] finess = null;
        public Computer(GameController controller) : base(controller)
        {
            players = new Player[number];
            networks = new NeuralNetwork[number] ;
            for (int index = 0; index < number; index++)
            {
                players[index] = new Player(controller);
                networks[index] = new NeuralNetwork(361 * 2, 25);
            }
        }
        private void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public override void Initialize(Blackhole origin, Sprite sprite, float angle, float distance, float radius, float mass, float velocity)
        {
            if (finess == null)
            {
                finess = new float[number];
                highest = new float[number];
            }
            else
            {
                for (int i = 0; i < number; i++)
                {
                    highest[i] = finess[i];
                }
                for (int i=  0; i < number / 2; i++)
                {
                    for (int j = i + 1; j < number; j++)
                    {
                        if (highest[i] < highest[j])
                        {
                            Swap(ref highest[i], ref highest[j]);
                            Swap(ref networks[i], ref networks[j]);
                            Swap(ref players[i], ref players[j]);
                        }
                    }
                }
                Console.WriteLine(highest[0]);
                for (int i = number / 2; i < 3 * number / 4; i++)
                {
                    networks[i] = NeuralNetwork.CrossOver(networks[0], networks[i + 1 - number / 2]);
                }
                for (int i = 3 * number / 4; i < number; i++)
                {
                    networks[i] = NeuralNetwork.Mutate(networks[0]);
                }
            }
            for (int index = 0; index < number; index++)
            {
                players[index].Initialize(origin, sprite.Clone(), angle + index * 2 * (float)Math.PI / number, distance, radius, mass, velocity);
                players[index].Maximum = max;
            }
            remain = number;
        }
        public override void Draw()
        {
            foreach (Player player in players) if (player.alive) player.Draw();
        }
        float next_input = 0, next_time = 0.005f;
        public override void Update(GameTime game_time = null)
        {
            bool check = false;
            float[] input = null;
            next_input += (float)game_time.ElapsedGameTime.TotalSeconds;
            if (next_input > next_time)
            {
                check = true;
                next_input -= next_time;
                input = new float[361 * 2];
                for (int index = 0; index < 360; index++)
                {
                    if (planets[index] == null)
                    {
                        input[2 * index] = 0;
                    }
                    else input[2 * index] = planets[index].Dist;
                    input[2 * index + 1] = (float)(index * Math.PI / 180 - Math.PI);
                }
            }
            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];
                if (player.alive)
                {
                    player.Update(game_time);
                    if (check) {
                        input[720] = player.Dist;
                        input[721] = player.Angle;
                        if (networks[i].Activate(input)) player.Press();
                        else player.Release();
                    }
                }
            }
        }
        public override bool Check(Planet planet)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].alive && !players[i].Check(planet))
                {
                    remain--;
                    players[i].alive = false;
                    finess[i] = players[i].Total();
                }
            }
            return remain == 0 ? false : true;
        }
        public override bool Check(Blackhole blackhole)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].alive && players[i].Check(blackhole))
                {
                    remain--;
                    players[i].alive = false;
                    blackhole.Affect(players[i]);
                    finess[i] = players[i].Total();
                }
            }
            return remain == 0 ? true : false;
        }
        public override float Maximum
        {
            set
            {
                max = value;
                for (int i = 0; i < players.Length; i++)
                {
                    Player player = players[i];
                    if (player != null) player.Maximum = value;
                }
            }
        }
    }
}
