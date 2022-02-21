using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject targetObject;


    public float panSpeed = 1f;
    public float rotateSpeed = 1f;

    public bool invertYAxis = true;

    private Vector2 mouseDelta;

    private RaycastHit[] hits;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnPanCamera(InputValue input)
    {
        if (!PlayerController.isBig)
        {
            mouseDelta = input.Get<Vector2>();
            if (mouseDelta.magnitude > 50) mouseDelta = Vector2.zero;
            if (invertYAxis) mouseDelta.y *= -1;
        }
    }

    private void Update()
    {
        targetObject.transform.localPosition += new Vector3(0, mouseDelta.y, 0)*panSpeed*Time.deltaTime;
        targetObject.transform.rotation *= Quaternion.AngleAxis(mouseDelta.x * rotateSpeed * Time.deltaTime, new Vector3(0, 1, 0));
    }

}
