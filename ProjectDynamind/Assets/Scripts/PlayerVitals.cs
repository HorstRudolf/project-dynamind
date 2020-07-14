using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{

    public int maxStamina;
    [SerializeField]
    private float stamina;

    private int staminaFallRate;

    private int staminaRegainRate;
    [SerializeField]
    private PlayerMovement.Status currentStatus;

    public float countdown = 3;

    [SerializeField]
    private PlayerMovement.GroundType groundTag;

    private float privateCountdown;

    // Start is called before the first frame update
    void Start()
    {
        stamina = maxStamina;

        staminaFallRate = 1;

        staminaRegainRate = 1;

        currentStatus = PlayerMovement.currentStatus;

        groundTag = PlayerMovement.groundTag;

        privateCountdown = countdown;

    }

    // Update is called once per frame
    void Update()
    {

        currentStatus = PlayerMovement.currentStatus;
        groundTag = PlayerMovement.groundTag;
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
            privateCountdown = countdown;
        }
        else if (currentStatus == PlayerMovement.Status.Sprinting && stamina >= 0)
        {
            stamina -= Time.deltaTime / staminaFallRate;
            privateCountdown = countdown;
        }
        else if (groundTag.ToString().Contains("Angle") && stamina >= 0)
        {
            stamina -= Time.deltaTime / staminaFallRate;
            privateCountdown = countdown;
        }
        else if (stamina <= maxStamina && privateCountdown > -1)
        {
            privateCountdown -= Time.deltaTime;
        }
        else if ((currentStatus == PlayerMovement.Status.Walking || currentStatus == PlayerMovement.Status.Exhausted) && stamina <= maxStamina && privateCountdown <= 0)
        {
            stamina += Time.deltaTime / staminaRegainRate;
        }


    }
}
