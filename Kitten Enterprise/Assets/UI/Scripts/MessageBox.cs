using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageBox : MonoBehaviour
{
    public bool HasFinished = false;
    private static float charDisplayInterval = 0.02f;
    private static float startDelay = 0.1f;
    private static float endDelay = 0.5f;

    private Coroutine displayCoroutine;
    [SerializeField] private TMP_Text MainText;
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text ConfirmText;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private Button CancelButton;

    [SerializeField] private Message message;

    public void SetMainText(string text)
    {
        MainText.text = text;
    }

    public void SetConfirmText(string text)
    {
        ConfirmText.text = text;
    }

    public void SetMessage(Message msg)
    {
        message = msg;

        if(message.type == MessageType.CharacterDialogue)
        {
            NameText.text = message.TalkerName;
            NameText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            NameText.transform.parent.gameObject.SetActive(false);
        }
            
        
        StartDisplayingText();
    }

    public void EndCurrent()
    {
        StopCoroutine(displayCoroutine);

        MainText.text = message.DialogueText;
        HasFinished = true;
    }

    public void StartDisplayingText()
    {
        displayCoroutine = StartCoroutine(StartShowingCharacters());
    }

    private IEnumerator StartShowingCharacters()
    {
        MainText.text = "";
        HasFinished = false;

        yield return new WaitForSeconds(startDelay);

        var len = message.TextLenght();
        for(int i = 0; i < len; i++)
        {
            MainText.text += message.GetTextChar(i);
            yield return new WaitForSeconds(charDisplayInterval);
        }

        yield return new WaitForSeconds(endDelay);

        HasFinished = true;
    }
}
