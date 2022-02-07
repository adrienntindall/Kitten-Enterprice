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
    private Vector3 grabDelta;
    private Plane movementPlane;


    private void Update()
    {
        mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        mouseDown = Mouse.current.leftButton.IsPressed();
    }

    private void FixedUpdate()
    {
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
            grabbedObject.constraints = RigidbodyConstraints.FreezeRotation;
            movementPlane = new Plane(Camera.main.transform.forward, grabbedObject.transform.position);
        }
        if (grabbedObject == null) return;

        grabbedObject.AddForce(-grabbedObject.velocity, ForceMode.VelocityChange);

        float dist = 0;

        movementPlane.Raycast(mouseRay, out dist);

        grabbedObject.AddForce(dragSpeed * (mouseRay.GetPoint(dist) - movementPlane.ClosestPointOnPlane(grabbedObject.transform.position)), ForceMode.VelocityChange);


        Debug.Log(mouseRay.GetPoint(dist));
        Debug.Log(movementPlane.ClosestPointOnPlane(grabbedObject.transform.position));
        Debug.Log("/n");
    }

}
