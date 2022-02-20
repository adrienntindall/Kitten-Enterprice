using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instacne;
    [SerializeField] private MessageBox box;
    [SerializeField] private Dialogue dialogue;
    private int currentMessage = 0;

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
        StartDialogue();
    }

    private void StartDialogue()
    {
        //disable player input;
        
        box.gameObject.SetActive(true);
        box.SetMessage(dialogue.GetMessage(currentMessage++));
    }

    private void EndDialogue()
    {
        box.gameObject.SetActive(false);

        //enable player input;
    }
    private void Update() 
    {
        if(Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            if(dialogue.GetMessageCount() <= currentMessage)
            {
                EndDialogue();
                return;
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

    public void TriggerDialogue(Dialogue dial)
    {
        currentMessage = 0;
        dialogue = dial;

        StartDialogue();
    }
}
