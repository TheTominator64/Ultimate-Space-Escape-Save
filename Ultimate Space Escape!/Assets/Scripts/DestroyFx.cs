using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFx : MonoBehaviour
{
    public int seconds = 0; 
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, seconds);
    }
}
