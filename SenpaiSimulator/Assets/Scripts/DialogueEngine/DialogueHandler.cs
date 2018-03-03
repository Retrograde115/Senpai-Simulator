using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {

    //several of the methods in this class need to be static, but the Unity inspector needs access to their variables, so they all have equivalent duplicate public/static variables
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    private static Queue<string> sentences;

    public static DialogueHandler I { get; private set; }

    private void Awake()
    {
        I = this;
    }

    //initialize sentence queue
    void Start () {
        sentences = new Queue<string>();
    }

    //this method starts the dialogue, using a Conversation class as a constructor
    public void StartDialogue (Conversation conversation, DialogueTrigger dialogueTrigger)
    {
        Player.Instance.state = Player.State.TALKING;
        
        //brings the UI to the screen
        animator.SetBool("IsOpen", true);

        Debug.Log("Starting conversation with " + conversation.name);

        nameText.text = conversation.name;

        //clears sentence queue just in case there is anything left in there
        sentences.Clear();

        //converts the array of sentences in the Conversation class into a queue of strings
        foreach (string sentence in conversation.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(dialogueTrigger);
    }

    public void DisplayNextSentence(DialogueTrigger dialogueTrigger)
    {
        if (sentences.Count == 0)
        {
            EndDialogue(dialogueTrigger);
            dialogueTrigger.dialogueBlock++;
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue(DialogueTrigger dialogueTrigger)
    {
        Player.Instance.state = Player.State.FREE;
        animator.SetBool("IsOpen", false);
        print("End of conversation.");
    }
}
