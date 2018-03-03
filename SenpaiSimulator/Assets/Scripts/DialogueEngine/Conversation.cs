using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this whole class is meant to be used by other classes, do not tinker
[System.Serializable]
public class Conversation {

    public string name; //this is currently set in the inspector, will be read from TextAsset later

    public List<string> sentences = new List<string>();
    private string[] assetLines;

    //reads defined text lines from designated TextAsset
    public List<string> ReadFromTextAsset(TextAsset textAsset, int startLine, int endLine)
    {
        assetLines = textAsset.text.Split('\n');

        for (int i = startLine; i <= endLine; i++)
        {
            sentences.Add(assetLines[i]);
        }

        return sentences;
    }

}
