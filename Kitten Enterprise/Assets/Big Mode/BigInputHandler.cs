using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BigInputHandler : MonoBehaviour
{
    public float dragSpeed;

    public AudioSource laserSFX;

    private bool isRotational = false;

    private Laser currentLaser;
    private Ray mouseRay;
    public static bool mouseDown = false;

    public static Rigidbody grabbedObject = null;
    private Collider grabbedCollider = null;
    private Plane movementPlane;
    private MoveableObject[] allMoveables;

    private Vector3 forwardsVector;
    private Vector3 rightVector;

    private Quaternion r0;
    private Vector3 pivotPoint;

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
                grabbedObject.angularVelocity = Vector3.zero;
                grabbedObject.maxAngularVelocity = 10f;
                foreach (MoveableObject obj in allMoveables)
                {
                    obj.objectRigidbody.isKinematic = false;
                }
                grabbedObject = null;
                
            }
            laserSFX.Stop();
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
            movementPlane = grabbedObject.GetComponent<MoveableObject>().getMovementPlane();
            isRotational = grabbedObject.GetComponent<MoveableObject>().isRotational;
            if (isRotational)
            {
                Vector3 v0 = (movementPlane.ClosestPointOnPlane(hit.point) - movementPlane.ClosestPointOnPlane(grabbedObject.transform.position));
                r0 = Quaternion.RotateTowards(grabbedObject.rotation, Quaternion.LookRotation(v0, movementPlane.normal), Mathf.Infinity);
                grabbedObject.constraints = getRotationalConstraints();
                movementPlane = new Plane(movementPlane.normal, movementPlane.distance);
                pivotPoint = movementPlane.ClosestPointOnPlane(grabbedObject.transform.position);
            }
            else grabbedObject.constraints = RigidbodyConstraints.FreezeRotation;
            grabbedObject.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            forwardsVector = Quaternion.FromToRotation(Vector3.up, movementPlane.normal) * Camera.main.transform.forward;
            forwardsVector = movementPlane.ClosestPointOnPlane(forwardsVector).normalized;
            rightVector = -Vector3.Cross(forwardsVector, movementPlane.normal);
        }
        if (grabbedObject == null) return;

        
        if (!laserSFX.isPlaying) laserSFX.Play();

        grabbedObject.AddForce(-grabbedObject.velocity, ForceMode.VelocityChange);
        grabbedObject.angularVelocity = Vector3.zero;

        float dist = 0;

        movementPlane.Raycast(mouseRay, out dist);

        if (isRotational)
        {
            Vector3 movementVector = (mouseRay.GetPoint(dist) - movementPlane.ClosestPointOnPlane(pivotPoint));
            float angle = Vector3.Angle(grabbedObject.rotation*rightVector, movementVector) * Mathf.Deg2Rad;


            angle *= Mathf.Sign(Vector3.Dot(Vector3.Cross(grabbedObject.rotation*rightVector, movementVector), movementPlane.normal));

            Vector3 angularDisplacement = movementPlane.normal * angle;

            grabbedObject.angularVelocity = angularDisplacement / Time.fixedDeltaTime;
        }
        else
        {
            Vector3 movementVector = (mouseRay.GetPoint(dist) - movementPlane.ClosestPointOnPlane(grabbedObject.transform.position));
            Vector3 closePoint = transform.position;

            if (!movementVector.Equals(Vector3.zero) && grabbedCollider.Raycast(new Ray(grabbedCollider.transform.position + movementVector.normalized * 99, -movementVector), out hit, Mathf.Infinity))
            {
                closePoint = hit.point;
            }

            float colliderRadius = (grabbedCollider.transform.position - closePoint).magnitude;

            int trueLayer = grabbedObject.gameObject.layer;

            grabbedObject.gameObject.layer = 2; //Ignore Raycast Layer

            if (Physics.Raycast(new Ray(grabbedCollider.transform.position, movementVector), out hit, colliderRadius + (dragSpeed + 2) * Time.fixedDeltaTime))
            {
                float trueDist = hit.distance - colliderRadius;
                if (dragSpeed * movementVector.magnitude >= trueDist)
                {
                    movementVector *= trueDist / movementVector.magnitude;
                    movementVector /= dragSpeed;
                    movementVector /= Time.fixedDeltaTime;
                }
                if (trueDist <= 0.1f)
                {
                    movementVector *= 0;
                }
            }

            grabbedObject.gameObject.layer = trueLayer;


            grabbedObject.AddForce(dragSpeed * movementVector, ForceMode.VelocityChange);
        }
    }

    public void setCurrentLaser(Laser l)
    {
        currentLaser = l;
    }

    private RigidbodyConstraints getRotationalConstraints()
    {
        RigidbodyConstraints constraints = RigidbodyConstraints.FreezePosition;
        if(movementPlane.normal.x != 0)
        {
            constraints = constraints | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else if(movementPlane.normal.y != 0)
        {
            constraints = constraints | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else if(movementPlane.normal.z != 0)
        {
            constraints = constraints | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        }
        return constraints;
    }
}
