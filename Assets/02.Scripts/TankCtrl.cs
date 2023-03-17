using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    private Rigidbody rb;
    private PhotonView pv;
    private CinemachineVirtualCamera cvc;
    private AudioSource audio;

    private float v => Input.GetAxis("Vertical");
    private float h => Input.GetAxis("Horizontal");

    [SerializeField] private float moveSpeed = 50.0f;
    [SerializeField] private float turnSpeed = 100.0f;
    [SerializeField] private TMP_Text nickNameText;
    [SerializeField] private Image hpBar;

    private float initHp = 100.0f; // 초기 생명치
    private float currHp = 100.0f; // 현재 생명치

    public GameObject cannonPrefab;
    public Transform firePos;
    public AudioClip fireSfx;

    private Renderer[] renderers;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();
        cvc = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();

        rb.centerOfMass = new Vector3(0, -5.0f, 0);

        // 탱크의 모든 랜더러 컴포넌트 추출
        renderers = GetComponentsInChildren<Renderer>();

        nickNameText.text = pv.Owner.NickName;


        if (pv.IsMine == true)
        {
            cvc.Follow = tr;
            cvc.LookAt = tr;
        }
        else
        {
            rb.isKinematic = true;
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
                //Fire();
                // RPC (Remote Procedure Call)
                pv.RPC("Fire", RpcTarget.AllViaServer, pv.Owner.ActorNumber);
            }
        }
    }

    [PunRPC]
    void Fire(int shooterId)
    {
        audio.PlayOneShot(fireSfx, 0.8f);
        var obj = Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        obj.GetComponent<Cannon>().shooterId = shooterId;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("CANNON"))
        {
            // ActionNumber -> NickName
            int shooterId = coll.gameObject.GetComponent<Cannon>().shooterId;
            Player shooter = PhotonNetwork.CurrentRoom.GetPlayer(shooterId);

            currHp -= 20.0f;
            hpBar.fillAmount = currHp / initHp;

            if (currHp <= 0.0f)
            {
                if (pv.IsMine)
                {
                    // [Jason Lee]님은 [Killer]에게 살해당했습니다.
                    string msg = $"<color=#00ff00>[{pv.Owner.NickName}]</color> 님은 <color=#ff0000>[{shooter.NickName}]</color>에게 폭파됐습니다.";
                    GameManager.instance.SendMessage2(msg);
                }

                TankDestroy();
            }
        }
    }

    private void TankDestroy()
    {
        SetVisible(false);
        Invoke("RespawnTank", 3.0f);
    }

    private void RespawnTank()
    {
        // HP 초기화
        currHp = initHp;
        hpBar.fillAmount = 1.0f;

        // 랜덤한 위치로 이동처리 

        // 탱크를 활성화
        SetVisible(true);
    }

    private void SetVisible(bool isVisible)
    {

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }

        tr.Find("Canvas").gameObject.SetActive(isVisible);
        //tr.Find("Canvas").GetComponent<Canvas>().enabled = isVisible;
    }



}
