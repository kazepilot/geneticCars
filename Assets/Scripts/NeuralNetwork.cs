using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork
{
    private Neuron _n1;
    private Neuron _n2;
    private Neuron _n3;

    public NeuralNetwork()
    {
        _n1 = new Neuron();
        _n2 = new Neuron();
        _n3 = new Neuron();
    }

    public NeuralNetwork(Neuron n1, Neuron n2, Neuron n3)
    {
        _n1 = n1;
        _n2 = n2;
        _n3 = n3;
    }

    public NeuralNetwork(NeuralNetwork original)
    {
        Neuron[] items = original.Layers();
        _n1 = items[0];
        _n2 = items[1];
        _n3 = items[2];
        Score = original.Score;
    }

    public int Predict(List<float> input)
    {
        input.Add(1.0f);
        float[] inputs = input.ToArray();
        float[] values = {
            _n1.Activate(inputs),
            _n2.Activate(inputs),
            _n3.Activate(inputs)
        };

        float sum = 0;
        foreach (var value in values)
        {
            sum += Mathf.Exp(value);
        }

        for (var i = 0; i < values.Length; i++)
        {
            values[i] = Mathf.Exp(values[i])/sum;
        }

        var max = values.Max();

        return values.ToList().IndexOf(max);

    }

    public Neuron[] Layers()
    {
        return new[]
        {
            _n1.Copy(),
            _n2.Copy(),
            _n3.Copy()
        };
    }

    public void Mutate()
    {
        float evolve_rate = 0.1f;
        _n1.Mutate(evolve_rate);
        _n2.Mutate(evolve_rate);
        _n3.Mutate(evolve_rate);
    }

    public NeuralNetwork Copy()
    {
        return new NeuralNetwork(this);
    }

    public float Score { set; get; }

    public string Export()
    {
        return _n1.Export() + "\n" + _n2.Export() + "\n" + _n3.Export();
    }
}
