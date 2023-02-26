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
    public int HealthPoints;
    public int AttackPoints;
    public bool DeathFinished = false;
    public bool FightFinished = false;
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
        if(StageManager.m_CurrentPhase == TurnPhases.FIGHT && FightFinished == false)
        {
            ResolveEffect();
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if(hit.collider.CompareTag("EnemyCard") && transform.CompareTag("PlayerCard"))
                {
                    hit.collider.GetComponent<CardController>().HealthPoints -= AttackPoints;
                    hit.collider.GetComponent<CardController>().Health.text = hit.collider.GetComponent<CardController>().HealthPoints.ToString();
                }
            }
            else if (Physics.Raycast(transform.position, -transform.forward, out hit))
            {
                if (hit.collider.CompareTag("PlayerCard") && transform.CompareTag("EnemyCard"))
                {
                    hit.collider.GetComponent<CardController>().HealthPoints -= AttackPoints;
                    hit.collider.GetComponent<CardController>().Health.text = hit.collider.GetComponent<CardController>().HealthPoints.ToString();
                }
            }
            
            FightFinished = true;
        }
        else if(StageManager.m_CurrentPhase == TurnPhases.END && DeathFinished == false)
        {
            if (HealthPoints <= 0)
                Destroy(gameObject);
            DeathFinished = true;
        }
    }

    private void ResolveEffect()
    {
        
    }
}
