using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.IO;

//FINITE LEVEL DONE
//FOR THE FUTURE INFINITE LEVEL....
public class BuildLevel : MonoBehaviour {
	public GameObject testEnemy; //just to test the path.

	public GameObject EnemyNormal;
	public GameObject EnemySlow;
	public GameObject EnemyFast;
	public static int NumEnemies;
	private bool Spawning;
	
	public GameObject Grid;
	public Transform StartPoint;

	public enum SpawnMode
	{
		ENDLESS,
		NOTENDLESS
	}
	public SpawnMode GameMode;

	private string LevelPath = "Levels/";
	private string PatternPath = "Patterns/";
	private GameObject GridParent;
	private GameObject StartGrid;
	private GameObject EndGrid;
	private GameObject[][] TheLevel;
	public static List<Transform> ThePath;

	private float GridNext;
	// Use this for initialization
	void Start () 
	{
		Spawning = false;
		NumEnemies = 0;
		GridParent = new GameObject ("GridParent");
		GridNext = Grid.GetComponent<BoxCollider> ().size.x;
		GridParent.transform.position = Vector3.zero;
		ReadLevel (LevelPath + SingletonManager.Levels[SingletonManager.CurLevelIndex]);
		ConnectLevel ();
		ThePath = new List<Transform> ();
		BuyMode.StartGrid = StartGrid;
		BuyMode.EndGrid = EndGrid;
		BuyMode.TheLevel = TheLevel;
		Camera.main.GetComponent<BuyMode> ().Dijkstra (StartGrid, EndGrid, TheLevel,ref ThePath);
		if(GameMode == SpawnMode.NOTENDLESS)
			StartCoroutine (SpawnLevel ("PatternNewFormat"));
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (NumEnemies);
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

	/*time/number between spawns
	  -time between individuals
	  -number of individuals

      -time between types
      -time between waves
      format:delimiter is '/'
      type(a,b,c,...,z)/time betweeen indivdual(float)/number of individuals (int)/time between types (float)
	  ex:
		a/2.0/5/10.0f/b/3.0/3/5.0/c/1.0f/5/0.0/15.0f
	*/
	bool SpawnPatterns(string filename, ref string [] pattern)
	{
		try
		{
			TextAsset readtext = Resources.Load(filename) as TextAsset;
			pattern = readtext.text.Split("\n"[0]);
			return true;
		}
		catch(Exception e)
		{
			print(e.Message);
			return false;
		}
	}

	IEnumerator SpawnLevel(string filename)
	{
		string [] lines = new string[0];
		char [] delim = {'/'};
		SpawnPatterns(PatternPath+filename, ref lines);
		for(int i = 0; i < lines.Length; i++)
		{
			//Debug.Log(lines[i]);
			string [] patterns = lines[i].Split(delim);
			yield return StartCoroutine(SpawnWaves(patterns));
		}
		Debug.Log ("LEVEL FINISHED");
	}

	IEnumerator SpawnWaves(string [] patterns)
	{
		Spawning = true;
		float timeBetweenWaves = 0.0f;
		float.TryParse (patterns [patterns.Length - 1], out timeBetweenWaves);
		float timeBetweenTypes = 0.0f;
		int numberToSpawn = 0;
		float timeBetweenIndividuals = 0.0f;
		GameObject toSpawn;
		for(int i = 0; i < patterns.Length; i+=4)
		{
			if(patterns[i].Length > 1)
				break;
			float.TryParse(patterns[i+1], out timeBetweenIndividuals);
			int.TryParse(patterns[i+2], out numberToSpawn);
			float.TryParse(patterns[i+3], out timeBetweenTypes);
			switch(patterns[i])
			{
			case "a":
				toSpawn = EnemyNormal;
				break;
			case "b":
				toSpawn = EnemyFast;
				break;
			case "c":
				toSpawn = EnemySlow;
				break;
			default:
				toSpawn = EnemyNormal;
				break;
			}
			StartCoroutine(SpawnIndividuals(toSpawn,numberToSpawn,timeBetweenIndividuals));
			yield return new WaitForSeconds(timeBetweenTypes);
		}
		Spawning = false;
		while(NumEnemies > 0 && Spawning == false)
		{
			yield return 0;
		}
		yield return new WaitForSeconds (timeBetweenWaves);
	}


	IEnumerator SpawnTypes(string patt, float timeBetweenTypes, float timeBetweenWaves)
	{
		Spawning = true;
		for(int i = 0; i < patt.Length; i++)
		{
			switch(patt[i])
			{
			case '1':
				StartCoroutine(SpawnIndividuals(EnemyNormal,3,2.0f));
				break;

			case '2':
				StartCoroutine(SpawnIndividuals(EnemyFast,2,2.0f));
				break;
			default:
				break;
			}
			yield return new WaitForSeconds(timeBetweenTypes);
		}
		Spawning = false;
		while(NumEnemies > 0 && Spawning == false)
		{
			yield return 0;
		}
		//Debug.Log (NumEnemies);
		yield return new WaitForSeconds (timeBetweenWaves);
	}

	IEnumerator SpawnIndividuals(GameObject enemyType,int number, float timeBetweenIndividuals)
	{
		for(int i = 0; i < number; i++)
		{
			GameObject enem = (GameObject)Instantiate(enemyType,StartGrid.transform.position,Quaternion.identity);
			enem.GetComponent<testEnemyPath> ().MyPath = ThePath;
			enem.GetComponent<testEnemyPath> ().StartGrid = StartGrid;
			enem.GetComponent<testEnemyPath> ().EndGrid = EndGrid;
			enem.GetComponent<testEnemyPath>().TheLevel = TheLevel;
			NumEnemies++;
			//Debug.Log(NumEnemies);
			yield return new WaitForSeconds(timeBetweenIndividuals);
		}
	}
}
