using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 1;

    void Start()
    {
        
    }

    void Update()
    {
        if (health <= 0) { Death(); }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }
}
