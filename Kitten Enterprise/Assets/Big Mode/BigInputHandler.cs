using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BigInputHandler : MonoBehaviour
{
    public float dragSpeed;

    private Laser currentLaser;
    private Ray mouseRay;
    public static bool mouseDown = false;

    public static Rigidbody grabbedObject = null;
    private Collider grabbedCollider = null;
    private Plane movementPlane;
    private MoveableObject[] allMoveables;

    private void Awake()
    {
        allMoveables = FindObjectsOfType<MoveableObject>();
    }

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
                grabbedObject.collisionDetectionMode = CollisionDetectionMode.Continuous;
                foreach(MoveableObject obj in allMoveables)
                {
                    obj.objectRigidbody.isKinematic = false;
                }
                grabbedObject = null;
                
            }
            
            return;
        }
        if ((grabbedObject == null) && Physics.Raycast(mouseRay, out hit))
        {
            if (hit.collider.tag != "Moveable") return;
            foreach (MoveableObject obj in allMoveables)
            {
                obj.objectRigidbody.isKinematic = true;
            }            
            grabbedObject = hit.collider.attachedRigidbody;
            grabbedCollider = hit.collider;
            grabbedObject.isKinematic = false;
            grabbedObject.constraints = RigidbodyConstraints.FreezeRotation;
            movementPlane = grabbedObject.GetComponent<MoveableObject>().getMovementPlane();
            grabbedObject.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        if (grabbedObject == null) return;

        grabbedObject.AddForce(-grabbedObject.velocity, ForceMode.VelocityChange);

        float dist = 0;

        movementPlane.Raycast(mouseRay, out dist);

        Vector3 movementVector = (mouseRay.GetPoint(dist) - movementPlane.ClosestPointOnPlane(grabbedObject.transform.position));

        Vector3 closePoint = transform.position;

        if(!movementVector.Equals(Vector3.zero) && grabbedCollider.Raycast(new Ray(grabbedCollider.transform.position + movementVector.normalized*99, -movementVector), out hit, Mathf.Infinity))
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
                movementVector /= dragSpeed;
                movementVector /= Time.fixedDeltaTime;
            }
            if(trueDist <= 0.1f)
            {
                movementVector *= 0;
            }
        }

        grabbedObject.gameObject.layer = trueLayer;

        grabbedObject.AddForce(dragSpeed * movementVector, ForceMode.VelocityChange);
    }

    public void setCurrentLaser(Laser l)
    {
        currentLaser = l;
    }
}
