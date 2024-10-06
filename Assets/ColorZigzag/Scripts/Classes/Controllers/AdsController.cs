using UnityEngine;
using System.Collections;
//using GoogleMobileAds.Api;
//using UnityEngine.Advertisements;

public class AdsController {

    public static AdsController instance;

    public Actions.VoidVoid onEnd;

    //private InterstitialAd interstitial;
    //private BannerView bannerView;

    private void RequestInterstitial () {
       
        Debug.Log ("RequestInterstitial");
        
        #if UNITY_ANDROID
            string adUnitId = Settings.adsIdAndroid;
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif
        /*
        interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);

        
        interstitial.AdFailedToLoad += (a, b) => {

            Debug.Log (a + "__AD_FAIL__" + b);
        };

        interstitial.AdLoaded += (a, b) => {

            Debug.Log (a + "__AD_COOL__" + b);
        };
        
        interstitial.AdClosed += (a, b) => {

            onEnd ();
        };
        */
    }
   
    private void RequestBanner () {

        #if UNITY_ANDROID
            string adUnitId = Settings.bannerIdAndroid;
        #elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
        #else
            string adUnitId = "unexpected_platform";
        #endif
        /*
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);*/
    }

    public void Show (Actions.VoidVoid _onEnd) {
        
        onEnd = _onEnd;

        Debug.Log ("TryShow");
        /*
        if (interstitial.IsLoaded()) {

            Debug.Log ("Show");
            interstitial.Show();
            RequestInterstitial ();
            onEnd ();
        } else {
        */
            onEnd ();
        //}
        
    }

    

    public void ShowBanner () {
        
        Debug.Log ("TryShowBanner");
        //bannerView.Show();
    }

    public void HideBanner () {
        
        Debug.Log ("HideBanner");
        /*
        if (bannerView != null) {
            
            bannerView.Hide();
        }*/
    }

    public void ShowUnityAd (Actions.VoidVoid _onEnd) {
        
        
        onEnd = _onEnd;

        //ShowOptions options = new ShowOptions();
        //options.resultCallback = HandleShowResult;

        //Advertisement.Show ("video", options);
        
    }

    public void ShowRewardedAd (Actions.VoidVoid _onEnd) {
        
        onEnd = _onEnd;

        Debug.Log ("Try Show Rewarded Ad");

        /*if (Advertisement.IsReady ("rewardedVideo")) {

            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show ("rewardedVideo", options);
        }*/
    }
    /*
    private void HandleShowResult (ShowResult result) {

        switch (result) {
            case ShowResult.Finished:

                Debug.Log("The ad was successfully shown.");
                onEnd ();
                break;

            case ShowResult.Skipped:

                Debug.Log("The ad was skipped before reaching the end.");
                break;

            case ShowResult.Failed:

                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }*/
    
	public AdsController () {

        instance = this;
        RequestInterstitial ();
        RequestBanner ();
    }
}
