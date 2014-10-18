using UnityEngine;
using System.Collections;

public class BuyMode : MonoBehaviour {
	public LayerMask Grid;


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
		if(Physics.Raycast(ray, out hit, 100, Grid) && Input.GetMouseButtonDown(0))
		{
			//Debug.Log("TIME TO BUY");
			//BUY THINGS.
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = hit.collider.gameObject.transform.position;

		}
		else if(Physics.Raycast(ray,out hit, 100, Grid) && !Input.GetMouseButtonDown(0))
		{
			//Debug.Log("HOVER");
			//hover
		}
		else
		{
			//Debug.Log("DIDN'T HIT A GRID");
			//didn't hit anything
		}
	}
}
