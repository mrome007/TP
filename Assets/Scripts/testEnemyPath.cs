using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testEnemyPath : MonoBehaviour {
	public List<Transform> MyPath;
	public float Speed;
	private int CurrentIndex;
	// Use this for initialization
	void Start () 
	{
		CurrentIndex = MyPath.Count-1;
		Speed = 10.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Vector3.Distance(gameObject.transform.position, MyPath[CurrentIndex].position) <= 1.0f)
		   CurrentIndex--;
		if(CurrentIndex < 0)
		{
			Destroy(gameObject);
			return;
		}
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,MyPath[CurrentIndex].position, 
		                                                    Speed * Time.deltaTime);

	
	}
}
