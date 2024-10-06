using UnityEngine;
using System.Collections;

public class GUIImageAlpha : GUIImage {
	
	
    private readonly bool isExpensive;

	
	public GUIImageAlpha (bool _isExpensive  = false) : base (false) {
		
        if (isExpensive)
		    gameObject = GamePullController.CreateImageAlphaExpensive ();
        else
		    gameObject = GamePullController.CreateImageAlpha ();

        isExpensive = _isExpensive;
		GUIController.Add(this);
	}
	
    
	public GUIImageAlpha (string _texture, float _layer, Vector2 _positionInMeters, Vector2 _sizeInMeters, bool _isStatic = true
        , bool _isExpensive  = false) : this (_isExpensive) {

        layer = _layer;
        isStatic = _isStatic;
		textureName = _texture;
        sizeInMeters = _sizeInMeters;
        positionInMeters = _positionInMeters;
    }
	
	public GUIImageAlpha (string _texture,float? leftPos,float? topPos,float? rightPos,float? botPos,float widthInPixels
        , float heightInPixels, float _layer = 0, bool _useScale = false, bool _isExpensive = false) : this (_isExpensive) {

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

        if (isExpensive) 
		    GamePullController.DestroyImageAlphaExpensive(gameObject);
        else
		    GamePullController.DestroyImageAlpha(gameObject);

		GUIController.Remove(this);
		
	}
}
