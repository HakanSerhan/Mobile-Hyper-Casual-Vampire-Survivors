using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float healthPoint;

    [SerializeField] private float maxHealthPoint;
    public ParticleSystem hit;

    // Start is called before the first frame update
    void Start()
    {
        healthPoint = maxHealthPoint;

    }
    void Update()
    {
        Die();
    }
    public void Die()
    {
        if (healthPoint <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        healthPoint -= damage;

        Debug.Log(healthPoint);
        hit.Emit(1);
    }
}
