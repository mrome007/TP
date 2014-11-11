using UnityEngine;
using System.Collections;

public class SingleFireAMMO : TowerWeaponAMMO {

	public float AmmoSpeed;
	public float DistanceTilDestroy;

	float Distance;
	// Use this for initialization
	void Start () 
	{
		Distance = 0.0f;
		StartCoroutine ("FireSingleShot");
	}

	IEnumerator FireSingleShot()
	{
		while(Distance <= DistanceTilDestroy)
		{
			Distance += Time.deltaTime * AmmoSpeed;
			transform.Translate(Vector3.forward * Time.deltaTime * AmmoSpeed);
			yield return 0;
		}
		Destroy (gameObject);
	}

}
