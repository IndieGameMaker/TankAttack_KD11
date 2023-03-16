using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateTank();
    }

    void CreateTank()
    {
        Vector3 pos = new Vector3(Random.Range(-10.0f, 10.0f),
                                  5.0f,
                                  Random.Range(-10.0f, 10.0f));
        PhotonNetwork.Instantiate("Tank", pos, Quaternion.identity, 0);
    }
}
