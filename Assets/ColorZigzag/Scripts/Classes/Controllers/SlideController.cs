using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlideController : IUpdatable {

    private const int version = 3;

    public static SlideController instance;

	public enum Mode {Slide, Zoom, SlideAndZoom, ReadOnly}
    public static Mode mode;

    public static float frictionDelta = 1;
	public static float frictionZoom = 0.002f;

    public static Vector2 directedWidth;
    public static Vector2 directedHeight;
	
	private static bool isSliding;
	private static bool isZooming;

	private static Vector2 firstTouchStart;
	private static Vector2 secondTouchStart;
	public static float zoomDeltaStart;
	public static float mapWidthStart;
	
	private static Vector2 firstTouch;
	private static Vector2 secondTouch;
	
	private static Vector2 delta;
	public static float zoomDelta;

	private static Vector3 cameraPositionStart;

    private static int lastTouches;
    
    /* TODO CHANGE PC TOUCHES
    private static bool isMouseButtonDown = false;

    private static int touchCount {

        get {

            
            if (GameController.platform == GameController.Platform.PC) {

                return (isMouseButtonDown ? 1 : 0);
            }

            return Input.touchCount;
        }
    }
    private static List < Vector2 > touches {

        get {

            List < Vector2 > res = new List<Vector2> ();
            if (GameController.platform == GameController.Platform.PC) {

                res.Add (Input.mousePosition);
            } else {

            }

        }
    }*/

    public SlideController (Vector2 _mapWidth, Vector2 _mapHeight, Mode _mode = Mode.SlideAndZoom, int _version = 1) {

        instance = this;

		directedWidth = _mapWidth;
		directedHeight = _mapHeight;
        mode = _mode;

        firstTouchStart = new Vector2 (-100,-100);
        secondTouchStart = new Vector2 (-100,-100);
        firstTouch = new Vector2 (0,0);
        secondTouch = new Vector2 (0,0);
        delta = new Vector2 (0,0);
        zoomDeltaStart = 0;
        mapWidthStart = 0;
        zoomDelta = 0;
        lastTouches = 0;
        isZooming = false;
        isSliding = false;

        if (version != _version)
            Debug.LogWarning ("Wrong version of SlideController");

	}

	public SlideController (float _mapWidth = 0, float _mapHeight = 0, Mode _mode = Mode.SlideAndZoom, int _version = 1)
        : this (new Vector2 (_mapWidth, _mapWidth), new Vector2 (_mapHeight, _mapHeight), _mode, _version) { }

	private static bool SetCameraPositionX (float x) {

		if (directedWidth.x + directedWidth.y  <= CameraController.widthInMeters) {

			CameraController.cameraPosition = new Vector2 (0 / 2f, CameraController.cameraPosition.y);
			return false;
		}

		if ( (x >  (directedWidth.y - CameraController.widthInMeters/2) )) {
			CameraController.cameraPosition = new Vector2((directedWidth.y - CameraController.widthInMeters/2),Camera.main.transform.position.z);
			return false;
		}

		if ((-x >  (directedWidth.x - CameraController.widthInMeters/2 ) )) {
			CameraController.cameraPosition = new Vector2(-((directedWidth.x - CameraController.widthInMeters/2) )
			                                              ,Camera.main.transform.position.z);
			return false;
		}



		CameraController.cameraPosition = new Vector2(x,Camera.main.transform.position.z);

		return true;
	}
	
	private static bool SetCameraPositionY (float y) {
		
		if (directedHeight.x + directedHeight.y<= CameraController.heightInMeters) {

			CameraController.cameraPosition = new Vector2 (CameraController.cameraPosition.x,0);
			return false;
		}
        
        if ( (y >  (directedHeight.y - CameraController.heightInMeters/2) )) {
			CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x, (directedHeight.y - CameraController.heightInMeters/2));
			return false;
		}

        if ( (-y >  (directedHeight.x - CameraController.heightInMeters/2) )) {
			CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x, - (directedHeight.x - CameraController.heightInMeters/2));
			return false;
		}

		CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x,y);
		
		return true;
	}


	private static void SetTouchesStart () {

		if (GameController.platform == GameController.Platform.PC) {

			if (Input.GetMouseButtonDown (1)) {

				secondTouchStart = Input.mousePosition;
			} else {

				firstTouchStart = Input.mousePosition;
			}
		} else {

			firstTouchStart = Input.touches[0].position;

			if (Input.touchCount > 1) {

				secondTouchStart = Input.touches[1].position;
            }
		}

	}

	
	private static void SetTouches () {
		
		if (GameController.platform == GameController.Platform.PC) {
			firstTouch = Input.mousePosition;
			secondTouch = secondTouchStart;
		} else {
			if (Input.touchCount >=1)
				firstTouch = Input.touches[0].position;

			if (Input.touchCount > 1) 
				secondTouch = Input.touches[1].position;
		}
		
	}

	public static void ResizeCamera(float width) {
		
		if (directedHeight.x + directedHeight.y <= CameraController.GetHeightInMeters (width)) {
	
			CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(Mathf.Max (directedHeight.x, directedHeight.y))
                , Mathf.Max (directedWidth.x, directedWidth.y) ) );
			SetCameraPositionX (CameraController.cameraPosition.x);
			SetCameraPositionY (CameraController.cameraPosition.y);
			return;
		}
		
		if (width <= 19) {

			CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(Mathf.Max (directedHeight.x, directedHeight.y)), 19) );
			SetCameraPositionX (CameraController.cameraPosition.x);
			SetCameraPositionY (CameraController.cameraPosition.y);
			return;
		}

		if (width >= directedWidth.x + directedWidth.y ) {

			CameraController.ResizeCamera( Mathf.Min(CameraController.GetWidthInMeters(Mathf.Max (directedHeight.x, directedHeight.y))
                , Mathf.Max (directedWidth.x, directedWidth.y)) );
			SetCameraPositionX (CameraController.cameraPosition.x);
			SetCameraPositionY (CameraController.cameraPosition.y);
			return;
		}

		if (Camera.main.transform.position.x >=  (directedWidth.y - width/2)) {

			CameraController.cameraPosition = new Vector2((directedWidth.y - width/2),Camera.main.transform.position.z);
		}
		
		if (- Camera.main.transform.position.x >=  (directedWidth.x - width/2) ) {
			
			CameraController.cameraPosition = new Vector2(-((directedWidth.x - width/2) ),Camera.main.transform.position.z);
		}

        if ( (Camera.main.transform.position.z >  (directedHeight.y - CameraController.GetHeightInMeters(width)/2) )) {

			CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x, (directedHeight.y - CameraController.GetHeightInMeters(width)/2));
		}

        if ( (-Camera.main.transform.position.z >  (directedHeight.x - CameraController.GetHeightInMeters(width)/2) )) {

			CameraController.cameraPosition = new Vector2(Camera.main.transform.position.x, (directedHeight.x - CameraController.GetHeightInMeters(width)/2));
		}

		CameraController.ResizeCamera(width);
		SetCameraPositionX (CameraController.cameraPosition.x);
		SetCameraPositionY (CameraController.cameraPosition.y);


	}

	public void Update (float deltaTime) {

        if (GameController.platform == GameController.Platform.PC) {

			float xAxisValue = Input.GetAxis("Horizontal");
			float yAxisValue = Input.GetAxis("Vertical");
			float zAxisValue = Input.GetAxis("Perspective");
			if (xAxisValue*xAxisValue+yAxisValue*yAxisValue>0) {

				CameraController.cameraPosition += new Vector2(xAxisValue, yAxisValue);
			}
			
			if (zAxisValue!=0) {
				CameraController.cameraSize += zAxisValue;
			}

            GameController.OnOver (Input.mousePosition);

			if (Input.GetMouseButtonDown(0)) {

				isSliding = true;
				SetTouchesStart ();
				cameraPositionStart = Camera.main.transform.position;
				GameController.OnButtonDown(Input.mousePosition);
			}	

			if (Input.GetMouseButtonUp(0)) {

				isSliding = false;

                if (Vector2.Distance(firstTouchStart,firstTouch) < 20) {
                    
				    GameController.OnClick(Input.mousePosition);
                }

                GameController.OnButtonUp(Input.mousePosition);
			} 

		} else {

			if (Input.touchCount == 0) {
				
                if (lastTouches >= 1) {

                    GameController.OnButtonUp(new Vector2 (-10007, -10007));
                }	

				isSliding = false;
				isZooming = false;
			}

			
			if (Input.touchCount >= 2) {

				if (!isZooming) {

					isSliding = false;
					isZooming = true;
					SetTouchesStart ();
					zoomDeltaStart = Vector2.Distance(firstTouchStart,secondTouchStart);
					mapWidthStart = CameraController.widthInMeters;
				}
			}

			if (Input.touchCount == 1) {

                GameController.OnOver (Input.GetTouch(0).position);

				if (Input.GetTouch(0).phase == TouchPhase.Began) {
					GameController.OnButtonDown(Input.GetTouch(0).position);
				}
				
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {

					GameController.OnButtonUp(Input.GetTouch(0).position);
                    
                    if (Vector2.Distance(firstTouchStart,firstTouch) < 7) {

                        GameController.OnClick(Input.mousePosition);
                    }
				}

				
				if (Input.GetTouch(0).phase != TouchPhase.Stationary) {

					if (!isSliding) {
						isZooming = false;
						isSliding = true;
						SetTouchesStart ();
						cameraPositionStart = Camera.main.transform.position;
					}
				}

			}
		}
        lastTouches = Input.touchCount;
		SetTouches ();

        if (mode == Mode.Slide || mode == Mode.SlideAndZoom) {

		    if (isSliding) {
			    delta = firstTouchStart - firstTouch;
                
			    if (!SetCameraPositionX(cameraPositionStart.x + delta.x*frictionDelta)) {
                    /*
				    float oldY = firstTouchStart.y;
				    SetTouchesStart ();
				    firstTouchStart = new Vector2 (firstTouchStart.x,oldY); 

				    cameraPositionStart = new Vector3 (Camera.main.transform.position.x,Camera.main.transform.position.y,
				                                       cameraPositionStart.z); */
			    }
			
			    if (!SetCameraPositionY(cameraPositionStart.z + delta.y*frictionDelta)) {
				/*
				    float oldX = firstTouchStart.x;
				    SetTouchesStart ();
				    firstTouchStart = new Vector2 (oldX,firstTouchStart.y); 

				    cameraPositionStart = new Vector3 (cameraPositionStart.x,Camera.main.transform.position.y,
				                                       Camera.main.transform.position.z); */
			    }
		    }
        }

        if (mode == Mode.Zoom || mode == Mode.SlideAndZoom) {

            if (isZooming) {
			    zoomDelta = Vector2.Distance(firstTouch,secondTouch);

			    ResizeCamera((1+(zoomDeltaStart - zoomDelta)*frictionZoom)*mapWidthStart);

		    }
        }

	}


}
