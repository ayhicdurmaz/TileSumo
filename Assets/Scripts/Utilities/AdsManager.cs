using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{


    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    AdRequest bannerRequest;
    

    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AdsManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    void Start() {
        try
        {
            //Initialize google ads
            MobileAds.Initialize(initStatus => {});
            SceneManager.activeSceneChanged += OnSceneChanged;
            RequestBanner();
            RequestInterstitial();
            CreateAndLoadRewardedAd();
        }
        catch 
        {
            Debug.Log("Error!!!!!");
        }
        
    }


    private  void OnSceneChanged(Scene scene, Scene _scene) {
        bannerView.LoadAd(bannerRequest);

        if(Singleton.getSceneChange%4 == 0){
            Debug.Log("Interstitial Ad Must Show");
            if (this.interstitial.IsLoaded()) {
                this.interstitial.Show();
                RequestInterstitial();
            }
        }  

    }

    public void UserChoseToWatchAd() {
        if (this.rewardedAd.IsLoaded()) {
            this.rewardedAd.Show();
        }
    }


    private void RequestBanner() {
        string adUnitId = Singleton.bannerID;

        this.bannerView = new BannerView(adUnitId, AdSize.IABBanner, AdPosition.Bottom);

        // Create an empty ad request.
        bannerRequest = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(bannerRequest);
    }


    private void RequestInterstitial() {

        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    public void CreateAndLoadRewardedAd() {

        this.rewardedAd = new RewardedAd(Singleton.rewardedID);

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleUserEarnedReward(object sender, Reward args) {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }


    public void HandleRewardedAdClosed(object sender, EventArgs args) {
        this.CreateAndLoadRewardedAd();
    }
}
