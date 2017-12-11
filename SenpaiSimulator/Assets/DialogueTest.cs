using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour {

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
		
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit, 100))
			{
				if (hit.transform  != null)
				{
					PrintName(hit.transform.gameObject);
				}
			}
		}
	}

	private void PrintName(GameObject testObj)
	{
		Debug.Log(testObj.name);
	}

}
