using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 게임 버전
    [SerializeField] private const string version = "1.0";

    // 유저명
    [SerializeField] private string nickName = "Zackiller";

    void Awake()
    {
        // 방장이 로딩한 씬을 자동으로 로딩하는 기능(속성)
        PhotonNetwork.AutomaticallySyncScene = true;
        // 게임 버전 설정
        PhotonNetwork.GameVersion = version;
        // 닉 네임 설정
        PhotonNetwork.NickName = nickName;
        // Photon Cloud 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

}
