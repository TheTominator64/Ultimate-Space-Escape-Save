using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FakeBall : MonoBehaviour
{
    float _speed = 11f;
    Rigidbody _rigidbody;
    Vector3 _velocity;
    Renderer _renderer;
    GameObject _currentLevel;

    //Experimental tings for attract (easy) mode
    private Transform target;
    public bool EasyOn = true;
    public float step;
    public bool Wait = true;



    //Gives FakeBall a collision and weight
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    //Allows FakeBall to launch up at a constant velocity
    void Launch()
    {
        _rigidbody.velocity = Vector3.up * _speed;
        if (EasyOn == true)
        {
            Wait = false;
        }
    }

    //Destorys to FakeBall if it goes offscreen, the escape button is pressed (For GameOver), or if the level has been completed
    void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
        _velocity = _rigidbody.velocity;

        if (!_renderer.isVisible)
        {
            Destroy(gameObject);
        }

        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }

        if (_rigidbody.velocity.y <= 1.5)
        {
            if (_rigidbody.velocity.y >= -1.5)
            {
                StartCoroutine(CheckBallVelocity());
            }
        }
    }

    // Is meant to correct or speed up the velocity of the ball if it moves too horizontally. 
    // Maybe create a system that checks how long it's been since it hit an object (not wall)?
    IEnumerator CheckBallVelocity()
    {
        yield return new WaitForSeconds(4);

        if (_rigidbody.velocity.y <= 1.5)
        {
            if (_rigidbody.velocity.y >= -1.5)
            {
                if (_rigidbody.velocity.y >= 0)
                {
                    if (transform.position.y <= 17)
                    {
                        _rigidbody.velocity += Vector3.down;
                        yield return null;
                    }
                    else
                    {
                        _rigidbody.velocity += Vector3.up;
                        yield return null;
                    }
                }
                else
                {
                    _rigidbody.velocity += Vector3.down;
                    yield return null;
                }
            }
        }

    }

    //Allows the FakeBall to bounce off of other objects
    private void OnCollisionEnter(Collision collision)
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
    }

    private void OnTriggerStay(Collider other)
    {
        if (EasyOn == true)
        {
            if (other.gameObject.tag == "Brick")
            {
                if (Wait == false)
                {
                    StartCoroutine(AttractionToBrick());
                }
            }
        }
    }

    //Possible easy mode for the game, where fakeball is attracted to Brickz?
    public IEnumerator AttractionToBrick()
    {
        target = GameObject.FindGameObjectWithTag("Brick").transform;

        if (target == null)
        {
            yield return null;
        }
        else
        {
            Wait = true;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            yield return new WaitForSeconds(0.05f);
            Wait = false;
        }
    }
}
