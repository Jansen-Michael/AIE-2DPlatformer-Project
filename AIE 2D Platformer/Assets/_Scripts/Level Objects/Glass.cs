using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    public Sprite glassBrick;
    public Sprite damagedGlassBrick;
    public Sprite crackedGlassBrick;
    public int maxHealth;
    private int health;

    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }

        switch(health)
        {
            case 3:
                spriteRender.sprite = glassBrick;
                break;
            case 2:
                spriteRender.sprite = damagedGlassBrick;
                break;
            case 1:
                spriteRender.sprite = crackedGlassBrick;
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
