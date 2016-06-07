using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movementScript : MonoBehaviour {


	Dictionary<int, Dictionary<int,string>> cordinates; //first int is x
	Dictionary<int,string> minus4 = new Dictionary<int,string>();
	Dictionary<int,string> minus3 = new Dictionary<int, string>();
	Dictionary<int,string> minus2 = new Dictionary<int, string>();
	Dictionary<int,string> minus1 = new Dictionary<int, string>();
	Dictionary<int,string> zero = new Dictionary<int, string>();
	Dictionary<int,string> one = new Dictionary<int, string>();
	Dictionary<int,string> two = new Dictionary<int, string>();
	Dictionary<int,string> three = new Dictionary<int, string>();
	Dictionary<int,string> four = new Dictionary<int, string>();
	Dictionary<int,string> five = new Dictionary<int, string>();
	int currentX = 0;
	int currentY = 0;
	GameState gs;

	// Use this for initialization
	void Start () {

		minus4.Add (-4,"You reach a high cliff \nand the sea opens up in front of you.");
		minus4.Add (-3,"The trees give away to a rocky shores.");
		minus4.Add (-2,"You've wandered to an empty shore.\n Driftwood has dried grey.");
		minus4.Add (-1,"The waves crash onto the shore,\n washing away your footprints.");
		minus4.Add (0, "Far in the distance, among the waves,\n doplhins are playing.");
		minus4.Add (1, "The river meets the ocean.");
		minus4.Add (2, "The trees grow thinner.\n You've reached the seashore.");
		minus4.Add (3, "The ground becomes muddier.\n A swamp makes inpossible to continue further north.");
		minus4.Add (4, "Many flowers bloom in the moist ground.");
		minus4.Add (5, "The trees look twisted and wierd.");
		
		minus3.Add (-4, "The ground is uneven and it's hard to move forward.");
		minus3.Add (-3, "There are no almost no signs of life\n in the rocky landscape.");
		minus3.Add (-2, "You can see squirrels up in the trees.");
		minus3.Add (-1, "The ground is covered in moss.");
		minus3.Add (0, "You come to a small clearing. The sky is beautiful.");
		minus3.Add (1, "The river flows quietly.");
		minus3.Add (2, "The trees here are higher but scarcer.");
		minus3.Add (3, "As you walk your feet sink into the ground.\n The smell of swamp fills the air.");
		minus3.Add (4, "The grass is high.\nYou can hear chirring of the bugs.");
		minus3.Add (5, "The swampwater reaches your waist.\nYou cannot continue further east.");
		
		minus2.Add (-4, "In the west you can see steep cliffs\n that seem to reach up to the clouds.");
		minus2.Add(-3,"The forest is dense and quiet.");
		minus2.Add(-2,"The bushes around you are full of berries.");
		minus2.Add(-1,"You walk on top of huge roots\nin order not to step on the blooming flowers.");
		minus2.Add(0,"The river is deep and wide here.");
		minus2.Add(1,"You walk along the riverbend.");
		minus2.Add(2,"Bevers have built a dam here in the river.");
		minus2.Add(3,"In the distane you see a deer.");
		minus2.Add(4,"You can't see the water from the reeds.");
		minus2.Add(5,"Among the reeds, you see a lonely red flower.");
		
		minus1.Add(-4,"The ground is uneaven and hard to trail here.");
		minus1.Add(-3,"You find the footprints of a bear on the ground.");
		minus1.Add(-2,"Here the riwer is so small that\n you can almost jump over it.");
		minus1.Add(-1,"Birds are swimming across the river.");
		minus1.Add(0,"You see a rabbit among the grass.\nIt quickly runs away.");
		minus1.Add(1,"The trees are so tall that\n they must be at least a thousand years old.");
		minus1.Add(2,"The forest is full of sounds of its residents.");
		minus1.Add(3,"You pass by a hollow tree.\n It looks like it could fall any moment.");
		minus1.Add(4,"Among the root you can barely see\n the ground from the mushrooms.");
		minus1.Add(5,"The ground is is full of holes\n made by badgers.");
		
		zero.Add(-4,"The high cliffs throw a shadow over everything.");
		zero.Add(-3,"You find a cave. \nIt looks like a bear might live there.");
		zero.Add(-2,"You see bear trying to catch fish in the river.");
		zero.Add(-1,"You cross a small stream and quench your thirst.");

		zero.Add(0,"You are in the middle of the forest.\nThis is where your camp is.");

		zero.Add(1,"You see behives in the trees.\nYou can almost smell the honey.");
		zero.Add(2,"You see a beautiful butterfly \nwith the most vibrant colours.");
		zero.Add(3,"This part of the forest seems calmer than the rest.");
		zero.Add(4,"The trees here look so beautiful\nthat they could be nymphs.");
		zero.Add(5,"You've come to the shore of a lake. \nThe water is crystal clear.");
		
		one.Add(-4,"The trees make a wall here that makes it\nimpossible to move further westward.");
		one.Add(-3,"You see a pair of eyes staring at you, \nalmost hidden by the leaves.");
		one.Add(-2,"A tree bends over the river, almost like a bridge.\nYou walk across it.");
		one.Add(-1,"A lonely owl hoots,\nhidden somewhere above you.");
		one.Add(0,"You find an apple tree.");
		one.Add(1,"An eagle flies past,\n high up in the air.");
		one.Add(2,"You find the nest of some small animal.");
		one.Add(3,"The lake seems almost too quiet.");
		one.Add(4,"Water lilies grow in shallow waters of the lake.");
		one.Add(5,"The lake seems to go on forever.");
		
		two.Add(-4,"The ground becomes higher. \nYou can feel the wind.");
		two.Add(-3,"You find a small valley,\n hidden from the world.");
		two.Add(-2,"You've come to a waterfall.\n You wonder if there's a cave behind it.");
		two.Add(-1,"You can feel a waterfall in the distance.");
		two.Add(0,"The air is completely still.\nYou can hear no sounds.");
		two.Add(1,"The ground here is very uneven.");
		two.Add(2,"You climb atop of a hill\nand look back at the forest.");
		two.Add(3,"There are puddles on the ground.\nThey reflect the sky.");
		two.Add(4,"You can see swans swimming in the lake.");
		two.Add(5,"You sit on the lake shore,\nresting for a while.");
		
		three.Add(-4,"You see trees all around you.");
		three.Add(-3,"The forest looks eery.\nIt's like it doesn't want you here.");
		three.Add(-2,"The water of the river looks dark as it rushes down.");
		three.Add(-1,"There has been a forest fire.\nThe trees are black and scarred.");
		three.Add(0,"You find the most beautiful field.");
		three.Add(1,"You step on a sharp rock. \nOuch.");
		three.Add(2,"You are so high that you can see the sea from here, \n far in the north.");
		three.Add(3,"You stumble down the hill.");
		three.Add(4,"The trees and grass is so lush here\n that you can hardly see ahead.");
		three.Add(5,"You find a pack of angry wolves.\nIt's a bad idea to go further east.");
		
		four.Add(-4,"There is a woodchuck chuking wood.\n The sound echoes in the forest.");
		four.Add(-3,"You hear a hammering sound.");
		four.Add(-2,"The river is so fierce that\n it feels like it's alive.");
		four.Add(-1,"The trees seem to have faces\n and look at you with malice.");
		four.Add(0,"The forest in the west looks unwelcoming.");
		four.Add(1,"You find sparkillng pond. \nYou want to take a dip.");
		four.Add(2,"Trees here have big leaves.");
		four.Add(3,"You see a hedgehog.");
		four.Add(4,"There is a big rock here.");
		four.Add(5,"You find a trail that almost looks like a road,\n leading out from the forest.");
		
		five.Add(-4,"A landslide has blocked your path towards west.");
		five.Add(-3,"You fall into a hole that \nhad been covered by fallen leaves.");
		five.Add(-2,"You find a spring.\nThe water is ice-cold.");
		five.Add(-1,"The river emerges from an underground spring.");
		five.Add(0,"The branches look like they'd want grab you.");
		five.Add(1,"There is a cave that almost looks like a house.\nA witch would feel at home here.");
		five.Add(2,"You walk around enjoying the smell of the forest.");
		five.Add(3,"Mist covers the forest floor.");
		five.Add(4,"The trees here look grey and tired.");
		five.Add(5,"The mist is so dence that you cannot go further south or east.");



		string desc = "";
		cordinates = new Dictionary<int, Dictionary<int,string>> ();
		for(int x = 5; x>-5; x--) {
			cordinates.Add(x, new Dictionary<int, string>());
			for (int y = 5; y>-5; y--){
				switch(x){
				case -4: desc = minus4[y]; break;
				case -3: desc = minus3[y]; break;
				case -2: desc = minus2[y]; break;
				case -1: desc = minus1[y]; break;
				case 0: desc = zero[y]; break;
				case 1: desc = one[y]; break;
				case 2: desc = two[y]; break;
				case 3: desc = three[y]; break;
				case 4: desc = four[y]; break;
				case 5: desc = five[y]; break;
				default: Debug.Log("An error in describeLocation has happened"); break;
				}
				cordinates[x].Add(y,desc);
			}
		}

		gs = GameObject.Find("GameController").GetComponent<GameState>();
	}

	public string getCurrentLocation(){
		string locationDesc = "Error";

		if (cordinates.ContainsKey (currentX)) {
			Dictionary<int,string> tempDict = cordinates[currentX];
			locationDesc = tempDict[currentY];

		} else {
			Debug.Log("Couldn't find the x-location");
		}

		gs.textControl.writeLine(locationDesc);
		if (currentX == 0 && currentY == 0) {
			string state = gs.stateVariables["statefire"];
			if(state.Equals("notlit")){
				//gs.textControl.writeLine("The fire has gone out.");
				//				locationDesc += "\nThe fire has gone out.";
			}
			if(state.Equals("out")){
				gs.textControl.writeLine("The fire has gone out.");
//				locationDesc += "\nThe fire has gone out.";
			}
			if(state.Equals("burning")){
				gs.textControl.writeLine("The fire is still burning.");
				//locationDesc += "\nThe fire is still burning.";
			}
			if(state.Equals("dying")){
				gs.textControl.writeLine("The fire is slowly dying.");
				//locationDesc += "\nThe fire is slowly dying.";
			}
		}


		return locationDesc;
	}

	public int[] getCordintes(){
		int[] cordArray = new int[2]{currentX,currentY};
		return cordArray;
	}

	public void move(string direction){
		switch (direction) {
		case "north": 
			currentX += -1; 
			if(currentX<-4){
				currentX = -4;
			}					
			break;
		case "south": 
			currentX += +1;
			if(currentX>5){
				currentX = 5;
			}	
			break;
		case "east": 
			currentY += +1;
			 if(currentY>5){
				currentY = 5;
			}	
			break;
		case "west": 
			currentY += -1;
			if(currentY<-4){
				currentY = -4;
			}
			break;
		default: Debug.Log("Problem with the direction command"); break;
		}
	}
}
