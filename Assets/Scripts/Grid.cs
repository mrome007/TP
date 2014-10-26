using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public GameObject whatsInside;
	public bool isAvailable = true;
	public GameObject[] nextTo;
	public bool hasBeenVisited = false;
	public GameObject previous;
	public int distance = 10000;
}
