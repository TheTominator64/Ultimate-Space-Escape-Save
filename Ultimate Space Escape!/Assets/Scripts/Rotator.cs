using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector3 rotator;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }
}
