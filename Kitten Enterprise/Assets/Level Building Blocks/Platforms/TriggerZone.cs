using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public Rigidbody rb;
    public RigidbodyConstraints[] constraints;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Trigger")
        {
            rb.constraints = RigidbodyConstraints.None;
            foreach(RigidbodyConstraints con in constraints)
            {
                rb.constraints = rb.constraints | con;
            }
            rb.isKinematic = false;
            rb.useGravity = true;

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
