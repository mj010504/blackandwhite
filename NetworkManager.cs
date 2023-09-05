using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    float currentTurnTime; // 현재 턴 남은 시간
    bool isTurnTimeRunning; // 턴 제한 시간이 실행 중인지 여부
    public bool isChoice; // 숫자 타일을 선택할 차례인지 여부
    public bool isMyTurn = true; // 현재 플레이어의 턴 여부

    int blackindex;
    int whiteindex;

    public int value;


   
    public GameObject clickedObject;
    public Vector3 originalScale;

    [Header("DisconnectPanel")]
    public GameObject DisconnectPanel;
    public InputField NicknameInput;
    

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public GameObject TimeLimit;
    public Text CurrentTime;
    public Text GameText;
    public Text MyNickname;
    public Text OpponentNickname;
    public Button CheckBtn;
    public Button CancleBtn;
    public GameObject pan1;
    public GameObject pan2;

    //숫자타일
    public GameObject zero;
    public GameObject one;
    public GameObject two;
     public GameObject three;
      public GameObject four;
    public GameObject five;
     public GameObject six;
    public GameObject seven;
    public GameObject eight;
    public GameObject nine;

    //흑백 숫자타일
    public GameObject zerobox;
    public GameObject onebox;
    public GameObject twobox;
    public GameObject threebox;
    public GameObject fourbox;
    public GameObject fivebox;
    public GameObject sixbox;
    public GameObject sevenbox;
    public GameObject eightbox;
    public GameObject ninebox;





    //숫자타일
    public GameObject [] numbers = new GameObject[10];

    //흑백 숫자 타일
    public GameObject[] boxes = new GameObject[10];
    public GameObject [] blackboxes = new GameObject[5];
     public GameObject [] whiteboxes = new GameObject[5];


    // Start is called before the first frame update
    void Start()
    {
        
        Screen.SetResolution(540, 960, false);
        DisconnectPanel.SetActive(true);
        RoomPanel.SetActive(false);

        CurrentTime.text = currentTurnTime.ToString("F0");

       

        numbers[0] = zero;
        numbers[1] = one;
        numbers[2] = two;
        numbers[3] = three;
        numbers[4] = four;
        numbers[5] = five;
        numbers[6] = six;
        numbers[7] = seven;
        numbers[8] = eight;
        numbers[9] = nine;
        
        blackboxes[0] = zerobox;
        blackboxes[1] = twobox;
        blackboxes[2] = fourbox;
        blackboxes[3] = sixbox;
        blackboxes[4] = eightbox;

        whiteboxes[0] = onebox;
        whiteboxes[1] = threebox;
        whiteboxes[2] = fivebox;
        whiteboxes[3] = sevenbox;
        whiteboxes[4] = ninebox;  

        boxes[0] = zerobox;
        boxes[1] = onebox;
        boxes[2] = twobox;
        boxes[3] = threebox;
       boxes[4] = fourbox;
       boxes[5] = fivebox;
       boxes[6] = sixbox;
        boxes[7] = sevenbox;
        boxes[8] = eightbox;
       boxes[9] = ninebox;  
        
        blackindex = 0;
        whiteindex = 0;
        

    }

    void Update() {

    if (isTurnTimeRunning)
    {
        currentTurnTime -= Time.deltaTime;
        CurrentTime.text = currentTurnTime.ToString("F0");
        if (currentTurnTime <= 0)
        {
           EndTurn(); // 시간 초과 시 턴 종료
        }
    }

  

    }

    

    public void Connect()
    {   
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

//방에 들어왔을때 게임
    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {

            string myNickname = PhotonNetwork.LocalPlayer.NickName;
            string opponentNickname = PhotonNetwork.PlayerListOthers[0].NickName;
            StartGame(myNickname, opponentNickname);
           
            isChoice = true;
             
        }

         
    }


    void ShowPanel(GameObject CurPanel) {
        DisconnectPanel.SetActive(false);
        RoomPanel.SetActive(false);
        CurPanel.SetActive(true);
        CheckBtn.gameObject.SetActive(false);
        CheckBtn.gameObject.SetActive(false);
    }

    
    void StartGame(string myNickname, string opponentNickname) {
         photonView.RPC("StartGameRPC", RpcTarget.All, myNickname, opponentNickname);
    }

    [PunRPC]
    void StartGameRPC(string myNickname, string opponentNickname) {
       photonView.RPC("ShowTextRPC", RpcTarget.All, "Game Start", 3.0f);  
       MyNickname.text = myNickname;
       OpponentNickname.text = opponentNickname; 
    }

    [PunRPC]
    void ShowTextRPC(string str, float timeLimit) {
        GameText.text = str;
        StartCoroutine(ClearTextAfterDelay(timeLimit));
    }

     private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 일정 시간만큼 기다림
        GameText.text = ""; // 텍스트 비우기
    }

    void StartTurn() {
          photonView.RPC("StartTurnRPC", RpcTarget.All, 15.0f);   
    }

    [PunRPC]
    void StartTurnRPC(float timeLimit) 
{ 
    currentTurnTime = timeLimit;
    isTurnTimeRunning = true;
    
}
    [PunRPC]
void EndTurnRPC() {
    isTurnTimeRunning = false;
}

void EndTurn() {
    photonView.RPC("EndTurnRPC", RpcTarget.All);
}



//숫자타일과 숫자흑백타일 위치 변경 함수

    public void SwapRandomNumbers() {
    int indexA = Random.Range(0, 10);
    int indexB = Random.Range(0, 10);

    
        Vector3 tempPosition = numbers[indexA].transform.position;
        numbers[indexA].transform.position = numbers[indexB].transform.position;
        numbers[indexB].transform.position = tempPosition;
    
}


public void SwapRandomBoxes() {
    int indexA = Random.Range(0, 10);
    int indexB = Random.Range(0, 10);

    
        Vector3 tempPosition = boxes[indexA].transform.position;
        boxes[indexA].transform.position = boxes[indexB].transform.position;
        boxes[indexB].transform.position = tempPosition;

}

public void MainGame()
{
        StartTurn();
        photonView.RPC("ShowTextRPC", RpcTarget.All, "Choice the Tile", 15.0f);
       
 }

public IEnumerator StartMainGameAfterDelay(float delay) {
    yield return new WaitForSeconds(delay);
    MainGame();
}

    public void Check() {
        clickedObject.transform.localScale = originalScale;
        clickedObject.transform.position = pan2.transform.position;

          for(int i = 0; i <= 9; i++ ) {
            
            if(boxes[i] == clickedObject) {
                value = i;
                return;
            }
        }

    
        photonView.RPC("ChoicedTileRPC", RpcTarget.Others, value);

        CheckBtn.gameObject.SetActive(false);
        CancleBtn.gameObject.SetActive(false);

        EndTurn();

        
    }

    [PunRPC]
    public void ChoicedTileRPC(int value) {
        if(value % 2 == 0) {
             blackboxes[blackindex].transform.position = pan1.transform.position;
             blackindex ++;
        }
        else {
            whiteboxes[whiteindex].transform.position = pan1.transform.position;
            whiteindex++;
        }

    }


    public void Cancel() {
        clickedObject.transform.localScale = originalScale;
        CheckBtn.gameObject.SetActive(false);
        CancleBtn.gameObject.SetActive(false);
        
    }


}