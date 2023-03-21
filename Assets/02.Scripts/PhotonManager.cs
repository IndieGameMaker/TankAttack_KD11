using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // 게임 버전
    [SerializeField] private const string version = "1.0";
    // 유저명
    [SerializeField] private string nickName = "Zackiller";

    private string userId;

    [Header("UI")]
    [SerializeField] private TMP_InputField nickNameIF;
    [SerializeField] private TMP_InputField roomNameIF;

    [Header("Button")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button makeRoomButton;

    void Awake()
    {
        // 방장이 로딩한 씬을 자동으로 로딩하는 기능(속성)
        PhotonNetwork.AutomaticallySyncScene = true;
        // 게임 버전 설정
        PhotonNetwork.GameVersion = version;
        // 닉 네임 설정
        PhotonNetwork.NickName = nickName;

        if (PhotonNetwork.IsConnected == false)
        {
            // Photon Cloud 접속
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 닉네임을 로드
        string defaultNickName = $"USER_{UnityEngine.Random.Range(0, 1000):0000}";
        userId = PlayerPrefs.GetString("USER_ID", defaultNickName);

        // Input Field에 닉네임 설정
        nickNameIF.text = userId;

        // 버튼 이벤트 연결
        loginButton.onClick.AddListener(() => OnLoginButtonClick());
        makeRoomButton.onClick.AddListener(() => OnMakeRoomButtonClick());
    }

    private void OnMakeRoomButtonClick()
    {
        // 닉네임 입력여부를 확인
        SetNickName();

        // 룸 명 입력여부 확인
        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            roomNameIF.text = $"ROOM_{UnityEngine.Random.Range(0, 10000):00000}";
        }

        // 룸 속성 정의
        var ro = new RoomOptions
        {
            MaxPlayers = 20,
            IsOpen = true,
            IsVisible = true
        };

        // 룸 생성
        PhotonNetwork.CreateRoom(roomNameIF.text, ro);

        // var ro1 = new RoomOptions();
        // ro1.MaxPlayers = 20;
        // ro1.IsOpen = true;
        // ro1.IsVisible = true;
    }

    private void OnLoginButtonClick()
    {
        // 닉네임 입력여부를 확인
        SetNickName();

        PhotonNetwork.JoinRandomRoom();
    }

    void SetNickName()
    {
        if (string.IsNullOrEmpty(nickNameIF.text))
        {
            userId = $"USER_{UnityEngine.Random.Range(0, 1000):0000}";
            nickNameIF.text = userId;
        }

        userId = nickNameIF.text;

        // PlayerPrefs
        PlayerPrefs.SetString("USER_ID", userId);

        PhotonNetwork.NickName = userId;
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
        // PhotonNetwork.JoinRandomRoom();
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

    public override void OnCreatedRoom()
    {

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

            // PhotonNetwork.IsMessageQueueRunning = false;
            // UnityEngine.SceneManagement.SceneManager.LoadScene("BattleField");
            // PhotonNetwork.IsMessageQueueRunning = true;
        }
    }

    public GameObject roomPrefab;
    public Transform contentTr; // 차일화 시킬 부모의 Transform
    // 룸 목록을 저장할 Dictionary 선언
    public Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();

    // 룸 목록이 변경될때마다 호출해주는 콜백
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var room in roomList)
        {
            string roomInfo = $"{room.Name} ({room.PlayerCount}/{room.MaxPlayers})";

            if (room.RemovedFromList == true) // 삭제된 룸
            {
                // 딕셔너리에서 룸 삭제
            }
            else // 새로 생성된 룸 , 변경된 룸
            {
                // 처음 생성된 룸 , 딕셔너리에서 검색해서 없는 경우
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    // 룸 프리팹 생성
                    GameObject _room = Instantiate(roomPrefab, contentTr);
                    // 룸 정보 생성

                    // 딕셔너리에 추가
                }
                else // 룸 정보가 변경된 경우
                {
                    // 딕셔너리에서 검색 후 정보를 변경
                }
            }

        }
    }

    #endregion
}
