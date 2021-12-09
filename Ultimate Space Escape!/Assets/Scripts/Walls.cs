using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour
{
    public int hits = 0;

  
    private void OnCollisionEnter(Collision collision)
    {
        hits--;
        if (hits > -0)
        {
            if (collision.gameObject.tag == "Player")
            {
                return;
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("BallBounce");
            }
        }
    }
}
