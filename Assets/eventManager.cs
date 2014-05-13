using UnityEngine;
using System.Collections;

public class eventManager : MonoBehaviour {

	Queue events = new Queue();
	float timeLastLaunched = 0;
	public float timeInBetween = 0.5f;

	public  void addEvent(string message)
	{
		events.Enqueue (message);
	}

	void Update()
	{
		if(Time.time - timeLastLaunched >= timeInBetween && events.Count > 0)
		{
			(Instantiate (Resources.Load ("event", typeof(GameObject))) as GameObject).GetComponent<UILabel> ().text = (string)events.Dequeue();
			timeLastLaunched = Time.time;
		}
	}

}
