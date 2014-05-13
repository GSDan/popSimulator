using UnityEngine;
using System.Collections;

public class causeOfDeath
{
	public string name;

	//CHANCES OF DEATH FOR AGE + GENDER
	//statistics are deaths per million in UK
	float underOne;
	float oneToFour;
	float fiveToFourteen;
	float fifteenTwentyFour;
	float twentyFiveThirtyFour;
	float thirtyFiveFourtyFour;
	float fourtyFivefiftyFour;
	float fiftyFiveSixtyFour;
	float sixtyFiveSeventyFour;
	float seventyFiveEightyFour;
	float eightyFivePlus;

	float femaleunderOne;
	float femaleoneToFour;
	float femalefiveToFourteen;
	float femalefifteenTwentyFour;
	float femaletwentyFiveThirtyFour;
	float femalethirtyFiveFourtyFour;
	float femalefourtyFivefiftyFour;
	float femalefiftyFiveSixtyFour;
	float femalesixtyFiveSeventyFour;
	float femaleseventyFiveEightyFour;
	float femaleeightyFivePlus;

	public causeOfDeath(string description, string[,] data, int maleRow )
	{
		underOne = float.Parse(data [3, maleRow]);
		oneToFour = float.Parse(data [4, maleRow]);
		fiveToFourteen = float.Parse(data [5, maleRow]);
		fifteenTwentyFour = float.Parse(data [6, maleRow]);
		twentyFiveThirtyFour = float.Parse(data [7, maleRow]);
		thirtyFiveFourtyFour = float.Parse(data [8, maleRow]);
		fourtyFivefiftyFour = float.Parse(data [9, maleRow]);
		fiftyFiveSixtyFour = float.Parse(data [10, maleRow]);
		sixtyFiveSeventyFour = float.Parse(data [11, maleRow]);
		seventyFiveEightyFour = float.Parse(data [12, maleRow]);
		eightyFivePlus = float.Parse(data [13, maleRow]);

		femaleunderOne = float.Parse(data [3, maleRow + 1]);
		femaleoneToFour = float.Parse(data [4, maleRow + 1]);
		femalefiveToFourteen = float.Parse(data [5, maleRow + 1]);
		femalefifteenTwentyFour = float.Parse(data [6, maleRow + 1]);
		femaletwentyFiveThirtyFour = float.Parse(data [7, maleRow + 1]);
		femalethirtyFiveFourtyFour = float.Parse(data [8, maleRow + 1]);
		femalefourtyFivefiftyFour = float.Parse(data [9, maleRow + 1]);
		femalefiftyFiveSixtyFour = float.Parse(data [10, maleRow + 1]);
		femalesixtyFiveSeventyFour = float.Parse(data [11, maleRow + 1]);
		femaleseventyFiveEightyFour = float.Parse(data [12, maleRow + 1]);
		femaleeightyFivePlus = float.Parse(data [13, maleRow + 1]);

		name = description;

	}


	public bool checkForDeath(personSim sim, int currentYear)
	{
		return roulette(getChance(currentYear - sim.yearBorn, sim.isMale));
	}

	public int populationToll(int[] popCount)
	{
		int toll = 0;

		for(int i = 0; i < popCount.Length; i++)
		{
			int male = popCount[i]/2;
			int female = popCount[i]/2;

			male -= (int)(male * getChance(i, true)/1000000);
			female -= (int)(female * getChance(i, false)/1000000);

			toll += popCount[i] - (male+female);
			popCount[i] = male + female;
		}

		return toll;
	}


	float getChance(int age, bool isMale)
	{
		if(age < 2)
		{
			if(isMale) return underOne;
			return femaleunderOne;
		}
		if(age < 5)
		{
			if(isMale) return oneToFour;
			return femaleoneToFour;
		}
		if(age < 15)
		{
			if(isMale) return fiveToFourteen;
			return femalefiveToFourteen;
		}
		if(age < 25)
		{
			if(isMale) return fifteenTwentyFour;
			return femalefifteenTwentyFour;
		}
		if(age < 35)
		{
			if(isMale) return twentyFiveThirtyFour;
			return femaletwentyFiveThirtyFour;
		}
		if(age < 45)
		{
			if(isMale) return thirtyFiveFourtyFour;
			return femalethirtyFiveFourtyFour;
		}
		if(age < 55)
		{
			if(isMale) return fourtyFivefiftyFour;
			return femalefourtyFivefiftyFour;
		}
		if(age < 65)
		{
			if(isMale) return fiftyFiveSixtyFour;
			return femalefiftyFiveSixtyFour;
		}
		if(age < 75)
		{
			if(isMale) return sixtyFiveSeventyFour;
			return femalesixtyFiveSeventyFour;
		}
		if(age < 85)
		{
			if(isMale) return seventyFiveEightyFour;
			return femaleseventyFiveEightyFour;
		}
		
		if(isMale) return eightyFivePlus;
		return femaleeightyFivePlus;
	}

	bool roulette(float chance)
	{
		if(Random.Range(0, 1000000) <= chance)
		{
			return true; //bit the dust!
		}
		return false;
	}

	public void alterChances(float mod)
	{
		if( underOne * mod >= 0) underOne *= mod;
		if( oneToFour * mod >= 0)oneToFour *= mod;
		if( fiveToFourteen * mod >= 0)fiveToFourteen *= mod;
		if( fifteenTwentyFour * mod >= 0)fifteenTwentyFour *= mod;
		if( twentyFiveThirtyFour * mod >= 0)twentyFiveThirtyFour *= mod;
		if( thirtyFiveFourtyFour * mod >= 0)thirtyFiveFourtyFour *= mod;
		if( fourtyFivefiftyFour * mod >= 0)fourtyFivefiftyFour *= mod;
		if( fiftyFiveSixtyFour * mod >= 0)fiftyFiveSixtyFour *= mod;
		if( sixtyFiveSeventyFour * mod >= 0)sixtyFiveSeventyFour *= mod;
		if( seventyFiveEightyFour * mod >= 0)seventyFiveEightyFour *= mod;
		if( eightyFivePlus * mod >= 0)eightyFivePlus *= mod;
		
		if( femaleunderOne * mod >= 0)femaleunderOne *= mod;
		if( femaleoneToFour * mod >= 0)femaleoneToFour *= mod;
		if( femalefiveToFourteen * mod >= 0)femalefiveToFourteen *= mod;
		if( femalefifteenTwentyFour * mod >= 0)femalefifteenTwentyFour *= mod;
		if( femaletwentyFiveThirtyFour * mod >= 0)femaletwentyFiveThirtyFour *= mod;
		if( femalethirtyFiveFourtyFour * mod >= 0)femalethirtyFiveFourtyFour *= mod;
		if( femalefourtyFivefiftyFour * mod >= 0)femalefourtyFivefiftyFour *= mod;
		if( femalefiftyFiveSixtyFour * mod >= 0)femalefiftyFiveSixtyFour *= mod;
		if( femalesixtyFiveSeventyFour * mod >= 0)femalesixtyFiveSeventyFour *= mod;
		if( femaleseventyFiveEightyFour * mod >= 0)femaleseventyFiveEightyFour *= mod;
		if( femaleeightyFivePlus * mod >= 0)femaleeightyFivePlus *= mod;

		//Debug.Log ("Chance at cancer at 80: " + seventyFiveEightyFour.ToString());
	}

}

