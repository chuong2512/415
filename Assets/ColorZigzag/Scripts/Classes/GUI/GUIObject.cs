using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public abstract class GUIObject {

	private GameObject _gameObject;
	public GameObject gameObject {

        get {

            if (_gameObject == null) {

                Debug.LogWarning ("GameObject is null");
            }

            return _gameObject;
        }
        
        set {

            _gameObject = value;
        }

    }

    public bool isStatic {

        get { return gameObject.isStatic; }
        set { gameObject.isStatic = value; }
    } 

    public bool isAddictedToCamera {

        get {

            return gameObject.transform.parent == Camera.main.transform;
        }

        set {

            if (value) {

                gameObject.transform.parent = Camera.main.transform;
                gameObject.transform.localRotation = Quaternion.Euler (0, 0, 0);
            }

        }
    }

	public bool useScale;

	private  float? _left = null;
	private float? _top = null;
	private float? _bottom = null;
	private float? _right = null;

	public float layer = 0;

    public bool isClickable {

        get { return gameObject.GetComponent <BoxCollider> () == null ? false : gameObject.GetComponent <BoxCollider> ().enabled; }
        set {

            if (gameObject.GetComponent <BoxCollider> () != null) {

                gameObject.GetComponent <BoxCollider> ().enabled = value;
            }
        }
    }

    public Color color {

        get {
            if (gameObject.GetComponent <Renderer> ()) {
                
                if ( gameObject.GetComponent <Renderer> ().material.HasProperty ("_Color")) {

                    return gameObject.GetComponent <Renderer> ().material.color;
                }
                
                return new Color (0, 0, 0, 0);
            }

            return new Color (0, 0, 0, 0);
        }

        set {

            if (gameObject.GetComponent <Renderer> ()) {
                
                if ( gameObject.GetComponent <Renderer> ().material.HasProperty ("_Color")) {

		            gameObject.GetComponent <Renderer> ().material.color = value;
                }
            }
        }
    }
	
	public Texture texture {

		get {return gameObject.GetComponent<Renderer> ().material.mainTexture;}
		set {

            if (value == null) {

                Debug.LogError ("Texture is null: " + gameObject.name + "::" + textureName);
            }
            gameObject.GetComponent<Renderer> ().material.mainTexture = value;
        } 
	}

    private string _textureName;
    public string textureName {

        get { return _textureName; }
        set {

            _textureName = value;
            texture = ResourcesController.Load (_textureName) as Texture;
        } 

    }

	public float? left {
		
		get { return _left;}
		set {
			_left = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_right = null;
			SetPosition ();
		}
	}
	
	public float? right {
		
		get { return _right;}
		set {
			_right = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_left = null;
			SetPosition ();
		}
	}
	
	public float? top {
		
		get { return _top;}
		set {
			_top = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_bottom = null;
			SetPosition ();
		}
	}
	
	public float? bottom {
		
		get { return _bottom;}
		set {
			_bottom = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			_top = null;
			SetPosition ();
		}
	}


	public void SetPosition () {

		float? x = null;
		float? y = null;
		
		if (left != null) {
			x = CameraController.cameraPosition.x - CameraController.widthInMeters/2f + left*CameraController.pixelSize;
		}
		if (right != null) {
			x = CameraController.cameraPosition.x + CameraController.widthInMeters/2f - right*CameraController.pixelSize;
		}
		
		if (top != null) {
			y = CameraController.cameraPosition.y + CameraController.heightInMeters/2f - top*CameraController.pixelSize;
		}
		if (bottom != null) {
			y = CameraController.cameraPosition.y - CameraController.heightInMeters/2f + bottom*CameraController.pixelSize;
		}

		
		if (x != null && y != null) {

			positionInMeters = new Vector2 ((float)x,(float)y);

		}

	}


	protected Vector2 _sizeInPixels = new Vector2 (-1000,-1000);
	public Vector2 sizeInPixels {

		get { return _sizeInPixels; }
		set { 

            if (value == new Vector2 (-1, -1)) {

				_sizeInPixels = new Vector2 (texture.width,texture.height) * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			    SetSize ();
				return;
			}
			_sizeInPixels = value * (useScale?GUIController.width/GUIController.GUIBackgroundWidth:1);
			SetSize ();
		} 


	}

	public void SetSize () {

		if (sizeInPixels == new Vector2 (-1000, -1000))
			return;

		sizeInMeters = sizeInPixels * CameraController.pixelSize; 
	}

	public Vector2 positionInMeters {
		
		get {
            
            if (isAddictedToCamera) {
                
                return new Vector2 (gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);
            } else {
                
                return new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z);
            }
        }
		set {
            
            if (isAddictedToCamera) {
                
                gameObject.transform.localPosition = new Vector3 (value.x,value.y, -layer);
            } else {
                
                gameObject.transform.position = new Vector3 (value.x,GUIController.layer+layer,value.y);
            }
        }
		
	}
	public Vector2 sizeInMeters {
		
		get {
			  if (gameObject == null)
				return new Vector2 ();
			return new Vector2(gameObject.transform.localScale.x,gameObject.transform.localScale.y); }
		set {
			if (gameObject == null)
				return;

			if (value.x < 0 || value.y < 0) {

				gameObject.transform.localScale = new Vector3 (value.x < 0 ? -value.x * texture.width / 50f : value.x
                    , value.y < 0 ? -value.y * texture.height / 50f : value.y
                    , 1);
				return;
			}

			gameObject.transform.localScale = new Vector3 (value.x,value.y,1);
		}
		
	}

    public float rotation {
        
        get { return gameObject.transform.rotation.eulerAngles.z; }
        set {

            gameObject.transform.rotation = Quaternion.Euler (gameObject.transform.rotation.eulerAngles.x
                                                                    , gameObject.transform.rotation.eulerAngles.y
                                                                    , value);
        }

    }

    public void Mirror (bool x, bool y) {

        gameObject.transform.localScale = new Vector3 ((x ? -1 : 1) * gameObject.transform.localScale.x
                                                        , gameObject.transform.localScale.y
                                                        , (y ? -1 : 1) * gameObject.transform.localScale.z);
    }
	
	public abstract void Destroy ();

}
