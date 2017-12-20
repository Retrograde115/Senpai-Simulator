using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandler : MonoBehaviour {

    private Queue<string> sentences;
    
    // Use this for initialization
	void Start () {
        sentences = new Queue<string>();
        
	}

    public void StartDialogue (Conversation conversation)
    {
        Debug.Log("Starting conversation with " + conversation.name);
    }
}
