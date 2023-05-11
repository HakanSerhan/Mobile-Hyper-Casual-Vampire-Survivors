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
        if(hp <= 0)
        {
            Die();
        }
        
    }
    public void Die()
    {
        isAlive = false;
        animController.SetBool("die",true);
        animController.SetBool("run", false);
        animController.SetBool("Idle", false);
        
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

                animController.SetBool("run", true);
                animController.SetBool("Idle", false);

            }
        }
        else
        {
            if (!animController)
                return;

            animController.SetBool("run", false);
            animController.SetBool("Idle", true);
        }
    }

    void CheckEnemy()
    {
        closestEnemy = ClosestEnemy();
        float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
        if (distance < checkDist + 0.5 && closestEnemy.name != "player")
        {
            Debug.Log(closestEnemy.name);
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
