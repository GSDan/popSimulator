using UnityEngine;
using System.Collections;

public class eventNotifier : MonoBehaviour {

	public float ttl = 3f;
	public float distPerSec = 0.15f;
	
	float startTime;

	void Start()
	{
		startTime = Time.time;
		TweenAlpha.Begin (gameObject, ttl, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position += new Vector3(0,distPerSec * Time.deltaTime, 0);

		if(Time.time - startTime > ttl)
		{	
			DestroyObject(gameObject);
		}

	}
}
