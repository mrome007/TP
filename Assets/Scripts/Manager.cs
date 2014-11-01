using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
	//instance of the singleton.
	private SingletonManager SM;
	// Use this for initialization
	void Awake () 
	{
		SM = SingletonManager.GetInstance ();
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(0.79125f * Screen.width,0f,200f,200f),"CHANGE MODE"))
		{
			SM.ChangeMode();
			//Debug.Log(SingletonManager.Play_Mode);
		}

	}
}
