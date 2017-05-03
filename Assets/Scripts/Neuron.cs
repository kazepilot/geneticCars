using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActivationFunction
{
    RELU,
    TANH,
    SIGMOID
}


public class Neuron
{
    private float[] _weights;
    private ActivationFunction _func;

    public Neuron(ActivationFunction function = ActivationFunction.RELU)
    {
        _weights = new[]
        {
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f),
            Random.Range(-1.0f, 1.0f)
        };
        _func = function;
    }

    public Neuron(float[] weights, ActivationFunction function)
    {
        _weights = new float[weights.Length];
        for (int i = 0; i < _weights.Length; i++)
        {
            _weights[i] = weights[i];
        }
        _func = function;
    }

    public float Activate(float[] input)
    {
        float sum = 0;

        if (input.Length != _weights.Length)
        {
            throw new Exception("Dimensions do not match");
        }

        for (int i = 0; i < input.Length; i++)
        {
            sum += input[i]*_weights[i];
        }

        switch (_func)
        {
            case ActivationFunction.RELU:
                sum = (sum > 0) ? sum : 0;
                break;
            case ActivationFunction.TANH:
                sum = (float) Math.Tanh(sum);
                break;
            case ActivationFunction.SIGMOID:
                sum = 1/(1 + Mathf.Exp(-1*sum));
                break;
        }

        return sum;
    }

    public float[] GetWeights()
    {
        return _weights;
    }

    public ActivationFunction GetFunction()
    {
        return _func;
    }

    public Neuron Copy()
    {
        return new Neuron(_weights, _func);
    }

    public void Mutate(float rate)
    {
        for (int i = 0; i < _weights.Length; i++)
        {
            if (Random.Range(0.0f, 1.0f) < rate)
            {
                _weights[i] += Random.Range(-1.0f, 1.0f) * 0.2f;
            }
        }
    }

    public void Merge(Neuron n1, float s1, Neuron n2, float s2)
    {
        float[] w1 = n1.GetWeights();
        float[] w2 = n2.GetWeights();

        if (w1.Length != w2.Length)
        {
            throw new Exception("Merge arrays do not match");
        }
        float prob = s1/(s1 + s2);

        for (int i = 0; i < w1.Length; i++)
        {
            _weights[i] = (Random.Range(0.0f, 1.0f) < prob) ? w1[i] : w2[i];
        }

        _func = (Random.Range(0.0f, 1.0f) < prob) ? n1.GetFunction() : n2.GetFunction();
    }

    public string Export()
    {
        string results = "";
        for (int i = 0; i < _weights.Length; i++)
        {
            results += _weights[i] + " ";
        }

        return results;
    }

}
