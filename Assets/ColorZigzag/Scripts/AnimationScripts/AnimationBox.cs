using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AnimationBox {
	
	private static List <TextureList> animations_ = new List<TextureList> ();
	private static List <string> names = new List<string> ()
	#region Animation names
		{ "CockroachRun"
		, "AssasinWalk"
		, "AssasinKill"
		, "StandartBearerWalk"
		, "Bang"
		, "BangNoFire"
		, "GuardsmanWalk"
		, "BtnStartOnButtonUp"
		, "BtnStartOnButtonDown"
		, "CannoneerTowerAttack"
		};
	#endregion

	private static bool isLoaded = false;

	public static void Load(string name){
		
		TextureList target = new TextureList(name);
		Object [] ResourcesList = Resources.LoadAll("Animations/"+name, typeof(Texture));

		target.textures = new List<Texture> ();
		for (int i = 0; i < ResourcesList.Length; i++){
			target.textures.Add(ResourcesList[i] as Texture);
		}
		
		animations_.Add(target);
	}

	
	public static TextureList GetAnimation(string name){
		
		foreach (TextureList result in animations_){
			if (result.name == name) return result;
		}
		return null;
	}
	
	public static void LoadAnimations () {

		if (isLoaded)
			return;

		foreach (string animation in names){
			Load(animation);
		}
		isLoaded = true;
	}
}
