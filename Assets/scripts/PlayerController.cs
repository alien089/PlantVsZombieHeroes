using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public StageManager StageManager;
    public GameObject HandPosList;
    public GameObject HandCardBtnList;
    public GameObject PlayerHand;
    public GameObject MyBoard;
    public GameObject EnemyBoard;
    public GameObject Health;
    public GameObject Mana;
    public bool Player = false;
    public string test = "";

    public int HealthPoints = 20;
    public int ActualManaPoints = 0;
    public int TotalManaPoints = 1;

    private Card[] m_Hand = new Card[6];
    private GameObject[] PhysicHand = new GameObject[6];
    private Card m_ChoosedCard = null;
    private int m_ChoosedInt;
    private bool Accepted = false;

    private System.Random m_Rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        Mana.GetComponent<Text>().text = ActualManaPoints.ToString();
        if (Player)
        {
            for (int i = 0; i < 5; i++)
            {
                int ChoosedCard = m_Rnd.Next(0, StageManager.CardPlayerList.Count);
                GameObject tmp;
                tmp = Instantiate(StageManager.CardPlayerList[ChoosedCard].UIBody, HandPosList.transform.GetChild(i).transform.position, Quaternion.identity, PlayerHand.transform);
                PhysicHand[i] = tmp;
                m_Hand[i] = StageManager.CardPlayerList[ChoosedCard];
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                int ChoosedCard = m_Rnd.Next(0, StageManager.CardEnemyList.Count);
                GameObject tmp;
                tmp = Instantiate(StageManager.CardEnemyList[ChoosedCard].UIBody, HandPosList.transform.GetChild(i).transform.position, Quaternion.identity, PlayerHand.transform);
                PhysicHand[i] = tmp;
                m_Hand[i] = StageManager.CardEnemyList[ChoosedCard];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {       
        switch (StageManager.m_CurrentPhase)
        {
            case TurnPhases.DRAW:
                
                if (Player)
                {
                    if(FIndFirstEmpty() != -1)
                    {
                        int ChoosedCard = m_Rnd.Next(0, StageManager.CardPlayerList.Count);
                        GameObject tmp;
                        tmp = Instantiate(StageManager.CardPlayerList[ChoosedCard].UIBody, HandPosList.transform.GetChild(FIndFirstEmpty()).transform.position, Quaternion.identity, PlayerHand.transform);
                        PhysicHand[FIndFirstEmpty()] = tmp;
                        m_Hand[FIndFirstEmpty()] = StageManager.CardPlayerList[ChoosedCard];
                    }
                }
                else
                {
                    if (FIndFirstEmpty() != -1)
                    {
                        int ChoosedCard = m_Rnd.Next(0, StageManager.CardEnemyList.Count);
                        GameObject tmp;
                        tmp = Instantiate(StageManager.CardEnemyList[ChoosedCard].UIBody, HandPosList.transform.GetChild(FIndFirstEmpty()).transform.position, Quaternion.identity, PlayerHand.transform);
                        PhysicHand[FIndFirstEmpty()] = tmp;
                        m_Hand[FIndFirstEmpty()] = StageManager.CardEnemyList[ChoosedCard];
                    }
                    StageManager.EndPhase();
                }
                break;
            case TurnPhases.PLAYER:
                if (Player)
                    for (int j = 0; j < transform.GetChild(7).childCount; j++)
                        transform.GetChild(7).GetChild(j).gameObject.SetActive(true);

                if (!Player)
                    for (int j = 0; j < transform.GetChild(7).childCount; j++)
                        transform.GetChild(7).GetChild(j).gameObject.SetActive(false);
                break;
            case TurnPhases.ENEMY:
                if (Player)
                    for (int j = 0; j < transform.GetChild(7).childCount; j++)
                        transform.GetChild(7).GetChild(j).gameObject.SetActive(false);

                if (!Player)
                    for (int j = 0; j < transform.GetChild(7).childCount; j++)
                        transform.GetChild(7).GetChild(j).gameObject.SetActive(true);
                break;
            case TurnPhases.PLAYERABILITY:
                for (int j = 0; j < transform.GetChild(7).childCount; j++)
                    transform.GetChild(7).GetChild(j).gameObject.SetActive(false);

                if (Player)
                {

                }
                else
                {

                }
                break;
            case TurnPhases.ENEMYABILITY:

                break;
            case TurnPhases.FIGHT:
                if (CheckFightPhase() == true)
                {
                    for (int i = 1; i < MyBoard.transform.childCount; i++)
                    {
                        MyBoard.transform.GetChild(i).GetComponent<CardController>().FightFinished = false;
                        EnemyBoard.transform.GetChild(i).GetComponent<CardController>().FightFinished = false;
                    }
                    StageManager.EndPhase();
                }
                break;
            case TurnPhases.END:
                if (CheckDeathPhase() == 0)
                {
                    for (int i = 1; i < MyBoard.transform.childCount; i++)
                    {
                        MyBoard.transform.GetChild(i).GetComponent<CardController>().DeathFinished = false;
                        EnemyBoard.transform.GetChild(i).GetComponent<CardController>().DeathFinished = false;
                    }
                    StageManager.EndPhase();
                }
                else if(CheckDeathPhase() == -1)
                    StageManager.EndPhase();
                break;
        }
    }

    private int FIndFirstEmpty()
    {
        bool finded = false;
        int firstPos = -1;

        for(int i = 0; i < m_Hand.Length; i++)
        {
            if(finded == false)
            {
                if (m_Hand[i] == null)
                {
                    firstPos = i;
                    finded = true;
                }
            }
        }

        return firstPos;
    }

    public void ChooseCard(int i)
    {
        m_ChoosedCard = m_Hand[i];
        if(m_ChoosedCard.ManaPoints <= ActualManaPoints)
        {
            m_ChoosedInt = i;
            Accepted = true;

            for (int j = 0; j < transform.GetChild(6).childCount; j++)
                transform.GetChild(6).GetChild(j).gameObject.SetActive(true);
        }
    }

    public void PlaceCard(Transform linkedPosition)
    {
        if (Accepted)
        {
            for (int j = 0; j < transform.GetChild(6).childCount; j++)
                transform.GetChild(6).GetChild(j).gameObject.SetActive(false);

            GameObject tmp;
            tmp = Instantiate(m_ChoosedCard.PhysicBody, linkedPosition.position, Quaternion.identity, MyBoard.transform);
            tmp.GetComponent<CardController>().StageManager = StageManager;
            m_Hand[m_ChoosedInt] = null;
            ActualManaPoints -= m_ChoosedCard.ManaPoints;
            Mana.GetComponent<Text>().text = ActualManaPoints.ToString();
            Destroy(PhysicHand[m_ChoosedInt]);
            Accepted = false;
        }
    }

    private bool CheckFightPhase()
    {
        for (int i = 1; i < MyBoard.transform.childCount; i++)
        {
            int both = 0;
            if (MyBoard.transform.GetChild(i).GetComponent<CardController>().FightFinished == false)
            {
                both++;
            }
            if (EnemyBoard.transform.GetChild(i).GetComponent<CardController>().FightFinished == false)
            {
                both++;
            }
            if (both > 0)
                return false;
        }
        return true;
    }
    
    private int CheckDeathPhase()
    {
        if(MyBoard.transform.childCount > 1)
        {
            for (int i = 1; i < MyBoard.transform.childCount; i++)
            {
                int both = 0;
                if (MyBoard.transform.GetChild(i).GetComponent<CardController>().DeathFinished == false)
                {
                    both++; 
                }
                if (EnemyBoard.transform.GetChild(i).GetComponent<CardController>().DeathFinished == false)
                {
                    both++;
                }
                if (both == 2)
                    return 1;
            }
            return 0;
        }
        return -1;
    }
}
