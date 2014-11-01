using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class BuyMode : MonoBehaviour {
	public LayerMask Grid;
	public GameObject testEnemy;

	public enum LevelChange
	{
		CHANGED,
		NOT_CHANGED
	}
	public static LevelChange HasLevelChanged;
	public static GameObject StartGrid;
	public static GameObject EndGrid;
	public static GameObject[][] TheLevel;
	public static GameObject ClickedGrid;
	public List<Transform> ThePath;

	GameObject currentGrid;
	Color Hover = new Color(1f,0f,0f);
	Color Old = new Color (1f, 1f, 1f);

	// Use this for initialization
	void Start ()
	{
		ThePath = new List<Transform> ();
		HasLevelChanged = LevelChange.NOT_CHANGED;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(SingletonManager.Play_Mode != SingletonManager.PlayMode.BUYmode)
			return;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100, Grid))
		{
			if(currentGrid)
				ChangeColor(currentGrid,Old);
			currentGrid = hit.collider.gameObject;
			//Debug.Log(currentGrid.name);
			ChangeColor(currentGrid,Hover);
			HasLevelChanged = LevelChange.NOT_CHANGED;
			if(Input.GetMouseButtonDown(0))
			{
				//BUY OPERATIONS
				ClickedGrid = hit.collider.gameObject;

				hit.collider.gameObject.tag = "Taken";
				hit.collider.gameObject.GetComponent<Grid>().isAvailable = false;

				if(Dijkstra(StartGrid,EndGrid,TheLevel,ref ThePath))
					HasLevelChanged = LevelChange.CHANGED;
				else
				{
					hit.collider.gameObject.tag = "NotTaken";
					hit.collider.gameObject.GetComponent<Grid>().isAvailable = true;
				}
				ChangeColor(currentGrid,Old);
			}

		}
		else
		{
			if(currentGrid)
				ChangeColor(currentGrid, Old);
			currentGrid = null;
		}
	}

	void ChangeColor(GameObject cur, Color col)
	{
		Transform c = cur.transform.GetChild (0);
		Mesh curMesh = c.gameObject.GetComponent<MeshFilter> ().mesh;
		if(curMesh.colors.Length == 0 || curMesh.colors[0] != col)
		{
			Vector3 [] vertices = curMesh.vertices;
			Color [] colors = new Color[vertices.Length];
			for(int i = 0; i < vertices.Length; i++)
				colors[i] = col;
			curMesh.colors = colors;
		}
	}

	public void SetPath(List<Transform> np)
	{
		ThePath = np;
	}

	public bool Dijkstra(GameObject start, GameObject end, GameObject [][] thelevel, ref List<Transform> tpath)
	{
		//HasLevelChanged = LevelChange.CHANGED;
		List<Transform> currPath = new List<Transform> ();
		BinaryHeap bhp = new BinaryHeap (200);
		for(int i = 0; i < thelevel.Length; i++)
		{
			for(int j = 0; j < thelevel[i].Length; j++)
			{
				if(thelevel[i][j].tag != "Taken")
				{
					Grid cur = thelevel[i][j].GetComponent<Grid>();
					if(cur)
					{
						cur.distance = 10000;
						cur.hasBeenVisited = false;
					}
				}
			}
		}
		
		Grid s = start.GetComponent<Grid> ();
		s.distance = 0;
		s.hasBeenVisited = true;
		bhp.add (start);
		
		while(!bhp.empty())
		{
			GameObject min = bhp.extractMin();
			Grid curmin = min.GetComponent<Grid>();
			curmin.hasBeenVisited = true;
			for(int i = 0; i < curmin.nextTo.Length; i++)
			{
				int currentDist = curmin.distance;
				Grid nodesToVisit = curmin.nextTo[i].GetComponent<Grid>();
				if(nodesToVisit.isAvailable)
				{
					int oldDis = nodesToVisit.distance;
					int newDis = currentDist + 1;
					if(newDis < oldDis)
					{
						nodesToVisit.distance = newDis;
						nodesToVisit.previous = min;
						bhp.add(curmin.nextTo[i]);
					}
				}
			}
		}
		
		GameObject theend = end;
		Grid checkEnd = theend.GetComponent<Grid>();// = theend.GetComponent<Grid> ();
		if (!checkEnd.hasBeenVisited)
			return false;
		while(theend.name != start.name)
		{
			currPath.Add(theend.transform);
			checkEnd = theend.GetComponent<Grid>();
			theend = checkEnd.previous;
		}
		currPath.Add (theend.transform);
		tpath =  currPath;
		return true;
		//t = currPath;
		//HasLevelChanged = LevelChange.NOT_CHANGED;
	}
}
