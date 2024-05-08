using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class AdmobManager : MonoBehaviour
{
    public string appId = "";

    [SerializeField]
    private GameObject errorPanel;

#if UNITY_ANDROID
    string bannerId = "ca-app-pub-3940256099942544/9214589741";
    string interId = "ca-app-pub-3940256099942544/1033173712";
    string rewardedId = "ca-app-pub-3940256099942544/5224354917";
    string nativeId = "ca-app-pub-3940256099942544/2247696110";
# elif UNITY_IPHONE
    string bannerId = "ca-app-pub-1150107775860771/7082272260";
    string interId = "ca-app-pub-1150107775860771/3143027259";
    string rewardedId = "ca-app-pub-1150107775860771/7469338974";
    string nativeId = "ca-app-pub-1150107775860771/9305891171";
# endif

    BannerView bannerView;
    InterstitialAd interstitialAd;
    RewardedAd rewardedAd;
    // NativeAd nativeAd;

    public static AdmobManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Ads Initialsed !!");
        });
    }

    #region Banner

    public void LoadBannerAd(AdSize adSize)
    {
        CreateBannerView(adSize);

        ListenToBannerEvents();

        if (bannerView == null)
        {
            CreateBannerView(adSize);
        }

        var adRequest = new AdRequest();
        // adRequest.Keywords.Add("unity-admob-sample");
        adRequest.Extras.Add("collapsible", "bottom");
        bannerView.LoadAd(adRequest);
    }

    void CreateBannerView(AdSize adSize)
    {
        if (bannerView != null)
        {
            DestroyBannerAd();
        }
        bannerView = new BannerView(bannerId, adSize, AdPosition.Bottom);
    }

    void ListenToBannerEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Banner view paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }

    #endregion

    #region Interstital

    public void LoadInterstitialAd()
    {
        // Time.timeScale = 0;
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        var adRequest = new AdRequest();
        // adRequest.Keywords.Add("unity-admob-sample");

        InterstitialAd.Load(interId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Time.timeScale = 1;
                return;
            }

            interstitialAd = ad;
            InterstialEvent(interstitialAd);
        });
    }

    public void ShowInterstitalAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void InterstialEvent(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Interstitial ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Time.timeScale = 1;
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Time.timeScale = 1;
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region Rewarded

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();
        // adRequest.Keywords.Add("unity-admob-sample");

        RewardedAd.Load(rewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                return;
            }

            rewardedAd = ad;
            RewardedAdEvent(rewardedAd);
        });
    }

    public void ShowRewardedAd(string rewardType = "NoReward", int rewardAmount = 0)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                switch (rewardType)
                {
                    case "NoReward":
                        break;
                    case "Coin":
                        // CoinReward(rewardAmount);
                        // AudioManager.Instance.PlaySFX(AudioManager.Instance.onPayButton);
                        break;
                    default:
                        break;
                }
            });
        }
        else
        {
        }
    }

    public void RewardedAdEvent(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log("Rewarded ad paid {0} {1}." +
                adValue.Value +
                adValue.CurrencyCode);
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
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    #endregion

    #region Native

    public Image img;

    // [Obsolete]
    // public void RequestNativeAd()
    // {
    //     AdLoader adLoader = new AdLoader.Builder(nativeId).ForNativeAd().Build();

    //     adLoader.OnNativeAdLoaded += this.HandleNativeAdLoaded;
    //     adLoader.OnAdFailedToLoad += this.HandleNativeAdFailToLoad;

    //     adLoader.LoadAd(new AdRequest.Builder().Build());
    // }

    // private void HandleNativeAdFailToLoad(object sender, AdFailedToLoadEventArgs e)
    // {
    //     if (errorPanel)
    //     {
    //         /*        errorPanel.SetActive(true);*/
    //     }
    // }

    // private void HandleNativeAdLoaded(object sender, NativeAdEventArgs e)
    // {
    //     this.nativeAd = e.nativeAd;

    //     Texture2D iconTexture = this.nativeAd.GetIconTexture();
    //     Sprite sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.one * 0.5f);

    //     img.sprite = sprite;
    // }

    #endregion

    #region extra

    // void CoinReward(int coinAmount)
    // {
    //     GameSharedUI.Instance.UpdateCoinsUIText(GameManager.Instance.GetCoins(), coinAmount);
    //     GameManager.Instance.AddCoins(coinAmount);
    // }

    // void DiamondReward(int diamondAmount)
    // {
    //     GameSharedUI.Instance.UpdateDiamondsUIText(GameManager.Instance.GetDiamonds(), diamondAmount);
    //     GameManager.Instance.AddDiamons(diamondAmount);
    // }

    #endregion
}
