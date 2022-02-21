using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    CharacterDialogue,
    Narration 
}

[System.Serializable]
public struct Message
{
    public string TalkerName;
    [TextArea]
    public string DialogueText;
    public MessageType type;

    public int TextLenght()
    {
        return DialogueText.Length;
    }

    public char GetTextChar(int index)
    {
        if(index > TextLenght())
            return '\r';
        return DialogueText[index];
    }
}
