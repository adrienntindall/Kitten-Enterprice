using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueToTrigger;
    private bool hasTriggerd = false;

    public void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            DialogueManager.Instacne.TriggerDialogue(dialogueToTrigger);
        }
    }
}
