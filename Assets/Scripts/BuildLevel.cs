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
		//FindPathStartToEnd (StartGrid,EndGrid,TheLevel,ref ThePath);
		//BuyMode.Dijkstra (StartGrid, EndGrid, TheLevel, ref ThePath);
		//Debug.Log (ThePath.Count);
		BuyMode.StartGrid = StartGrid;
		BuyMode.EndGrid = EndGrid;
		BuyMode.TheLevel = TheLevel;
		Camera.main.GetComponent<BuyMode> ().Dijkstra (StartGrid, EndGrid, TheLevel,ref ThePath);
		GameObject t = (GameObject)Instantiate (testEnemy, StartGrid.transform.position, Quaternion.identity);
		t.GetComponent<testEnemyPath> ().MyPath = ThePath;
		t.GetComponent<testEnemyPath> ().StartGrid = StartGrid;
		t.GetComponent<testEnemyPath> ().EndGrid = EndGrid;
		t.GetComponent<testEnemyPath>().TheLevel = TheLevel;
		//StartCoroutine (Camera.main.GetComponent<BuyMode>().Dijkstra (StartGrid, EndGrid, TheLevel, Camera.main.GetComponent<BuyMode>().SetPath));
		Debug.Log (Camera.main.GetComponent<BuyMode> ().ThePath.Count + "*******");
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
	
}
