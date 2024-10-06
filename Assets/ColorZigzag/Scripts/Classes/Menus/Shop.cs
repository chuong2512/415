using UnityEngine;
using System.Collections.Generic;

public class Shop {

    private class ShopElement {

        public string name;
        public int price;
        public int prefab;
        public string imageName;
        public bool isBought;

        public ShopElement (string _name, int _price, int _prefab, string _imageName) {

            name = _name;
            price = _price;
            prefab = _prefab;
            isBought = Settings.IsOpenedTexture (name) || name == "Sphere";
            imageName = _imageName;
        }
    }

    private static List <ShopElement> shopElements;
    private static List <GUIObject> objects;

    public static int maxToShow = 4;

    private static int delta;

    private void LoadShopElements () {
        
        shopElements = new List<ShopElement> ();

        shopElements.Add (new ShopElement ("Sphere", 0, 1, "Textures/Shop/Sphere"));
        shopElements.Add (new ShopElement ("Cube", 300, 2, "Textures/Shop/Cube"));
        shopElements.Add (new ShopElement ("Capsule", 300, 3, "Textures/Shop/Capsule"));
        shopElements.Add (new ShopElement ("Tetraeder", 300, 4, "Textures/Shop/Tetraeder"));
    }

    private void DrawShopElement (ShopElement shopElement, Vector2 position, bool isSelected) {

        var preview = new GUIButton (shopElement.imageName, -3, position, new Vector2 (-1, -1), false);
        preview.isAddictedToCamera = true;
        preview.positionInMeters = position;
        objects.Add (preview);

        if (isSelected) {

            var square = new GUIButton ("Textures/Interface/ShopMenu/Square", -0.4f, position, new Vector2 (-1, -1), false);
            square.isAddictedToCamera = true;
            square.positionInMeters = position;
            objects.Add (square);

            var used = new GUIButton ("Textures/Interface/ShopMenu/Used", -0.4f, position, new Vector2 (-1, -1), false);
            used.isAddictedToCamera = true;
            used.positionInMeters = position + new Vector2 (90 / 50f, 90 / 50f);
            objects.Add (used);
        }

        if (shopElement.isBought) {
            
            var opened = new GUIButton ("Textures/Interface/ShopMenu/Opened", -1, position, new Vector2 (-1, -1), false);
            opened.isAddictedToCamera = true;
            opened.positionInMeters = position;
            objects.Add (opened);

            opened.OnClick = () => {

                Settings.currentPrefabName = shopElement.name;
                Settings.currentPrefab = shopElement.prefab;
                Destroy ();
                new Shop (delta);
            };

            var openedName = new GUIText ("" + shopElement.name, -0.5f, position, new Vector2 (0.16f, 0.16f));
            openedName.isAddictedToCamera = true;
            openedName.positionInMeters = position + new Vector2 (-0f, - 104 / 50f);
            objects.Add (openedName);

        } else {

            var blocked = new GUIButton ("Textures/Interface/ShopMenu/Blocked", -1, position, new Vector2 (-1, -1), false);
            blocked.isAddictedToCamera = true;
            blocked.positionInMeters = position;
            objects.Add (blocked);

            blocked.OnClick = () => {

                if (Settings.money >= shopElement.price) {

                    Settings.money -= shopElement.price;
                    GameController.moneyText.text = Settings.money + "";
                    Settings.OpenTexture (shopElement.name);
                    Settings.currentPrefabName = shopElement.name;
                    Settings.currentPrefab = shopElement.prefab;

                    Destroy ();
                    new Shop (delta);
                }

            };
            
            var blockedPrice = new GUIText ("" + shopElement.price, -0.5f, position, new Vector2 (0.2f, 0.2f));
            blockedPrice.isAddictedToCamera = true;
            blockedPrice.positionInMeters = position + new Vector2 (-0.7f, - 85 / 50f);
            objects.Add (blockedPrice);
        }
    }

    public Shop (int _delta = 0) {

        delta = _delta;
        LoadShopElements ();
        objects = new List<GUIObject> ();

        int lastI = 1;

        for (int i = 0; i + delta < shopElements.Count && i < maxToShow; i++) {

            DrawShopElement (shopElements [i + delta], new Vector2 ((i % 2 == 0 ? -1 : 1) * 4f, (i / 2 - 1f) * (-258 / 50f - 1f))
                , Settings.currentPrefabName == shopElements [i + delta].name); 
            lastI ++;
        }

        Vector2 arrowsCenter = new Vector2 (0, ((lastI) / 2 - 1.3f) * (-258 / 50f - 1f));
        
        var arrowLeft = new GUIButton ("Textures/Interface/ShopMenu/ArrowLeft", -1, new Vector2 (), new Vector2 (-1, -1), false);
        arrowLeft.isAddictedToCamera = true;
        arrowLeft.positionInMeters = arrowsCenter - new Vector2 (3, 0);
        arrowLeft.OnClick = () => {

            GameController.instance.AnimateButton (arrowLeft, () => {
            
                if (delta - maxToShow >= 0) {

                    Destroy ();
                    new Shop (delta - maxToShow);
                }
            }, null, null);
        };
        objects.Add (arrowLeft);
        
        var arrowRight = new GUIButton ("Textures/Interface/ShopMenu/ArrowRight", -1, new Vector2 (), new Vector2 (-1, -1), false);
        arrowRight.isAddictedToCamera = true;
        arrowRight.positionInMeters = arrowsCenter + new Vector2 (3, 0);
        arrowRight.OnClick = () => {

            GameController.instance.AnimateButton (arrowRight, () => {
            
                if (delta + maxToShow < shopElements.Count) {

                    Destroy ();
                    new Shop (delta + maxToShow);
                }
            }, null, null);
        };
        objects.Add (arrowRight);

        
        var backButton = new GUIButton ("Textures/Interface/StartMenu/ExitButton", -1, new Vector2 (), new Vector2 (-1, -1), false);
        backButton.isAddictedToCamera = true;
        backButton.positionInMeters = arrowsCenter - new Vector2 (0, 3f);
        backButton.OnClick = () => {

            GameController.instance.AnimateButton (backButton, () => {
                
                Destroy ();
                new GameController ();
            });
        };
        objects.Add (backButton);

    }

    public void Destroy () {

        foreach (var a in objects) {

            a.Destroy ();
        }
    }
}
