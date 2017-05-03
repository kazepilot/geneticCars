using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Agent : MonoBehaviour
{

    public Vector2 velocity;

    private const float RAY_DISTANCE = 2f;
    private const int ROTATION_ANGLE = 45;

    private NeuralNetwork _brains;

    private bool _isAlive = true;
    private float _speed = 1f;
    private Rigidbody2D _rigidbody2D;
    private float _score = 0;
    private int _frames = 0;
    private Vector2 _prevPosition;
    private int _move;
    private Vector2 _velVector2;
    private Vector2 _pPos;
    private int _rotations = 0;

    // Use this for initialization
    void Start ()
	{
        _brains = new NeuralNetwork();
	    _rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        _frames++;
        if (_isAlive)
        {
           Vector3 rotation = transform.rotation.eulerAngles;

           float[] distances = new float[3];
           distances[0] = (GetDistance(Quaternion.Euler(0, 0, rotation.z) * Vector2.left)); // -45
           distances[1] = (GetDistance(Quaternion.Euler(0, 0, rotation.z) * Vector2.up));
           distances[2] = (GetDistance(Quaternion.Euler(0, 0, rotation.z) * Vector2.right)); // + 45

           int forward = 1;
           int _move = _brains.Predict(distances.ToList());
           switch (_move)
           {
               case 0:
                   rotation.z += ROTATION_ANGLE;
                   _rotations++;
                   break;
               case 1:
                    rotation.z -= ROTATION_ANGLE;
                    _rotations++;
                    break;
            }
            rotation.z = Mathf.RoundToInt(rotation.z);
            transform.rotation = Quaternion.Euler(rotation);

            _velVector2 = transform.up * _speed * forward;

            _rigidbody2D.velocity = _velVector2;
            velocity = _rigidbody2D.velocity;

            if (Mathf.Abs(transform.position.y - _pPos.y) < 0.01)
            {
                transform.position = new Vector2(transform.position.x, _pPos.y);
            }

            _pPos = transform.position;

            if (_frames % 10 == 0)
            {
                /*
                if(_rotations > 2)
                    Die();
                _rotations = 0;
                */
            }

            if (_frames % 30 == 0)
            {
                float dist = Vector2.Distance(transform.position, _prevPosition);
                if (dist < 0.4)
                {
                    Die();
                }
                else
                {
                    _score += dist;
                    _prevPosition = transform.position;
                }
            }
        }
    }

    public void Reset(Vector2 pos)
    {
        _isAlive = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
        transform.position = pos;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _score = 0;
        _frames = 0;
        _rotations = 0;
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void Die()
    {
        _isAlive = false;
        _rigidbody2D.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 0.2f, 0.3f);
    }

    float GetDistance(Vector2 direction)
    {
        int layerMask = 9;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, RAY_DISTANCE, layerMask);
        Debug.DrawRay(transform.position, direction * RAY_DISTANCE, Color.green);
        if (hit.collider != null)
        {
            float distance = Vector2.Distance(transform.position, hit.point);
            return distance / RAY_DISTANCE;
        }

        return 1.0f;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogWarning(other.gameObject.name);
    }

    public void Mutate()
    {
        _brains.Mutate();
    }

    public float Score
    {
        get
        {
            return _score;
        }
    }

    public NeuralNetwork Brains
    {
        get
        {
            _brains.Score = _score;
            return new NeuralNetwork(_brains);
            
        }
        set
        {
            _brains = value;
            
        }
    }

    public bool Alive
    {
        get
        {
            return _isAlive;
        }
    }
}
