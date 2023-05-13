using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class playercontroller : MonoBehaviour
{
    public Rigidbody rb;
    public Animator animController;
    public float speed;
    public float rotationSpeed = 1000f;
    public static float hp;
    public float maxhp;
    public float checkDist;
    GameObject closestEnemy;
    GameObject[] enemies;
    private CharacterController controller;
    public bl_Joystick floatingJoystick;
    private bool isAlive = true;
    private GameObject target;
    private bool idle;
    private bool run;
    private bool isAttacking = false;
    public Transform Gun;
    public float rpm;
    public int FireDamage;
    public float interval = 1f;
    private float timer = 0f;
    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        hp = maxhp;
        Debug.Log("Start HP:" + hp);
    }
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        floatingJoystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<bl_Joystick>();
    }

    private void Update()
    {
        if (isAlive == true)
        {
            Move();
        }
        CheckEnemy();
        if (hp <= 0)
        {
            Die();
        }
        CheckAttack();
        Attack();
        CheckAnim();
    }
    public void Die()
    {
        isAlive = false;
    }
    public void CheckAttack()
    {
        if (idle == true && target != null)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }
    public void Attack()
    {
        if (isAttacking)
        {
            //  transform.LookAt(target.transform);
            Vector3 relativePos = target.transform.position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            Shoot(Gun.transform, target.transform);
        }
    }
    public void CheckAnim()
    {
        if (run == true && idle == false && isAttacking == false && isAlive != false)
        {
            animController.SetBool("run", true);
            animController.SetBool("Idle", false);
            animController.SetBool("Idle", false);
            animController.SetBool("weaponidle", false);
            animController.SetBool("attack", false);
        }
        else if (run == false && isAttacking == false && isAlive != false)
        {
            animController.SetBool("run", false);
            animController.SetBool("Idle", true);
            animController.SetBool("attack", false);
            animController.SetBool("weaponidle", false);

        }
        else if (isAttacking == true && isAlive == true)
        {

            if (timer == 0)
            {
                animController.SetBool("run", false);
                animController.SetBool("Idle", false);
                animController.SetBool("attack", true);
                animController.SetBool("weaponidle", false);
            }
            else
            {
                animController.SetBool("run", false);
                animController.SetBool("Idle", false);
                animController.SetBool("attack", false);
                animController.SetBool("weaponidle", true);
            }

        }
        else if (isAlive == false)
        {
            animController.SetBool("die", true);
            animController.SetBool("run", false);
            animController.SetBool("Idle", false);
            animController.SetBool("attack", false);
            animController.SetBool("weaponidle", false);

        }
    }
    public void Shoot(Transform origin, Transform target)
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {

            RaycastHit rayHit;

            Physics.Raycast(origin.position, (target.position - origin.position).normalized, out rayHit);

            if (rayHit.collider != null)
            {
                if (rayHit.collider.gameObject.tag == "enemy")
                {
                    rayHit.collider.gameObject.GetComponent<Health>().TakeDamage(FireDamage);
                    timer = 0f;
                }
            }

        }

    }
    private void CreateWeaponTracer(Vector3 startPos, Vector3 targetPos)
    {
        Vector3 dir = (targetPos - startPos).normalized;

    }
    public void Move()
    {
        //OLD ANIMATION


        //if (joystick.Vertical <= -0.35 || joystick.Vertical >= 0.35 || joystick.Horizontal <= -0.35 || joystick.Horizontal >= 0.35)
        //{
        //    rb.velocity = new Vector3(joystick.Horizontal * speed, rb.velocity.y, joystick.Vertical * speed);
        //    Vector3 vel = rb.velocity;
        //    if (vel.magnitude >= 2)
        //    {
        //        transform.rotation = Quaternion.LookRotation(vel);
        //        isRunning = true;
        //    }
        //}
        //else
        //{
        //    isRunning =  false;
        //}

        float horizontalInput = floatingJoystick.Horizontal;
        float verticalInput = floatingJoystick.Vertical;

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        if (floatingJoystick.Vertical <= -0.35 || floatingJoystick.Vertical >= 0.35 || floatingJoystick.Horizontal <= -0.35 || floatingJoystick.Horizontal >= 0.35)
        {
            controller.SimpleMove(movementDirection * speed);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                //transform.rotation = Quaternion.LookRotation(movementDirection);
                if (!animController)
                    return;

                run = true;
                idle = false;
            }
        }
        else
        {
            if (!animController)
                return;

            run = false;
            idle = true;
        }
    }

    void CheckEnemy()
    {
        closestEnemy = ClosestEnemy();
        float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
        if (distance < checkDist + 0.5 && closestEnemy.name != "player")
        {
            target = closestEnemy;
        }
        else
        {
            target = null;
        }
        enemies = GameObject.FindGameObjectsWithTag("enemy");

    }
    GameObject ClosestEnemy()
    {
        GameObject closestHere = gameObject;
        float leastDistance = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            float distanceHere = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceHere < leastDistance)
            {
                leastDistance = distanceHere;
                closestHere = enemy;
            }
        }

        return closestHere;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkDist);
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(hp);

    }

}
