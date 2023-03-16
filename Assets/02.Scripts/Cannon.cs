using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float speed = 2000.0f;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
    }

    void OnCollisionEnter()
    {
        Destroy(this.gameObject);
    }

}
