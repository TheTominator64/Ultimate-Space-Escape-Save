using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandalTheThird : MonoBehaviour
{
	public GameObject[] children;
    public int hits;

	private void OnCollisionEnter(Collision targetObj)
	{
		if (targetObj.gameObject.tag == "Ball")
		{
			foreach (GameObject child in children)
            {
				child.GetComponent<BrickBoss>().hits--;
            }
            hits--;
            if (hits < -0)
            {
                Destroy(gameObject, 2f);
            }
		}

	}
}
