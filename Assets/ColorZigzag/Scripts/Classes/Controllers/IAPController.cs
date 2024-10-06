using System;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Purchasing;
    
public class IAPController /*: IStoreListener */{

    public static IAPController instance;

    //private static IStoreController storeController;                                                                  
    //private static IExtensionProvider storeExtensionProvider;                                                         

    private static string [] productIDConsumable = { "noAds" };              
                
    private static string [] productNameGooglePlayConsumable = { "noAds" }; 
        
    private static Actions.VoidVoid onBought;

    public IAPController () {

        if (instance != null) {

            return;
        }

        instance = this;
        /*
        if (storeController == null) {
            
            InitializePurchasing ();
        }*/
    }
    /*
    public void InitializePurchasing () {

        
        if (IsInitialized ()) {

            return;
        }
            
        
        var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
            
        for (int i = 0; i < productIDConsumable.Length; i++) {

            builder.AddProduct (productIDConsumable [i], ProductType.Consumable, new IDs () {
                { productNameGooglePlayConsumable [i], GooglePlay.Name }});
        }

        UnityPurchasing.Initialize (this, builder);
    }
        
        
    private bool IsInitialized () {

        return storeController != null && storeExtensionProvider != null;
    }*/
        
        
    public void BuyConsumable (int id, Actions.VoidVoid _onBought) {

        onBought = _onBought;

        //BuyProductID (productIDConsumable [id]);
    }
        /*
    void BuyProductID (string productId) {

        
        try {

            if (IsInitialized ()) {

                Product product = storeController.products.WithID (productId);
                
                if (product != null && product.availableToPurchase) {

                    Debug.Log (string.Format ("Purchasing product asychronously: '{0}'", product.definition.id));
                    storeController.InitiatePurchase (product);
                }
                
                else {

                    Debug.Log ("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            
            else {
                
                Debug.Log ("BuyProductID FAIL. Not initialized.");
            }
        }
        
        catch (Exception e) {

            Debug.Log ("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }
        
        
    
    public void RestorePurchases () {

        
        if (!IsInitialized ()) {

            Debug.Log ("RestorePurchases FAIL. Not initialized.");
            return;
        }
            
        
        if (Application.platform == RuntimePlatform.IPhonePlayer || 
            Application.platform == RuntimePlatform.OSXPlayer) {

            Debug.Log ("RestorePurchases started ...");
                
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            
            apple.RestoreTransactions ((result) => {
                
                Debug.Log ("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        
        else {

            Debug.Log ("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
        
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions) {

        Debug.Log ("OnInitialized: OK");
            
        storeController = controller;
        storeExtensionProvider = extensions;
    }
        
        
    public void OnInitializeFailed (InitializationFailureReason error) {

        Debug.Log ("OnInitializeFailed InitializationFailureReason:" + error);
    }
        
    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args) {

        for (int i = 0; i < productIDConsumable.Length; i++) {

            if (String.Equals (args.purchasedProduct.definition.id, productIDConsumable [i], StringComparison.Ordinal)) {

                Debug.Log (string.Format ("ProcessPurchase: OK. Product: '{0}'", args.purchasedProduct.definition.id));

                onBought ();
                return PurchaseProcessingResult.Complete;
            }
        }
        
        Debug.Log (string.Format ("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        return PurchaseProcessingResult.Complete;
    }
        
        
    public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason) {

        Debug.Log (string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
    }
        */
}