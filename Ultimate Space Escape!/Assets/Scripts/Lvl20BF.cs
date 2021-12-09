using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Lvl20BF : MonoBehaviour
{
	[Header("General Brick Things")]
	public int hits = 0;
	public int points = 100;
	public Vector3 rotator;
	public Material hitMaterial;
	public LootDrops other;
	public ParticleSystem destroyEffect;

	[Header("Explosion Brick")]
	public GameObject explosionEffect;
	public float radius = 5f;
	public PowerUp powerup;
	public Material _orgMaterialCheck; //For ezplosive laser brickz.
	public bool blinkOn = false;

	[Header("Movement and Ghost Brick")]
	public GameObject ghostBrickPrefab;
	[SerializeField]
	public Vector3 targetPos1;
	[SerializeField]
	public Vector3 targetPos2;
	[SerializeField]
	public float speed = 5f;
	public bool canMove;
	public bool firstMove;
	public bool moveableBrick;

	[Header("LaserThings")]
	public Player player;
	public Laser laser;
	public Currency currency;
	public GameObject playerPrefab;
	public bool useLaser = false;
	public bool useSteal = false;
	public GameObject currencyPrefab;


	[Header("EasyMode mayhaps?")]
	public Ball ball;
	public SphereCollider myCollider;

	[Header("Boss Connections")]
	public GameObject coneLeft;
	public GameObject orb;
	public GameObject coneRight;
	public ParticleSystem hitEffect;
    private Material _orgMaterialLeftCone;
	private Material _orgMaterialOrb;
	private Material _orgMaterialRightCone;
	public SphereCollider shockwave;
	public ParticleSystem shockwaveFX;
	public GameObject victorShadow;
	Rigidbody _rigidbody;
	int rand = 0;
	int oldRand;
	Color color1; //Sets a color variable to color of each part of Lvl20BF
	Color color2;
	Color color3;
	public Material portal;
	bool Invincibility = false;
	bool Teleporting = false;
	bool Shockwaver = false;
	int calc = 0;

	
	Vector3[] teleportPos = new Vector3[] 
	{ 
		new Vector3(-10,10,0), new Vector3(7, 1, 0), new Vector3(20,5,0), new Vector3(-20,3,0), new Vector3(-7,-2,0), new Vector3(6,12,0), new Vector3(-5,6,0), 
		new Vector3(10,-7,0), new Vector3(-18,-7,0), new Vector3(20,-4,0) 
	};
    void Start()
    {
		victorShadow.SetActive(false);
		_rigidbody = GetComponent<Rigidbody>();
		shockwave = GetComponent<SphereCollider>();
		shockwave.radius = 1;
		_orgMaterialLeftCone = coneLeft.GetComponent<MeshRenderer>().material;
		_orgMaterialOrb = orb.GetComponent<MeshRenderer>().material;
		_orgMaterialRightCone = coneRight.GetComponent<MeshRenderer>().material;
		firstMove = true;
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
		color1 = _orgMaterialLeftCone.color; //Sets a color variable to color of each part of Lvl20BF
		color2 = _orgMaterialOrb.color;
		color3 = _orgMaterialRightCone.color;
		if (gameObject.tag == "SpawnGhostBrick")
        {
			spawnObject();
			Destroy(gameObject);
		}
	}
	
	IEnumerator TeleportPosition()
    {
		victorShadow.SetActive(false);
		_rigidbody.transform.position = teleportPos[rand];
		color1.a = 1f;
		color2.a = 1f;
		color3.a = 1f;
		_orgMaterialLeftCone.color = color1; //Sets the more transparent color to each of the part's colors.
		_orgMaterialOrb.color = color2;
		_orgMaterialRightCone.color = color3;
		yield return new WaitForSeconds(1);
		Shockwaver = false;
		shockwave.radius = 1;
		yield return new WaitForSeconds(1.5f * hits);
		Teleporting = false;
		oldRand = rand;
		rand = Random.Range(0, 10);
		while (rand == oldRand)
		{
			rand = Random.Range(0, 10);
		}
		victorShadow.SetActive(true);
		victorShadow.transform.position = teleportPos[rand];
		yield return null;
	}

    void FixedUpdate()
    {
        transform.Rotate(rotator * Time.deltaTime);

		if (myCollider != null)
		{
			if (ball.EasyOn == true)
			{
				myCollider.enabled = true;
			}
		}

		
		if (Teleporting == false)
        {
			if (_orgMaterialLeftCone.color.a > 0)
			{
				color1.a -= Time.deltaTime / 3; //Causes them to slowly turn transparent over time
				color2.a -= Time.deltaTime / 3;
				color3.a -= Time.deltaTime / 3;

				_orgMaterialLeftCone.color = color1; //Sets the more transparent color to each of the part's colors.
				_orgMaterialOrb.color = color2;
				_orgMaterialRightCone.color = color3;

				//Sets the new, more transparent material to each of the parts to Lvl20BF;
				coneLeft.GetComponent<MeshRenderer>().material = _orgMaterialLeftCone;
				orb.GetComponent<MeshRenderer>().material = _orgMaterialOrb;
				coneRight.GetComponent<MeshRenderer>().material = _orgMaterialRightCone;

			}
			else
            {
				Teleporting = true;
				Shockwaver = true;
				StartCoroutine(TeleportPosition());
				Instantiate(shockwaveFX, transform.position, Quaternion.identity);
				FindObjectOfType<AudioManager>().Play("ShockwaveSfx");
			}
		}
		else
        {
			if (Shockwaver == true)
            {
				if (calc < 15)
				{
					shockwave.radius += 1;
					calc++;
				}
				if (calc >= 15)
				{
					if (calc < 25)
					{
						shockwave.radius += 0.7f;
						calc++;
					}
					else if (calc < 35)
					{
						shockwave.radius += 0.2f;
						calc++;
					}
					else if (calc < 50)
					{
						shockwave.radius -= 0.08f;
						calc++;
					}
					else
					{
						calc = 0;
					}
				}
			}	
		}	
	}
	
	//Allows Brick to take damage and eventually break after being hit by a ball
	private void OnCollisionEnter(Collision targetObj)
	{
		if (targetObj.gameObject.tag == "Ball")
		{
			if (Invincibility == true)
            {
				return;
            }
			else
            {
				hits--;
				if (hits < -0)
				{
					Invincibility = true;
					if (destroyEffect != null)
					{
						Instantiate(destroyEffect, transform.position, Quaternion.identity);
					}

					Explode();
					other.calculateLoot();
					GameManager.Instance.Score += points;
					FindObjectOfType<AudioManager>().Play("DyingBoss20");
					Destroy(GameObject.FindGameObjectWithTag("BluePortal"), 0.5f);
					Destroy(gameObject, 0.5f);
				}
				else
				{
					Invincibility = true;
					StartCoroutine(InvincibilityFrames());
					FindObjectOfType<AudioManager>().Play("BrickHurt");
				}
			}
			
		}
		
	}

	IEnumerator InvincibilityFrames()
    {
		Instantiate(hitEffect, transform.position, Quaternion.identity);
		HitMaterial();
		yield return new WaitForSeconds(.2f);
		RestoreMaterial();
		yield return new WaitForSeconds(.2f);
		HitMaterial();
		yield return new WaitForSeconds(.2f);
		RestoreMaterial();
		yield return new WaitForSeconds(.2f); 
		HitMaterial();
		yield return new WaitForSeconds(.2f);
		RestoreMaterial();
		yield return null;
		Invincibility = false;
	}

	//Allows original Material to come back after the hit material
	void RestoreMaterial()
	{
		coneLeft.GetComponent<Renderer>().material = _orgMaterialLeftCone;
		orb.GetComponent<Renderer>().material = _orgMaterialOrb;
		coneRight.GetComponent<Renderer>().material = _orgMaterialRightCone;
	}

	void HitMaterial()
    {
		coneLeft.GetComponent<Renderer>().sharedMaterial = hitMaterial;
		orb.GetComponent<Renderer>().material = hitMaterial;
		coneRight.GetComponent<Renderer>().material = hitMaterial;
	}

	public void Explode()
	{
		//Shows Explosive Effect
		Instantiate(explosionEffect, transform.position, transform.rotation);

		Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

		//Detects what type of gameobject the nearby object is, doing things accordingly. 
		//Still don't know how to activate powerups, or if I even want to, so for now they just get destroyed and spawn extra loot.
		foreach (Collider nearbyObject in collidersToDestroy)
		{
			if (nearbyObject.gameObject.tag == "Untagged")
            {
				return;
            }
			if (nearbyObject.gameObject.tag == "Brick")
			{
				nearbyObject.GetComponent<Brick>().other.calculateLoot();
				GameManager.Instance.Score += nearbyObject.GetComponent<Brick>().points;
				Destroy(nearbyObject.gameObject);
			}
			if (nearbyObject.gameObject.tag == "Weird")
			{
				if (nearbyObject.gameObject.name == "Explosive Laser Brick")
				{
					return;
				}
				else if (nearbyObject.gameObject.name == "Explosive Laser Brick (1)")
				{
					return;
				}
				else
				{
					useLaser = false;
					nearbyObject.GetComponent<Brick>().other.calculateLoot();
					GameManager.Instance.Score += nearbyObject.GetComponent<Brick>().points;
					if (player.transform.localScale.x >= 1)
					{
						if (player.transform.localScale.x >= 1.2)
						{
							if (player.transform.localScale.x >= 1.45)
							{
								if (player.transform.localScale.x >= 1.75)
								{
									if (player.transform.localScale.x >= 2.15)
									{
										if (player.transform.localScale.x == 2.5)
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
						Destroy(nearbyObject.gameObject);
					}
				}
			}
			if (nearbyObject.gameObject.tag == "PowerUp")
			{
				PowerUp refPowerup = GetComponent<PowerUp>();
				GameManager.Instance.Score += refPowerup.points * 2;
				Destroy(nearbyObject.gameObject);
			}
			if (nearbyObject.gameObject.tag == "Multiball")
			{
				Multiball refPowerup = GetComponent<Multiball>();
				GameManager.Instance.Score += refPowerup.points * 2;
				Destroy(nearbyObject.gameObject);
			}
			if (nearbyObject.gameObject.tag == "Ghost")
			{
				GameManager.Instance.Score += nearbyObject.GetComponent<Brick>().points;
				nearbyObject.GetComponent<Brick>().other.calculateLoot();
				Destroy(nearbyObject.gameObject);
			}
		}
	}

		public void OnTriggerEnter(Collider targetObj)
		{
			if (gameObject.tag == "Player")
			{
				return;
			}
		}


		public void spawnObject()
		{
			if (ghostBrickPrefab == null)
			{
				return;
			}
			else
			{
				Instantiate(ghostBrickPrefab, transform.position, Quaternion.identity);
			}
		}

		//Allows for the laser to toggle on and off, shotting for one second and then disappearing for 3.
		IEnumerator ToggleLaser()
		{
			if (laser == null)
			{
				yield return null;
			}
			else
			{
				if (useLaser == true)
				{
					if (hits >= 0)
					{
						laser.lr.enabled = true;
						laser.Laseroff = false;
						yield return new WaitForSeconds(1);
						laser.lr.enabled = false;
						laser.Laseroff = true;
						yield return new WaitForSeconds(3);
						useLaser = false;
						yield return null;
					}
				}

				if (useSteal == true)
				{
					if (hits >= 0)
					{

						laser.lr.enabled = true;
						laser.Laseroff = false;
						yield return new WaitForSeconds(1);
						laser.lr.enabled = false;
						laser.Laseroff = true;
						yield return new WaitForSeconds(3);
						useSteal = false;
						yield return null;
					}
				}
			}
		}
}
