using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform tr;
    private RaycastHit hit;

    [SerializeField] private float turnSpeed = 10.0f;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        // 메인 카메라에서 레이케스트를 발사
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
    }
}
