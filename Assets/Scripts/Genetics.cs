using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Genetics
{
    private List<Agent> _agents;
    private int _gen;
    private List<int> _topScores;

    public Genetics()
    {
        _agents = new List<Agent>();
        _topScores = new List<int>();
        _gen = 1;
    }

    public void AddAgent(Agent a)
    {
        _agents.Add(a);
    }

    public void Evolve()
    {
        List<Agent> agents = _agents.OrderBy(x => x.Brains.Score).Reverse().Take(_agents.Count/10).ToList();
        List<NeuralNetwork> brains = new List<NeuralNetwork>();
        foreach (var agent in agents)
        {
            brains.Add(agent.Brains);
        }
        int score = (int) brains[0].Score;
        _topScores.Add(score);
        string filename = _gen + "_" + score + ".txt";
        File.WriteAllText("results/" + filename, brains[0].Export());

        if (score < 5)
        {
            _gen++;
            for (int i = 0; i < _agents.Count; i++)
            {
                _agents[i].Mutate();
            }
            return;
        }


        for (int i = 0; i < _agents.Count; i++)
        {
            NeuralNetwork p1 = brains[Random.Range(0, brains.Count)];
            NeuralNetwork p2 = brains[Random.Range(0, brains.Count)];
            Neuron[] parent1 = p1.Layers();
            Neuron[] parent2 = p2.Layers();
            Neuron a = new Neuron();
            Neuron b = new Neuron();
            Neuron c = new Neuron();
            a.Merge(parent1[0], p1.Score, parent2[0], p2.Score);
            b.Merge(parent1[1], p1.Score, parent2[1], p2.Score);
            c.Merge(parent1[2], p1.Score, parent2[2], p2.Score);
            NeuralNetwork new_brains = new NeuralNetwork(a, b, c);
            new_brains.Mutate();

            _agents[i].Brains = new_brains;
        }

        _gen++;

    }

    public void Reset(Vector2 position)
    {
        foreach (var agent in _agents)
        {
            agent.Reset(position);
        }
    }

    public int Alive()
    {
        int alive = 0;
        foreach (var agent in _agents)
        {
            if (agent.Alive)
                alive++;
        }
        return alive;
    }

    public float Score
    {
        get
        {
            float score = 0;
            foreach (var agent in _agents)
            {
                if (agent.Score > score)
                {
                    score = agent.Score;
                }
            }
            return score;
        }
    }

    public int Generation
    {
        get
        {
            return _gen;
            
        }
    }
}
