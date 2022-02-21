using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BigModeSwapper : MonoBehaviour
{
    public CinemachineVirtualCamera targetCamera;
    public GameObject catBig;

    public GameObject[] disableList;

    private Animator catBigAnimator;

    private void Awake()
    {
        catBigAnimator = catBig.GetComponent<Animator>();
        catBig.SetActive(false);
    }

    public IEnumerator exitBigMode()
    {

        catBigAnimator.SetTrigger("Shrink");

        yield return new WaitForSeconds(catBigAnimator.GetCurrentAnimatorStateInfo(0).length);

        foreach (GameObject obj in disableList)
        {
            obj.SetActive(true);
        }

        catBig.SetActive(false);

        targetCamera.Priority = 1;
        PlayerController.isBig = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator enterBigMode()
    {
        PlayerController.playerController.movementSound.Stop();
        targetCamera.Priority = 11;
        PlayerController.playerController.startDialogue();
        catBig.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        catBigAnimator.SetTrigger("GrowBig");

        yield return new WaitForSeconds(catBigAnimator.GetCurrentAnimatorStateInfo(0).length);

        foreach(GameObject obj in disableList)
        {
            obj.SetActive(false);
        }

        PlayerController.isBig = true;
        PlayerController.playerController.endDialogue();
        Cursor.lockState = CursorLockMode.None;
        PlayerController.currentSwapper = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(enterBigMode());
        }
    }
}
