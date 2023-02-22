using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageManager : MonoBehaviour
{
    public List<Card> CardList = new List<Card>();
    public GameObject ActualPhase;
    public GameObject TurnPosList;

    private TurnPhases m_CurrentPhase;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndPhase()
    {
        ActualPhase.SetActive(true);
        if (m_CurrentPhase == TurnPhases.FIGHT)
        {
            ActualPhase.SetActive(false);
            m_CurrentPhase = TurnPhases.DRAW;
        }
        else
            m_CurrentPhase = m_CurrentPhase + 1;

        if(m_CurrentPhase != TurnPhases.DRAW)
            ActualPhase.transform.position = m_PhasePositions[(int)m_CurrentPhase].position;
    }
}
