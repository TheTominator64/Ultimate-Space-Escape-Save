using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BrickBoss : MonoBehaviour
{
	[Header("General Brick Things")]
	public int hits = 0;
	public int points = 100;
	public Vector3 rotator;
	public Material hitMaterial;
	public LootDrops other;

	[Header("Explosion Brick")]
	public GameObject explosionEffect;
	public float radius = 50f;

	[Header("Movement and Ghost Brick")]
	public GameObject nextBossPhase;
	[SerializeField]
	public Vector3 targetPos1;
	[SerializeField]
	public Vector3 targetPos2;
	[SerializeField]
	public float speed = 5f;
	public bool canMove;
	public bool firstMove;

	[Header("LaserThings")]
	public Laser[] lasers; //Array is used so that for 2nd and 3rd phase, multiple lasers can be used and contained within said array.
	public GameObject playerPrefab;
	bool useLaser = false;

	Material _orgMaterial;
	Renderer _renderer;
	

    void Start()
    {
		firstMove = true;
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
   		_renderer = GetComponent<Renderer>();
		_orgMaterial = _renderer.sharedMaterial;
    }
	
	
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);

		if (gameObject.tag == "Weird")
        {
			if (useLaser == false)
            {
				useLaser = true;
				StartCoroutine(ToggleLaser());

			}

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
	
	//Allows Brick to take damage and eventually break after being hti by a ball
	private void OnCollisionEnter(Collision targetObj)
	{
		if(gameObject.name == "Randal The Third (Clone)")
        {
			return;
        }
		if (targetObj.gameObject.tag == "Ball")
		{
			hits--;
			if (hits < -0)
			{
				if (gameObject.tag == "Explosive")
				{
					Explode();
				}
				GetComponent<MeshRenderer>().enabled = false;
				GetComponent<BoxCollider>().enabled = false;
				StartCoroutine(SpawnNextBoss());
				GameManager.Instance.Score += points;
				other.calculateLoot();
				other.calculateLoot();
				FindObjectOfType<AudioManager>().Play("BrickBombExplosion");
				GameObject.Find("playerPrefab(Clone)").transform.localScale *= 5f;
				useLaser = false;
				Destroy(GameObject.Find("Ball(Clone)"));
				Destroy(gameObject);
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

	void Explode ()
    {
		//Shows Explosive Effect
		Instantiate(explosionEffect, transform.position, transform.rotation);

		Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);

		foreach (Collider nearbyObject in collidersToDestroy)
        {
			if (nearbyObject.gameObject.tag == "Brick")
            {
				other.calculateLoot();
				GameManager.Instance.Score += points;
				Destroy(nearbyObject.gameObject);
			}

			if (nearbyObject.gameObject.tag == "PowerUp")
			{
				Destroy(nearbyObject.gameObject);
			}

			if (nearbyObject.gameObject.tag == "Multiball")
			{
				Destroy(nearbyObject.gameObject);
			}
		}

	}

	//This allows for a boss phase to spawn the next boss phase.
	IEnumerator SpawnNextBoss()
    {
		Instantiate(nextBossPhase, transform.position, Quaternion.identity);
		yield return null;
	}

	IEnumerator ToggleLaser()
    {
		if (useLaser == true)
        {
			foreach (Laser laser in lasers)
            {
				laser.lr.enabled = true;
				laser.Laseroff = false;
				yield return new WaitForSeconds(1);
				laser.lr.enabled = false;
				laser.Laseroff = true;
			}
			yield return new WaitForSeconds(3);
			useLaser = false;
			yield return null;
		}
	}
}
