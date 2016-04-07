using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;


public class colorManager : SceneSingleton<colorManager> {
	public int curColor;
	public int nextColor1;
	public int nextColor2;
	public int numOfColors;

	public int chooseColor(int range){
		Random rnd = new Random();
	int color = Random.Range(1, range);
		return color;
	}

	// Use this for initialization
	void Start () {
		numOfColors = GameManager.main.levelNumColors;
		curColor = chooseColor(numOfColors);
		nextColor1 = chooseColor(numOfColors);
		nextColor2 = chooseColor(numOfColors);
	
	}
	
	// Update is called once per frame
	void Update () {
	if(BombScript.main.flag == 0){
		curColor = nextColor1;
		nextColor1 = nextColor2;
		nextColor2 = chooseColor(numOfColors);
		BombScript.main.flag = 1;

	}

}
