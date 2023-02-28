using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    public Card ThisCard = new Card();
    public StageManager StageManager;
    public TMP_Text Health;
    public TMP_Text Attack;
    public int Line;
    public int HealthPoints;
    public int AttackPoints;
    public bool DeathFinished = false;
    public bool FightFinished = false;

    public TurnPhases PhaseFightLine;
    public TurnPhases PhaseEndLine;

    private string m_TypeCard;
    private Vector3 m_CardForward;
    private string m_MyTag;
    private string m_EnemyTag;
    // Start is called before the first frame update
    void Awake()
    {
        AttackPoints = ThisCard.AttackPoints;
        HealthPoints = ThisCard.HealthPoints;

        Attack.text = AttackPoints.ToString();
        Health.text = HealthPoints.ToString();

        CheckWhichCard();
    }

    // Update is called once per frame
    void Update()
    {
        //check if there is an enemy in front of the card, if there is also attack
        if (StageManager.m_CurrentPhase == PhaseFightLine && FightFinished == false)
        {
            ResolveEffect();
            RaycastHit hit;
            if(Physics.Raycast(transform.position, m_CardForward, out hit))
            {
                if(hit.collider.CompareTag(m_EnemyTag) && transform.CompareTag(m_MyTag))
                {
                    CardAttack(hit);    
                }
                if (hit.collider.CompareTag("Player") && transform.CompareTag(m_MyTag))
                {
                    PlayerAttack(hit);
                }
            }
            
            FightFinished = true;
        }
        else if(StageManager.m_CurrentPhase == PhaseEndLine && DeathFinished == false)
        {
            if (HealthPoints <= 0)
                Destroy(gameObject);
            DeathFinished = true;
        }
    }

    private void ResolveEffect()
    {
        
    }

    private void CardAttack(RaycastHit hit)
    {
        hit.collider.GetComponent<CardController>().HealthPoints -= AttackPoints;
        hit.collider.GetComponent<CardController>().Health.text = hit.collider.GetComponent<CardController>().HealthPoints.ToString();
    }

    private void PlayerAttack(RaycastHit hit)
    {
        hit.collider.GetComponent<PhysicPlayer>().Player.HealthPoints -= AttackPoints;
        hit.collider.GetComponent<PhysicPlayer>().Player.Health.text = hit.collider.GetComponent<PhysicPlayer>().Player.HealthPoints.ToString();
    }

    private void CheckWhichCard()
    {
        m_TypeCard = ThisCard.name.Remove(0, 7);

        if (m_TypeCard == "p")
        {
            m_CardForward = transform.forward;
            m_MyTag = "PlayerCard";
            m_EnemyTag = "EnemyCard";
        }
        else
        {
            m_CardForward = -transform.forward;
            m_MyTag = "EnemyCard";
            m_EnemyTag = "PlayerCard";
        }
    }
}
