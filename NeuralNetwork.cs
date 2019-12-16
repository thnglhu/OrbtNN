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
        public NeuralNetwork(int input_number, int hidden_number, bool init = true)
        {
            this.input_number = input_number;
            this.hidden_number = hidden_number;
            chromosome = new float[hidden_number * (input_number + 1)];
            bias_weight = new float[2];
            if (init)
            {
                Random random = new Random();
                for (int index = 0; index < hidden_number * (input_number + 1); index++)
                {
                    chromosome[index] = (float)(200 * random.NextDouble() - 100);
                }
            }
        }
        private NeuralNetwork() { }
        public bool Activate(float[] input)
        {
            float final = biases[1] * bias_weight[1];
            for (int index = 0; index < hidden_number; index++)
            {
                float result = bias_weight[0] * biases[0];
                for (int preindex = 0; preindex < input_number; preindex++)
                    result += input[preindex] * chromosome[index * input_number + preindex];
                final += chromosome[input_number * hidden_number + index] * result;
            }
            return final < 0;
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
            float chance = 0.1f;
            int bound = clone.hidden_number * (clone.input_number + 1);
            Random random = new Random();
            while (chance >= random.NextDouble())
            {
                int index = random.Next(0, bound + 2);
                if (index < bound) clone.chromosome[index] = (float)(200 * random.NextDouble() - 100);
                else clone.bias_weight[index - bound] = (float)(200 * random.NextDouble() - 100);
                chance *= 0.95f;
            }
            return clone;
        }
        public static NeuralNetwork CrossOver(NeuralNetwork father, NeuralNetwork mother)
        {
            int input = mother.input_number, hidden = mother.hidden_number;
            NeuralNetwork child = new NeuralNetwork(input, hidden, false);
            Random random = new Random();
            for (int index = 0; index < hidden * (input + 1) + 2; index++)
            {
                if (index < hidden * (input + 1)) child.chromosome[index] = (random.Next(2) == 1 ? mother : father).chromosome[index];
                else child.bias_weight[index - hidden * (input + 1)] = (random.Next(2) == 1 ? mother : father).bias_weight[index - hidden * (input + 1)];
            }

            return child;
        }
    }
}
