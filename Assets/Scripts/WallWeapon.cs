using UnityEngine;
using System.Collections;

public class WallWeapon : TowerWeapons
{
	public int Duration;
	public GameObject GridItsOn;
	private int WavePlaced;
	override public void Fire()
	{
		Debug.Log ("FIRE");
	}
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
