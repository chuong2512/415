using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController {
	
	private static List <ObjectAnimation> animations = new List<ObjectAnimation> ();
	private static List <ObjectAnimation> animationsToRemove = new List<ObjectAnimation> ();

	public static void Create () {

		animations = new List<ObjectAnimation> ();
	}

	public static void Add(ObjectAnimation objectAnimation) {

		animations.Add(objectAnimation);
	}
	
	public static void Remove(ObjectAnimation objectAnimation) {

		animationsToRemove.Add(objectAnimation);
	}

	public static void Update (float deltaTime) {

		for (int i = animations.Count - 1; i>=0; i--) {
			animations[i].Update(deltaTime);
		}

		foreach (var animation in animationsToRemove) {
			animations.Remove(animation);
		}

	}

}
