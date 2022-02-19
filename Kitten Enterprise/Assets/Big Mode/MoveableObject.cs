using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Vector3 normal = new Vector3(0, 1, 0);

    public Rigidbody objectRigidbody;
    public bool isRotational = false;

    private Plane movementPlane;

    private void Awake()
    {
        movementPlane = new Plane(normal, transform.position);
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public Plane getMovementPlane()
    {
        return movementPlane;
    }
}
