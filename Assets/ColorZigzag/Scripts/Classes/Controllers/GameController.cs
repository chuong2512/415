using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameController {

	public enum Platform {PC, Phone}
	public static Platform platform;
    
    public static GameController instance;

    private bool isGameOver;

    private int forbidLevel;
    private Dictionary <string, bool> forbids;
    
    private bool isGameStarted;

    private static GUIText scoreText;
    public static GUIText moneyText;
    private static GUIImage background;

    private const int maxLineSize = 4;
    private int currentLineSize;
    private float groundBlockSize = 3f;

    private enum Direction {X, Z};
    private Direction groundDirection;
    private Direction playerDirection;
    
    private Vector3 startPosition = new Vector3 (0, 25f, 0);
    private Vector3 currentPosition;
    
    private List <GameObject> groundBlocks;
    private List <GameObject> gates;
    private List <Vector2> changeDirectionPositions;

    private GameObject player;
    public static int playerNumber {

        get {

            return Settings.currentPrefab;
        }
    }

    private int currentLineGate = -1;

    private int playerColorIndex;

    private int blocksToStay;

    private int _score = 0;

    private float coinChance = 0.08f;

    private int score {

        get { return _score; }
        set {

            _score = value;
            scoreText.text = _score + "";
        }
    }

    public void Forbid () {

        forbidLevel ++;
    }

    public void Forbid (string type) {

        if (forbids.ContainsKey (type)) {

            forbids [type] = true;
        } else {

            forbids.Add (type, true);
        }
    }

    public void Allow () {
    
        forbidLevel --;
    }

    public void Allow (string type) {

        if (forbids.ContainsKey (type)) {

            forbids [type] = false;
        } else {

            forbids.Add (type, false);
        }
    }

    private bool IsForbidden () {

        int res = forbidLevel;

        foreach (var f in forbids) {

            if (f.Value) {

                res ++;
            }
        }
        
        return res > 0;
    }
    
    public GameObject currentBlock = null;
    
	public static void OnClick(Vector2 position) {
		
		Ray ray;
		RaycastHit hit;

        if (Settings.scene == Settings.Scene.Game) {

            if (!instance.isGameOver && instance.isGameStarted && (UpdateController.toFixedUpdate != null)) {

                instance.playerColorIndex = (instance.playerColorIndex + 1) % instance.colors.Count;
                instance.player.GetComponent <Renderer> ().material.color = instance.colors [instance.playerColorIndex];
            }
        }

        ray = Camera.main.ScreenPointToRay(position);

		if (Physics.Raycast(ray,out hit, 100)) {

			if (hit.transform.gameObject.name.Contains("GUIButton"))
				GUIController.OnClick(hit.transform.gameObject);
		}
	}

	public static void OnButtonDown(Vector2 position) {
		
		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay(position);

		if (Physics.Raycast(ray,out hit, 100)) {
			
            if (hit.transform.gameObject.name.Contains ("GUIButton"))
				GUIController.OnButtonDown(hit.transform.gameObject);
		}
		
	}
	
    public static void OnOver(Vector2 position) {
		
		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray,out hit, 100)) {
    
            if (hit.transform.gameObject.name.Contains ("GUIButton") && Vector3.Distance (hit.point, hit.transform.position) < hit.transform.localScale.x * 0.3f)
				GUIController.OnOver(hit.transform.gameObject);
		}
		
	}

	public static void OnButtonUp(Vector2 position) {
		
		GUIController.OnButtonUp ();
	}

	public GameController () {

        //UpdateController.Timer (5, () => {AdsController.instance.Show (()=> { }); });
        instance = this;
        
        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		Settings.scene = Settings.Scene.Game;

		AnimationBox.LoadAnimations ();
		AnimationController.Create ();
		new GUIController ();
        new SlideController (1080 / 50f, 1920 / 50f, SlideController.Mode.ReadOnly, 3);

        UpdateController.SetGameCamera ();
        new CameraController ();

		Create ();
	}

    public void Create () {

        UpdateController.Timer (0.2f, () => {

            if (UpdateController.theme == null) {

                UpdateController.theme = AudioController.instance.CreateAudio ("Theme", true, true);
            }
        });
        
        isGameStarted = false;

        Camera.main.transform.localScale = new Vector3 (1, 1, 1);

        forbids = new Dictionary <string, bool> ();

        CameraController.ResizeCamera (1920 / 50f);

        background = new GUIImage ("Textures/Interface/Background" + Random.Range (0, 4), -100, new Vector2 (), CameraController.sizeInMeters, false);
        background.isAddictedToCamera = true;
        background.positionInMeters = new Vector2 (0, 0);
        
        if (moneyText == null) {

            moneyText = new GUIText (Settings.money + "", -1, new Vector2 (0, 0), new Vector2 (0.16f, 0.16f));
            moneyText.isAddictedToCamera = true;
            moneyText.positionInMeters = new Vector2 (CameraController.widthInMeters / 2f - moneyText.sizeInMeters.x / 2f - 0.3f - 2f - 0.7f
                , CameraController.heightInMeters / 2f - moneyText.sizeInMeters.y / 2f - 1.2f);

            moneyText.gameObject.SetActive(false);
            
            var moneyIcon = new GUIImage ("Textures/Interface/MoneyIcon", -1,new Vector2 (), new Vector2 (-1, -1), false);
            moneyIcon.isAddictedToCamera = true;
            moneyIcon.positionInMeters = moneyText.positionInMeters + new Vector2 (1.7f, 0);
        }

        StartMenu ();

        
    }

    private List <Color> colors = new List<Color> () {

        new Color (255 / 255f , 83 / 255f, 83 / 255f)
        , new Color (136 / 255f , 255 / 255f, 83 / 255f)
        , new Color (83 / 255f , 200 / 255f, 255 / 255f)
    };

    private float playerSpeed = 7f;
    private float timeToWait;

    private Vector2 playerPosition {

        get {

            return new Vector2 (player.transform.position.x, player.transform.position.z); 
        }

        set {

            player.transform.position = new Vector3 (
                    value.x
                    , player.transform.position.y
                    , value.y
                );
        }
    }

    private void MoveDownBlock (GameObject block, float startY, float endY, Actions.VoidVoid onEnd = null) {

        UpdateController.LaunchIt (20, 0.015f, (t) => {
            
            var x = (t / 19f) * (t / 19f);

            block.transform.position = new Vector3 (
                    block.transform.position.x
                    , startY * (1 - x) + endY * x
                    , block.transform.position.z
                );
        }, onEnd);
    }

    private void CreateGroundBlock (bool useAnimation = true) {
        
        var newBlock = GamePullController.CreateBlock ();
        newBlock.transform.localScale = new Vector3 (groundBlockSize, groundBlockSize, groundBlockSize);
        newBlock.transform.position = currentPosition;
        newBlock.GetComponent <Renderer> ().material.color = new Color (253 / 255f, 255 / 255f, 83 / 255f);
        newBlock.GetComponent <BoxCollider> ().isTrigger = false;

        groundBlocks.Add (newBlock);

        if (useAnimation) {

            MoveDownBlock (newBlock, startPosition.y + 6f, startPosition.y);
        }

        if (currentLineSize >= maxLineSize) {

            currentLineSize = 1;
            currentLineGate = Random.Range (2, maxLineSize);
            groundDirection = groundDirection == Direction.X ? Direction.Z : Direction.X;

            changeDirectionPositions.Add (new Vector2 (currentPosition.x, currentPosition.z));

        }

        if (currentLineGate == currentLineSize) {

            var gate = GamePullController.CreateBlock ();
            gate.transform.localScale = new Vector3 (groundBlockSize * (groundDirection == Direction.X ? 0.2f : 1)
                , groundBlockSize
                , groundBlockSize * (groundDirection == Direction.Z ? 0.2f : 1));
            gate.transform.position = currentPosition + new Vector3 (0, groundBlockSize, 0);
            gate.transform.rotation = Quaternion.Euler (0, 0, 0);
            gate.GetComponent <Renderer> ().material.color = colors [Random.Range (0, colors.Count)];
            gate.GetComponent <BoxCollider> ().isTrigger = true;
            
            gates.Add (gate);

            if (useAnimation) {

                MoveDownBlock (gate, startPosition.y + 6f + groundBlockSize, startPosition.y + groundBlockSize);
            }
        } else {

            if (Random.value < coinChance) {

                var coin = GamePullController.CreateCoin ();
                coin.transform.localScale = new Vector3 (30, 30, 30);
                coin.transform.rotation = Quaternion.Euler (-90, 0, 0);
                coin.transform.position = currentPosition + new Vector3 (0, groundBlockSize / 2f + 0.5f, 0);
            }
        }

        currentPosition += new Vector3 ((groundDirection == Direction.X ? 1 : 0) * groundBlockSize, 0, (groundDirection == Direction.Z ? 1 : 0) * groundBlockSize);

        currentLineSize ++;
    }

    private void StartGame () {
        
        if (isGameStarted) {

            return;
        }

        groundBlocks = new List<GameObject> ();
        gates = new List<GameObject> ();
        changeDirectionPositions = new List<Vector2> ();

        currentPosition = startPosition;
        groundDirection = Direction.Z;
        playerDirection = Direction.Z;
        currentLineSize = 1;
        currentLineGate = -1;
        playerColorIndex = 0;

        isGameStarted = true;
        isGameOver = false;

        blocksToStay = 3 * (maxLineSize) - 1;

        for (int i = 0; i < blocksToStay; i++) {

            CreateGroundBlock (false);
        }

        scoreText = new GUIText ("0", -1, new Vector2 (0, 0), new Vector2 (0.41f, 0.41f));
        scoreText.isAddictedToCamera = true;
        scoreText.positionInMeters = new Vector2 (0, CameraController.heightInMeters / 2f 
            - scoreText.sizeInMeters.y / 2f - 4f);

        score = 0;
        
        Debug.Log (score);

        CreateGroundBlock ();

        player = GamePullController.CreatePlayer (playerNumber);
        player.transform.rotation = Quaternion.Euler (Settings.Rotation (playerNumber));
        player.transform.position = startPosition + new Vector3 (0, (groundBlockSize + 1f) / 2f, 0);
        player.GetComponent <Renderer> ().material.color = colors [playerColorIndex];

        if (player.GetComponent <CollisionDetector> () == null) {

            player.AddComponent <CollisionDetector> ();
        }

        player.GetComponent <CollisionDetector> ()
                .SetTriggerListeners ((c) => {

                    if (c.gameObject.name.Contains ("Coin")) {
                        
                        Settings.money += 5;
                        moneyText.text = Settings.money + "";
                        GamePullController.DestroyCoin (c.gameObject);
                        AudioController.instance.CreateAudio ("Success");
                        return;
                    }

                    if (c.gameObject.GetComponent <Renderer> ().material.color != 
                        player.GetComponent <Renderer> ().material.color) {
                        
                        AudioController.instance.CreateAudio ("Fail");
                        EndLevel ();
                    } else {
                        
                        AudioController.instance.CreateAudio ("Success");
                        c.gameObject.GetComponent <Renderer> ().material.color = new Color (0.8f, 0.8f, 0.8f);
                    }
                });

        if (player.GetComponent <Rigidbody> () == null) {

            player.AddComponent <Rigidbody> ()
                .useGravity = false;

            player.GetComponent <Rigidbody> ().freezeRotation = true;
        }

        SetCameraPosition ();

        timeToWait = 0f;

    }

    private float Speed (int n) {
        
        return 7f + 10f * Mathf.Min ( Mathf.Pow (n / 160f, 1.3f), 1);
    }

    private void StartMenu () {

        Actions.VoidVoid destroyStartMenu = null;

        var texturesShopButton = new GUIButton ("Textures/Interface/StartMenu/TexturesShopButton", -1, new Vector2 (), new Vector2 (-1, -1));
        texturesShopButton.isAddictedToCamera = true;
        texturesShopButton.positionInMeters = new Vector2 (-texturesShopButton.sizeInMeters.x / 2f - 3f
            , -CameraController.heightInMeters / 2f + texturesShopButton.sizeInMeters.y / 2f + 0.5f + 100 / 50f);
        texturesShopButton.OnClick = () => {

            AnimateButton (texturesShopButton, () => {

                destroyStartMenu ();
                new Shop ();
            });
        };


        var soundsButton = new GUIButton ("Textures/Interface/StartMenu/Sounds" + (Settings.sounds ? "On" : "Off"), -1, new Vector2 (), new Vector2 (-1, -1));
        soundsButton.isAddictedToCamera = true;
        soundsButton.positionInMeters = new Vector2 (0, -CameraController.heightInMeters / 2f + soundsButton.sizeInMeters.y / 2f + 0.5f + 100 / 50f);
        soundsButton.OnClick = () => {

            AnimateButton (soundsButton, () => {

            }, () => {

                if (Settings.sounds) {
                    
                    Settings.sounds = false;
                    Settings.music = false;
                } else {

                    Settings.sounds = true;
                    Settings.music = true;
                }
                soundsButton.textureName = "Textures/Interface/StartMenu/Sounds" + (Settings.sounds ? "On" : "Off");
            });
        };
        
        var exitButton = new GUIButton ("Textures/Interface/StartMenu/ExitButton", -1, new Vector2 (), new Vector2 (-1, -1));
        exitButton.isAddictedToCamera = true;
        exitButton.positionInMeters = new Vector2 (soundsButton.sizeInMeters.x / 2f + 3f
            , -CameraController.heightInMeters / 2f + soundsButton.sizeInMeters.y / 2f + 0.5f + 100 / 50f);
        exitButton.OnClick = () => {

            AnimateButton (exitButton, () => {

                Application.Quit ();
            });
        };
        
        var playButton = new GUIButton ("Textures/Interface/StartMenu/PlayButton", -1, new Vector2 (), new Vector2 (-1, -1));
        playButton.isAddictedToCamera = true;
        playButton.positionInMeters = new Vector2 (0, 1);
        playButton.OnClick = () => {

            GooglePlayServicesController.ReportProgress ("PressPlayButton", 100);

            AnimateButton (playButton, () => {
                
                destroyStartMenu ();
                StartGame ();
            }, () => { }, "Textures/Interface/StartMenu/PlayCirlce");
        };

        var nameText = new GUIImage ("Textures/Interface/StartMenu/GameName", -2f, new Vector2 (), new Vector2 (-1, -1));
        nameText.isAddictedToCamera = true;
        nameText.positionInMeters = new Vector2 (0, 7f);

        destroyStartMenu = () => {

            nameText.Destroy ();
            soundsButton.Destroy ();
            texturesShopButton.Destroy ();
            playButton.Destroy ();
            exitButton.Destroy ();
        };
    }

	private void Update (float deltaTime) {

        if (!isGameOver && isGameStarted) {

		    if (Input.GetKeyDown(KeyCode.Escape)) {
            
                if (!MenusController.RemoveMenus ()) {

                    
                }

            }
        }
        
        if (!IsForbidden ()) {
            
            SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		    SlideController.instance.Update (deltaTime);
        }
        
        Allow ();
	}

    public void AnimateButton (GUIObject target, Actions.VoidVoid onEnd = null, Actions.VoidVoid onMiddle = null
        , string animateTexture = "Textures/Interface/RoundButtonCircle") {

        AudioController.instance.CreateAudio ("Click");

        if (animateTexture != null) {

            var targetCircle = new GUIImage (animateTexture, -1, new Vector2 (), new Vector2 (-1, -1));
            targetCircle.isAddictedToCamera = true;
            targetCircle.positionInMeters = target.positionInMeters;

            UpdateController.LaunchIt (20, Time.fixedDeltaTime, (t) => {

                targetCircle.sizeInMeters = new Vector2 (-1, -1) * (1 * ( 1 - t / 20f) + 1.25f * (t / 20f));
            }, () => {

                targetCircle.Destroy ();
            });
        }

        UpdateController.LaunchIt (10, Time.fixedDeltaTime, (t) => {

            target.sizeInMeters = new Vector2 (-1, -1) * (1 * ( 1 - t / 10f) + 0.75f * (t / 10f));
        }, () => {

            if (onMiddle != null) {

                onMiddle ();
            }

            UpdateController.LaunchIt (10, Time.fixedDeltaTime, (t) => {

                target.sizeInMeters = new Vector2 (-1, -1) * (1 * ( t / 10f) + 0.75f * (1 - t / 10f));
            }, onEnd);
        });
    }

    private void SetCameraPosition () {

        Camera.main.transform.position = player.transform.position + new Vector3 (-13.56f, 49.03f - 27f, -8.8f - 4.8f);
    }

    private void FixedUpdate (float deltaTime) {

        if (timeToWait > 0) {

            timeToWait -= deltaTime;
            return;
        }

        if (!isGameOver && isGameStarted) {
            
            while (groundBlocks.Count < blocksToStay) {

                CreateGroundBlock ();
            }

            playerPosition += new Vector2 ((playerDirection == Direction.X ? 1 : 0) * playerSpeed * deltaTime
                , (playerDirection == Direction.Z ? 1 : 0) * playerSpeed * deltaTime);

            SetCameraPosition ();

            for (int i = changeDirectionPositions.Count - 1; i >= 0; i--) {

                if (Vector2.Distance (playerPosition, changeDirectionPositions [i]) < 0.1f) {

                    playerPosition = changeDirectionPositions [i];
                    playerDirection = playerDirection == Direction.X ? Direction.Z : Direction.X;
                    changeDirectionPositions.RemoveAt (i);
                }
            }

            for (int i = groundBlocks.Count - 1; i >= 0; i--) {

                if (playerPosition.x > groundBlocks [i].transform.position.x + groundBlockSize / 2f
                    || playerPosition.y > groundBlocks [i].transform.position.z + groundBlockSize / 2f) {

                    var toDelete = groundBlocks [i];
                    groundBlocks.RemoveAt (i);

                    MoveDownBlock (toDelete, toDelete.transform.position.y, toDelete.transform.position.y - 6f, () => {

                        GamePullController.DestroyBlock (toDelete);
                    });

                    score ++;

                    if (score % 10 == 0) {

                        Settings.money ++;
                        moneyText.text = Settings.money + "";
                        moneyText.gameObject.SetActive(false);
                    }

                    playerSpeed = Speed (score);
                    Debug.Log (playerSpeed);
                }
            }

            for (int i = gates.Count - 1; i >= 0; i--) {

                if (playerPosition.x > gates [i].transform.position.x + groundBlockSize / 2f
                    || playerPosition.y > gates [i].transform.position.z + groundBlockSize / 2f) {

                    var toDelete = gates [i];
                    gates.RemoveAt (i);

                    MoveDownBlock (toDelete, toDelete.transform.position.y, toDelete.transform.position.y - 6f, () => {

                        GamePullController.DestroyBlock (toDelete);
                    });
                }
            }
        }
	}

    private void EndLevel () {
        
        forbidLevel = 0;
        Forbid ();
        Forbid ();

        isGameOver = true;

        UpdateController.toFixedUpdate = null;

        Settings.bestScore = score;

        var black = new GUIImage ("Textures/Interface/Black", -2, new Vector2 (0, 0), new Vector2 (1920 / 50f, 1920 / 50f));
        black.isAddictedToCamera = true;
        black.positionInMeters = new Vector2 (0, 0);

        var moneyAddedText = new GUIText ("0", -1, new Vector2 (0, 0), new Vector2 (0.31f, 0.31f) * 0.75f);
        moneyAddedText.isAddictedToCamera = true;
        moneyAddedText.text = "+" + score / 10;
        moneyAddedText.positionInMeters = new Vector2 (1f, scoreText.positionInMeters.y - moneyAddedText.sizeInMeters.y / 2f - scoreText.sizeInMeters.y / 2f - 3f);
        
        var moneyIcon = new GUIImage ("Textures/Interface/MoneyIcon", -1,new Vector2 (), new Vector2 (-1, -1), false);
        moneyIcon.isAddictedToCamera = true;
        moneyIcon.positionInMeters = moneyAddedText.positionInMeters - new Vector2 (2f, 0);

        var bestText = new GUIText ("0", -1, new Vector2 (0, 0), new Vector2 (0.31f, 0.31f) * 0.75f);
        bestText.isAddictedToCamera = true;
        bestText.text = (Settings.bestScore == score ? "New record" : "Best " + Settings.bestScore);
        bestText.positionInMeters = new Vector2 (0, moneyAddedText.positionInMeters.y - bestText.sizeInMeters.y / 2f - moneyAddedText.sizeInMeters.y / 2f - 2f);

        /*
        var likeButton = new GUIButton ("Textures/Interface/LevelEnd/LikeButton", -1, new Vector2 (), new Vector2 (-1, -1));
        likeButton.isAddictedToCamera = true;
        likeButton.OnClick = () => {

            AnimateButton (likeButton, () => {
                
                Application.OpenURL (Settings.likeURL);
            });
        };
        
        likeButton.positionInMeters = new Vector2 (-likeButton.sizeInMeters.x / 2f - 2f
            , -CameraController.heightInMeters / 2f + likeButton.sizeInMeters.y / 2f + 0.5f + 100 / 50f);
            */

        var noSpamButton = new GUIButton ("Textures/Interface/LevelEnd/NoAdsButton", -1, new Vector2 (), new Vector2 (-1, -1));
        noSpamButton.isAddictedToCamera = true;
        noSpamButton.OnClick = () => {

            AnimateButton (noSpamButton, () => {

                Debug.Log ("No Ads Try");
                IAPController.instance.BuyConsumable (0, () => {

                    Settings.isAds = false;
                    Debug.Log ("No ads success");
                    AdsController.instance.HideBanner ();
                });
            });
        };

        noSpamButton.positionInMeters = new Vector2 (noSpamButton.sizeInMeters.x / 2f + 2f
            , -CameraController.heightInMeters / 2f + noSpamButton.sizeInMeters.y / 2f + 0.5f + 100 / 50f);

        noSpamButton.gameObject.SetActive(false);
        
        UpdateController.Timer (0.2f, () => {

            var restartText = new GUIText ("0", -1, new Vector2 (0, 0), new Vector2 (0.21f, 0.21f) * 0.75f);
            restartText.isAddictedToCamera = true;
            restartText.text = "TAP TO RESTART";
            restartText.positionInMeters = new Vector2 (0, noSpamButton.positionInMeters.y + restartText.sizeInMeters.y + noSpamButton.sizeInMeters.y + 0.5f);

            var restartButton = new GUIButton ("Textures/Interface/White", -2, new Vector2 (), new Vector2 (100, 100));
            restartButton.isAddictedToCamera = true;
            restartButton.positionInMeters = restartText.positionInMeters;
            restartButton.OnClick = () => {

                Destroy ();
                new GameController ();
            };

            int rnd = Random.Range (0, 4);

            if (rnd == 0) {

                AdsController.instance.Show (() => {

                });
            }

            if (rnd == 1) {

                restartText.positionInMeters += new Vector2 (0, 3);

                //var videoForRewardText = new GUIText ("+20", -0.5f, new Vector2 (), new Vector2 (0.16f, 0.16f));
                //videoForRewardText.isAddictedToCamera = true;
                //videoForRewardText.positionInMeters = new Vector2 (1.22f, -4.27f);

                var videoForRewardButton = new GUIButton ("Textures/Interface/LevelEnd/SpamForRewardButton", -1, new Vector2 (), new Vector2 (-1, -1));
                videoForRewardButton.isAddictedToCamera = true;
                videoForRewardButton.positionInMeters = restartText.positionInMeters - new Vector2 (0, 3);
                videoForRewardButton.OnClick = () => {
                   
                    AnimateButton (videoForRewardButton, () => {
                        
                        AdsController.instance.ShowRewardedAd (() => {
                            
                            Settings.money += 20;
                            moneyText.text = Settings.money + "";
                            videoForRewardButton.Destroy ();
                            //videoForRewardText.Destroy ();
                        });
                    }, () => { }, "Textures/Interface/LevelEnd/SpamForRewardCircle");

                };
            }

        });

        CameraController.cameraMaxSize = 100;
    }

    public void Destroy () {
        
        GamePullController.Destroy ();
        moneyText = null;
        AudioController.instance.ClearSounds ();
        UpdateController.StopAllTimers ();
    }

}
