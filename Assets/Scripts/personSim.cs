using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class personSim
{
	public int yearBorn { get; private set;}
	public int yearDied { get; private set;}
	public bool isMale { get; private set;}

	public personSim guard1 { get; private set;}
	public personSim guard2 { get; private set;}
	public List<personSim> children { get; private set;}
	public personSim partner { get; private set; }
	public bool isMarried { get; set;}

	public Color eyeColour { get; private set; }
	public Color hairColour { get; private set; }
	public string hairDescription { get; private set; }

	public bool isAlive { get; private set;}
	public causeOfDeath diedFrom { get; private set; }

	public string firstName { get; private set; }
	public string surname { get; private set; }

	string[] maleNames = {"Jon","Dan", "Hugo", "Robin", "Wojtek", "Leeroy", "Craig", "Daemon", "Laurence", "William", 
							"Albert", "Luigi", "Mario", "Roy", "Kyle", "Sam", "Tim", "Vinny", "Greg", "Brad", 
							"Jeff", "Dave", "Ryan", "Alexis", "Drew", "Peter", "Chris", "Richard", "Alf", "Matthew"};

	string[] femaleNames = {"Sarah", "Lucy", "Naomi", "Emma", "Laura", "Kate", "Lisa", "Claudia", "Liara", "Leia",
							"Holly", "Rebecca", "Cheryl", "Fran", "Louise", "Sabrina", "Sadie", "Cynthia", "Jess", "Anita",
							"Nigella", "Jenny", "Alice", "Marie", "Chie", "Juliet", "Juno", "Megan", "Stacy", "Laura"};

	string[] surnames = {"Skywalker", "Organa", "Richardson", "White", "Snow", "Laurie", "Solo", "Potter", "Trent", "Baker",
						"Waite", "Collins", "Firth", "Magee", "Smith", "Sharpe", "Murphy", "Booth", "Hudson", "Thompson", 
						"Harris", "Parker", "Slater", "Watson", "Griffiths", "Simpson", "Mario", "Stewart", "Wood", "Jenkins"};

	string[] hairTypes = {"curly", "straight", "wavy"}; string[] hairModifiers = {"long", "short", "neat", "unkempt"};

	string[] charThoughts = new string[8]; // the character's thoughts on current situations

	Color[] eyeColours = {Color.blue, Color.cyan, Color.gray, Color.green, hexColor(139, 69, 19, 1) /* brown */};
	Color[] hairColours = {Color.black, hexColor(139, 69, 19, 1) /* brown */, hexColor(254,255,161, 1) /* blonde */, hexColor(244,176,72,1) /* ginger */};


	public static Color hexColor(float r, float g, float b, float a){
		Color color = new Color(r/255, g/255, b/255, a/255);
		return color;
	}
	

	public personSim(int currentYear, float maleMod = 0, personSim guard1 = null, personSim guard2 = null)
	{
		children = new List<personSim> ();
		yearBorn = currentYear;
		isAlive = true;
		this.guard1 = guard1;
		this.guard2 = guard2;
		
		//assign gender
		if(UnityEngine.Random.value >= 0.5f + -maleMod)
		{
			isMale = true;
		}
		else
		{
			isMale = false;
		}

		if(guard1 != null && guard2 != null && guard1.isMale != guard2.isMale)
		{
			//guardians are biological parents, randomly inherit features!
			if(UnityEngine.Random.value >= 0.5f)		
				this.eyeColour = guard1.eyeColour;
			else
				this.eyeColour = guard2.eyeColour;

			if(UnityEngine.Random.value >= 0.5f)		
				this.hairColour = guard1.hairColour;
			else
				this.hairColour = guard2.hairColour;

			if(UnityEngine.Random.value >= 0.5f)		
				this.hairDescription = guard1.hairDescription;
			else
				this.hairDescription = guard2.hairDescription;

			//I'm old fashioned....
			if(guard1.isMale)
				generateName (guard1.surname);
			else
				generateName (guard2.surname);

		}

		else
		{
			if (guard1 != null && guard2 != null && guard1.isMale == guard2.isMale)
			{
				//child is adopted, take name only
				if(UnityEngine.Random.value >= 0.5f)		
					generateName(guard1.surname);
				else
					generateName(guard2.surname);
			}
			else generateName();//no parents given, randomly gen everything

			eyeColour = eyeColours[UnityEngine.Random.Range(0, eyeColours.Length)];
			hairColour = hairColours[UnityEngine.Random.Range(0, hairColours.Length)];
			hairDescription = hairModifiers[UnityEngine.Random.Range(0, hairModifiers.Length)] + ", " + hairTypes[UnityEngine.Random.Range(0, hairTypes.Length)];
		}
		

	}

	public int getAge(int currentYear)
	{
		if (isAlive)
			return currentYear - yearBorn;
		else
			return -1;
	}

	public bool yearlyUpdate(List<personSim> chars, causeOfDeath[] causes, int currentYear)
	{
		int age = getAge(currentYear);
		if(isAlive){

			if(partner != null && partner.isAlive)
			{
				partner.diedThisYear (causes, currentYear);
			}

			if(partner != null && partner.isAlive && age >= 18 && age <= 43  && children.Count < 3)
			{
				//chance of gaining child
				if(UnityEngine.Random.value >= 0.93f)
				{
					personSim child = new personSim(currentYear, 0, this, partner);
					chars.Add(child);
					children.Add(child);
					partner.children.Add(child);

					if(partner.isMale != isMale)
						Debug.Log(getName() + " and " + partner.getName() + " gave birth to " + child.getName() + " in " + currentYear.ToString());
					else
						Debug.Log(getName() + " and " + partner.getName() + " adopted " + child.getName() + " in " + currentYear.ToString());

					GameObject.FindGameObjectWithTag("trackerManager").GetComponent<charTrackerManager>().addTracker(child);
				}
			}

			//check if should marry
			if(partner != null && partner.isAlive && isMarried == false && UnityEngine.Random.value >= 0.9f)
			{
				partner.isMarried = true;
				isMarried = true;
				Debug.Log(getName() + " married " + partner.getName() + " in " + currentYear + "!");
				if(partner.isMale)
				{	
					string message = getName() + " is now ";
					surname = partner.surname;
					Debug.Log(message + getName());

					foreach(personSim child in children)
					{
						child.surname = surname;
					}
				}
				else
				{
					partner.surname = surname;
				}
			}

			//check if find a partner
			else if(partner == null && age >= 17 && UnityEngine.Random.value >= 0.85f)
			{
				//create partner with adjusted gender odds
				if(isMale)partner = new personSim( yearBorn + UnityEngine.Random.Range(-5, 5), -0.3f);
				else partner = new personSim(currentYear  + UnityEngine.Random.Range(-5, 5), 0.3f);
				Debug.Log(getName() + " started going out with " + partner.getName() + " in " + currentYear);
			}

			return diedThisYear (causes, currentYear);
		}

		return false;
	}


	void generateName(string surname = "")
	{
		if(isMale)
		{
			firstName = maleNames[UnityEngine.Random.Range(0, maleNames.Length)];
		}
		else
		{
			firstName = femaleNames[UnityEngine.Random.Range(0, femaleNames.Length)];
		}

		if (surname == "")
			this.surname = surnames [UnityEngine.Random.Range (0, surnames.Length)];
		else 
			this.surname = surname;
	}
	

	bool diedThisYear(causeOfDeath[] causes, int currentYear)
	{
		if(isAlive)
		{
			foreach(causeOfDeath cause in causes)
			{
				if(cause.checkForDeath(this, currentYear))
				{
					isAlive = false;
					yearDied = currentYear;
					diedFrom = cause;
					Debug.Log(getName() + " died in " + currentYear + " of " + cause.name + ", age " + (currentYear - yearBorn) );
					return true;
				}
			}
		}
		return false;
	}

	void updateThoughts(int currentYear)
	{
		//have we talked about...
		bool age = false;
		bool kids = false;
		bool parents = false;
		bool part = false;
		bool marriage = false;
		bool money = false;

		//loop through all possible messages and say as many unique things as appropriate
		for(int i = 0; i < charThoughts.Length; i++)
		{
			//looking after parents?
			if(!parents && (guard1 != null && guard2 != null) && (guard1.getAge(currentYear) > 80 ||  guard1.getAge(currentYear) > 80))
			{
				charThoughts[i] = "I'm having to dedicate a lot of time to looking after my parents";
				if(getAge(currentYear) > 60)
				{
					charThoughts[i] += " despite getting on in years myself";
				}
				if(children.Count > 0)
				{
					charThoughts[i] += ". But I also have my own family to take care of";
				}
				charThoughts[i] += "...";
				parents = true;
			}

			//feeling broody?
			else if(!kids && children.Count == 0 && getAge(currentYear) > 28)
			{
				if(getAge(currentYear) < 52)
				{
					charThoughts[i] = "I'd like to start a family soon";
					if(isMarried)
					{
						charThoughts[i] += ", I hope " + partner.firstName + " agrees";
					}
					if(getAge(currentYear) > 38)
					{
						charThoughts[i] += ". Time is running out";
					}
					if(partner == null)
					{
						charThoughts[i] += " - I need to find a partner..";
					}
					charThoughts[i] += ".";
				}
				else 
				{
					charThoughts[i] += "I'd have liked to have had children, but it's too late now.";
				}
				kids = true;
			}

			//has kids already!
			else if(!kids && children.Count > 0)
			{
				charThoughts[i] = "";
				//is old?
				if(getAge(currentYear) > 80)
				{
					if(children.Count > 1)
						charThoughts[i] = "I'm glad I have my children to support me in my old age, pensions aren't what they used to be. ";
					else
						charThoughts[i] = "I'm glad my child " + children[0].firstName + " is able to support me, the pension might not be enough otherwise. ";
				}

				bool grand = false;
				bool kidsAlive = false;
				int avAge = 0;

				foreach(personSim child in children)
				{
					if(child.isAlive) 
					{
						kidsAlive = true;
						avAge += child.getAge(currentYear);
					}
					if(child.children.Count > 0)
					{
						//has grandchildren
						grand = true;
					}
				}

				if(grand)
				{
					charThoughts[i] += " I'm happily a grandparent!";
				}
				else if(avAge > 30 && kidsAlive)
				{
					charThoughts[i] += " Hopefully I'll be a grandparent soon.";
				}

				kids = true;
			}
		}

	}

	public string getName(){ return firstName + " " + surname;}	
}

