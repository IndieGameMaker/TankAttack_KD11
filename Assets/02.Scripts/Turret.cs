using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret : MonoBehaviour
{
    private Transform tr;
    private RaycastHit hit;
    private PhotonView pv;

    [SerializeField] private float turnSpeed = 10.0f;

    void Start()
    {
        tr = GetComponent<Transform>();
        pv = tr.root.GetComponent<PhotonView>();

        this.enabled = pv.IsMine;
    }

    void Update()
    {
        // 메인 카메라에서 레이 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

        // 레이케스트 투사
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
        {
            // 터렛기준의 월드좌표를 로컬좌표 계산
            Vector3 pos = tr.InverseTransformPoint(hit.point);

            // Atan2를 이용해 두 좌표의 각도를 계산
            float angle = Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg;

            // 터렛을 회전
            tr.Rotate(Vector3.up * Time.deltaTime * angle * turnSpeed);
        }
    }
}
