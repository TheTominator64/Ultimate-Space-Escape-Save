using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 22f;
	Rigidbody _rigidbody;
    Renderer _renderer;
    [SerializeField] private Vector3 _velocity;
    private Vector3 rotator;
    private Vector3 veloChecker;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject goopEffect;
   

    bool LaserThroughCD = false;


    private Transform target;
    public bool EasyOn;
    public float step;
    public bool Wait = true;

    private Vector3 RandomVector(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(min, max);
        var z = Random.Range(min, max);
        return new Vector3(x, y, z);
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
		_renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    void Launch()
    {
        //Launches Ball up at constant velocity
        _rigidbody.velocity = Vector3.up * _speed;
        if (EasyOn == true)
        {
            Wait = false;
        }
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized * _speed;
		_velocity = _rigidbody.velocity;

        if (fireEffect.activeSelf == true)
        {
            Instantiate(fireEffect, transform.position, transform.rotation);
        }

        if (goopEffect.activeSelf == true)
        {
            Instantiate(goopEffect, transform.position, transform.rotation);
        }

        if (_rigidbody.velocity.y <= 1.5)
        {
            if (_rigidbody.velocity.y >= -1.5)
            {
                StartCoroutine(CheckBallVelocity());
            }
        }

        //Destroys Ball when it goes offscreen
        if (!_renderer.isVisible)
        {
			GameManager.Instance.Balls--;

            FindObjectOfType<AudioManager>().Play("BallDead");

            Destroy(gameObject);

        }
    }
	
    //Allows Ball to collide and bounce off of other objects
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            veloChecker = Vector3.Reflect(_velocity, collision.contacts[0].normal);
            if(veloChecker.y < 0)
            {
                veloChecker.y *= -1;
            }
            _rigidbody.velocity = veloChecker;
        }
        else
        {
            _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (LaserThroughCD == false)
        {
            if (collider.gameObject.tag == "LaserThrough")
            {
                _rigidbody.velocity = ReflectVector();
                LaserThroughCD = true;
                StartCoroutine(OnTriggerCooldown());
            }
        }
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
            else if (other.gameObject.tag == "Explosive")
            {
                if (Wait == false)
                {
                    StartCoroutine(AttractionToBrick());
                }
            }
            else if (other.gameObject.tag == "Weird")
            {
                if (Wait == false)
                {
                    StartCoroutine(AttractionToBrick());
                }
            }
        }
    }

    private Vector3 ReflectVector()
    {
        var x = _velocity.x * -1;
        var y = _velocity.y * -1;
        var z = _velocity.z * 1;
        return new Vector3(x, y, z);
    }

    IEnumerator OnTriggerCooldown()
    {
        yield return new WaitForSeconds(1f / 10f);
        LaserThroughCD = false;
        
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
                        _rigidbody.velocity += Vector3.down * 4;
                        yield return null;
                    }
                    else
                    {
                        _rigidbody.velocity += Vector3.up  * 2;
                        yield return null;
                    }
                }
                else
                {
                    _rigidbody.velocity += Vector3.down * 2;
                    yield return null;
                }
            }
        }

    }

    public void InstantiateFireEffect()
    {

    }

    public void InstantiategoopEffect()
    {

    }

    //Four following public voids allow for fire effect and goop effect to be toggled on and off by other scripts, without directly referencing the GameObjects;
    public void FireEffectOn()
    {
        fireEffect.SetActive(true);
    }

    public void FireEffectOff()
    {
        fireEffect.SetActive(false);
    }

    public void GoopEffectOn()
    {
        goopEffect.SetActive(true);
    }

    public void GoopEffectOff()
    {
        goopEffect.SetActive(false);
    }

    //Possible easy mode for the game, where ball is attracted to Brickz?
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
            yield return new WaitForSeconds(0.005f);
            Wait = false;
        }
    }
}
