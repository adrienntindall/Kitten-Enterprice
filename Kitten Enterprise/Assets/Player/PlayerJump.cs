using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public static bool isJumping = false;

    private bool isJumpButtonHeld = false;

    public float jumpMaxHold = .2f;
    private float t = 0;

    public float jumpForce = 1.2f;
    public float gravity = 1f;
    public float terminalVelocity = 1f;
    private Vector3 prevGravitationalVelocity;

    public void handleJump()
    {
        if ((t < jumpMaxHold) && isJumpButtonHeld)
        {
            prevGravitationalVelocity = jumpForce * PlayerMovement.movementPlane.normal;
            isJumping = true;
        }
        
        prevGravitationalVelocity += -gravity * PlayerMovement.movementPlane.normal * Time.fixedDeltaTime;

        bool downwards = (-Vector3.Dot(PlayerMovement.movementPlane.normal, prevGravitationalVelocity) > 0);

        if (downwards && (prevGravitationalVelocity.magnitude > terminalVelocity)) prevGravitationalVelocity *= terminalVelocity / prevGravitationalVelocity.magnitude;

        RaycastHit hit;

        float colliderRadius = PlayerController.playerCollider.bounds.extents.y;

        if (downwards && Physics.Raycast(new Ray(PlayerController.playerCollider.transform.position, prevGravitationalVelocity), out hit, colliderRadius + terminalVelocity*Time.fixedDeltaTime))
        {
            float trueDist = hit.distance - colliderRadius;
            if (prevGravitationalVelocity.magnitude >= trueDist)
            {
                prevGravitationalVelocity *= trueDist / prevGravitationalVelocity.magnitude;
                prevGravitationalVelocity /= Time.fixedDeltaTime;
            }
            if(trueDist <= 0.05f)
            {
                isJumping = false;
                t = 0;
            }
        }

        PlayerMovement.playerRigidbody.AddForce(prevGravitationalVelocity, ForceMode.VelocityChange);

        t += Time.fixedDeltaTime;

    }

    public void OnJump()
    {
        isJumpButtonHeld = !isJumpButtonHeld;
    }
}
