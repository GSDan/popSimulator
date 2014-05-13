using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class charTrackerManager : MonoBehaviour {

	List<charTracker> chars = new List<charTracker>();
	int numDead = 0;

	int lastBorn = 0;

	public void updateTrackers(GameObject[] bars, int currentYear)
	{
		bool finished = false;

		for(int i = 0; i < chars.Count; i++)
		{
			if(chars[i] != null && chars[i].trackedSim.isAlive )
			{
				chars[i].updatePos(bars, currentYear);
			}
			else if(chars[i] != null)
			{
				DestroyObject(chars[i].gameObject);
				chars.RemoveAt(i);
				i = 0;
			}
		}
		

	}

	public void addTracker(personSim sim)
	{
		charTracker tracker = (Instantiate (Resources.Load ("charIcon", typeof(GameObject))) as GameObject).GetComponent<charTracker> ();

		if(sim.yearBorn - lastBorn < 10)
		{
			if(chars.Count >= 3 && chars[chars.Count-2].trackedSim.yearBorn - sim.yearBorn >= 10)
			{
				tracker.yOffset = chars[chars.Count-2].yOffset;
			}
			else
			{
				tracker.yOffset = chars[chars.Count-1].yOffset + 50;
			}
		}

		tracker.transform.parent = transform;
		tracker.transform.localScale = Vector3.one;
		tracker.trackedSim = sim;
		tracker.transform.GetComponentInChildren<UILabel> ().text = sim.firstName;
		chars.Add(tracker);

		lastBorn = sim.yearBorn;
	}


}
