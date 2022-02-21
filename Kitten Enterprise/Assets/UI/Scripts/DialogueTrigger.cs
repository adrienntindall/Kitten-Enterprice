using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogueToTrigger;
    private bool hasTriggerd = false;

    public CinemachineVirtualCamera camera = null;
    public BadPath movemntToTrigger;

    public bool isFinal = false;

    public void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player" && !hasTriggerd)
        {
            DialogueManager.Instacne.TriggerDialogue(dialogueToTrigger, camera);
            hasTriggerd = true;

            if(movemntToTrigger != null)
            {
                movemntToTrigger.StartMove();
            }

            if(isFinal)
            {
                DialogueManager.Instacne.isFinalDialogue = true;
            }
        }
    }
}
