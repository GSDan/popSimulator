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
	int coupledSince = 0;
	int marriedSince = 0;
	int lastYearUsed = 0;

	public Color eyeColour { get; private set; }
	public Color hairColour { get; private set; }
	public string hairDescription { get; private set; }

	public bool isAlive { get; private set;}
	public causeOfDeath diedFrom { get; private set; }

	public string firstName { get; private set; }
	public string surname { get; private set; }

	charTrackerManager charManager;
	eventManager evManager;

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
		evManager = GameObject.FindGameObjectWithTag ("eventManager").GetComponent<eventManager> ();
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

	public int getAge(int currentYear = -3)
	{
		if(currentYear == -3)
		{
			currentYear = lastYearUsed;
		}
		else
		{
			lastYearUsed = currentYear;
		}

		if (isAlive)
			return currentYear - yearBorn;
		else
			return -1;
	}

	public bool yearlyUpdate(List<personSim> chars, causeOfDeath[] causes, int currentYear)
	{

		if(charManager == null)
		{
			charManager = GameObject.FindGameObjectWithTag("trackerManager").GetComponent<charTrackerManager>();
		}

		int age = getAge(currentYear);
		if(isAlive){

			if(partner != null && partner.isAlive)
			{
				partner.diedThisYear (causes, currentYear);
			}

			if(partner != null && partner.isAlive && age >= 18 && age <= 46  && children.Count < 2 && charManager.getNumChars() < 8)
			{
				//chance of gaining child
				if((UnityEngine.Random.value >= 0.95f) || (charManager.getNumChars() < 3 && (UnityEngine.Random.value >= 0.85f)))
				{
					personSim child = new personSim(currentYear, 0, this, partner);
					chars.Add(child);
					children.Add(child);
					partner.children.Add(child);

					if(partner.isMale != isMale)
						evManager.addEvent(getName() + " and " + partner.getName() + " gave birth to " + child.getName() + " in " + currentYear.ToString());
					else
						evManager.addEvent(getName() + " and " + partner.getName() + " adopted " + child.getName() + " in " + currentYear.ToString());

					GameObject.FindGameObjectWithTag("trackerManager").GetComponent<charTrackerManager>().addTracker(child);
				}
			}

			//check if should marry
			if(partner != null && partner.isAlive && isMarried == false && UnityEngine.Random.value >= 0.9f)
			{
				partner.isMarried = true;
				isMarried = true;
				marriedSince = currentYear;
				evManager.addEvent(getName() + " married " + partner.getName() + " in " + currentYear + "!");
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
				evManager.addEvent(getName() + " started going out with " + partner.getName() + " in " + currentYear);
				coupledSince = currentYear;
			}

			updateThoughts(currentYear);

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
					evManager.addEvent(getName() + " died in " + currentYear + " of " + cause.name + ", age " + (currentYear - yearBorn) );
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
			charThoughts[i] = "";

			//looking after parents?
			if(!parents && (guard1 != null && guard2 != null))
			{
				if(guard1.getAge(currentYear) > 80 ||  guard1.getAge(currentYear) > 80)
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
				}
				if(!guard1.isAlive && currentYear - guard1.yearDied < 10)
				{
					if(!guard2.isAlive)
						charThoughts[i] += " I still miss my parents.";
					else if(guard1.isMale)
						charThoughts[i] += " I still miss my Dad.";
					else
						charThoughts[i] += " I still miss my Mum.";
				}
				else if(!guard2.isAlive && currentYear - guard2.yearDied < 10)
				{
					if(guard2.isMale)
						charThoughts[i] += " I still miss my Dad.";
					else
						charThoughts[i] += " I still miss my Mum.";
				}
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
				//is old?
				if(getAge(currentYear) > 80)
				{
					if(children.Count > 1)
						charThoughts[i] = "I'm glad I have my children to support me in my old age, pensions aren't what they used to be. ";
					else
						charThoughts[i] = "I'm glad my child " + children[0].firstName + " is able to support me, the pension might not be enough otherwise. ";
				}

				personSim childDead = null;
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
					else{
						childDead = child;
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
					charThoughts[i] += " Hopefully I'll be a grandparent soon!";
				}
				else if(children.Count > 1 && !kidsAlive)
				{
					charThoughts[i] = "My children died before me, how could this happen??";
				}
				else if(children.Count > 0 && childDead != null)
				{
					charThoughts[i] = "Poor little " + childDead.firstName + ".";
				}

				if(avAge > 0 && avAge < 5 && kidsAlive && childDead == null)
				{
					if(children.Count > 1)
						charThoughts[i] = "My kids are so cute!";					
					else
						charThoughts[i] = children[0].firstName + " is such a cute kid.";
				}

				kids = true;
			}
			else if(!part && currentYear - coupledSince < 4  && partner.isAlive)
			{
				if(partner.isMale)
					charThoughts[i] = "Isn't " + partner.firstName + " a dreamboat? I'm so glad I met him.";
				else
					charThoughts[i] += partner.firstName + " is amazing. I'm so glad I met her.";
				part = true;
			}
			else if(!marriage && partner != null && partner.isAlive)
			{
				if(isMarried && currentYear - marriedSince > 50)
				{
					charThoughts[i] = "I've been married to " + partner.firstName + " for such a long time, I'd be lost without ";
					if(partner.isMale)
						charThoughts[i] += "him.";
					else
						charThoughts[i] += "her.";
				}

				else if(isMarried && currentYear - marriedSince < 5)
				{
					charThoughts[i] = "I'm still getting used to the married life. " + partner.firstName + " treats me so well.";
				}

				else if(!isMarried && currentYear - coupledSince > 2)
				{
					charThoughts[i] = "I think " + partner.firstName + " has marriage in mind. Maybe I should propose?";
				}
				marriage = true;
			}
			else if(!part && partner!=null && !partner.isAlive)
			{
				charThoughts[i] = "I miss " + partner.firstName + ".";
				if(partner.yearDied - partner.yearBorn < 50)
				{
					charThoughts[i] += " I was hoping we would grow old together.";
				}
				else if(partner.yearDied - partner.yearBorn > 80)
				{
					if(partner.isMale)
						charThoughts[i] += " At least he saw a ripe old age. What a gent he was...";
					else
						charThoughts[i] += " At least she saw a ripe old age. What a beauty she was...";
				}

				part = true;
			}

			else if(!age)
			{
				if(getAge(currentYear) < 8)
				{
					charThoughts[i] = "I like turtles.";
				}
				else if(getAge(currentYear) > 14 && getAge(currentYear) < 18)
				{
					charThoughts[i] = "School is hard!";
				}
				else if(getAge(currentYear) > 17 && getAge(currentYear) < 23)
				{
					charThoughts[i] = "University is hard - and expensive! I'm sick of eating nothing but Pot Noodles...";
				}
				else if(getAge(currentYear) > 110)
				{
					charThoughts[i] = "I didn't go through three World Wars for this!";
				}
				else if(getAge(currentYear) > 100)
				{
					charThoughts[i] = "I got my congratulations letter from HRH, but the royals haven't been the same since the mind-controlling ants invaded...";
				}
				age = true;
			}

			else if(!money)
			{
				bool kidsAlive = false;
				int avAge = 0;
				
				foreach(personSim child in children)
				{
					if(child.isAlive) 
					{
						kidsAlive = true;
						avAge += child.getAge(currentYear);
					}
				}

				if(children.Count > 0) avAge /= children.Count;

				if(children.Count > 1 && kidsAlive)
				{
					if(avAge > 25)				
						charThoughts[i] = "I'm glad my children can finally start supporting themselves, bringing them up was expensive. I might need help from them in the future.";
					else if(avAge < 25 && avAge > 17)
						charThoughts[i] = "Putting kids through University is expensive! My bank account is crying!";
					else if(avAge < 8)
						charThoughts[i] = "Who knew that raising kids was so expensive?!";
				}
				money = true;
			}
		}

	}

	public string getName(){ return firstName + " " + surname;}	

	public string getThoughts()
	{
		string final = "";

		foreach(string thought in charThoughts)
		{
			if(thought != "")
			final += thought + "\n";
		}

		return final;
	}
}

