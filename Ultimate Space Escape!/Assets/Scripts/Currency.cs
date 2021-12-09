using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour
{
    public float speed = 5f;
    public int points = 50;
    [SerializeField] private Vector3 rotator;
    [SerializeField] private Vector3 _velocity;
    Rigidbody _rigidbody;

    
    [Header("Magnet Tings")]
    public bool MagnetOn = false;
    private Transform target;
    public float step;
    public SphereCollider laserStealDetect;

    bool StealAttract = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (gameObject.tag == "StolenCoin")
        {
            laserStealDetect = GetComponent<SphereCollider>();
            laserStealDetect.enabled = false;
        }
        if (gameObject.tag == "Vault" && transform.root.gameObject == null)
        {
            transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
            Invoke("Launch", 0.5f);
        }
        if (gameObject.tag == "Vault" || gameObject.tag == "StolenCoin")
        {
            return;
        }
        else
        {
            transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
            Invoke("Launch", 0.5f);
        }
    }

    void Launch()
    {
        //Launches Ball up at constant velocity
        _rigidbody.velocity = Vector3.down * speed;
    }


    //Allows for Currency model to rotate
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);

        if (gameObject.tag == "StolenCoin")
        {
            if (StealAttract == true)
            {
                StartCoroutine(StealLaserAttractor());
                StealAttract = false;
            }
        }
    }


    public void ActivateCollider()
    {
        if (gameObject.tag == "StolenCoin")
        {
            laserStealDetect.enabled = true;
        }
        else
        {
            return;
        }
    }

    IEnumerator StealLaserAttractor()
    {
        if (GameObject.FindWithTag("LaserSteal") == null)
        {
            yield return null;
        }
        else
        {
            target = GameObject.FindGameObjectWithTag("LaserSteal").transform;
            if (target == null)
            {
                yield return null;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                yield return new WaitForSeconds(0.1f);
                StealAttract = true;
            }
        }
    }
    
    // Currency can only be picked up by the player paddle
    private void OnTriggerEnter(Collider targetObj) 
    {
        if (targetObj == null)
        {
            return;
        }
        else
        {
            if (targetObj.gameObject.tag == "Magnet")
            {

                StartCoroutine(Move());

            }
            else if(targetObj.gameObject.tag == "Player")
            {
                    if (gameObject.tag == "Vault")
                    {
                        // Gives player +10 coins, plays sfx, and gives points for vaults
                        FindObjectOfType<AudioManager>().Play("VaultCollect");          
                        GameManager.Instance.Coins = GameManager.Instance.Coins + Random.Range(7 , 16);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (gameObject.tag == "StolenCoin")
                        {
                            return;
                        }
                        else
                        {
                            // Gives player +1 coin, plays sfx, and gives points for coins
                            FindObjectOfType<AudioManager>().Play("CoinCollect");
                            GameManager.Instance.Coins++;
                            GameManager.Instance.Score += points;
                            Destroy(gameObject);
                        }
                    }
            }

            if (targetObj.gameObject.tag == "Ball")
            {
                if (gameObject.tag == "Vault")
                {
                    //Lets Currency fall at a constant rate
                    _rigidbody.velocity = Vector3.down * speed;
                }
            }
            if (targetObj.gameObject.tag == "LaserSteal")
            {
                if (gameObject.tag == "Untagged")
                {
                    return;
                }
                if (targetObj is SphereCollider)
                {
                    if (gameObject.tag == "StolenCoin")
                    {
                        StealAttract = true;
                    }
                }
            }
        }
    }

    public IEnumerator Move()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        while(MagnetOn == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
