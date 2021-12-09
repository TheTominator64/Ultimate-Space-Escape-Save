using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float duration = 4f;
    public int points = 100;
    [SerializeField] private float multiplier = 2f;
    [SerializeField] private Vector3 rotator;
    [SerializeField] private Vector3 targetPos1;
    [SerializeField] private Vector3 targetPos2;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool canMove;
    [SerializeField] private bool firstMove;
    [SerializeField] private bool movingPowerUp;
    [SerializeField] private CapsuleCollider explosionCollision;
    [SerializeField] private Collider trigCollider;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject pickupEffect;


    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
    }
  
    //Allows for PowerUp model to rotate
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);

        if (movingPowerUp == true)
        {
            if (transform.position == targetPos1)
            {
                firstMove = false;
            }
            if (transform.position == targetPos2)
            {
                firstMove = true;
            }
            if (canMove)
            {
                if (firstMove)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos1, speed * Time.deltaTime);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos2, speed * Time.deltaTime);
                }
            }
        }
        else
        {
            return;
        }

    }

    //PowerUp only activates when collided with GameObjects with the "Ball" tag
    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            StartCoroutine( Pickup(other) );
        }
    }

    //Where all of the PowerUp magic happens
    public IEnumerator Pickup(Collider ball)
    {

        Instantiate(pickupEffect, transform.position, transform.rotation);

        FindObjectOfType<AudioManager>().Play("PowerUp");

        //Makes Ball larger, and adds points.
        ball.transform.localScale *= multiplier;
        GameManager.Instance.Score += points;

       trigCollider.enabled = false;
       meshRenderer.enabled = false;
        explosionCollision.enabled = false;

        yield return new WaitForSeconds(duration);

        //Allows the Ball to shrink back to a normal size after PowerUp duration runs out
        if (ball != null)
        {
            FindObjectOfType<AudioManager>().Play("BallShrink");
            ball.transform.localScale /= multiplier;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       

    }

}
