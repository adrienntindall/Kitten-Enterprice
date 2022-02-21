using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public AudioSource jumpSound;
    public AudioSource landSound;

    public static bool isJumping = false;

    private bool isJumpButtonHeld = false;

    public float jumpMaxHold = .2f;
    private float t = 0;

    public float jumpForce = 1.2f;
    public float gravity = 1f;
    public float terminalVelocity = 1f;
    private Vector3 prevGravitationalVelocity;

    private Vector3 respawn;

    /////CLOUD_EFFECT
    /////Public variables of particle prefabs
    public GameObject jumpParticle;
    public GameObject hitGroundParticle;
    /////////////////////////////////////////////////////////////////////////////////////

    public void handleJump()
    {
        if ((t < jumpMaxHold) && isJumpButtonHeld)
        {
            prevGravitationalVelocity = jumpForce * PlayerMovement.movementPlane.normal;
            /////CLOUD_EFFECT
            /////this calls the SpawnStartJumpParticle method when the character starts jumping
            if (isJumping == false)
            {
                SpawnStartJumpParticle();
            }
            /////////////////////////////////////////////////////////////////////////////////////
            PlayerController.playerController.movementSound.Stop();
            isJumping = true;
        }

        prevGravitationalVelocity += -gravity * PlayerMovement.movementPlane.normal * Time.fixedDeltaTime;

        bool downwards = (-Vector3.Dot(PlayerMovement.movementPlane.normal, prevGravitationalVelocity) > 0);

        if (downwards && (prevGravitationalVelocity.magnitude > terminalVelocity)) prevGravitationalVelocity *= terminalVelocity / prevGravitationalVelocity.magnitude;

        RaycastHit hit;

        float colliderRadius = PlayerController.playerCollider.bounds.extents.y;

        if (downwards && Physics.Raycast(new Ray(PlayerController.playerCollider.transform.position, prevGravitationalVelocity), out hit, colliderRadius + terminalVelocity * Time.fixedDeltaTime))
        {
            float trueDist = hit.distance - colliderRadius;
            if (prevGravitationalVelocity.magnitude >= trueDist)
            {
                prevGravitationalVelocity *= trueDist / prevGravitationalVelocity.magnitude;
                prevGravitationalVelocity /= Time.fixedDeltaTime;
            }
            if (trueDist <= 0.05f)
            {
                /////CLOUD_EFFECT
                /////this calls the SpawnStartJumpParticle method when the character hits the ground
                ///// or stops being in the state of jumping
                if (isJumping == true)
                {
                    SpawnHitGroundParticle();
                }
                /////////////////////////////////////////////////////////////////////////////////////
                isJumping = false;
                t = 0;
            }
        }
        else if(downwards && transform.position.y < -20)
        {
            transform.position = respawn;
        }


        PlayerMovement.playerRigidbody.AddForce(prevGravitationalVelocity, ForceMode.VelocityChange);

        t += Time.fixedDeltaTime;

    }

    public void OnJump()
    {
        isJumpButtonHeld = !isJumpButtonHeld;
    }


    /////CLOUD_EFFECT
    /////Methods for spawing particles

    private void SpawnStartJumpParticle()
    {
        GameObject particleInstance = GameObject.Instantiate(jumpParticle, transform.position, Quaternion.identity);
        particleInstance.SetActive(true);
        particleInstance.transform.SetParent(this.transform);
        respawn = transform.position;
        jumpSound.Play();
    }

    private void SpawnHitGroundParticle()
    {
        GameObject particleInstance = GameObject.Instantiate(hitGroundParticle, transform.position, transform.rotation);
        particleInstance.SetActive(true);
        landSound.Play();
    }

    /////////////////////////////////////////////////////////////////////////////////////
}
