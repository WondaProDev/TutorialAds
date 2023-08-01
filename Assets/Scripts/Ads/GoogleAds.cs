using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

public class GoogleAds : MonoBehaviour
{

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    bool isDebugging = true;

    public delegate void AdEventHandler(bool isError);
    public static event AdEventHandler OnAdCLose;
    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.SetiOSAppPauseOnBackground(true);
        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

#if UNITY_IPHONE
        deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add("d4d7719e-3107-4326-8f92-bc01c9679225");
#endif

        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus status)
    {
        Debug.Log("Initialization complete.");
        LoadInterstitialAd();
        LoadRewardedAd();
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // the main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
        });
    }

    public void LoadInterstitialAd()
    {
        string adUnitId = "";
#if UNITY_ANDROID
        if(isDebugging){
            //This is Test Id
            adUnitId = "ca-app-pub-3940256099942544/1033173712";
        }else{
            //This is Main Id
            adUnitId = "....";
        }
#elif UNITY_IPHONE
        if(isDebugging){
            //This is Test Id
            adUnitId = "ca-app-pub-3940256099942544/4411468910";
        }else{
            //This is Main Id
            adUnitId = "....";
        }
#else
         adUnitId = "unused";
#endif
        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        // Load an interstitial ad
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError loadError) =>
            {
               if (loadError != null)
                {
                    interstitialAd = null;
                    Debug.Log("Interstitial ad failed to load with error: " +
                        loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    interstitialAd = null;
                    Debug.Log("Interstitial ad failed to load.");
                    return;
                }
                Debug.Log("Interstitial ad loaded.");
                interstitialAd = ad;
                ad.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Interstitial ad closed.");
                    OnAdCloseEvent(false);
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    interstitialAd = null;
                    Debug.Log("Interstitial ad failed to show with error: " +
                                error.GetMessage());
                };
            });
    }

     public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            OnAdCloseEvent(true);
            Debug.Log("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
        }
    }

     public void LoadRewardedAd()
    {
        string adUnitId = "";
#if UNITY_ANDROID
        if(isDebugging){
            //This is Test Id
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
        }else{
            //This is Main Id
            adUnitId = "....";
        }
#elif UNITY_IPHONE
        if(isDebugging){
            //This is Test Id
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
        }else{
            //This is Main Id
            adUnitId = "....";
        }
#else
         adUnitId = "unused";
#endif
        // Clean up interstitial before using it
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");
        // Load an interstitial ad
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError loadError) =>
            {
               if (loadError != null)
                {
                    rewardedAd = null;
                    Debug.Log("Rewared ad failed to load with error: " +
                        loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    rewardedAd = null;
                    Debug.Log("Rewared ad failed to load.");
                    return;
                }
                Debug.Log("Rewared ad loaded.");
                rewardedAd = ad;
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    rewardedAd = null;
                    Debug.Log("Rewared ad failed to show with error: " +
                                error.GetMessage());
                };
            });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Show((Reward reward) =>
            {
                OnAdCloseEvent(false);
                // OnAdClosedEvent.Invoke();
                Debug.Log("Rewarded ad granted a reward: " + reward.Amount);
            });
        }
        else
        {
            OnAdCloseEvent(true);
            Debug.Log("Rewarded ad is not ready yet.");
            LoadRewardedAd();
        }
    }


    private void OnAdCloseEvent(bool isError)
    {
        if (OnAdCLose != null)
        {
            OnAdCLose(isError);
            Debug.Log("OnAdEvent: Close");
        }
    }

   
}
 