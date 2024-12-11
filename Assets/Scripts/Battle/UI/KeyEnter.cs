using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyEnter : MonoBehaviour
{
	public string inputName;
	private Button buttonMe;
	// Use this for initialization
	void Start()
	{
		buttonMe = GetComponent<Button>();
	}

	void Update()
	{
		if (Input.GetButtonDown(inputName))
		{
			buttonMe.onClick.Invoke();
		}


	}
}
