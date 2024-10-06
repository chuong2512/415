using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureList {
	
	public List<Texture> textures;
	public string name;
	
	public TextureList (string name_) {
		name = name_;
		textures = new List<Texture> ();
	}
	
}
