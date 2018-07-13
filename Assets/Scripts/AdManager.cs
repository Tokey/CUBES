using UnityEngine;
using System.Collections;
using admob;



public class AdManager : MonoBehaviour
{

    public string BannerID;

    public string RewardVideoID;

    public string FullScreenAdID;

    public static AdManager Instance { set; get; }

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);


        #if UNITY_EDITOR
        #elif UNITY_ANDROID
        Admob.Instance().initAdmob(BannerID, FullScreenAdID);
        Admob.Instance().loadInterstitial();
        Admob.Instance().loadRewardedVideo(RewardVideoID);
        #endif
    }

    public void Load()
    {
        #if UNITY_EDITOR
        #elif UNITY_ANDROID
        Admob.Instance().loadInterstitial();
        Admob.Instance().loadRewardedVideo(RewardVideoID);
        #endif
    }
    public void ShowBanner()
    {
        #if UNITY_EDITOR
        #elif UNITY_ANDROID
		Admob.Instance ().showBannerRelative (AdSize.Banner, AdPosition.BOTTOM_CENTER, 1);
        #endif

    }
    public void RemoveBanner()
    {
        #if UNITY_EDITOR
        #elif UNITY_ANDROID
        Admob.Instance().removeBanner();
        #endif
    }
    public void ShowVideo()
    {

        #if UNITY_EDITOR
        #elif UNITY_ANDROID
		if (Admob.Instance ().isInterstitialReady())
		Admob.Instance().showInterstitial();
        Load();
        #endif
    }

    public void ShowRewardedVideo()
    {

        #if UNITY_EDITOR
        #elif UNITY_ANDROID
		if (Admob.Instance().isRewardedVideoReady()) {
           
		Admob.Instance().showRewardedVideo();
         PlayerPrefs.SetInt("Crystals", PlayerPrefs.GetInt("Crystals") + (int)Random.Range(1,50));
         AudioManager.instance.PlaySound("Crystal");
		}
        else {
            //AudioManager.instance.PlaySound("NoCrystal");
        }
        Load();
        #endif
    }

}
