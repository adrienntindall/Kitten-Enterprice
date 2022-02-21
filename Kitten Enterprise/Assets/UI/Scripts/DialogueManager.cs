using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instacne;
    [SerializeField] private MessageBox box;
    [SerializeField] private Dialogue dialogue;
    private int currentMessage = 0;

    public bool isFinalDialogue = false;

    private CinemachineVirtualCamera currentCam;
    private void Awake() 
    {
        if(!Instacne)
        {
            Instacne = this;
        }    
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        if(dialogue != null)
            StartDialogue();
    }

    private void StartDialogue()
    {
        //disable player input;
        PlayerController.playerController.startDialogue();
        
        box.gameObject.SetActive(true);
        box.SetMessage(dialogue.GetMessage(currentMessage++));
    }

    private void EndDialogue()
    {
        box.gameObject.SetActive(false);

        PlayerController.playerController.endDialogue();

        if(currentCam != null)
        {
            currentCam.Priority = 1;
            currentCam = null;
        }

        if(isFinalDialogue)
        {
            SceneManager.LoadScene("Credits");
        }
    }
    private void Update() 
    {
        if(Keyboard.current[Key.Space].wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(dialogue.GetMessageCount() <= currentMessage)
            {
                if(!box.HasFinished)
                {
                    box.EndCurrent();
                }
                else 
                {
                    EndDialogue();
                }
            }
            else if(box.HasFinished)
            {
                box.SetMessage(dialogue.GetMessage(currentMessage++));
            }
            else 
            {
                box.EndCurrent();
            }
        }   
    }

    public void TriggerDialogue(Dialogue dial, CinemachineVirtualCamera cam = null)
    {
        currentMessage = 0;
        dialogue = dial;

        if(cam != null)
            TriggerCamera(cam);

        StartDialogue();
    }

    public void TriggerCamera(CinemachineVirtualCamera cam)
    {
        currentCam = cam;
        currentCam.Priority = 20;
    }
}
