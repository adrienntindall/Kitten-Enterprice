using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;

    //Audio Sources
    public AudioSource movementSound;

    public static bool isBig = false;

    public static Collider playerCollider;
    
    public static BigModeSwapper currentSwapper;

    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private bool isInDialogue = false;

    private void Awake()
    {
        playerController = this;
        playerCollider = GetComponent<Collider>();
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
    }

    private void FixedUpdate()
    {
        playerMovement.resetMovement();
        if (!isBig && !isInDialogue)
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

    public void startDialogue()
    {
        isInDialogue = true;
    }

    public void endDialogue()
    {
        isInDialogue = false;
    }
}
