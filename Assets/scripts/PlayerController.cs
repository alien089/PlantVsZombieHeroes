using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public StageManager StageManager;
    public GameObject HandPosList;
    public GameObject HandCardBtnList;
    public GameObject PlayerHand;
    public GameObject MyBoard;
    public GameObject EnemyBoard;
    public Text Health;
    public Text Mana;
    public GameObject PlayerBody;
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


    /// <summary>
    /// Function for the initial draw and setting up mana points in UI, it needs the correct card list in input
    /// </summary>
    /// <param name="CardList"></param>
    public void StartPhase(List<Card> CardList)
    {
        Mana.text = ActualManaPoints.ToString();
        Health.text = HealthPoints.ToString();

        for (int i = 0; i < 5; i++)
        {
            int ChoosedCard = m_Rnd.Next(0, CardList.Count);
            GameObject tmp;
            tmp = Instantiate(CardList[ChoosedCard].UIBody, HandPosList.transform.GetChild(i).transform.position, Quaternion.identity, PlayerHand.transform);
            PhysicHand[i] = tmp;
            m_Hand[i] = CardList[ChoosedCard];
        }
    }

    /// <summary>
    /// standard drawing phase, intantiates a random card in the player's hand, it needs the correct card list in input
    /// </summary>
    /// <param name="CardList"></param>
    public void DrawPhase(List<Card> CardList)
    {
        if (FIndFirstEmpty() != -1)
        {
            int ChoosedCard = m_Rnd.Next(0, CardList.Count);
            GameObject tmp;
            tmp = Instantiate(CardList[ChoosedCard].UIBody, HandPosList.transform.GetChild(FIndFirstEmpty()).transform.position, Quaternion.identity, PlayerHand.transform);
            PhysicHand[FIndFirstEmpty()] = tmp;
            m_Hand[FIndFirstEmpty()] = CardList[ChoosedCard];
        }
    }

    /// <summary>
    /// deactivates player's buttons and activates enemy's buttons
    /// </summary>
    public void PlayerPhase()
    {
        if (Player)
            for (int j = 0; j < transform.GetChild(8).childCount; j++)
                transform.GetChild(8).GetChild(j).gameObject.SetActive(true);

        if (!Player)
            for (int j = 0; j < transform.GetChild(8).childCount; j++)
                transform.GetChild(8).GetChild(j).gameObject.SetActive(false);
    }

    /// <summary>
    /// deactivates enemy's buttons and activates player's buttons
    /// </summary>
    public void EnemyPhase()
    {
        if (Player)
            for (int j = 0; j < transform.GetChild(8).childCount; j++)
                transform.GetChild(8).GetChild(j).gameObject.SetActive(false);

        if (!Player)
            for (int j = 0; j < transform.GetChild(8).childCount; j++)
                transform.GetChild(8).GetChild(j).gameObject.SetActive(true);
    }

    /// <summary>
    /// deactivates player's buttons (WIP)
    /// </summary>
    public void PlayerAbilityPhase()
    {
        for (int j = 0; j < transform.GetChild(8).childCount; j++)
            transform.GetChild(8).GetChild(j).gameObject.SetActive(false);
    }


    /// <summary>
    /// when the cards have finished their activities set their fight values to default, is need the line where are the cards
    /// </summary>
    public bool FightPhase(int line)
    {
        if (CheckFightPhase(line) == 0)
        {
            for (int i = 0; i < MyBoard.transform.GetChild(line).childCount; i++)
            {
                MyBoard.transform.GetChild(line).GetChild(i).GetComponent<CardController>().FightFinished = false;
            }
            return true;
        }
        else if (CheckFightPhase(line) == -1)
            return true;
        return false;
    }

    /// <summary>
    /// when the cards have finished their activities set their death values to default, is need the line where are the cards
    /// </summary>
    /// <param name="line"></param>
    public bool EndPhase(int line)
    {
        if (CheckDeathPhase(line) == 0)
        {
            for (int i = 0; i < MyBoard.transform.GetChild(line).childCount; i++)
            {
                MyBoard.transform.GetChild(line).GetChild(i).GetComponent<CardController>().DeathFinished = false;
            }
            return true;
        }
        else if (CheckFightPhase(line) == -1)
            return true;
        return false;
    }

    /// <summary>
    /// [function linked to a button] used for select from player's hand the choosed card
    /// </summary>
    /// <param name="i"></param>
    public void ChooseCard(int i)
    {
        m_ChoosedCard = m_Hand[i];
        if(m_ChoosedCard.ManaPoints <= ActualManaPoints)
        {
            m_ChoosedInt = i;
            Accepted = true;

            for (int j = 0; j < transform.GetChild(7).childCount; j++)
                transform.GetChild(7).GetChild(j).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// [function linked to a button] used for instantiate the coosed card in the player's board
    /// </summary>
    /// <param name="linkedPosition"></param>
    public void PlaceCard(Transform linkedPosition)
    {
        if (Accepted)
        {
            for (int j = 0; j < transform.GetChild(7).childCount; j++)
                transform.GetChild(7).GetChild(j).gameObject.SetActive(false);

            string positionString = linkedPosition.name.Remove(0, 8);
            int position = Convert.ToInt32(positionString.Remove(1, 1));

            int child;
            if(position < 4)
                child = 1;
            else
                child = 2;

            GameObject tmp;
            tmp = Instantiate(m_ChoosedCard.PhysicBody, linkedPosition.position, Quaternion.identity, MyBoard.transform.GetChild(child).transform);
            tmp.GetComponent<CardController>().StageManager = StageManager;

            //check which is the line where the card is
            if (position < 4)
            {
                tmp.GetComponent<CardController>().PhaseFightLine = TurnPhases.FIGHT1;
                tmp.GetComponent<CardController>().PhaseEndLine = TurnPhases.END1;
            }
            else
            {
                tmp.GetComponent<CardController>().PhaseFightLine = TurnPhases.FIGHT2;
                tmp.GetComponent<CardController>().PhaseEndLine = TurnPhases.END2;
            }

            m_Hand[m_ChoosedInt] = null;
            ActualManaPoints -= m_ChoosedCard.ManaPoints;
            Mana.GetComponent<Text>().text = ActualManaPoints.ToString();
            Destroy(PhysicHand[m_ChoosedInt]);
            Accepted = false;
        }
    }

    public void HideBoard()
    {
        for (int j = 0; j < transform.GetChild(7).childCount; j++)
            transform.GetChild(7).GetChild(j).gameObject.SetActive(false);
    }

    /// <summary>
    /// Find first empty slot in the player's hand
    /// </summary>
    /// <returns></returns>
    private int FIndFirstEmpty()
    {
        bool finded = false;
        int firstPos = -1;

        for (int i = 0; i < m_Hand.Length; i++)
        {
            if (finded == false)
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

    /// <summary>
    /// check if every card on both boards has done their fight phase, , is need the line where are the cards
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private int CheckFightPhase(int line)
    {
        if (MyBoard.transform.GetChild(line).childCount > 0)
        {
            for (int i = 0; i < MyBoard.transform.GetChild(line).childCount; i++)
            {
                int both = 0;
                if (MyBoard.transform.GetChild(line).GetChild(i).GetComponent<CardController>().FightFinished == false)
                {
                    both++;
                }
                if (both > 0)
                    return 1;
            }
            return 0;
        }
        return -1;
    }

    /// <summary>
    /// check if every card on both boards has done their death phase, is need the line where are the cards
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private int CheckDeathPhase(int line)
    {
        if(MyBoard.transform.GetChild(line).childCount > 0)
        {
            for (int i = 0; i < MyBoard.transform.GetChild(line).childCount; i++)
            {
                int both = 0;
                if (MyBoard.transform.GetChild(line).GetChild(i).GetComponent<CardController>().DeathFinished == false)
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
