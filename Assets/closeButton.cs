using UnityEngine;
using System.Collections;

public class closeButton : MonoBehaviour {

	public GameObject infoPanel;
	public GameObject mainPanel;
	public GameObject chart;
	public GameObject options;

	
	public void OnClick()
	{
		infoPanel.GetComponent<UIPanel> ().enabled = false;
		mainPanel.GetComponent<UIPanel> ().enabled = true;
		chart.SetActive(true);

		if(options.activeInHierarchy)
		{
			UIInput[] inputs = options.GetComponentsInChildren<UIInput>();

			foreach(UIInput input in inputs)
			{
				input.Submit();
			}
		}
	}
}
