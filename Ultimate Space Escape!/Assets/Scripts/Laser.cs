using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Use Laser")]
    public LineRenderer lr;
    [SerializeField]
    private Transform startPoint;
    bool Shrunk = false;
    public bool Laseroff = false;
    public RaycastHit hit;

    [Header("Use Stealing Laser")]
    bool Steal = false;
    public Player player;
    public GameObject stolenCoin;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Laseroff == false)
        {
            lr.SetPosition(0, startPoint.position);
            if (Physics.Raycast(transform.position, -transform.up, out hit))
            {
                if (hit.collider)
                {
                    if (hit.transform.tag == "LaserThrough")
                    {
                        lr.SetPosition(1, -transform.up * 5000);
                    }
                    else
                    {
                        lr.SetPosition(1, hit.point);
                    }
                }

                if (hit.transform.tag == "Player")
                {
                    /* if (gameObject.tag == "StealingLaser")
                    {
                        if (Steal == false)
                        {
                            if (GameObject.Find("GameManager").GetComponent<GameManager>()._coins > 0)
                            {
                                Instantiate(stolenCoin, transform.position, Quaternion.identity);
                                GameObject.Find("GameManager").GetComponent<GameManager>()._coins--;
                                Steal = true;
                                StartCoroutine(EnemyLaserCoolDown());
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    { */
                        if (Shrunk == false)
                        {
                          /*  if (gameObject.tag == "StealingLaser") //Code taken out bc it's not currently used.
                            {
                                return;
                            }
                            else
                            { */
                                hit.transform.localScale /= 1.5f;
                                Shrunk = true;
                                StartCoroutine(EnemyLaserCoolDown());
                           // }
                       // }
                    }
                }
            }

            else
            {
                lr.SetPosition(1, -transform.up * 5000);
            }
        }
    }



    // Brings the paddle back to normal size, aka the laser effect is removed after 5 seconds.
    public IEnumerator EnemyLaserCoolDown()
    {
        if (Shrunk == true)
        {
            yield return new WaitForSeconds(5);
            Shrunk = false;
            if (player.transform.localScale.x >= 1)
            {
                if(player.transform.localScale.x >= 1.2)
                {
                    if(player.transform.localScale.x >= 1.45)
                    {
                        if(player.transform.localScale.x >= 1.75)
                        {
                            if(player.transform.localScale.x >= 2.15)
                            {
                                if(player.transform.localScale.x == 2.5)
                                {
                                    GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                                }
                                else
                                {
                                    GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                                }
                            }
                            else
                            {
                                GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                            }
                        }
                        else
                        {
                            GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
                        }
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    }
                }   
                else
                {
                    GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
            yield return null;
        }

        if (Steal == true)
        {
            yield return new WaitForSeconds(3);
            Steal = false;
            yield return null;
        }
    }
}
