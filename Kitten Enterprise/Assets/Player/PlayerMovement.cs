using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;

    public static Rigidbody playerRigidbody;

    public static Plane movementPlane = new Plane(new Vector3(0, 1, 0), Vector3.zero);

    private Vector3 forwardsVector, rightVector;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    //setting up movement so we don't have to compute this literally every frame
    public void startMovement()
    {
        forwardsVector = Camera.main.transform.forward;
        forwardsVector = movementPlane.ClosestPointOnPlane(forwardsVector).normalized;
        rightVector = -Vector3.Cross(forwardsVector, movementPlane.normal);
    }

    public void OnMove(InputValue input)
    {
        startMovement();

        Vector2 inputVector = input.Get<Vector2>();
        Vector3 movementVector = inputVector.x * rightVector + inputVector.y * forwardsVector;
       
        playerRigidbody.AddForce(-playerRigidbody.velocity, ForceMode.VelocityChange);
        
        playerRigidbody.AddForce(movementVector * movementSpeed, ForceMode.VelocityChange);
    }
}
