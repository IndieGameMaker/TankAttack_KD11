using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float speed = 2000.0f;
    [SerializeField] private GameObject expEffect;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * speed);
        Destroy(this.gameObject, 5.0f);
    }

    void OnCollisionEnter()
    {
        // 무작위 각도 생성
        Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
        var obj = Instantiate(expEffect, transform.position, rot);

        Destroy(this.gameObject);

        Destroy(obj, 6.0f);
    }

}
