using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public static bool isJumping = false;

    public float maxJumpTime = .7f;
    private float t = 0;

    public float jumpV0 = 1f;
    private float jumpForce = 1.2f;
    private float gravity = 1f;
    private Vector3 prevGravitationalVelocity;

    private void FixedUpdate()
    {
        prevGravitationalVelocity += -gravity*PlayerMovement.movementPlane.normal*Time.fixedDeltaTime;

        if(isJumping && t < maxJumpTime)
        {
            prevGravitationalVelocity += jumpForce * PlayerMovement.movementPlane.normal * Time.fixedDeltaTime;
        }

        PlayerMovement.playerRigidbody.AddForce(prevGravitationalVelocity, ForceMode.VelocityChange);



        t += Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //add logic to check if the player's on the ground as opposed to a wall, etc
        prevGravitationalVelocity = Vector3.zero;
        isJumping = false;
        
    }

    private void OnCollisionStay(Collision collision)
    {
        prevGravitationalVelocity = Vector3.zero;
        t = 0;
    }

    public void OnJump()
    {
        if (!isJumping && t < maxJumpTime)
        {
            prevGravitationalVelocity += jumpV0 * PlayerMovement.movementPlane.normal;
            isJumping = true;
        }
    }
}
