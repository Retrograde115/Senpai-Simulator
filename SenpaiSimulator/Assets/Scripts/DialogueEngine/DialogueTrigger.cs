using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueTrigger : MonoBehaviour {

    //initalized in inspector, this allows you to control which script and which lines of the script are read
    public string characterName;
    public int[] startLine;
    public int[] endLine;
    public TextAsset textAsset;


    public int dialogueBlock; //defines which section of script to read, iterates by one every time a conversation is seen to the end
    ManagedUpdate updater;

    Conversation boxChanScript = new Conversation(); //BoxChanScript is a placeholder name


    //includes this class in the UpdateManager's list
    void Start()
    {
        UpdateManager.AddManagedUpdate(this, updater);
        boxChanScript.name = characterName;
    }

    public void UpdateThis()
    {
        //derives from player state machine, reads player state to control dialogue controls
        switch (Player.Instance.state)
        {
            //checks every frame for whether the player is trying to interact with the gameObject
            case Player.State.FREE:
                if (Player.Instance.targetObject == gameObject && Input.GetKeyUp(KeyCode.E))
                {
                    //this is to make sure dialogueBlock isn't out of range, and defaults it to the last block defined if it is
                    if (dialogueBlock > startLine.Length - 1)
                    {
                        dialogueBlock = startLine.Length - 1;
                    }

                    //here, we use the RFTA method to initialize the conversation, then tell DialogueHandler to take over
                    boxChanScript.sentences.Clear();
                    boxChanScript.sentences = boxChanScript.ReadFromTextAsset(textAsset, startLine[dialogueBlock], endLine[dialogueBlock]);
                    DialogueHandler.I.StartDialogue(boxChanScript, this);
                }
                break;

            //controls for when dialogue is running
            case Player.State.TALKING:
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    DialogueHandler.I.DisplayNextSentence(this);
                }

                if (Input.GetKeyUp(KeyCode.Escape))
                {
                    DialogueHandler.I.EndDialogue(this);
                }
                break;

            default:
                break;
        }
    }
}
