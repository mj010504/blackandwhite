using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun
{
    NetworkManager NM;
    PhotonView PV;
    public int score;
    public bool isMyTurn;

    void Start()
    {
        score = 0;
       PV = photonView;
       NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
       
        arrangement();

          // 게임 시작 시, 두 번째 플레이어의 턴으로 시작
        if (photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            isMyTurn = true;
        }
        else 
            isMyTurn = false;
    }

    void Update() {
        
          if (photonView.IsMine)
        {
            // 내 턴인 경우 입력을 받아 처리
            if (isMyTurn && NM.isChoice) 
            {
               NM.StartMainGameAfterDelay(3.0f);
            }
        }
    }

    public void arrangement() 
    {
        if (photonView.IsMine) {
            NM.SwapRandomNumbers();
            NM.SwapRandomBoxes();
        }
    }

        void EndTurn()
    {
        if (photonView.IsMine)
        {
            // 턴을 상대 플레이어에게 전환
            photonView.RPC("SwitchTurn", RpcTarget.All);
         }

    }

    [PunRPC]
    void SwitchTurn() {
        isMyTurn = !isMyTurn;
    }


    
}
