using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour{
	public int amount = 1;

	public Coord coord;

	Food(int amnt){
		amount = amnt;
	}
	Food(){
		amount = 1;
		coord = new Coord ();
	}
}
