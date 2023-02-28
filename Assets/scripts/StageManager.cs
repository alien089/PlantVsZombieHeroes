using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StageManager : MonoBehaviour
{
    public List<Card> CardPlayerList = new List<Card>();
    public List<Card> CardEnemyList = new List<Card>();
    public GameObject ActualPhase;
    public GameObject TurnPosList;
    public PlayerController Player;
    public PlayerController Enemy;
    
    public TurnPhases m_CurrentPhase;
    private Transform[] m_PhasePositions = new Transform[6];
    // Start is called before the first frame update
    void Start()
    {
        m_CurrentPhase = TurnPhases.DRAW;
        for (int i = 0; i < m_PhasePositions.Length; i++)
        {
            m_PhasePositions[i] = TurnPosList.transform.GetChild(i).transform;
        }
        ActualPhase.transform.position = m_PhasePositions[(int)m_CurrentPhase].position;
        ActualPhase.SetActive(false);

        Player.ActualManaPoints = Player.TotalManaPoints;
        Enemy.ActualManaPoints = Enemy.TotalManaPoints;

        Player.StartPhase(CardPlayerList);
        Enemy.StartPhase(CardEnemyList);
    }

    private void Update()
    {
        switch(m_CurrentPhase)
        {
            case TurnPhases.DRAW:
                Player.DrawPhase(CardPlayerList);
                Enemy.DrawPhase(CardEnemyList);
                EndPhase();
                break;
            case TurnPhases.PLAYER:
                Player.PlayerPhase();
                Enemy.PlayerPhase();
                break;
            case TurnPhases.ENEMY:
                Player.EnemyPhase();
                Enemy.EnemyPhase();
                break;
            case TurnPhases.PLAYERABILITY:
                Player.PlayerAbilityPhase();
                Enemy.PlayerAbilityPhase();
                break;
            case TurnPhases.FIGHT1:
                if(Player.FightPhase(1) && Enemy.FightPhase(1))
                    m_CurrentPhase = m_CurrentPhase + 1;
                break;
            case TurnPhases.END1:
                if (Player.EndPhase(1) && Enemy.EndPhase(1))
                    m_CurrentPhase = m_CurrentPhase + 1;
                break;
            case TurnPhases.FIGHT2:
                if (Player.FightPhase(2) && Enemy.FightPhase(2))
                    m_CurrentPhase = m_CurrentPhase + 1;
                break;
            case TurnPhases.END2:
                if (Player.EndPhase(2) && Enemy.EndPhase(2))
                    EndPhase();
                break;
        }
    }

    public void EndPhase()
    {
        ActualPhase.SetActive(true);
        if (m_CurrentPhase == TurnPhases.END2)
        {
            ActualPhase.SetActive(false);

            Player.TotalManaPoints += 1;
            Player.ActualManaPoints = Player.TotalManaPoints;
            Player.Mana.GetComponent<Text>().text = Player.ActualManaPoints.ToString();

            Enemy.TotalManaPoints += 1;
            Enemy.ActualManaPoints = Enemy.TotalManaPoints;
            Enemy.Mana.GetComponent<Text>().text = Enemy.ActualManaPoints.ToString();

            m_CurrentPhase = TurnPhases.DRAW;
        }
        else
            m_CurrentPhase = m_CurrentPhase + 1;

        if(m_CurrentPhase != TurnPhases.DRAW)
            ActualPhase.transform.position = m_PhasePositions[(int)m_CurrentPhase].position;
    }
}
