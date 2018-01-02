using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : PlayerState{

    //several of the methods in this class need to be static, but the Unity inspector needs access to their variables, so they all have equivalent duplicate public/static variables
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public static Text _nameText;
    public static Text _dialogueText;
    public static Animator _animator;
    public static int dialogueCounter = 0;

    private static Queue<string> sentences;

    //flag meant to be used for game state machine
    public static bool isTalking;

    //initialize sentence queue
    void Start () {
        sentences = new Queue<string>();
	}

    //this method starts the dialogue, using a Conversation class as a constructor
    public void StartDialogue (Conversation conversation)
    {
        PlayerState.state = State.TALKING;
        
        //initializes public/static variables to be equivalent
        _nameText = nameText;
        _dialogueText = dialogueText;
        _animator = animator;

        //brings the UI to the screen
        animator.SetBool("IsOpen", true);

        Debug.Log("Starting conversation with " + conversation.name);

        nameText.text = conversation.name;

        isTalking = true;

        //clears sentence queue just in case there is anything left in there
        sentences.Clear();

        //converts the array of sentences in the Conversation class into a queue of strings
        foreach (string sentence in conversation.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public static void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        _dialogueText.text = sentence;
    }

    public static void EndDialogue()
    {
        PlayerState.state = State.FREE;
        _animator.SetBool("IsOpen", false);
        isTalking = false;
        print("End of conversation.");
        dialogueCounter++;
    }
}
