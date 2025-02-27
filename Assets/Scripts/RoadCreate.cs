﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreate : MonoBehaviour
{

	public GameObject road;

	public GameObject coin;

	public GameObject walker;

	// Use this for initialization
	void Start()
	{

		for (int i = 0; i < 100; i++)
		{
			if(i < 20 && i != 15) //15番目はGoalがある
				Instantiate(road, new Vector3(0, 0, i * 60), Quaternion.Euler(-90, 0, 0));

			Instantiate(coin, new Vector3(Random.Range(-9.0f, 9.0f), 12, i * Random.Range(14, 16)), Quaternion.identity);
			Instantiate(walker, new Vector3(Random.Range(-9.0f, 9.0f), 10.5f, (i + 5) * Random.Range(14, 16)), Quaternion.identity);
		}

	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
