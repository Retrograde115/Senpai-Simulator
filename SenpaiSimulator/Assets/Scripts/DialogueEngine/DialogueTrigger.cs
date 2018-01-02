using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : PlayerState {

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
        if (PlayerCursor.targetObject == me && state == State.FREE && Input.GetKeyUp(KeyCode.E))
        {
            TriggerDialogue();
        }

        if (state == State.TALKING && Input.GetKeyUp(KeyCode.Space))
        {
            DialogueHandler.DisplayNextSentence();
        }

        if (state == State.TALKING == true && Input.GetKeyUp(KeyCode.Escape))
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
