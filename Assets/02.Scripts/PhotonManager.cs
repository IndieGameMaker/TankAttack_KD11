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

    #region 포톤_콜백_함수
    // 포톤 클라우드에 접속완료했을 때 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        Debug.Log("접속 완료!");

        // 무작위 룸으로 접속 시도
        PhotonNetwork.JoinRandomRoom();
    }

    // 무작위 롬 접속 시도 실패시 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 접속 실패 {returnCode}, msg = {message}");
    }


    #endregion

}
