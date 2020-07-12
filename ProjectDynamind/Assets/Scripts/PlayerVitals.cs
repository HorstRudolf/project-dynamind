﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{

    public int maxStamina;
    private float stamina;

    private int staminaFallRate;

    private int staminaRegainRate;

    private PlayerMovement.Status currentStatus;

    public float countdown = 3;



    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;

        staminaFallRate = 1;

        staminaRegainRate = 1;

        currentStatus = PlayerMovement.currentStatus;

    }

    // Update is called once per frame
    void Update()
    {
        currentStatus = PlayerMovement.currentStatus;
        CheckStatus();
    }

    void CheckStatus()
    {
    

        if (stamina <= 0)
        {
            PlayerMovement.currentStatus = PlayerMovement.Status.Exhausted;
        }

        if (stamina >= 0 && currentStatus != PlayerMovement.Status.LadderClimbing)
        {
            PlayerMovement.currentStatus = PlayerMovement.Status.Walking;
        }

        if (currentStatus == PlayerMovement.Status.LadderClimbing && stamina >= 0)
        {
            stamina -= Time.deltaTime / staminaFallRate;
            countdown = 3;
        }
        else if (currentStatus == PlayerMovement.Status.Sprinting && stamina >= 0)
        {
            stamina -= Time.deltaTime / staminaFallRate;
            countdown = 3;
        }
        else if (stamina <= maxStamina && countdown > -1)
        {
            countdown -= Time.deltaTime;
        }
        else if ((currentStatus == PlayerMovement.Status.Walking || currentStatus == PlayerMovement.Status.Exhausted) && stamina <= maxStamina && countdown <= 0)
        {
            stamina += Time.deltaTime / staminaRegainRate;
        }


    }
}
