using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectAnimation {
	
	private  List<Texture> textures = new List<Texture> ();
	private GameObject gameObject;
	private  float FPS; 
	private  int currentTexture = 0;
	private  float deltaTime = 0;
	private  int slidesCount;

	private bool ignorePause;

	private Actions.VoidVoid OnEnd;

	
	public ObjectAnimation(string _name, GameObject _gameObject, float _FPS = 16, bool _ignorePause = false) {

		ignorePause = _ignorePause;
		AnimationController.Add(this);
		gameObject = _gameObject;
		Load(_name, _FPS);
	}
	
	public ObjectAnimation(GameObject _gameObject, bool _ignorePause = false) {

		ignorePause = _ignorePause;
		AnimationController.Add(this);
		gameObject = _gameObject;
	}
	
	public void Load(string _name, float _FPS = 16){

		if (AnimationBox.GetAnimation(_name) != null)
			textures = AnimationBox.GetAnimation(_name).textures; 
		else 
			Debug.LogWarning("No animation '" + _name+"'");

		if (_FPS<0) 
			FPS = -textures.Count/_FPS;
		else 
			FPS = _FPS;
	}
	
	private void SetTexture (int index=-1){
		if (index == -1) 
			index = currentTexture;

		if (index >= textures.Count) 
			return;

		gameObject.GetComponent<Renderer> ().material.mainTexture = textures[index];
	}
	
	public void Play(int _slidesCount, Actions.VoidVoid _OnEnd = null) { // -1 - Play one Circle, -2 Play forever
		OnEnd = _OnEnd;

		if (_slidesCount == -1 ) 
			slidesCount = textures.Count; 
		else
			slidesCount = _slidesCount;

		currentTexture = 0;
		deltaTime = 0;
		SetTexture ();
	}
	
	public void Stop (){
		slidesCount = 0;
		currentTexture = 0;
		deltaTime = 0;
		SetTexture ();
	}
	
	public void Update(float _deltaTime) {

		//if ((GameController.isPaused || !GameController.isStarted) && !ignorePause)
		//	return;

		if (slidesCount!=0) {
			deltaTime +=_deltaTime;

			if (deltaTime>=1/FPS) {
				deltaTime=0;

				if (slidesCount > 0){
					slidesCount--;

					if (slidesCount == 0) 
						if (OnEnd != null) {
							OnEnd ();
					}
				}

				currentTexture %= textures.Count;
				currentTexture++;
				SetTexture ();
			}
		
		}
		
	}

	public void Destroy () {

		AnimationController.Remove(this);
	}
	
}
