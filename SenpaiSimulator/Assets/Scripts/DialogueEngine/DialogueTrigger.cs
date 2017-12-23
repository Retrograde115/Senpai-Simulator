using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Conversation boxChanScript;
    GameObject me;
    
    public void Start()
    {
        me = gameObject;
    }

    public void Update()
    {
        if (PlayerCursor.targetObject == me && Input.GetKeyUp(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueHandler>().StartDialogue(boxChanScript);
    }

    
}
