using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour {

    public TextAsset[] textfile;
    public int scriptNum = 0;
    public string[] textLines;
    public int currentLine;
    public int endLine;
    public Text displayText;


    private void Awake()
    {
        
    }

    void Start()
    {
        //TextImport();
        Debug.Log(textfile);
    }
	
	void TextImport()
	{
        textfile = new TextAsset[3];

        if (textfile != null)
        {
            textLines = (textfile[scriptNum].text.Split("\n"[0]));
        }

        if (endLine == 0)
        {
            endLine = textLines.Length - 1;
        }
    }

   /* void Update()
    {
        displayText.text = textLines[currentLine];

        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }
    }*/

}
