using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public static Text _nameText;
    public static Text _dialogueText;
    public static Animator _animator;

    private static Queue<string> sentences;
    public static bool isTalking;
    
    // Use this for initialization
	void Start () {
        sentences = new Queue<string>();
        _nameText = nameText;
        _dialogueText = dialogueText;
        _animator = animator;

	}

    public void StartDialogue (Conversation conversation)
    {
        animator.SetBool("IsOpen", true);

        Debug.Log("Starting conversation with " + conversation.name);

        nameText.text = conversation.name;

        isTalking = true;

        sentences.Clear();

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
        _animator.SetBool("IsOpen", false);
        isTalking = false;
        print("End of conversation.");
    }
}
