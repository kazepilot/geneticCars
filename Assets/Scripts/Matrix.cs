using UnityEngine;
using Random = UnityEngine.Random;

public class Matrix
{
    private float[,] _weights;
    private int _x;
    private int _y;

    public Matrix(int x, int y)
    {
        _x = x;
        _y = y;
        _weights = new float[x, y];
    }

    public int x
    {
        get { return _x; }
    }

    public int y
    {
        get { return _y; }
    }

    public float this[int x, int y]
    {
        get { return _weights[x, y]; }
        set { _weights[x, y] = value; }
    }

    public Matrix Copy
    {
        get
        {
            Matrix m = new Matrix(_x, _y);
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    m[i, j] = _weights[i, j];
                }
            }
            return m;
        }
    }

    public Matrix T
    {
        get
        {
            Matrix m = new Matrix(_y, _x);
            for (var i = 0; i < _x; i++)
            {
                for (var j = 0; j < _y; j++)
                {
                    m[j, i] = _weights[i, j];
                }
            }
            return m;
        }
    }

    public void Mutate(float range)
    {
        for (var i = 0; i < _x; i++)
        {
            for (var j = 0; j < _y; j++)
            {
                _weights[i, j] = ((Random.Range(0.0f, 1.0f) < range)
                    ? _weights[i, j] + Random.Range(-1.0f, 1.0f) * 0.1f
                    : _weights[i, j]);
            }
        }
    }

    public void sigmoid()
    {
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                _weights[i, j] = 1/(1 + Mathf.Exp(-_weights[i, j]));
            }
        }
    }

    public void softmax()
    {
        float sum = 0f;

        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                sum += _weights[i, j];
            }
        }

        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                _weights[i, j] /= sum;
            }
        }
    }

    public void tanh()
    {
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                _weights[i, j] = Mathf.Tan(_weights[i, j]);
            }
        }
    }

    public void relu()
    {
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                _weights[i, j] = Mathf.Max(0, _weights[i, j]);
            }
        }
    }

    public static Matrix operator *(Matrix m1, Matrix m2)
    {
        Matrix m = new Matrix(m1.x, m2.y);

        // Debug.Log(m1.x + " " + m1.y + " | " + m2.x + " " + m2.y);

        for (int i = 0; i < m1.x; i++)
        {
            for (int j = 0; j < m2.y; j++)
            {
                float temp = 0;
                for (int k = 0; k < m1.y; k++)
                {
                    temp += m1[i, k] * m2[k, j];
                }
                m[i, j] = temp;
            }
        }

        return m;
    }

    public static Matrix RandomMat(int x, int y)
    {
        Matrix m = new Matrix(x, y);
        for (var i = 0; i < x; i++)
        {
            for (var j = 0; j < y; j++)
            {
                m[i, j] = Random.Range(-1.0f, 1.0f);
            }
        }

        return m;
    }
}