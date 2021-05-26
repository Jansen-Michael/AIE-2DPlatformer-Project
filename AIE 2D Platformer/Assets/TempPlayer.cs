using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public Boomerang boomerang;
    public float speed = 20f;
    private float horizontalMoveInput;
    private float verticalMoveInput;
    private Rigidbody2D rb;

    public GameObject chargeBar;
    public float timeToMaxCharge = 1.2f;
    private float chargeTime;
    private float chargePercentage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
        verticalMoveInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontalMoveInput * speed, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, verticalMoveInput * speed);
        Boomerang();
    }

    private void Boomerang()
    {
        Vector2 mouseLocation = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

        if (boomerang.isWithPlayer())
        {
            if (Input.GetMouseButton(0))
            {
                chargeTime += Time.deltaTime;
                chargePercentage = chargeTime / timeToMaxCharge;
                chargePercentage = Mathf.Clamp01(chargePercentage);
                chargeBar.transform.localScale = new Vector3(chargePercentage, chargeBar.transform.localScale.y);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector2 throwDirection = new Vector2(mouseLocation.x - transform.position.x, mouseLocation.y - transform.position.y).normalized;
                boomerang.ThrowBoomerang(throwDirection, chargePercentage);
                chargeTime = 0;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (!boomerang.isWithPlayer()) { boomerang.FreezeBoomerang(); }
        }
    }
}
