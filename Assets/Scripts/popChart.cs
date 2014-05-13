using UnityEngine;
using System.Collections;

public class popChart : MonoBehaviour {

	public float totalPop { get; set;}

	Transform bar;
	TextMesh ageLabel;


	void Start()
	{
		bar = transform.FindChild ("bar");
	}

	public void updateAge(int age)
	{
		if(ageLabel == null)
		{
			ageLabel = transform.FindChild ("age").GetComponent<TextMesh> ();
		}
		ageLabel.text = age.ToString();

		if( age != 0 && age %10 != 0)
		{
			ageLabel.gameObject.SetActive(false);
		}
	}

	public void updateChart(float newVal)
	{
		float percent = newVal / totalPop * 100;

		//change bar scale, record change diff
		float tempScaleY = bar.localScale.y;
		bar.localScale = new Vector3 (1, percent * 8, 1);
		tempScaleY -= bar.localScale.y;

		//adjust bar position to compensate for height change
		bar.localPosition -= new Vector3 (0, tempScaleY/2, 0);
	}

}
