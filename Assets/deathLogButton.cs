using UnityEngine;
using System.Collections;

public class deathLogButton : MonoBehaviour {

	GameObject deathCont;
	UIPanel GUIDeathpanel;
	UIPanel GUIMainpanel;
	UILabel GUIDeathList;
	eventManager manager;
	GameObject chart;

	public void OnClick()
	{
		if(GUIDeathpanel == null)
		{
			deathCont = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").transform.FindChild("deathLog").gameObject;


			GUIDeathpanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").GetComponent<UIPanel>();
			GUIMainpanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("inGame").gameObject.GetComponent<UIPanel>();
			GUIDeathList = deathCont.transform.FindChild("deathlist").GetComponent<UILabel> ();
			manager = GameObject.FindGameObjectWithTag ("eventManager").GetComponent<eventManager> ();
			chart = GUIMainpanel.transform.FindChild("Chart").gameObject;
		}

		GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").transform.FindChild("charViewer").gameObject.SetActive(false);
		deathCont.SetActive (true);

		GUIDeathpanel.enabled = true;
		GUIMainpanel.enabled = false;
		GUIDeathList.text = manager.getDeathMessages ();
		chart.SetActive (false);
	}
}
