using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] private string enemyName;
    public Transform target;
    public float speed;
    Rigidbody rb;
    public float distance;
    private bool FollowOn = true;
    public float attackDistance = 2f;
    public float attackDelay = 0.25f;
    private GameObject player;
    private Animator animator;
    private bool isAttacking = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        animator = GetComponent<Animator>();
    }
    void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance && !isAttacking && playercontroller.hp > 0)
        {
            StartCoroutine(Attack());
        }
        CheckAnim();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) < distance)
        {
            FollowOn = true;
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed + Time.fixedDeltaTime);
            rb.MovePosition(pos);
            transform.LookAt(target);

        }
        else
        {
            FollowOn = false;
        }




    }


    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(attackDelay);
        player.GetComponent<playercontroller>().TakeDamage(10);
        isAttacking = false;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
    public void CheckAnim()
    {
        if (FollowOn == false && isAttacking == false)
        {
            animator.SetBool("idle", true);
            animator.SetBool("walk", false);
            animator.SetBool("attack", false);
        }
        else if (FollowOn == true && isAttacking == false)
        {
            animator.SetBool("idle", false);
            animator.SetBool("walk", true);
            animator.SetBool("attack", false);
        }
        else if (isAttacking == true)
        {
            animator.SetBool("idle", false);
            animator.SetBool("walk", false);
            animator.SetBool("attack", true);
        }

    }

}
