﻿using UnityEngine;
using System.Collections;

public class GroScript : MonoBehaviour {

	public int colorType;
	private int RA = 1, RAA = 2, RAAA = 3, BA = 4, BAA = 5, BAAA = 6, GA = 7, GAA = 8, GAAA = 9, Y = 10; 

	public float growthRate; //per some in
	public float outputRate; //per second

	public float[] growthLevels;
	public int currentLevel = 0;
	public float currGrowthNeeded;
	public int MAX_LEVEL = 200;

	public int blopsTaken = 0;
	public int blopsOutput = 0;
	public float currentOutputWait = 0.0f; //amount of output rate that hasn't been output

	public float currentGrowthAmount = 0.0f; 
	public float currentOutputRate = 0.0f;
	public int OUTPUT_AT = 1;
	public int MAX_HEIGHT = 38;
	public bool timeToGrow = false;
	public bool timeToOutput = false;

	public int purchaseCost;

	public bool placed;

	// Use this for initialization
	void Start () {
		if (placed != true){
			placed = false;
		}
		string ct = this.gameObject.tag;
		if (ct == "Red Gro A") {
			colorType = RA;
		} else if (ct == "Red Gro AA") {
			colorType = RAA;
		} else if (ct == "Red Gro AAA") {
			colorType = RAAA;
		} else if (ct == "Blue Gro A") {
			colorType = BA;
		} else if (ct == "Blue Gro AA") {
			colorType = BAA;
		} else if (ct == "Blue Gro AAA") {
			colorType = BAA;
		} else if (ct == "Green Gro A") {
			colorType = GA;
		} else if (ct == "Green Gro AA") {
			colorType = GAA;
		} else if (ct == "Green Gro AAA") {
			colorType = GAAA;
		} else if (ct == "Yellow Gro") {
			colorType = Y;
		} else {
			colorType = RA;
		}

		SetValues(colorType);
		SetGrowthCurve();

		Random.seed = System.DateTime.Now.Millisecond;
	}

	// Update is called once per frame
	void Update () {
		if (!placed) {
			return;
		}

		currentOutputWait += outputRate * Time.deltaTime;
		if (currentOutputWait >= OUTPUT_AT) {
			Output();
		}

		currentGrowthAmount += 0.01f;//growthRate * Time.deltaTime;
		if (currentGrowthAmount >= currGrowthNeeded) {
			Grow ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (!placed) {
			return;
		}

		if (other.gameObject.tag == "Red Blop") {
			//for now just staticly add
			currentGrowthAmount += 0.1f;
		} else if (other.gameObject.tag == "Blue Blop") {
			currentGrowthAmount += 0.1f;
		} else if (other.gameObject.tag == "Green Blop") {
			currentGrowthAmount += 0.1f;
		} else if (other.gameObject.tag == "Yellow Blop") {
			currentGrowthAmount += 0.1f;
		} 

		Destroy (other.gameObject);

		blopsTaken++;
	}

	void SetValues(int colorType){
		if (colorType == RA) {
			growthRate = 0.01f;
			outputRate = 0.3f;
			purchaseCost = 15;
		} else if (colorType == RAA) {
			growthRate = 0.012f;
			outputRate = 0.8f;
			purchaseCost = 100;
		} else if (colorType == RAAA) {
			growthRate = 0.015f;
			outputRate = 1.2f;
			purchaseCost = 450;
		} else if (colorType == BA) {
			growthRate = 0.005f;
			outputRate = 1f;
			purchaseCost = 20;
		} else if (colorType == BAA) {
			growthRate = 0.008f;
			outputRate = 2f;
			purchaseCost = 175;
		} else if (colorType == BAAA) {
			growthRate = 0.01f;
			outputRate = 5f;
			purchaseCost = 1000;
		} else if (colorType == GA) {
			growthRate = 0.05f;
			outputRate = 0.1f;
			purchaseCost = 20;
		} else if (colorType == GAA) {
			growthRate = 0.075f;
			outputRate = 0.15f;
			purchaseCost = 175;
		} else if (colorType == GAAA) {
			growthRate = 0.1f;
			outputRate = 0.2f;
			purchaseCost = 1000;
		} else if (colorType == Y) {
			growthRate = 0.001f;
			outputRate = 0.01f;
			purchaseCost = 25000;
		} else {
			growthRate = 0.01f;
			outputRate = 0.3f;
			purchaseCost = 15;
		}
	}
	
	void SetGrowthCurve(){
		growthLevels = new float[MAX_LEVEL];
		for (int i = 0; i < growthLevels.Length; i++) {
			if(i < MAX_LEVEL/2){
				growthLevels[i] = 1f;
			} else  if ( i >= MAX_LEVEL/2 && i < (MAX_LEVEL * 0.80f)) {
				growthLevels[i] = 1.5f;
			} else if ( i >= (MAX_LEVEL * 0.80f)){
				growthLevels[i] = 2f;
			}
		}

		currGrowthNeeded = growthLevels [currentLevel];
	}

	void Grow(){
		if (currentLevel == MAX_LEVEL) {
			return;
		}

		while (currentGrowthAmount > currGrowthNeeded) {
			float x = transform.localScale.x;
			float y = transform.localScale.y;
			float z = transform.localScale.z;
			transform.localScale = new Vector3 (x, y + 0.1f, z);

			currentGrowthAmount = currentGrowthAmount - currGrowthNeeded;
			if(currentGrowthAmount < 0){
				currentGrowthAmount = 0;
			}
			currentLevel++;
			currGrowthNeeded = growthLevels [currentLevel];
		}

	}

	void Output(){
		GameObject blop;

		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;

		//needs to be more complex, put onto path
		int where = (int) Random.Range (1, 5);
		Vector3 blopPosition;
		if (where == 1) {
			blopPosition = new Vector3 (x + 1.0f, y, z);
		} else if (where == 2) {
			blopPosition = new Vector3 (x - 1.0f, y, z);
		} else if (where == 3) {
			blopPosition = new Vector3 (x, y, z + 1.0f);
		} else if (where == 4) {
			blopPosition = new Vector3 (x, y, z - 1.0f);
		} else {
			blopPosition = new Vector3 (x + 1.0f, y, z);
		}

		if (colorType == RA || colorType == RAA || colorType == RAA) {
			blop = (GameObject) Instantiate(Resources.Load("Red Blop"));
			blop.transform.position = blopPosition;
		
		} else if (colorType == BA || colorType == BAA || colorType == BAAA) {
			blop = (GameObject) Instantiate(Resources.Load("Blue Blop"));
			blop.transform.position = blopPosition;

		} else if (colorType == GA || colorType == GAA || colorType == GAAA) {
			blop = (GameObject) Instantiate(Resources.Load("Green Blop"));
			blop.transform.position = blopPosition;

		} else if (colorType == Y) {
			blop = (GameObject) Instantiate(Resources.Load("Yellow Blop"));
			blop.transform.position = blopPosition;

		} else {
			blop = (GameObject) Instantiate(Resources.Load("Red Blop"));
			blop.transform.position = blopPosition;
		}

		currentOutputWait = 0f;

		blopsOutput++;
	}
}
