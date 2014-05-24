using UnityEngine;
using System.Collections;

public class eventManager : MonoBehaviour {

	Queue events = new Queue();
	float timeLastLaunched = 0;
	string[] deaths = new string[10];

	public float timeInBetween = 0.5f;


	public  void addEvent(string message)
	{
		events.Enqueue (message);
	}

	public void addDeath(string message)
	{
		for(int i = deaths.Length -2; i >= 0; i--)
		{
			deaths[i+1] = deaths[i];
		}

		deaths [0] = message;

		addEvent (message);
	}

	public string getDeathMessages()
	{
		string finalString = "";

		for(int i = 0; i < deaths.Length; i++)
		{
			if(deaths[i] != null)
				finalString += deaths[i] + "\n";
		}

		return finalString;
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
