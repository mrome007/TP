using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
	//instance of the singleton.
	private SingletonForMode SM;
	// Use this for initialization
	void Start () 
	{
		SM = SingletonForMode.GetInstance ();
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(0.79125f * Screen.width,0f,200f,200f),"CHANGE MODE"))
		{
			SM.ChangeMode();
			Debug.Log(SingletonForMode.Play_Mode);
		}

	}
}
