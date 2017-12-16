using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEngine : MonoBehaviour {

	
	public GameObject boxChan;

	public Text scriptBC;

	public TextAsset textfile;
	public string[] textLines;

	public int currentLine;
	public int endAtLine;


	void Start()
	{
		if (textfile != null)
		{
			textLines = (textfile.text.Split("\n"[0]));
		}

		if (endAtLine == 0)
		{
			endAtLine = textLines.Length - 1;
		}
	}	

	void Update()
	{
		scriptBC.text = textLines[currentLine];

		if (Input.GetKeyDown(KeyCode.Return))
		{
			currentLine += 1;
		}
	}
}
