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
            float final = biases[1] * bias_weight[1];
            for (int index = 0; index < hidden_number; index++)
            {
                float result = bias_weight[0] * biases[0];
                for (int preindex = 0; preindex < input_number; preindex++)
                    result += input[preindex] * chromosome[index * input_number + preindex];
                final += chromosome[input_number * hidden_number + index] * result;
            }
            return final;
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
            double chance = 0.5;
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
