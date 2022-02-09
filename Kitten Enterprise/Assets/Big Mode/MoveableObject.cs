using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Vector3 normal = new Vector3(0, 1, 0);

    private Plane movementPlane;

    private void Awake()
    {
        movementPlane = new Plane(normal, transform.position);
    }

    public Plane getMovementPlane()
    {
        return movementPlane;
    }

}
