using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    public TMP_Text roomText;
    private RoomInfo roomInfo;

    void Awake()
    {
        // 하위에 있는 Button 캡션을 저장
        roomText = GetComponentInChildren<TMP_Text>();
    }

    // Getter, Setter
    // 프로퍼티
    public RoomInfo RoomInfo
    {
        // Getter 
        get
        {
            return roomInfo;
        }
        set
        {
            roomInfo = value;
            // 룸 정보를 roomInfo를 통해서 하위에 있는 버튼 캡션에 적용
            roomText.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
        }
    }
}
