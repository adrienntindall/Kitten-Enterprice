using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Collider playerCollider;

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
        playerMovement.handleMovement();
        playerJump.handleJump();
    }

}
