using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Transform destination;
    public float distance;

    [SerializeField] private bool isOrange;
    [SerializeField] private GameObject electricity;
    public Collider holder;

    public Vector3 rotator;

    private int xPosRand;
    private int yPosRand;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
        if (isOrange == false)
        {
            if (GameObject.FindGameObjectWithTag("OrangePortal"))
            {
                destination = GameObject.FindGameObjectWithTag("OrangePortal").GetComponent<Transform>();
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("BluePortal"))
            {
                destination = GameObject.FindGameObjectWithTag("BluePortal").GetComponent<Transform>();
            }
        }

    }
    private void FixedUpdate()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (Vector3.Distance(transform.position, other.transform.position) > distance)
        {
            if (other.gameObject.tag == "Ball")
            {
                if (destination)
                {
                    other.transform.position = new Vector3(destination.position.x, destination.position.y);
                }
                else
                {
                    xPosRand = Random.Range(-15, 15);
                    yPosRand = Random.Range(-8, 15);
                    other.transform.position = new Vector3(xPosRand, yPosRand);
                }
            }
            else
            {
                if (other.gameObject.tag != "Player")
                {
                    holder = other;
                    StartCoroutine(TeleportDelay());
                }
            }
        }
    }

    IEnumerator TeleportDelay()
    {
        yield return new WaitForSeconds(1f/2f);
        if (holder != null)
        {
            if(destination)
            {
                holder.transform.position = new Vector3(destination.position.x, destination.position.y);
            }
            else
            {
                xPosRand = Random.Range(-15, 15);
                yPosRand = Random.Range(-8, 15);
                holder.transform.position = new Vector3(xPosRand, yPosRand);
            }
            holder = null;
        }
    }
}
