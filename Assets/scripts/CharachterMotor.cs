using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterMotor : MonoBehaviour
{

    //script playerInventory
    PlayerInventory playerInv;

    //animation du perso
    Animation Animations;
    //vitesse de deplacement
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    //inputs
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public Vector3 jumpSpeed;
    CapsuleCollider PlayerCollider;

    // es mort ?

    public bool IsDead = false;

    // check terrain ou eau
    public Transform groundChecker;
    public LayerMask layerToCollide;
    public Transform WaterChecker;
    public LayerMask ZoneToCollide;

    // Variables concernant l'attaque
    public float attackCooldown;
    private bool isAttacking;
    private float currentCooldown;
    public float attackRange;
    public GameObject RayHit;

    // Use this for initialization
    void Start()
    {
        Animations = gameObject.GetComponent<Animation>();
        PlayerCollider = gameObject.GetComponent<CapsuleCollider>();
        playerInv = gameObject.GetComponent<PlayerInventory>();
        RayHit = GameObject.Find("RayHit");
    }
    bool IsGrounded()
    {
        float distance = 0.1f;
        Debug.DrawLine(groundChecker.position, groundChecker.position + groundChecker.forward * distance, Color.red, 10);
        return Physics.Raycast(groundChecker.position, groundChecker.forward, distance, layerToCollide);// PlayerCollider.bounds.center, new Vector3(PlayerCollider.bounds.center.x, PlayerCollider.bounds.min.y - 0.1f, PlayerCollider.bounds.center.z), 0.08f);
    }

    bool IsWater()
    {
        float distance = 0.1f;
        Debug.DrawLine(WaterChecker.position, WaterChecker.position + WaterChecker.forward * distance, Color.magenta, 10);
        return Physics.Raycast(WaterChecker.position, WaterChecker.forward, distance, ZoneToCollide);
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {


            // si on avance
            if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, walkSpeed * Time.deltaTime);

                if (!isAttacking)
                {
                    Animations.Play("walk");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }
            //sprint
            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, runSpeed * Time.deltaTime);
                Animations.Play("run");
            }
            // si on recule
            if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);

                if (!isAttacking)
                {
                    Animations.Play("walk");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }
            //rotation a gauche

            if (Input.GetKey(inputLeft))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
            }

            //rotation a droite

            if (Input.GetKey(inputRight))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }

            //si on ne bouge pas
            //Debug    .Log("Hello: " + Time.time);
            if (!Input.GetKey(inputFront) && !Input.GetKey(inputBack))
            {
                Animations.Play("idle");///////////////ONN ETAIT ICI MEC  !!! SOUVIENT TOI ... SOUVIENT TOOIII.
            }

            //saut

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                //prepa saut(necessaire en c# )
                Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
                v.y = jumpSpeed.y;

                //saut
                gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
            }

            //if (IsWater==Physics.Raycast(PlayerCollider))
            //{

            //}
            if (isAttacking)
            {
                currentCooldown -= Time.deltaTime;
            }

            if (currentCooldown <= 0)
            {
                currentCooldown = attackCooldown;
                isAttacking = false;
            }

        }

    }
        // Fonction d'attaque
        public void Attack()
        {
            if (!isAttacking)
            {
                Animations.Play("attack");

                RaycastHit hit;

                if (Physics.Raycast(RayHit.transform.position, transform.TransformDirection(Vector3.forward), out hit, attackRange))
                {
                    Debug.DrawLine(RayHit.transform.position, hit.point, Color.red);

                    if (hit.transform.tag == "Enemy")
                    {
                        hit.transform.GetComponent<EnemiAI>().ApplyDammage(playerInv.currentDamage);
                    }

                }
                isAttacking = true;
            }

        }
}

