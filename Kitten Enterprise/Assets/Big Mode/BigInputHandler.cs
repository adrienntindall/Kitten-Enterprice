using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BigInputHandler : MonoBehaviour
{
    public float dragSpeed;

    private Ray mouseRay;
    private bool mouseDown = false;

    private Rigidbody grabbedObject = null;
    private Collider grabbedCollider = null;
    private Plane movementPlane;


    private void Update()
    {
        if (!PlayerController.isBig) return;
        mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        mouseDown = Mouse.current.leftButton.IsPressed();
    }

    private void FixedUpdate()
    {
        if (!PlayerController.isBig) return;
        RaycastHit hit;
        if (!mouseDown)
        {
            if(grabbedObject != null)
            {
                grabbedObject.constraints = RigidbodyConstraints.FreezeAll;
                grabbedObject = null;
                
            }
            
            return;
        }
        if ((grabbedObject == null) && Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider.tag != "Moveable") return;
            grabbedObject = hit.collider.attachedRigidbody;
            grabbedCollider = hit.collider;
            grabbedObject.constraints = RigidbodyConstraints.FreezeRotation;
            movementPlane = grabbedObject.GetComponent<MoveableObject>().getMovementPlane();
        }
        if (grabbedObject == null) return;

        grabbedObject.AddForce(-grabbedObject.velocity, ForceMode.VelocityChange);

        float dist = 0;

        movementPlane.Raycast(mouseRay, out dist);

        Vector3 movementVector = (mouseRay.GetPoint(dist) - movementPlane.ClosestPointOnPlane(grabbedObject.transform.position));

        Vector3 closePoint = transform.position;

        if(grabbedCollider.Raycast(new Ray(grabbedCollider.transform.position + movementVector.normalized*99, -movementVector), out hit, Mathf.Infinity))
        {
            closePoint = hit.point;
        }

        float colliderRadius = (grabbedCollider.transform.position - closePoint).magnitude;

        int trueLayer = grabbedObject.gameObject.layer;

        grabbedObject.gameObject.layer = 2; //Ignore Raycast Layer

        if (Physics.Raycast(new Ray(grabbedCollider.transform.position, movementVector), out hit, colliderRadius + (dragSpeed + 2) * Time.fixedDeltaTime))
        {
            float trueDist = hit.distance - colliderRadius;
            if (dragSpeed*movementVector.magnitude >= trueDist)
            { 
                movementVector *= trueDist / movementVector.magnitude;
                movementVector /= dragSpeed * Time.fixedDeltaTime;
            }
            if(trueDist <= 0.1f)
            {
                movementVector *= 0;
            }
        }

        grabbedObject.gameObject.layer = trueLayer;

        grabbedObject.AddForce(dragSpeed * movementVector, ForceMode.VelocityChange);
    }
}
