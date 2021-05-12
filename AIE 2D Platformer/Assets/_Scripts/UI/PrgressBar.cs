using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrgressBar : MonoBehaviour
{
    private PlayerController player;
    private DashMove playerDash;

    public Image dashBar;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerDash = player.GetComponent<DashMove>();
    }

    // Update is called once per frame
    void Update()
    {
        float fillAmount = playerDash.currentDashChargeTimer / playerDash.dashChargeTime;
        dashBar.fillAmount = fillAmount;
    }
}
