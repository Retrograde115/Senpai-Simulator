using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation {

    public string name;

    [TextArea(3, 10)]
    public List<string> sentences = new List<string>();
    private string[] assetLines;

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
