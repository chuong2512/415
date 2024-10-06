using UnityEngine;
using System.Collections;

public class GUIButton : GUIImage {
	
	protected ObjectAnimation animation;
	
	public string OnClickAnimation = "";
	public string OnButtonDownAnimation = "";
	public string OnButtonUpAnimation = "";
	
	public float OnClickAnimationTime = 0.1f;
	public float OnButtonDownAnimationTime = 0.3f;
	public float OnButtonUpAnimationTime = 0.3f;
    
	public Actions.VoidVoid OnClick;
	public Actions.VoidVoid OnButtonDown;
	public Actions.VoidVoid OnButtonUp;
	public Actions.VoidVoid OnButtonDownOver;


	public GUIButton (bool make = true) : base (false) {

        if (make) {

            gameObject = GamePullController.CreateButton ();
		    animation = new ObjectAnimation (gameObject);
            isClickable = true;
		    GUIController.Add(gameObject,this);
        }
	}


	public GUIButton (string _texture, float _layer, Vector2 _positionInMeters, Vector2 _sizeInMeters, bool _isStatic = true
        , Actions.VoidVoid _OnClick = null, Actions.VoidVoid _OnButtonDown = null
        , Actions.VoidVoid _OnButtonUp = null, Actions.VoidVoid _OnButtonDownOver = null) : this () {

        layer = _layer;
        isStatic = _isStatic;
		textureName = _texture;
        sizeInMeters = _sizeInMeters;
        positionInMeters = _positionInMeters;

        OnClick = _OnClick;
        OnButtonDown = _OnButtonDown;
        OnButtonUp = _OnButtonUp;
        OnButtonDownOver = _OnButtonDownOver;

    }
	
	public GUIButton (string _texture,float? leftPos,float? topPos,float? rightPos,float? botPos,float widthInPixels
        , float heightInPixels, float _layer = 0, bool _useScale = false
        , Actions.VoidVoid _OnClick = null, Actions.VoidVoid _OnButtonDown = null
        , Actions.VoidVoid _OnButtonUp = null, Actions.VoidVoid _OnButtonDownOver = null) : this () {
		
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

        OnClick = _OnClick;
        OnButtonDown = _OnButtonDown;
        OnButtonUp = _OnButtonUp;
        OnButtonDownOver = _OnButtonDownOver;
	}



	public void Click () {

		if (OnClickAnimation != "") {

			animation.Load(OnClickAnimation,-OnClickAnimationTime);
			animation.Play(-1,OnClick);

		} else {
			if (OnClick != null)
				OnClick ();
		}

	}
	
	public void ButtonDown () {
		
		if (OnButtonDownAnimation != "") {
			
			animation.Load(OnButtonDownAnimation,-OnButtonDownAnimationTime);
			animation.Play(-1,OnButtonDown);
			
		} else {
			if (OnButtonDown!=null)
				OnButtonDown ();
		}
		
	}

    
	public void ButtonDownOver () {
		
		if (OnButtonDownOver!=null)
			OnButtonDownOver ();
		
	}
	
	public void ButtonUp () {
		
		if (OnButtonUpAnimation != "") {
			
			animation.Load(OnButtonUpAnimation,-OnButtonUpAnimationTime);
			animation.Play(-1,OnButtonUp);
			
		} else {
			if (OnButtonUp != null) 
				OnButtonUp ();
		}
		
	}

	public override void Destroy () {

		GamePullController.DestroyButton(gameObject);
		animation.Destroy ();
        isStatic = false;
		GUIController.Remove(gameObject,this);
	}


}
