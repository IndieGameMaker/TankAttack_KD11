using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private PhotonView pv;
    private CinemachineVirtualCamera cvc;

    private float v => Input.GetAxis("Vertical");
    private float h => Input.GetAxis("Horizontal");

    [SerializeField] private float moveSpeed = 50.0f;
    [SerializeField] private float turnSpeed = 100.0f;

    public GameObject cannonPrefab;
    public Transform firePos;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        cvc = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        if (pv.IsMine == true)
        {
            cvc.Follow = tr;
            cvc.LookAt = tr;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
            tr.Rotate(Vector3.up * Time.deltaTime * h * turnSpeed);

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(cannonPrefab, firePos.position, firePos.rotation);
            }
        }
    }
}
