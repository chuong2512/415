using UnityEngine;
using System.Collections;

public sealed class GUIText : GUIObject {

    public enum FontName {Font0, Font1, Font2, Font3, Font4, Font5, Font6, Font7 };

    public FontName fontName {

        set {

            switch (value) {
                
                case FontName.Font0:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont") as Texture;
                    break;
                case FontName.Font1:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont1") as Texture;
                    break;
                case FontName.Font2:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont2") as Texture;
                    break;
                case FontName.Font3:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont3") as Texture;
                    break;
                case FontName.Font4:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont4") as Texture;
                    break;
                case FontName.Font5:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont5") as Texture;
                    break;
                case FontName.Font6:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont6") as Texture;
                    break;
                case FontName.Font7:
                    texture = ResourcesController.Load ("Fonts/Textures/TextFont7") as Texture;
                    break;
                default: 
                    Debug.LogError ("Wrong FontName");
                    break;
            }
        }
    }

    public TextAnchor anchor {

        get { return gameObject.GetComponent <TextMesh> ().anchor; }
        set { gameObject.GetComponent<TextMesh> ().anchor = value; }
    }

    public bool isTranslatable;
    private string translatingText;

    public string text {

		get { return gameObject.GetComponent<TextMesh> ().text; }
		set {
            
            translatingText = value;
            gameObject.GetComponent<TextMesh> ().text = Settings.TranslateText (isTranslatable ? Settings.GetText(translatingText) : value);
        }
	}

    public void OnLanguageChange () {

        if (isTranslatable) {

            text = translatingText;
        }

        gameObject.GetComponent <TextMesh> ().font = ResourcesController.Load ("Fonts/TextFont" + Settings.language) as Font;
    }

    public GUIText () {

		gameObject = GamePullController.CreateText ();
        OnLanguageChange ();
		GUIController.Add(this);
        isClickable = false;
	}

    public GUIText (string _text, float _layer, Vector2 _positionInMeters, Vector2 _sizeInMeters, FontName _fontName = FontName.Font0
        , bool _isStatic = true, TextAnchor _anchor = TextAnchor.MiddleCenter, bool _isTranslatable = false) : this () {
        
        isTranslatable = _isTranslatable;
        layer = _layer;
        sizeInMeters = _sizeInMeters;
        isStatic = _isStatic;
        text = _text;
        positionInMeters = _positionInMeters;
        anchor = _anchor;
        fontName = _fontName;
    }

    public GUIText (string _text, float? _left, float? _top, float? _right, float? _bottom
        , Vector2 _sizeInPixels, FontName _fontName = FontName.Font0, float _layer = 0
        , TextAnchor _anchor = TextAnchor.MiddleCenter, bool _useScale = false, bool _isTranslatable = false) : this () {

        isTranslatable = _isTranslatable;

        layer = _layer;
		useScale = _useScale;

        sizeInPixels = _sizeInPixels;
		
		if (_left != null)
			left = _left;
		if (_top != null)
			top = _top;
		
		if (_right != null)
			right = _right;
		
		if (_bottom != null)
			bottom = _bottom;

        text = _text;
        anchor = _anchor;
        fontName = _fontName;
    }
    
	public override void Destroy () {

        GUIController.Remove (this);
		GamePullController.DestroyText(gameObject);
        gameObject = null;
	}
}
