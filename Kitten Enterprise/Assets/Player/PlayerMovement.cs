using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;

    public static Rigidbody playerRigidbody;

    public GameObject playerModel;

    private float initialAcceleration = 10;
    private float initialSpeed = 3;

    public static Plane movementPlane = new Plane(new Vector3(0, 1, 0), Vector3.zero);

    private Vector3 forwardsVector, rightVector;

    private Vector2 inputVector;

    private Quaternion prevRotation = Quaternion.AngleAxis(90, Vector3.right);

    private bool isMoving = false;
    private float t = 0;

    

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void handleMovement()
    {
        if (inputVector.Equals(Vector2.zero))
        {
            PlayerController.playerController.movementSound.Stop();
            return;
        }

        if(!PlayerJump.isJumping && !PlayerController.playerController.movementSound.isPlaying) PlayerController.playerController.movementSound.Play();

        startMovement();

        float speed = initialSpeed + initialAcceleration * t;

        speed = Mathf.Min(movementSpeed, speed);

        Vector3 movementVector = inputVector.x * rightVector + inputVector.y * forwardsVector;

        if(inputVector.x != 0 || inputVector.y != 0)
        {
            prevRotation = Quaternion.LookRotation(movementVector.normalized, movementPlane.normal);
        }

        playerModel.transform.rotation = prevRotation;

        playerRigidbody.AddForce(movementVector * speed, ForceMode.VelocityChange);

        t += Time.fixedDeltaTime;
    }


    public void startMovement()
    {
        forwardsVector = Quaternion.FromToRotation(Vector3.up, movementPlane.normal)*Camera.main.transform.forward;
        forwardsVector = movementPlane.ClosestPointOnPlane(forwardsVector).normalized;
        rightVector = -Vector3.Cross(forwardsVector, movementPlane.normal);
    }

    public void resetMovement()
    {
        playerRigidbody.AddForce(-playerRigidbody.velocity, ForceMode.VelocityChange);
    }

    public void OnMove(InputValue input)
    {
        inputVector = input.Get<Vector2>();
        bool temp = isMoving;
        isMoving = !inputVector.Equals(Vector2.zero);
        if(!temp && isMoving)
        {
            t = 0;
        }
    }

    public static void setMovementPlane(Vector3 normal)
    {
        movementPlane = new Plane(normal, Vector3.zero);
    }

   

}
