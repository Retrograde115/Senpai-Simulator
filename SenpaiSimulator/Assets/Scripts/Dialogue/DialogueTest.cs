using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour {

	Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	void Update()
	{
		
		if (Input.GetMouseButtonDown(0))
		{
		
			RaycastHit hit;
			Ray ray = new Ray();
			ray = cam.ScreenPointToRay(Input.mousePosition);

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
