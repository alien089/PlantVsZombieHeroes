using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StageManager StageManager;
    public GameObject HandPosList;
    public GameObject PlayerHand;

    private System.Random m_Rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            int CHoosedCard = m_Rnd.Next(0, StageManager.CardList.Count);
            GameObject tmp;
            tmp = Instantiate(StageManager.CardList[CHoosedCard].UIBody, HandPosList.transform.GetChild(i).transform.position, Quaternion.identity, PlayerHand.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
