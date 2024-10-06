using UnityEngine;
using System.Collections;

public class WaveElement {
	
	public int type;
	public int count;
	public float startTime;
	public float deltaTime;
	public int road;

	public bool used;
	public float timeForNext;
	
	public WaveElement (int _type,int _count, float _startTime, float _deltaTime, int _road) {
		type = _type;
		count = _count;
		startTime = _startTime;
		deltaTime = _deltaTime;
		road = _road;
		used = false;
		timeForNext = 0;
	}
	
}
