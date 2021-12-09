using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Multiball : MonoBehaviour
{
    public int points = 100;
    [SerializeField] private Vector3 rotator;
    [SerializeField] private Vector3 targetPos1;
    [SerializeField] private Vector3 targetPos2;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool canMove;
    [SerializeField] private bool firstMove;
    [SerializeField] private bool movingPowerUp;
    [SerializeField] private GameObject pickupEffect;
    [SerializeField] private GameObject fakeballPrefab;
 

    void Start()
    {
        firstMove = true;
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
    }

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
    }

    //When this powerup is hit, it spawns two fakeballs and than gets destroyed.
    public void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Ball"))
        {
            Instantiate(pickupEffect, transform.position, transform.rotation);

            FindObjectOfType<AudioManager>().Play("PowerUp");

            GameManager.Instance.Score += points;
            spawnObject();
            spawnObject();

            Destroy(gameObject);
        }
    }

    public void spawnObject()
    {
        Instantiate(fakeballPrefab, transform.position, Quaternion.identity);
    }
}

