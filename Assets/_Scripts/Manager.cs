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
		if(GUI.Button(new Rect(600f,300f,200f,50f),"CHANGE MODE"))
		{
			SM.ChangeMode();
			Debug.Log(SingletonForMode.Play_Mode);
		}

	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
