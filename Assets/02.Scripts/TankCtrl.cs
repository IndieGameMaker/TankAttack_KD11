using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;

    private float v => Input.GetAxis("Vertical");
    private float h => Input.GetAxis("Horizontal");

    [SerializeField] private float moveSpeed = 50.0f;
    [SerializeField] private float turnSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        tr.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);
    }
}
