using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Conversation boxChanScript;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueHandler>().StartDialogue(boxChanScript);
    }

    
}
