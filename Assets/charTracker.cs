using UnityEngine;
using System.Collections;

public class charTracker : MonoBehaviour {

	static UIPanel GUIProfilepanel;
	static UIPanel GUIMainpanel;
	static UILabel GUIname;
	static UILabel GUIage;
	static UILabel GUIrelationship;
	static UILabel GUIFlavour;
	static GameObject chart;

	public int yOffset = 0;

	public personSim trackedSim { get; set;}

	public void updatePos(GameObject[] bars, int currentYear)
	{
		if(trackedSim.getAge(currentYear) < bars.Length && trackedSim.isAlive)
		{
			GameObject thisBar = bars[trackedSim.getAge(currentYear)];
			transform.localPosition = thisBar.transform.localPosition + new Vector3(25,thisBar.transform.localScale.y + yOffset,0);
		}
	}


	public void OnClick()
	{
		if(GUIProfilepanel == null)
		{
			GUIProfilepanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("charViewer").gameObject.GetComponent<UIPanel>();
			GUIMainpanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("inGame").gameObject.GetComponent<UIPanel>();
			GUIname = GUIProfilepanel.transform.FindChild("name").GetComponent<UILabel> ();
			GUIage = GUIProfilepanel.transform.FindChild("age").GetChild(0).GetComponent<UILabel>();
			GUIrelationship = GUIProfilepanel.transform.FindChild("relationship").GetChild(0).GetComponent<UILabel>();
			GUIFlavour = GUIProfilepanel.transform.FindChild("flavourText").GetComponent<UILabel> ();
			chart = GUIMainpanel.transform.FindChild("Chart").gameObject;
		}

		GUIProfilepanel.enabled = true;
		GUIMainpanel.enabled = false;
		GUIname.text = trackedSim.getName ();
		GUIage.text = trackedSim.getAge ().ToString();
		GUIFlavour.text = trackedSim.getThoughts ();

		if(trackedSim.partner == null || (!trackedSim.partner.isAlive && !trackedSim.isMarried))
			GUIrelationship.text = "Single";
		else if(!trackedSim.partner.isAlive && trackedSim.isMarried)
		{
			if(trackedSim.isMale)
				GUIrelationship.text = "Widower";
			else
				GUIrelationship.text = "Widow";
		}
		else if(trackedSim.isMarried)
		{
			GUIrelationship.text = "Married to " + trackedSim.partner.getName();
		}
		else
		{
			GUIrelationship.text = "In a relationship with " + trackedSim.partner.getName();
		}
		chart.SetActive (false);
	}
}
