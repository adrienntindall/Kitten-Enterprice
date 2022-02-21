using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField]private List<Message> messages;

    public int GetMessageCount()
    {
        return messages.Count;
    }

    public Message GetMessage(int index)
    {        
        return messages[index];
    }
}
