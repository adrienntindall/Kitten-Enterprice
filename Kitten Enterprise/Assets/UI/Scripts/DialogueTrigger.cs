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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
