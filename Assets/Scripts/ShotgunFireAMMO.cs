using UnityEngine;
using System.Collections;

//work on different damage reduction rate at different distances.
public class ShotgunFireAMMO : TowerWeaponAMMO {

	public float AmmoSpeed;
	public float DistanceTilDestroy;
	public float DamageReduction;
	public float[] DistancesForDamageReductions;
	private float DamageMultiplier;
	float Distance;
	float OrigDam;
	int IndexForReduction;
	// Use this for initialization
	void Start () 
	{
		IndexForReduction = 0;
		OrigDam = Damage;
		Distance = 0.0f;
		DamageMultiplier = 1.0f;
		StartCoroutine ("FireShotgunShot");
	}
	


	IEnumerator FireShotgunShot()
	{
		while(Distance <= DistanceTilDestroy)
		{
			Distance += Time.deltaTime * AmmoSpeed;
			transform.Translate(Vector3.forward * Time.deltaTime * AmmoSpeed);
			if(IndexForReduction < DistancesForDamageReductions.Length && Distance > DistancesForDamageReductions[IndexForReduction])
			{
				if((DamageMultiplier - DamageReduction) >= 0)
					DamageMultiplier -= DamageReduction;
				else
					DamageMultiplier = 0.1f;
				IndexForReduction++;
				Debug.Log(DamageMultiplier);
				Damage = OrigDam * DamageMultiplier;
				Debug.Log(Damage);
			}

			yield return 0;
		}
		Destroy (gameObject);
	}
}
