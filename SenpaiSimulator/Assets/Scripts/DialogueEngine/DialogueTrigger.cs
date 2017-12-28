using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    GameObject me;
    public Conversation boxChanScript = new Conversation();
    public TextAsset textAsset;
    public int startLine;
    public int endLine;

    void Start()
    {
        me = gameObject;
    }

    void Update()
    {
        if (PlayerCursor.targetObject == me && Input.GetKeyUp(KeyCode.E))
        {
            TriggerDialogue();
        }

        if (DialogueHandler.isTalking == true && Input.GetKeyUp(KeyCode.Space))
        {
            DialogueHandler.DisplayNextSentence();
        }

        if (DialogueHandler.isTalking == true && Input.GetKeyUp(KeyCode.Escape))
        {
            DialogueHandler.EndDialogue();
        }
    }

    public void TriggerDialogue()
    {
        boxChanScript.sentences.Clear();

        switch (DialogueHandler.dialogueCounter)
        {
            case 0:
                startLine = 2;
                endLine = 7;
                break;

            default:
                startLine = 0;
                endLine = 0;
                break;
        }

        boxChanScript.sentences = boxChanScript.ReadFromTextAsset(textAsset, startLine, endLine);
        FindObjectOfType<DialogueHandler>().StartDialogue(boxChanScript);
    }

    
}
