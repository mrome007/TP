using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testEnemyPath : MonoBehaviour {
	public List<Transform> MyPath;
	public GameObject StartGrid;
	public GameObject EndGrid;
	public GameObject[][] TheLevel;

	public float Speed;
	private int CurrentIndex;

	public void SetPath(List<Transform> np)
	{
		MyPath = np;
	}
	// Use this for initialization
	void Start () 
	{
		CurrentIndex = MyPath.Count-1;
		Speed = 2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(BuyMode.HasLevelChanged == BuyMode.LevelChange.CHANGED)
		{
			int isitinpath = MyPath.IndexOf(BuyMode.ClickedGrid.gameObject.transform);
			if(CurrentIndex > isitinpath && isitinpath >= 0)
				StartCoroutine("StopGetPath");
		}
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

	IEnumerator StopGetPath()
	{
		Camera.main.GetComponent<BuyMode>().Dijkstra(MyPath[CurrentIndex].gameObject,EndGrid,BuyMode.TheLevel,ref MyPath);
		CurrentIndex = MyPath.Count-1;
		Speed = 0.0f;
		yield return new WaitForSeconds(0.5f);
		Speed = 2.0f;
	}
}
