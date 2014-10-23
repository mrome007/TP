using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Text;
using System.IO;

public class BuildLevel : MonoBehaviour {
	public GameObject Grid;
	public Transform StartPoint;

	private string LevelPath = "Levels/";
	private GameObject GridParent;

	private float GridNext;
	// Use this for initialization
	void Start () 
	{
		GridParent = new GameObject ("GridParent");
		GridNext = Grid.GetComponent<BoxCollider> ().size.x;
		GridParent.transform.position = Vector3.zero;
		ReadLevel (LevelPath + SingletonManager.Levels[SingletonManager.CurLevelIndex]);
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
			for(int i = 0; i < lines.Length; i++)
			{
				Vector3 pos = StartPoint.position;
				for(int j = 0; j < lines[i].Length; j++)
				{
					switch(lines[i][j])
					{
					case '1':
						GameObject Grids = (GameObject)Instantiate(Grid,pos,Quaternion.identity);
						Grids.transform.parent = GridParent.transform;
						break;
					default:
						break;
					}
					pos += new Vector3(GridNext,0f,0f);
				}
				StartPoint.position += new Vector3(0f,0f,GridNext);
			}
			return true;
		}
		catch(Exception e)
		{
			print(e.Message);
			return false;
		}
	}
}
