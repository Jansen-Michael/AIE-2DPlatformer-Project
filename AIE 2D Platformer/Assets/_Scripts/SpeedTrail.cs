using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTrail : MonoBehaviour
{
    public SpriteRenderer playerBody;
    public GameObject trail;
    public float trailDelay;
    private float trailDelaySeconds;
    public bool makeTrail = false;

    void Start()
    {
        trailDelaySeconds = trailDelay;
    }

    void Update()
    {
        if (makeTrail)
        {
            if (trailDelaySeconds > 0)
            {
                trailDelaySeconds -= Time.deltaTime;
            }
            else
            {
                // Generate Trail
                GameObject currentTrail = Instantiate(trail, transform.position, transform.rotation);
                Sprite currentSprite = playerBody.sprite;
                currentTrail.transform.localScale = playerBody.transform.localScale * 2;
                currentTrail.GetComponent<SpriteRenderer>().sprite = currentSprite;
                trailDelaySeconds = trailDelay;
                Destroy(currentTrail, 1f);
            }
        }
    }
}
