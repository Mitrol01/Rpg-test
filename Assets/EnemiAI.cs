using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiAI : MonoBehaviour
{

    //dist enemi/joueur
    private float Distance;

    //cible aquired ( se dirige vers l'enemi)
    public Transform Target;

    //distance de poursuite
    public float ChaseRange = 10;

    // portee des attaques
    public float AttackRange = 2.2f;

    //cooldown attaques
    public float AttackRepeatTime = 1;
    private float attacktime;

    //montant des degats infliges
    public float TheDamage;

    // agent de navigation
    private UnityEngine.AI.NavMeshAgent agent;

    // animation de l'enemi
    private Animation animations;

    // vie de l'ennemi
    public float enemyHealth;

    //dead
    private bool IsDead = false;

    // Use this for initialization
    void Start()
    {
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animations = gameObject.GetComponent<Animation>();
        attacktime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {



            // cherche le joueur en permanence
            Target = GameObject.FindWithTag("Player").transform;
            //agent.destination = Target.position;

            // calcule la distance entre joueur et enemi et reagi en consequence

            Distance = Vector3.Distance(Target.position, transform.position);


            //qud l'enemi es loin idle
            if (Distance > ChaseRange)
            {
                idle();
            }

            //qud l'enemi proche mais pas assez pour attaquer
            if (Distance < ChaseRange && Distance > AttackRange)
            {
                chase();
            }

            //qud l'enemi es proche pour attack
            if (Distance < AttackRange)
            {
                Attack();
            }
        }
    }
    //combat
    private void Attack()
    {
        //traverse pas le joueur
        agent.destination = transform.position;

        //si no cooldown

        if (Time.time> attacktime)
        {
            animations.Play("Attack_01");
            Target.GetComponent<PlayerInventory>().ApplyDamage(TheDamage);
            Debug.Log("L'ennemi a envoye " + TheDamage + " points de dégats");
            attacktime = Time.time + AttackRepeatTime;
        }
    }
    // poursuite
    private void chase()
    {
        animations.Play("Walk");
        agent.destination = Target.position;
    }

    private void idle()
    {
        animations.Play("Idle_01");
    }

    public void ApplyDammage(float TheDammage)
    {
        if (!IsDead)
        {
            enemyHealth = enemyHealth - TheDammage;
            print(gameObject.name + "a subit " + TheDammage + " points de dégâts.");

            if (enemyHealth <= 0)
            {
                Dead();
            }
        }
    }

    public void Dead()
    {
        IsDead = true;
        animations.Play("Die");
        Destroy(transform.gameObject, 5);
    }
}
