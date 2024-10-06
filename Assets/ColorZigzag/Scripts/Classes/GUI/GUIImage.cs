using UnityEngine;
using System.Collections;

public class GUIImage : GUIObject {
	
    public GUIImage (bool make = true) {

        if (make) {
            
		    gameObject = GamePullController.CreateImage ();
		    GUIController.Add(this);
            isClickable = false;
        }
	}

	public GUIImage (string _texture, float _layer, Vector2 _positionInMeters, Vector2 _sizeInMeters, bool _isStatic = true) : this () {

        layer = _layer;
        isStatic = _isStatic;
		textureName = _texture;
        sizeInMeters = _sizeInMeters;
        positionInMeters = _positionInMeters;
    }
	
	public GUIImage (string _texture,float? leftPos,float? topPos,float? rightPos,float? botPos,float widthInPixels
        , float heightInPixels, float _layer = 0, bool _useScale = false) : this () {
		
		layer = _layer;
		useScale = _useScale;
        
		textureName = _texture;
		sizeInPixels = new Vector2 (widthInPixels, heightInPixels);
		
		if (leftPos!=null)
			left = leftPos;
		if (topPos!=null)
			top = topPos;
		
		if (rightPos!=null)
			right = rightPos;
		
		if (botPos!=null)
			bottom = botPos;
		
	}
	
	public override void Destroy () {

		GamePullController.DestroyImage(gameObject);
        isStatic = false;
		GUIController.Remove(this);
		
	}
}
