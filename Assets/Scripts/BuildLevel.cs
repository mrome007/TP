using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.IO;

public class BuildLevel : MonoBehaviour {
	public GameObject testEnemy; //just to test the path.
	public GameObject Grid;
	public Transform StartPoint;

	private string LevelPath = "Levels/";
	private GameObject GridParent;
	private GameObject StartGrid;
	private GameObject EndGrid;
	private GameObject[][] TheLevel;
	private List<Transform> ThePath;

	private float GridNext;
	// Use this for initialization
	void Start () 
	{
		GridParent = new GameObject ("GridParent");
		GridNext = Grid.GetComponent<BoxCollider> ().size.x;
		GridParent.transform.position = Vector3.zero;
		ReadLevel (LevelPath + SingletonManager.Levels[SingletonManager.CurLevelIndex]);
		ConnectLevel ();
		ThePath = new List<Transform> ();
		ThePath = FindPathStartToEnd (StartGrid);
		Debug.Log (ThePath.Count);
		GameObject t = (GameObject)Instantiate (testEnemy, StartGrid.transform.position, Quaternion.identity);
		t.GetComponent<testEnemyPath> ().MyPath = ThePath;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	bool ReadLevel(string filename)
	{
		try
		{
			TextAsset readtext = Resources.Load(filename) as TextAsset;
			string [] lines = readtext.text.Split("\n"[0]);
			TheLevel = new GameObject[lines.Length][];
			for(int i = 0; i < lines.Length; i++)
			{
				TheLevel[i] = new GameObject[lines[i].Length];
				Vector3 pos = StartPoint.position;
				for(int j = 0; j < lines[i].Length; j++)
				{
					switch(lines[i][j])
					{
					case '1':
						GameObject Grids = (GameObject)Instantiate(Grid,pos,Quaternion.identity);
						Grids.name = Grids.name + "(" + i + "," + j + ")";
						Grids.transform.parent = GridParent.transform;
						Grids.AddComponent<Grid>();
						Grids.tag = "NotTaken";
						TheLevel[i][j] = Grids;
						//getting the starting spawn point and end point.
						if(i == lines.Length/2 && j == 0)
							EndGrid = Grids;
						else if(i == lines.Length/2 && j == lines[i].Length-1)
							StartGrid = Grids;
						break;
					default:
						break;
					}
					pos += new Vector3(GridNext,0f,0f);
				}
				StartPoint.position -= new Vector3(0f,0f,GridNext);
			}

			return true;
		}
		catch(Exception e)
		{
			print(e.Message);
			return false;
		}
	}

	void ConnectLevel()
	{
		//top left
		Grid corners = TheLevel [0] [0].GetComponent<Grid> ();
		corners.nextTo = new GameObject[2];
		corners.nextTo [0] = TheLevel [0] [1];
		corners.nextTo [1] = TheLevel [1] [0];

		//top right
		corners = TheLevel [0] [TheLevel[0].Length-1].GetComponent<Grid> ();
		corners.nextTo = new GameObject[2];
		corners.nextTo [0] = TheLevel [0] [TheLevel[0].Length-2];
		corners.nextTo [1] = TheLevel [1] [TheLevel[1].Length-1];

		//bottom left
		corners = TheLevel [TheLevel.Length-1] [0].GetComponent<Grid> ();
		corners.nextTo = new GameObject[2];
		corners.nextTo [0] = TheLevel [TheLevel.Length-2] [0];
		corners.nextTo [1] = TheLevel [TheLevel.Length-1] [1];

		//bottom right
		corners = TheLevel [TheLevel.Length-1] [TheLevel[TheLevel.Length-1].Length-1].GetComponent<Grid> ();
		corners.nextTo = new GameObject[2];
		corners.nextTo [0] = TheLevel [TheLevel.Length-1] [TheLevel[TheLevel.Length-1].Length-2];
		corners.nextTo [1] = TheLevel [TheLevel.Length-2] [TheLevel[TheLevel.Length-1].Length-1];

		Grid cur;
		for(int i = 0; i < TheLevel.Length; i++)
		{
			for(int j = 0; j < TheLevel[i].Length; j++)
			{
				if(i > 0 && i < TheLevel.Length-1 && j > 0 && j < TheLevel[i].Length-1)
				{
					cur = TheLevel[i][j].GetComponent<Grid>();
					cur.nextTo = new GameObject[4];
					cur.nextTo[0] = TheLevel[i-1][j];
					cur.nextTo[1] = TheLevel[i+1][j];
					cur.nextTo[2] = TheLevel[i][j-1];
					cur.nextTo[3] = TheLevel[i][j+1];
				}
				else if((i == 0 || i == TheLevel.Length-1) && j > 0 && j < TheLevel[i].Length-1)
				{
					cur = TheLevel[i][j].GetComponent<Grid>();
					cur.nextTo = new GameObject[3];
					cur.nextTo[0] = TheLevel[i][j-1];
					cur.nextTo[1] = TheLevel[i][j+1];
					cur.nextTo[2] = (i == 0) ? TheLevel[i+1][j] : TheLevel[i-1][j];
				}
				else if((j == 0 || j == TheLevel[i].Length-1) && i > 0 && i < TheLevel.Length-1)
				{
					cur = TheLevel[i][j].GetComponent<Grid>();
					cur.nextTo = new GameObject[3];
					cur.nextTo[0] = TheLevel[i-1][j];
					cur.nextTo[1] = TheLevel[i+1][j];
					cur.nextTo[2] = (j == 0) ? TheLevel[i][j+1] : TheLevel[i][j-1];
				}
			}
		}
	}


	public List<Transform> FindPathStartToEnd(GameObject start)
	{
		List<Transform> currPath = new List<Transform> ();
		BinaryHeap bhp = new BinaryHeap (200);
		for(int i = 0; i < TheLevel.Length; i++)
		{
			for(int j = 0; j < TheLevel.Length; j++)
			{
				if(TheLevel[i][j].tag != "Taken")
				{
					Grid cur = TheLevel[i][j].GetComponent<Grid>();
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
				if(curmin.isAvailable)
				{
					Grid nodesToVisit = curmin.nextTo[i].GetComponent<Grid>();
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

		GameObject theend = EndGrid;
		Grid checkEnd;// = theend.GetComponent<Grid> ();
		while(theend.name != start.name)
		{
			currPath.Add(theend.transform);
			checkEnd = theend.GetComponent<Grid>();
			theend = checkEnd.previous;
		}
		currPath.Add (theend.transform);
		return currPath;
	}


}
