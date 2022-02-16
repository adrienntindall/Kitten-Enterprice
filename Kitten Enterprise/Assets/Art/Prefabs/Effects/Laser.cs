using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //How to use:
    //Update the final position of the laser by setting SetFinalPosition (see below in its description)
    //This is by calling a script externally
    //The fire point is the origin of the ray
    //Line renderer is the line renderer object (the laser line itself)


    public LineRenderer lineRenderer;
    public Transform firePoint;

    private Vector3 finalPosition;

    void Awake()
    {
        //It disables the laser at start
        DisableLaser();
        //This line is for testing and lets you shoot the laser
        //when you don't update the laser end position externally
        finalPosition = firePoint.position + firePoint.forward * 5;
    }

    void Update()
    {
        //When you start pressing the Fire1 button it starts shooting the laser
        if (BigInputHandler.mouseDown && !lineRenderer.enabled)
        {
            if (BigInputHandler.grabbedObject == null) return;
            EnableLaser();
            SetFinalPosition(BigInputHandler.grabbedObject.transform.position);
        }
        //When you end pressing the Fire1 button it stops shooting the laser
        else if (!BigInputHandler.mouseDown && lineRenderer.enabled)
        {
            DisableLaser();
        }
        else if(BigInputHandler.mouseDown && lineRenderer.enabled)
        {
            UpdateLaser();
            Debug.Log("Updating laser!");
        }
    }

    public void EnableLaser()
    {
        lineRenderer.enabled = true;
    }

    //Updates laser, it assumes you are updating SetFinalPosition every frame
    //though it's okay to not do it when the laser is not enabled
    public void UpdateLaser()
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, finalPosition);
    }
    public void DisableLaser()
    {
        lineRenderer.enabled = false;
    }

    //This should be called every frame by an external script
    //though it's okay to not do it when the laser is not enabled
    public void SetFinalPosition(Vector3 finalPosition)
    {
        if (lineRenderer.enabled == false) return;
        this.finalPosition = finalPosition;
    }

}
