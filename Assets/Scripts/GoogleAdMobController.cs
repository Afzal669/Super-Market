using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using GoogleMobileAds.Samples;

public class GoogleAdMobController : MonoBehaviour
{
    public string androidInterstitialID;
    public string androidInterstitialVideoID;
    public string androidRewardedVideoID;

    public string androidBannerID;
    [Space(5)]
    public string iOSInterstitialID;
    public string iOSInterstitialVideoID;
    public string iOSRewardedVideoID;

    public string iOSBannerID;

    public static GoogleAdMobController adMobController;
    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private RewardedInterstitialAd rewardedInterstitialAd;
  

    // Helper class that implements consent using the
    // Google User Messaging Platform (UMP) Unity plugin.
    [SerializeField, Tooltip("Controller for the Google User Messaging Platform (UMP) Unity plugin.")]
    private GoogleMobileAdsConsentController _consentController;

    #region UNITY MONOBEHAVIOR METHODS

    //public Image image;

    bool isBannerShowing;
    public  bool isAdsRemoved;


    public bool isAdMobIntialized;

    public delegate void UnlockEnvironment();

    public UnlockEnvironment unlockEnvironment;



    internal static List<string> TestDeviceIds = new List<string>()
        {
            AdRequest.TestDeviceSimulator,
#if UNITY_IPHONE
            "96e23e80653bb28980d3f40beb58915c",
#elif UNITY_ANDROID
            "702815ACFC14FF222DA1DC767672A573"
#endif
        };
    private static bool? _isInitialized;
    private void Awake()
    {
        if (adMobController == null)
        {
            adMobController = this;
            isAdsRemoved = PlayerPrefs.GetInt("RemoveAds", 0) == 1;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void OnRemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAds", 1);
        HideBannerAd();
        isAdsRemoved = true;
    }
    public void Start()
    {
        // Configure TagForChildDirectedTreatment and test device IDs.
        //RequestConfiguration requestConfiguration =
        //    new RequestConfiguration.Builder()
        //    .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified).build();
        //MobileAds.SetRequestConfiguration(requestConfiguration);

        MobileAds.SetiOSAppPauseOnBackground(true);

        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Configure your RequestConfiguration with Child Directed Treatment
        // and the Test Device Ids.
        MobileAds.SetRequestConfiguration(new RequestConfiguration
        {
        });





        // Initialize the Google Mobile Ads SDK.
      //  MobileAds.Initialize(HandleInitCompleteAction);
      InitializeGoogleMobileAds();


      //  StartCoroutine(MultipleRequestBanner());
    }

    private void InitializeGoogleMobileAds()
    {
        // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
        if (_isInitialized.HasValue)
        {
            return;
        }

        _isInitialized = false;

        // Initialize the Google Mobile Ads Unity plugin.
        Debug.Log("Google Mobile Ads Initializing.");
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                _isInitialized = null;
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            _isInitialized = true;
        });

        RequestAndLoadRewardedAd();
        //    RequestBannerAd();
        if (!isAdsRemoved)
        {
            print("Loading IS Ads");
            RequestAndLoadInterstitialAd();
            RequestBannerAd();
        }
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        RequestAndLoadRewardedAd();
        //    RequestBannerAd();
        if (!isAdsRemoved)
        {
            print("Loading IS Ads");
            RequestAndLoadInterstitialAd();
            RequestBannerAd();
        }
        
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            
        });

        GoogleAdMobController.adMobController.isAdMobIntialized = true;
    }

    private void Update()
    {

    }

    #endregion

    #region HELPER METHODS

    private AdRequest CreateAdRequest()
    {
        //if (PlayerPrefs.GetString("ATT", "No") == "No")
        //{
        //    return new AdRequest.Builder()
        //      .AddExtra("npa", "1")  // Non_Personalized Ads
        //      .Build();
        //}
        //return new AdRequest.Builder()
        //      .AddExtra("npa", "0")  // Personalized Ads
        //      .Build();
        return new AdRequest();
    }
            //.AddKeyword("unity-admob-sample")

    public void OnApplicationPause(bool paused)
    {
        // Display the app open ad when the app is foregrounded.
        //if (!paused)
        //{
        //    ShowAppOpenAd();
            
        //}
    }

    #endregion

    #region BANNER ADS
    int _bcounter = 0;

    public void RequestBannerAd()
    {
        // These ad units are configured to always serve test ads.
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = androidBannerID;
#elif UNITY_IPHONE
        string adUnitId = iOSBannerID;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        AdSize adSize = new AdSize(320,50);
        bannerView = new BannerView(adUnitId, adSize,AdPosition.Top);
        //   bannerView.SetPosition(((int)Screen.safeArea.x-320)/2, ((int)Screen.safeArea.yMax-50)/2);

        // image.rectTransform.position = new Vector3((int) Screen.safeArea.xMax/2,(int)Screen.safeArea.yMax/2,0 );

        //  print("____" + bannerView.GetResponseInfo() + "---" + bannerView.GetHeightInPixels() + "--" + bannerView.GetWidthInPixels()+"--"+image.rectTransform.position
        //     );
        //  print("________________" + Screen.safeArea.xMax + "____" + Screen.safeArea.yMax + "____" + Screen.safeArea+ JsonUtility.ToJson(bannerView)+"__"+bannerView);
        // Add Event Handlers

        #region BannerEvents
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());

            // Inform the UI that the ad is ready.
           
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : " + error);
            if (_bcounter <= 5)
                RequestBannerAd();
            else
                Invoke(nameof(ResetBannerCounter), 120);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        #endregion
        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());

        if (bannerView != null)
        {
            bannerView.Hide();

        }
    }


    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    public void ResetBannerCounter()
    {
        _bcounter = 0;
    }

    public void ShowBannerAd()
    {
        print("Show Banner Ads");
        if (isAdsRemoved)
        {
            print("SReturned");
            print("____________ Ads Removed");

            return;
        }
        if (bannerView != null)
        {
            print("Banner showcalled");
            //  DestroyBannerAd();
            isBannerShowing = true;
            bannerView.Show();
        }
        else
        {
            isBannerShowing = false;
            RequestBannerAd();
            bannerView.Show();
           // StartCoroutine(AddBannerCheck());

            print("___Banner Ad is requested again on not loading");
        }
    }
    public void HideBannerAd()
    {
        isBannerShowing = false;
        //if (isAdsRemoved)
        //{
        //    return;
        //}

        if (bannerView != null)
        {
            bannerView.Hide();

        }

       

    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = androidInterstitialID;
#elif UNITY_IPHONE
        string adUnitId = iOSInterstitialID;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }
        //interstitialAd = new InterstitialAd(adUnitId);

        //// Add Event Handlers
        //interstitialAd.OnAdLoaded += HandleInterstitialOnAdLoaded;
        //interstitialAd.OnAdFailedToLoad += HandleInterstitialOnAdFailedToLoad;
        //interstitialAd.OnAdOpening += HandleInterstitialOnAdOpened;
        //interstitialAd.OnAdClosed += HandleInterstitialOnAdClosed;

        // Load an interstitial ad
        InterstitialAd.Load(adUnitId, CreateAdRequest(), (InterstitialAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            interstitialAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlers(ad);

            // Inform the UI that the ad is ready.
        });
    }
     
    public void ShowInterstitialAd()
    {
        print("Interstitial is requested to show");
        if (isAdsRemoved)
        {
            print("____________ Ads Removed");
            return;
        }
        
        if (interstitialAd != null &&  interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            print("Interstitial shown");
        }
        else
        {
           
            RequestAndLoadInterstitialAd();
            print("Interstitial is requested again on not loading");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion

    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = androidRewardedVideoID;
#elif UNITY_IPHONE
        string adUnitId = iOSRewardedVideoID;
#else
        string adUnitId = "unexpected_platform";
#endif


        RewardedAd.Load(adUnitId, CreateAdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }
            // If the operation failed for unknown reasons.
            // This is an unexpected error, please report this bug if it happens.
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            rewardedAd = ad;

            // Register to ad events to extend functionality.
            RegisterEventHandlersReward(ad);
        });


    }
    public bool IsRewardedVideoReady()
    {
        bool _isReady = (rewardedAd != null && rewardedAd.CanShowAd());
        return _isReady;
    }

    public void ShowRewardedAd()
    {
       
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                        reward.Amount,
                                        reward.Type));
            });
            print("Rewarded shown");
        }
        else
        {
           
            RequestAndLoadRewardedAd();
            print("Loading Rewarded Ads again On Not Loading");
        }
    }

    #endregion

    #region BannerAdCallbacks
    public void HandleBannerOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        //isBannerShowing = true;
    }

    public void HandleBannerOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: ");
        GoogleAdMobController.adMobController.isBannerShowing = false;
       // RequestBannerAd();
    }

    public void HandleBannerOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
      //  GoogleAdMobController.adMobController.isBannerShowing = true;

    }

    public void HandleBannerOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
       // GoogleAdMobController.adMobController.isBannerShowing = false;

    }

    public void HandleBannerOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion

    #region InterstitialAdCallbacks
    int _ICounter = 0;

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            RequestAndLoadInterstitialAd();

        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : "
                + error);
            if (_ICounter <= 5)
                RequestAndLoadInterstitialAd();
            else
                Invoke(nameof(ResetInterstitialCounter), 120);
        };
    }

    public void ResetInterstitialCounter()
    {
        _ICounter = 0;
    }

   

    public void HandleInterstitialOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion



    #region RewardedVideoAdCallbacks

    int _RCounter = 0;
    private void RegisterEventHandlersReward(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when the ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            RequestAndLoadRewardedAd();
            Debug.Log("Rewarded ad full screen content closed.");
            Debug.Log(" Add Reward Here");
            InAppFunctions.instance.OnClickPurchaseBtn(20);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : "
                + error);
            if (_bcounter <= 5)
                RequestAndLoadRewardedAd();
            else
                Invoke(nameof(ResetRewardCounter), 120);
            //if (StoreManager._storeManager != null)
            //{
            //    StoreManager._storeManager.FailTowShowReward();
            //}
        };
    }
    public void ResetRewardCounter()
    {
        _bcounter = 0;
    }

    #endregion

    /// <summary>
    /// Opens the AdInspector.
    /// </summary>
    public void OpenAdInspector()
    {
        Debug.Log("Opening ad Inspector.");
        MobileAds.OpenAdInspector((AdInspectorError error) =>
        {
            // If the operation failed, an error is returned.
            if (error != null)
            {
                Debug.Log("Ad Inspector failed to open with error: " + error);
                return;
            }

            Debug.Log("Ad Inspector opened successfully.");
        });
    }

    /// <summary>
    /// Opens the privacy options form for the user.
    /// </summary>
    /// <remarks>
    /// Your app needs to allow the user to change their consent status at any time.
    /// </remarks>
    public void OpenPrivacyOptions()
    {
        _consentController.ShowPrivacyOptionsForm((string error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to show consent privacy form with error: " +
                    error);
            }
            else
            {
                Debug.Log("Privacy form opened successfully.");
            }
        });
    }
}
