using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

public class citySim : MonoBehaviour {

	//User defined variables
	public string countryName = "England & Wales";
	public int numOfConcurrentDocumentedLives = 5;
	public int yearsToCompute = 100;
	public float birthRateChange = 0.02f;
	public float minBirthRatePerThousand = 8.5f;
	public float advancesVsCancer = 0.01f;
	public float advancesVsHeartDisease = 0.01f;
	public float advancesVsSmoking = 0.009f;
	public int maxNetYearlyImmigration = 196000;

	public TextAsset deathStatistics; 
	public TextAsset populationData;
	public GameObject graphParent;


	static string[,] deathsData;
	static int[] popData;

	List<personSim> sims;
	causeOfDeath[] causesOfDeath;
	static int startYear = DateTime.Now.Year - 1;
	int currentYear = startYear;

	float currentPop = 0;
	float birthRate = 0;

	GameObject[] chart;
	UILabel popCounter;
	UILabel yearLabel;
	UILabel avgAgeLabel;

	charTrackerManager iconTracker;

	public void updateBirthChange()
	{
		birthRateChange = float.Parse(UIInput.current.value,  CultureInfo.InvariantCulture) / 100;
		Debug.Log (birthRateChange);
	}

	public void updateMinBirth()
	{
		minBirthRatePerThousand = float.Parse(UIInput.current.value,  CultureInfo.InvariantCulture);
	}

	public void updateCancerChange()
	{
		advancesVsCancer = float.Parse(UIInput.current.value, CultureInfo.InvariantCulture) / 100;
	}

	public void updateHeartChange()
	{
		advancesVsHeartDisease = float.Parse(UIInput.current.value,  CultureInfo.InvariantCulture) / 100;
	}

	public void updateSmokingChange()
	{
		advancesVsSmoking = float.Parse(UIInput.current.value,  CultureInfo.InvariantCulture) / 100;
	}

	public void updateImmigration()
	{
		maxNetYearlyImmigration = int.Parse(UIInput.current.value, NumberStyles.AllowLeadingSign | 
		                                    NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite);
	}

	// Use this for initialization
	void Start () {
		GameObject cam = GameObject.FindGameObjectWithTag ("MainCamera").transform.FindChild ("inGame").gameObject;

		iconTracker = GameObject.FindGameObjectWithTag ("trackerManager").GetComponent<charTrackerManager> ();

		popCounter = cam.transform.FindChild ("popTotal").GetComponent<UILabel>();
		yearLabel = cam.transform.FindChild ("curYear").GetComponent<UILabel>();
		avgAgeLabel = cam.transform.FindChild ("avgAge").GetChild(0).GetComponentInChildren<UILabel> ();

		UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;

		deathsData = CSVReader.SplitCsvGrid (deathStatistics.text);

		string[,] temp = CSVReader.SplitCsvGrid (populationData.text);
		popData = new int[temp.GetLength(1)];

		chart = new GameObject[popData.Length];

		for(int i = 0; i < popData.Length; i++)
		{
			if(temp[0,i] != null)
			{
				popData[i] = int.Parse(temp[0,i]);
			}
		}

		sims = new List<personSim> ();

		causesOfDeath = new causeOfDeath[]{
			new causeOfDeath ("an intestinal infection", deathsData, 0, 95),
			new causeOfDeath ("a bacterial disease", deathsData, 2, 100),
			new causeOfDeath ("Septicaemia", deathsData, 4),
			new causeOfDeath ("cancer", deathsData, 6, 95),
			new causeOfDeath ("a disease of the blood", deathsData, 8, 100),
			new causeOfDeath ("diabetes", deathsData, 10),
			new causeOfDeath ("dementia", deathsData, 12, 93),
			new causeOfDeath ("Parkinson's disease", deathsData, 14, 98),
			new causeOfDeath ("Alzheimer’s disease", deathsData, 16, 98),
			new causeOfDeath ("heart disease", deathsData, 18, 98),
			new causeOfDeath ("pneumonia", deathsData, 20, 98),
			new causeOfDeath ("a lower respiratory disease", deathsData, 22),
			new causeOfDeath ("chronic obstructive pulmonary disease", deathsData, 24),
			new causeOfDeath ("a disease of the digestive system", deathsData, 26),
			new causeOfDeath ("a disease of the musculoskeletal system", deathsData, 28),
			new causeOfDeath ("a genital disease", deathsData, 30),
			new causeOfDeath ("conditions originating during birth", deathsData, 32),
			new causeOfDeath ("general senility", deathsData, 34, 98),
			new causeOfDeath ("sudden infant death syndrome", deathsData, 36),
			new causeOfDeath ("a fall", deathsData, 38, 95),
			new causeOfDeath ("an accident", deathsData, 40),
			new causeOfDeath ("self harm", deathsData, 42)
		};

		updateCurrentPopCount(); 

		for(int i = 0; i < popData.Length; i++)
		{
			chart[i] = Instantiate(Resources.Load("PopChart")) as GameObject;
			chart[i].transform.parent = graphParent.transform;
			chart[i].transform.localScale = new Vector3(8,8,1);
			chart[i].transform.localPosition = new Vector3( i*8 -460, 0, 0);
			chart[i].name = "Age_"+i.ToString();
			chart[i].GetComponent<popChart>().totalPop = currentPop;
			chart[i].GetComponent<popChart>().updateAge(i);
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			yearsToCompute += 5;
		}


		if(startYear + yearsToCompute > currentYear){

			birthRate = ((float)popData[0]/((float)currentPop/1000));
			causesOfDeath[3].alterChances(1 - advancesVsCancer);
			causesOfDeath[9].alterChances(1 - advancesVsHeartDisease);
			causesOfDeath[12].alterChances(1 - (advancesVsSmoking /2));

			if(startYear == currentYear)
			{
				personSim sim = new personSim(currentYear);
				sims.Add (sim);
				iconTracker.addTracker(sim);

			}

			currentYear++;
			
			yearLabel.text = currentYear.ToString();
			
			updateCurrentPopCount(); 
			updateChart();
			
			popCounter.text = currentPop.ToString("N0", new CultureInfo( "en-US", false ).NumberFormat);
			
			int deathToll = 0;
			
			foreach(causeOfDeath cause in causesOfDeath)
			{
				deathToll += cause.populationToll(popData);
			}
			
			
			//move population forward a year
			for(int i = popData.Length - 1; i >= 0; i--)
			{
				if(i > 0)
					popData[i] = popData[i-1];
				else
				{
					if(birthRate > minBirthRatePerThousand)
						popData[0] = (int)(popData[1] * (1 - birthRateChange));
					else
						popData[0] = popData[1];
				}
					
			}
			
			int count = 0;
			
			while(count < sims.Count)
			{
				sims[count].yearlyUpdate(sims, causesOfDeath, currentYear);					
				count++;
			}
			
			iconTracker.updateTrackers(chart, currentYear);			

		}
	}

	void updateCurrentPopCount()
	{
		addImmigrants ();
		int counter = 0;

		foreach(int pop in popData)
		{
			counter += pop;
		}

		currentPop = counter;
	}

	void updateChart()
	{
		float total = 0;

		for(int i = 0; i < chart.Length; i++)
		{	
			if(chart[i] != null)
			{
				chart[i].GetComponent<popChart>().totalPop = currentPop;
				chart[i].GetComponent<popChart>().updateChart((float)popData[i]);
				total += popData[i] * i;
			}
		}

		avgAgeLabel.text = (total / currentPop).ToString ("N1", new CultureInfo( "en-US", false ).NumberFormat);
	}

	void addImmigrants()
	{
		if(currentYear != startYear)
		{
			//add a little variation in number of immigrants and their ages
			float immigrants = UnityEngine.Random.Range (maxNetYearlyImmigration * 0.8f, maxNetYearlyImmigration);

			for(int i = 5; i <= 50; i++)
			{
				popData[i] += (int)UnityEngine.Random.Range (immigrants / 55, immigrants/25);
			}
		}
	}

	public void OnClick()
	{
		yearsToCompute += 5;
	}

}
