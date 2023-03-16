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

        // 로비에 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비에 입장했을 때 호출되는 콜백
    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장 완료!"); // 룸 목록을 전달받을 수 있는 로비에 입장완료

        // 무작위 룸으로 접속 시도
        PhotonNetwork.JoinRandomRoom();
    }

    // 무작위 롬 접속 시도 실패시 호출되는 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"룸 접속 실패 {returnCode}, msg = {message}");

        // 룸 속성 
        RoomOptions ro = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };

        // 룸 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    // 룸에 입장했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("방 생성 완료 및 방 입장");

        //PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0), Quaternion.identity, 0);
        // 방장만이 씬을 로드
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }
    }


    #endregion
}
