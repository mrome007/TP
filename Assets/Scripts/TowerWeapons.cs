using UnityEngine;
using System.Collections;

public abstract class TowerWeapons : MonoBehaviour
{
	public GameObject Upgrade; 
	public float Damage;
	public int Cost;
	public float ReloadTime;
	private Transform Target;
	public float FirePauseTime;

	abstract public void Fire();
}
