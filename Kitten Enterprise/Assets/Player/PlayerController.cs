using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isBig = false;

    public static Collider playerCollider;
    
    public static BigModeSwapper currentSwapper;

    private PlayerMovement playerMovement;
    private PlayerJump playerJump;

    private void Awake()
    {
        playerCollider = GetComponent<Collider>();
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void FixedUpdate()
    {
        playerMovement.resetMovement();
        if (!isBig)
        {
            playerMovement.handleMovement();
            playerJump.handleJump();
        }
    }

    public void OnExitBigMode()
    {
        if(isBig)
        {
            currentSwapper.exitBigMode();
        }
    }
}
