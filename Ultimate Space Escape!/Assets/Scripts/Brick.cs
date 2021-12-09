using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Brick : MonoBehaviour
{
	[Header("General Brick Things")]
	public int hits = 0;
	public int points = 100;
	public Vector3 rotator;
	public Material hitMaterial;
	public LootDrops other;
	public LootDrops lootFromExploded;
	public ParticleSystem destroyEffect;
	public ParticleSystem smokeEffect;

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

	private bool destruction;
	Material _orgMaterial;
	Renderer _renderer;
	
    void Start()
    {
		firstMove = true;
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
   		_renderer = GetComponent<Renderer>();
		_orgMaterial = _renderer.sharedMaterial;
		if (gameObject.tag == "SpawnGhostBrick")
        {
			spawnObject();
			Destroy(gameObject);
		}
	}
	
	
    void FixedUpdate()
    {
        transform.Rotate(rotator * Time.deltaTime);

		// This allows for the Laser Bricks to move back and forth between two positions, and use their laser.
		if (gameObject.tag == "Weird")
        {
			if (useLaser == false)
            {
				useLaser = true;
				StartCoroutine(ToggleLaser());
            }
		}

		if (powerup != null)
        {
			if (blinkOn == false)
            {
				blinkOn = true;
				StartCoroutine(BlinkingBombBrick());
            }
        }

		if (myCollider != null)
        {
			if (ball.EasyOn == true)
			{
				myCollider.enabled = true;
			}
		}

		

		if (gameObject.tag == "LaserSteal")
        {
			if (gameObject == null)
            {
				return;
            }
			else
            {
				myCollider.enabled = true;
				if (useSteal == false)
				{
					useSteal = true;
					StartCoroutine(ToggleLaser());
				}
			}
			
		}
		//Moves brick between two preset positions (targetPos 1 and 2)
		if (moveableBrick == true)
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

		//If laser causes brick hits to go below 0, destroys brick accordingly
		if (hits < 0)
        {
			if (destruction == false)
            {
				destruction = true;
				DestroyBrick();
            }
		}
    }

	//Allows Brick to take damage and eventually break after being hti by a ball
	private void OnCollisionEnter(Collision targetObj)
	{
		if (targetObj.gameObject.tag == "Ball")
		{
			hits--;
			if (hits < -0)
			{
				DestroyBrick();
			}
			else
			{
				_renderer.sharedMaterial = hitMaterial;
				Invoke("RestoreMaterial", 0.1f);
				FindObjectOfType<AudioManager>().Play("BrickHurt");
			}
		}
	}

	//Allows original Material to come back after the hit material
	void RestoreMaterial()
	{
		_renderer.sharedMaterial = _orgMaterial;
	}

	public void DestroyBrick()
    {
		if (destroyEffect != null)
		{
			Instantiate(destroyEffect, transform.position, Quaternion.identity);
		}

		moveableBrick = false;

		if (gameObject.tag == "Brick")
		{
			GameManager.Instance.Score += points;
			other.calculateLoot();
			FindObjectOfType<AudioManager>().Play("BrickBreak");
			Destroy(gameObject);
		}

		if (gameObject.tag == "Ghost")
		{
			GameManager.Instance.Score += points;
			other.calculateLoot();
			FindObjectOfType<AudioManager>().Play("GhostDeath");
			Destroy(gameObject);
		}

		//Initiates exploding effect for explosive bricks.
		if (gameObject.tag == "Explosive")
		{
			Explode();
			GameManager.Instance.Score += points;
			GetComponent<Collider>().enabled = false;
			FindObjectOfType<AudioManager>().Play("BrickBombExplosion");
			Destroy(gameObject, .5f);
		}
		//Does all of the extra things needed done when a laser brick is destroyed, including the option to spawn a different object, like a Ghost Brick.
		if (gameObject.tag == "Weird")
		{
			Invoke("spawnObject", 1f);
			GameManager.Instance.Score += points;
			other.calculateLoot();
			FindObjectOfType<AudioManager>().Play("BrickBombExplosion");
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<Collider>().enabled = false;
			useLaser = false;
			if (_orgMaterialCheck != null)
			{
				if (_orgMaterialCheck == _orgMaterial)
				{
					Explode();
				}
			}
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
			}
			Destroy(gameObject, 2f);
		}

		/*
		if (gameObject.tag == "LaserSteal")
		{
			StartCoroutine(DropStolenCoins());
			GameManager.Instance.Score += points;
			FindObjectOfType<AudioManager>().Play("BrickBreak");
			useSteal = false;
			Destroy(gameObject, .5f);
		}
		*/
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
			if (nearbyObject.gameObject.tag == "Brick")
			{
				Brick oop = nearbyObject.GetComponent<Brick>();
				oop.other.calculateLoot();
				GameManager.Instance.Score += oop.points;
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
					}
					Destroy(nearbyObject.gameObject);
				}
			}
			if (nearbyObject.gameObject.tag == "PowerUp")
			{
				PowerUp refPowerup = nearbyObject.GetComponent<PowerUp>();
				GameManager.Instance.Score += refPowerup.points;
				refPowerup.OnTriggerEnter(GameObject.FindGameObjectWithTag("Ball").GetComponent<Collider>());
			}
			if (nearbyObject.gameObject.tag == "Multiball")
			{
				Multiball refPowerup = nearbyObject.GetComponent<Multiball>();
				GameManager.Instance.Score += refPowerup.points;
				refPowerup.OnTriggerEnter(GameObject.FindGameObjectWithTag("Ball").GetComponent<Collider>());
				Destroy(nearbyObject.gameObject);
			}
			if (nearbyObject.gameObject.tag == "Ghost")
			{
				GameManager.Instance.Score += nearbyObject.GetComponent<GameObject>().GetComponent<Brick>().points;
				nearbyObject.GetComponent<Brick>().other.calculateLoot();
				Destroy(nearbyObject.gameObject);
			}
			if (nearbyObject.gameObject.tag == "Shockwave")
            {
				nearbyObject.GetComponent<Lvl20BF>().hits--;

			}
		}
	}
		IEnumerator BlinkingBombBrick()
		{
			_renderer.sharedMaterial = hitMaterial;
			yield return new WaitForSeconds(1);
			_renderer.sharedMaterial = _orgMaterial;
			yield return new WaitForSeconds(1);
			blinkOn = false;
		}

	IEnumerator DropStolenCoins()
	{
		if (GameObject.Find("StolenCoin(Clone)") is null)
		{
			yield return null;
		}
		else
		{
			currency.ActivateCollider();

			Collider[] colliderstoDestroy = Physics.OverlapSphere(transform.position, radius);
			foreach (Collider nearbyObject in colliderstoDestroy)
			{
				if (nearbyObject.gameObject.tag == "StolenCoin")
				{
					GameManager.Instance.Coins = GameManager.Instance.Coins + 1;
					GameManager.Instance.Score += points;
					Destroy(nearbyObject);
				}
			}
		}


	}

		public void OnTriggerEnter(Collider targetObj)
		{
		if (gameObject.tag == "LaserThrough")
		{
			if (targetObj.gameObject.tag == "Ball")
			{
				hits--;
				if (hits < -0)
				{
					if (destroyEffect != null)
					{
						Instantiate(destroyEffect, transform.position, Quaternion.identity);
					}

					other.calculateLoot();
					GameManager.Instance.Score += points;
					FindObjectOfType<AudioManager>().Play("LaserThroughDeath");
					Destroy(gameObject);
				}
				else
				{
					_renderer.sharedMaterial = hitMaterial;
					Invoke("RestoreMaterial", 0.1f);
					FindObjectOfType<AudioManager>().Play("RoboBrickzHit");
					Instantiate(smokeEffect, transform.position, Quaternion.Euler(0, 0, 0));
				}
			}
		}
		if (targetObj.gameObject.tag == "Shockwave")
        {
			StartCoroutine(MoveBrick());
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

	IEnumerator MoveBrick()
	{
		int oldRandx = 0;
		int oldRandy = 0;
		int randx = 0;
		int randy = 0;

		oldRandx = randx;
		oldRandy = randy;
		randx = Random.Range(-23, 22);
		randy = Random.Range(-5, 15);
		while (randx + 5 >= oldRandx && randx - 5 <= oldRandx)
		{
			oldRandx = randx;
			randx = Random.Range(-23, 22);
		}
		while (randy + 5 >= oldRandy && randy - 5 <= oldRandy)
		{
			oldRandy = randy;
			randy = Random.Range(-5, 15);
		}
		var balls = targetPos1;
		var victor = targetPos2;
		if (GameObject.FindGameObjectWithTag("Ball"))
        {
			balls = GameObject.FindGameObjectWithTag("Ball").transform.position;

		}
		if (GameObject.FindGameObjectWithTag("Shockwave"))
        {
			victor = GameObject.FindGameObjectWithTag("Shockwave").transform.position;
		}
		if (balls != targetPos1)
        {
			while (balls.x <= randx + 1 && balls.x >= randx - 1)
			{
				oldRandx = randx;
				randx = Random.Range(-23, 22);
			}
			while (balls.y <= randy + 1 && balls.y >= randy - 1)
			{
				oldRandy = randy;
				randy = Random.Range(-5, 15);
			}
		}
		if (victor != targetPos2)
        {
			while (victor.x <= randx + 2 && victor.x >= randx - 2)
			{
				oldRandx = randx;
				randx = Random.Range(-23, 22);
			}
			while (victor.y <= randy + 2 && victor.y >= randy - 2)
			{
				oldRandy = randy;
				randy = Random.Range(-5, 15);
			}
		}
		
		transform.position = new Vector3(randx, randy, 0);
		yield return null;
	}
}
