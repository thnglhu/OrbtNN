using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbtNN
{
    class NeuralNetwork
    {
        int input_number, hidden_number;
        float[] chromosome;
        float[] bias_weight;
        float[] input, hidden;
        static readonly float[] biases = new float[] { 1f, 1f };
        static Random random = new Random();
        public static float NextRandomRange(float minimum, float maximum)
        {
            return (float)(random.NextDouble() * (maximum - minimum) + minimum);
        }
        public NeuralNetwork(int input_number, int hidden_number, bool init = true)
        {
            this.input_number = input_number;
            this.hidden_number = hidden_number;
            chromosome = new float[hidden_number * (input_number + 1)];
            input = new float[input_number];
            hidden = new float[hidden_number];
            for (int index = 0; index < input_number; index++) input[index] = 1;
            bias_weight = new float[2];
            if (init)
            {
                for (int index = 0; index < hidden_number * (input_number + 1); index++)
                {
                    chromosome[index] = NextRandomRange(-1, 1);
                }
            }
        }
        private NeuralNetwork() { }
        public float Activate(float[] input)
        {
            Array.Copy(input, this.input, input.Length);
            float final = biases[1] * bias_weight[1];
            for (int index = 0; index < hidden_number; index++)
            {
                float result = bias_weight[0] * biases[0];
                for (int preindex = 0; preindex < input_number; preindex++)
                {
                    result += hidden[index] = input[preindex] * chromosome[index * input_number + preindex];
                }                    
                final += chromosome[input_number * hidden_number + index] * result;
            }
            return final;
        }
        public void Draw(GameController controller, Vector2 position)
        {
            //float dx = 10, dy = 100;
            //Vector2 output = position + new Vector2(0, 2 * dy);
            //for (int hidden_index = 0; hidden_index < hidden_number; hidden_index++)
            //{
            //    Vector2 begin = position + new Vector2(-dx * hidden_number / 4 + dx * hidden_index / 2, dy);
            //    for (int input_index = 0; input_index < input_number; input_index++)
            //    {
            //        Vector2 end = position + new Vector2(-dx * input_number / 2 + dx * input_index, 0);
            //        controller.DrawLine(begin, end, new Color(1 - input[input_index], 1, input[input_index]));
            //    }
            //    controller.DrawLine(begin, output, new Color(1 - (hidden[hidden_index] + 1) / 2, 1, (hidden[hidden_index] + 1) / 2));
            //}
        }
        public NeuralNetwork Clone()
        {
            NeuralNetwork clone = new NeuralNetwork(input_number, hidden_number, false);
            Array.Copy(bias_weight, clone.bias_weight, 2);
            Array.Copy(chromosome, clone.chromosome, clone.chromosome.Length);
            return clone;
        }
        public static NeuralNetwork Mutate(NeuralNetwork source)
        {
            NeuralNetwork clone = source.Clone();
            double chance = 0.8;
            int bound = clone.hidden_number * (clone.input_number + 1);
            for (int i = 0; i < (int)(clone.hidden_number * (clone.input_number + 1) *chance); i++)
            {
                int index = random.Next(0, bound + 2);
                if (index < bound) clone.chromosome[index] += NextRandomRange(-.1f, .1f);
                else clone.bias_weight[index - bound] += NextRandomRange(-.1f, .1f);
            }
            return clone;
        }
        public static NeuralNetwork CrossOver(NeuralNetwork father, NeuralNetwork mother)
        {
            int input = mother.input_number, hidden = mother.hidden_number;
            NeuralNetwork child = new NeuralNetwork(input, hidden, false);
            for (int index = 0; index < hidden * (input + 1) + 2; index++)
            {
                if (index < hidden * (input + 1)) child.chromosome[index] = (random.Next(2) == 1 ? mother : father).chromosome[index];
                else child.bias_weight[index - hidden * (input + 1)] = (random.Next(2) == 1 ? mother : father).bias_weight[index - hidden * (input + 1)];
            }

            return child;
        }
    }
}
