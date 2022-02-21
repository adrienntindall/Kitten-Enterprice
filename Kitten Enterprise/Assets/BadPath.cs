using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadPath : MonoBehaviour
{
    //This is very bad code writen in a pinch. Please do not use as inspiration!

    public List<Transform> waypoints;
    public int Speed;
    private int current = 0;

    public Transform target;
    
    private bool hasStarted = false;

    public void StartMove()
    {
        hasStarted = true;
    }

    void Update()
    {
        //Debug.Log(hasStarted + " " + current + " < " + waypoints.Count );
        if(hasStarted && current < waypoints.Count)
        {
            if(Vector3.Distance(target.position, waypoints[current].position) < 0.05f)
            {
                current++;
            }
            else
            {
                target.position = Vector3.Lerp(target.position, waypoints[current].position, Time.deltaTime * Speed);
            }
        }
    }
}
