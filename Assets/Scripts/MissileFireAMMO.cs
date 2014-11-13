using UnityEngine;
using System.Collections;

public class MissileFireAMMO : TowerWeaponAMMO
{
	public Transform TheTarget;
	public float AmmoSpeed;
	public float TurnSpeed;
	public float LaunchDuration;
	public float SlowDown;
	private Transform MissileRotation;
	float OrigSlow;
	// Use this for initialization
	void Start () 
	{
		OrigSlow = 0.3f;
		MissileRotation = transform;
		StartCoroutine ("FireMissileShot");
	}
	
	IEnumerator FireMissileShot()
	{
		yield return StartCoroutine("LaunchMissile");
		if(TheTarget)
		{
			while((TheTarget.transform.position - transform.position).magnitude > 4.0f)
			{
				MissileRotation.LookAt(TheTarget);
				transform.rotation = Quaternion.Lerp(transform.rotation,MissileRotation.rotation,
				                                     Time.deltaTime * TurnSpeed);
				transform.Translate(Vector3.forward * Time.deltaTime * AmmoSpeed);
				SlowDown -= Time.deltaTime;
				if(SlowDown <= 0)
				{
					AmmoSpeed += 20.0f;
					OrigSlow -= 0.1f;
					OrigSlow = Mathf.Clamp(OrigSlow,0.1f,0.5f);
					SlowDown = OrigSlow;
				}
				yield return 0;
			}
			Destroy (gameObject);
		}
	}

	IEnumerator LaunchMissile()
	{
		while(LaunchDuration > 0)
		{
			transform.Translate(Vector3.forward * Time.deltaTime * AmmoSpeed);
			LaunchDuration -= Time.deltaTime;
			SlowDown -= Time.deltaTime;
			if(SlowDown <= 0)
			{
				AmmoSpeed -= 10.0f;
				SlowDown = 0.2f;
			}
			yield return 0;
		}
		SlowDown = OrigSlow;
		if(TheTarget)
			transform.LookAt (TheTarget);
	}
}
