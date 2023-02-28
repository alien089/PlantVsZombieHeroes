using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    // Start is called before the first frame update
    void Awake()
    {
        AttackPoints = ThisCard.AttackPoints;
        HealthPoints = ThisCard.HealthPoints;

        Attack.text = AttackPoints.ToString();
        Health.text = HealthPoints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //check if there is an enemy in front of the card, if there is also attack
        if (StageManager.m_CurrentPhase == PhaseFightLine && FightFinished == false)
        {
            ResolveEffect();
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if(hit.collider.CompareTag("EnemyCard") && transform.CompareTag("PlayerCard"))
                {
                    DoAttack(hit);    
                }
            }
            else if (Physics.Raycast(transform.position, -transform.forward, out hit))
            {
                if (hit.collider.CompareTag("PlayerCard") && transform.CompareTag("EnemyCard"))
                {
                    DoAttack(hit);    
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

    private void DoAttack(RaycastHit hit)
    {
        hit.collider.GetComponent<CardController>().HealthPoints -= AttackPoints;
        hit.collider.GetComponent<CardController>().Health.text = hit.collider.GetComponent<CardController>().HealthPoints.ToString();
    }
}
