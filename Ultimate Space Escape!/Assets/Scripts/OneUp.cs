using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : MonoBehaviour
{
    float _speed = 10f;
    Vector3 _velocity;
    Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Invoke("Launch", 0.5f);
    }

    //Lets OneUp fall at a constant rate
    void Launch()
    {
        _rigidbody.velocity = Vector3.down * _speed;
    }

    void FixedUpdate()
    {
    }

    //Allows Ball to only be picked up by player paddle, giving player +1 Ball
    private void OnTriggerEnter(Collider targetObj)
	{
        if (targetObj.gameObject.tag == "Player")
        {
            if (targetObj is MeshCollider)
            {
                FindObjectOfType<AudioManager>().Play("BallCollect");
                GameManager.Instance.Balls++;
                Destroy(gameObject);
            }
        }  
	}
}
