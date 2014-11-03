using UnityEngine;
using System.Collections;

public class ShootMode : MonoBehaviour {
	public GameObject TowerAmmo;
	private Vector3 AmmoSpawn = new Vector3(0f,10.0f,-25f);

	private const float MAX_FIRE_ANGLE = Mathf.PI / 2.0f;
	private float mClick;
	private float mRelease;
	private Vector3 TargetPosition;
	private GameObject TheTowerFire;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(SingletonManager.Play_Mode != SingletonManager.PlayMode.SHOOTmode)
			return;
		if(Input.GetMouseButtonDown(0))
		{
			mClick = Input.mousePosition.y;
			Plane playerPlane = new Plane(Vector3.up, Vector3.zero);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist;
			if(playerPlane.Raycast(ray, out hitdist))
			{
				TargetPosition = ray.GetPoint(hitdist);
				Debug.Log(TargetPosition);
			}

		}
		else if(Input.GetMouseButtonUp(0))
		{
			mRelease = Input.mousePosition.y;

			float maxAngleScreenRatio = Screen.height / 2;

			float deltaY = mRelease - mClick;
			if (deltaY > maxAngleScreenRatio) 
				deltaY = maxAngleScreenRatio; 
			else if (deltaY < -maxAngleScreenRatio)
				deltaY = -maxAngleScreenRatio;

			float fireAngle = deltaY / maxAngleScreenRatio * MAX_FIRE_ANGLE;

			TheTowerFire = (GameObject)Instantiate(TowerAmmo, AmmoSpawn, Quaternion.identity);
			FireTowerAmmo fta = TheTowerFire.GetComponent<FireTowerAmmo>();
			Vector3 dir = TargetPosition - AmmoSpawn;
			if(Mathf.Sin(fireAngle) != 0)
				dir.y -= dir.y * Mathf.Sin(fireAngle);
			fta.dir = dir.normalized;
			fta.mAngle = fireAngle;
		}
	}
}
