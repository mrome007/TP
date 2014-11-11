using UnityEngine;
using System.Collections;

//NEED TO Tweak Laser Ammo behavior some more.
public class LaserFireAMMO : TowerWeaponAMMO {
	public GameObject MainLaser;
	public GameObject Charge;
	public float ChargeSpeed;
	public float LaserSpeed;
	public float PauseBeforeFire;
	public float Duration;

	private float Offset = 0.05f;
	private float StartingSize;
	// Use this for initialization
	void Start () 
	{
		StartingSize = Charge.transform.localScale.x;
		StartCoroutine ("FireLaserShot");
	}

	IEnumerator FireLaserShot()
	{
		yield return StartCoroutine ("ChargeShot");
		yield return new WaitForSeconds (PauseBeforeFire);
		yield return StartCoroutine ("LaserShot");
		yield return new WaitForSeconds (Duration);
		Destroy (gameObject);
	}
	
	IEnumerator ChargeShot()
	{
		float startSize = Charge.transform.localScale.x;
		float finalSize = startSize + 20.0f;

		while(startSize < finalSize - Offset)
		{
			Charge.transform.localScale = Vector3.Lerp(Charge.transform.localScale,new Vector3(finalSize,finalSize,finalSize),
			                                    Time.deltaTime * ChargeSpeed/5.0f);
			startSize = Charge.transform.localScale.x;
			yield return 0;
		}
		Debug.Log ("DONE CHARGING");
	}

	IEnumerator LaserShot()
	{
		float startLaser = MainLaser.transform.localScale.z;
		float finalLaser = startLaser + 2000.0f;

		while(startLaser < finalLaser - Offset)
		{

			Charge.transform.localScale = Vector3.Lerp(Charge.transform.localScale,new Vector3(StartingSize+3.0f,StartingSize+3.0f,StartingSize+3.0f),
			                                           Time.deltaTime * ChargeSpeed);
			MainLaser.transform.localScale = Vector3.Lerp(MainLaser.transform.localScale,new Vector3(1.0f,1.0f,finalLaser),
			                                              Time.deltaTime * LaserSpeed);
			startLaser = MainLaser.transform.localScale.z;
			yield return 0;
		}
	}
}
