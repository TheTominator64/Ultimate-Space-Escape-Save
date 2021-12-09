using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
   public Rigidbody _rigidbody;
   public SphereCollider myCollider;
   public MeshRenderer meshy;
   public Currency currency;

    [Header("Use Laser")]
    public LineRenderer lr;
    [SerializeField] private Transform startPoint;
    public RaycastHit hit;
    public Material transparent;
    public Material boughtLaser;
    public LootDrops lootdrops;
    public Brick brick;
    public bool Laseroff;
    public bool LaserUpgrade;
    public bool LaserCannonWait;

    [Header("Laser Upgrades (Don't mess with in Inspector)")]
    public bool NormalLaser;
    public bool BetterLaser;
    public bool EvenBetterLaser;
    public bool TheBestLaser;

    [Header("Updating Tings")]

    bool DestroyBrick = true;
    bool moveMouse = true;
    bool hindering = false;

    /* 
    bool DestroyWeird = true; (Code that is no longer used)
    bool DestroyExplosive = true;
    */

    [SerializeField] private GameObject stolenCoin;

    //Gives the Player Paddle a rigidbody
    void Start()
    {
        _rigidbody = GetComponent <Rigidbody>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        Laseroff = true;
    }

    //Makes Paddle follow the left and right direction wherever the player's mouse is
    void FixedUpdate()
    {
        if (moveMouse == true)
        {
            _rigidbody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, 50)).x, -17, 0));
        }

        if (currency.MagnetOn == true)
        {
            myCollider.enabled = true;
        }
    }

    //Was originally all in FixedUpdate(). Test out, and try to put as much as possible back into fixed update.
    private void Update()
    {
        if (LaserUpgrade == true)
        {
            GetComponent<LineRenderer>().material = boughtLaser;

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (LaserCannonWait == false)
                {
                    StartCoroutine(ToggleLaser());
                }
            }

            if (Laseroff == false)
            {
                lr.SetPosition(0, startPoint.position);
                if (Physics.Raycast(transform.position, transform.up, out hit))
                {
                    if (hit.collider)
                    {
                        if (hit.transform.tag == "Ball" || hit.transform.tag == "Wall" || hit.transform.tag == "Untagged")
                        {
                            lr.SetPosition(1, transform.up * 5000);
                        }
                        else
                        {
                            lr.SetPosition(1, hit.point);
                        }

                        if (hit.transform.tag == "Brick" || hit.transform.tag == "Weird" || hit.transform.tag == "Explosive")
                        {
                            if (DestroyBrick == true)
                            {
                                StartCoroutine(LaserHitBrick());
                            }
                        }
                    }
                }
                else
                {
                    lr.SetPosition(1, transform.up * 5000);
                }
            }
        }
    }

    // Make noise activate whenever something hits the Paddle; make it so that sound changes whether it's from a Ball or something else?
    private void OnCollisionEnter(Collision targetObj)
    {
        if (targetObj.gameObject.tag == "Ball")
        {
            FindObjectOfType<AudioManager>().Play("BallBounce");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Shockwave")
        {
            if (hindering == false)
            {
                hindering = true;
                StartCoroutine(HinderPaddle());
            }

        }
    }

    public void MeshInvisible()
    {
        meshy.enabled = true;
    }

    public void MeshVisible()
    {
        meshy.enabled = true;
    }
    //Makes the paddle uncontrollable to the player for a short duration, the paddle randomly teleporting on the screen in the process.
    IEnumerator HinderPaddle()
    {
        int i = 0;
        int oldRand = 0;
        int rand = 0;
        moveMouse = false;
        while (i < 3)
        {
            i++;
            oldRand = rand;
            rand = Random.Range(-25, 25);
            while (rand + 5 >= oldRand && rand - 5 <= oldRand)
            {
                oldRand = rand;
                rand = Random.Range(-25, 25);
            }
            _rigidbody.position = new Vector3(rand, -17, 0);
            FindObjectOfType<AudioManager>().Play("TeleportPaddle");
            yield return new WaitForSeconds(0.5f);
        }
        moveMouse = true;
        FindObjectOfType<AudioManager>().Play("TeleportPaddle");
        hindering = false;
        yield return null;
    }

    IEnumerator ToggleLaser()
    {
        if (NormalLaser == true)
        {
            if (BetterLaser == true)
            {
                if (EvenBetterLaser == true)
                {
                    if (TheBestLaser == true)
                    {
                        lr.enabled = true;
                        Laseroff = false;
                        yield return new WaitForSeconds(3);
                        LaserCannonWait = true;
                        lr.enabled = false;
                        Laseroff = true;
                        yield return new WaitForSeconds(5);
                        LaserCannonWait = false;
                        yield return null;
                    }
                    else
                    {
                        lr.enabled = true;
                        Laseroff = false;
                        yield return new WaitForSeconds(5f/2f);
                        LaserCannonWait = true;
                        lr.enabled = false;
                        Laseroff = true;
                        yield return new WaitForSeconds(6);
                        LaserCannonWait = false;
                        yield return null;
                    }
                }
                else
                {
                    lr.enabled = true;
                    Laseroff = false;
                    yield return new WaitForSeconds(3f /2f);
                    LaserCannonWait = true;
                    lr.enabled = false;
                    Laseroff = true;
                    yield return new WaitForSeconds(7);
                    LaserCannonWait = false;
                    yield return null;
                }
            }
            else
            {
                lr.enabled = true;
                Laseroff = false;
                yield return new WaitForSeconds(2f / 3f);
                LaserCannonWait = true;
                lr.enabled = false;
                Laseroff = true;
                yield return new WaitForSeconds(8);
                LaserCannonWait = false;
                yield return null;
            }
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator LaserHitBrick()
    {
        DestroyBrick = false;
        hit.collider.gameObject.GetComponent<Brick>().hits--;
        yield return new WaitForSeconds(1f/4f);
        DestroyBrick = true;
        yield return null;
    }


    /* Look Away! May bring this code back 0_0
    IEnumerator LaserDropsExplosive()
    {
        DestroyExplosive = false;
        yield return new WaitForSeconds(1f/2f);
        DestroyExplosive = true;
        brick.Explode();
        lootdrops.calculateLoot();
        lootdrops.calculateLoot();
        GameManager.Instance.Score += brick.points;
        FindObjectOfType<AudioManager>().Play("BrickBombExplosion");
        yield return null;
    }
    */

    public void StolenCoinSpawn()
    {
        Instantiate(stolenCoin,transform.position,Quaternion.identity);
    }


}
