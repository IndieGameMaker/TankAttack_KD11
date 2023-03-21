using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    [SerializeField] private Button exitButton;
    [SerializeField] private TMP_Text chatListText;
    [SerializeField] private TMP_InputField chatMsgIF;
    [SerializeField] private TMP_Text playerCountText;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private TMP_Text playerListText;

    private PhotonView pv;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        exitButton.onClick.AddListener(() => OnExitButtonClick());

        CreateTank();
        DisplayRoomInfo();
    }

    private void OnExitButtonClick()
    {
        // 방을 나가겠다는 요청
        PhotonNetwork.LeaveRoom();
    }

    // 클라이언트의 Network 데이터를 모드 Cleanup 한 후에 호출된 콜백
    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }


    void CreateTank()
    {
        Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f),
                                  5.0f,
                                  Random.Range(-10.0f, 10.0f));
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }

    public void SendMessage()
    {
        string msg = $"<color=#00ff00>[{PhotonNetwork.NickName}]</color> {chatMsgIF.text}";
        pv.RPC("ChatMessage", RpcTarget.AllBufferedViaServer, msg);
    }

    public void SendMessage2(string msg)
    {
        pv.RPC("ChatMessage", RpcTarget.AllBufferedViaServer, msg);
    }

    [PunRPC]
    public void ChatMessage(string msg)
    {
        chatListText.text += msg + "\n";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DisplayRoomInfo();
        string msg = $"<color=#00ff00>[{newPlayer.NickName}]</color> 입장 했습니다.";
        ChatMessage(msg);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DisplayRoomInfo();
        string msg = $"<color=#00ff00>[{otherPlayer.NickName}]</color> 퇴장 했습니다.";
        ChatMessage(msg);
    }

    private void DisplayRoomInfo()
    {
        // 현재 입한 룸에 대한 정보
        Room currentRoom = PhotonNetwork.CurrentRoom;

        string msg = $"{currentRoom.Name} : {currentRoom.PlayerCount}/{currentRoom.MaxPlayers}";
        playerCountText.text = msg;

        roomNameText.text = currentRoom.Name;
    }
}
