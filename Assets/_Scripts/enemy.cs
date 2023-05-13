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
    void Update()
    {

        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance && !isAttacking && playercontroller.hp > 0)
        {
            StartCoroutine(Attack());
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) < distance)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed + Time.fixedDeltaTime);
            rb.MovePosition(pos);
            transform.LookAt(target);

        }




    }


    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(attackDelay);
        player.GetComponent<playercontroller>().TakeDamage(10);
        isAttacking = false;
        animator.SetBool("attack", false);

    }


}
