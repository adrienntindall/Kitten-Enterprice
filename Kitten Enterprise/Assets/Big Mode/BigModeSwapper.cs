using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BigModeSwapper : MonoBehaviour
{
    public CinemachineVirtualCamera targetCamera;

    public void exitBigMode()
    {
        targetCamera.Priority = 1;
        PlayerController.isBig = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void enterBigMode()
    {
        PlayerController.playerController.movementSound.Stop();
        targetCamera.Priority = 11;
        PlayerController.isBig = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerController.currentSwapper = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            enterBigMode();
        }
    }
}
