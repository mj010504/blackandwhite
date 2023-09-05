using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class TileController : MonoBehaviourPunCallbacks
{

    public Vector3 originalScale;
    NetworkManager NM;
    RectTransform CheckBtnPos;
    RectTransform CancleBtnPos;

   

    public int value;
   
    // Start is called before the first frame update
    void Start()
    {
       
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
        originalScale = transform.localScale;
          CheckBtnPos = NM.CheckBtn.GetComponent<RectTransform>();
         CancleBtnPos = NM.CancleBtn.GetComponent<RectTransform>();

    }
 
 private void OnMouseDown()
    {
        Vector3 tilePos = transform.position;

        if (NM.isChoice) {

             Vector3 newPosition = new Vector3(tilePos.x-18.54f,tilePos.y -103.76f,0);
            CheckBtnPos.anchoredPosition = newPosition;
             newPosition = new Vector3(tilePos.x + 22f, tilePos.y-103.76f,0);
             CancleBtnPos.anchoredPosition = newPosition;

             // 클릭된 오브젝트와 원래 크기를 NetworkManager에 할당
         NM.clickedObject = gameObject;
         NM.originalScale = originalScale;

        // 클릭된 오브젝트의 크기를 크게 변경
        transform.localScale *= 1.5f;

        NM.CheckBtn.gameObject.SetActive(true);
        NM.CancleBtn.gameObject.SetActive(true);

        NM.isChoice = false;

        }
    }

   
   
}
 