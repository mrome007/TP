using UnityEngine;
using System.Collections;

public class BuyMode : MonoBehaviour {
	public LayerMask Grid;

	GameObject currentGrid;
	Color Hover = new Color(1f,0f,0f);
	Color Old = new Color (1f, 1f, 1f);
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(SingletonForMode.Play_Mode != SingletonForMode.PlayMode.BUYmode)
			return;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100, Grid))
		{
			if(currentGrid)
				ChangeColor(currentGrid,Old);
			currentGrid = hit.collider.gameObject;
			ChangeColor(currentGrid,Hover);
			if(Input.GetMouseButtonDown(0))
			{
				//BUY OPERATIONS
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
		Mesh curMesh = cur.GetComponent<MeshFilter> ().mesh;
		if(curMesh.colors.Length == 0 || curMesh.colors[0] != col)
		{
			Vector3 [] vertices = curMesh.vertices;
			Color [] colors = new Color[vertices.Length];
			for(int i = 0; i < vertices.Length; i++)
				colors[i] = col;
			curMesh.colors = colors;
		}

	}
}
