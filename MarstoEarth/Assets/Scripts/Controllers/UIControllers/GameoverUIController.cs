using Character;
using GoogleMobileAds.Api;
using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverUIController : MonoBehaviour
{
    public ReportContentUIController reportContent;
    private int saveCount;
    public UnityEngine.UI.Button addmobButton;

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    private RewardedAd rewardedAd;
    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += ()=>
    {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
    private void RegisterEventHandlers(RewardedAd ad)
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
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += ()=>
    {
            Debug.Log("Rewarded Ad full screen content closed.");
        addmobButton.interactable = false;
        // Reload the ad so that we can show another as soon as possible.
        LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            addmobButton.interactable = false;
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
                
                
            });
    }

    private void Awake()
    {
        LoadRewardedAd();
    }

    public void GotoTitle()
    {
        // UI 버튼에서 사용
        MapInfo.pauseRequest--;
        SaveReportContent();
        SceneManager.LoadScene("OutGameScene");
        gameObject.SetActive(false);
    }

    public void ShowRewardedAd()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                SpawnManager.Instance.player.Resurrect();

                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                MapInfo.pauseRequest--;
                gameObject.SetActive(false);
            });
        }
    }

    /// <summary>
    /// PP = PlayerPrefs
    /// </summary>
    public void SavePPContent(PlayerSaveInfo playerSaveInfo)
    {
        // JSON 형식으로 직렬화
        string jsonData = JsonUtility.ToJson(playerSaveInfo, true);
        // 지금까지 save들의 수 확인, 없으면 0
        if (PlayerPrefs.HasKey("saveCount"))
            saveCount = PlayerPrefs.GetInt("saveCount");
        else
            saveCount = 0;
        // 현재 Save Idx에 저장
        // ex : save0, save1, save2
        PlayerPrefs.SetString("save" + saveCount.ToString(), jsonData);
        // Save를 하나 추가
        PlayerPrefs.SetInt("saveCount", saveCount + 1);
    }

    public void SaveReportContent()
    {
        // 유저 정보를 JSON 객체로 생성
        PlayerSaveInfo playerSaveInfo = new PlayerSaveInfo();
        // 현재 시간을 가져옴
        DateTime currentDateTime = DateTime.Now;
        // 날짜와 시간을 원하는 형식으로 변환
        string formattedDate = currentDateTime.ToString("yyyy / MM / dd");
        string formattedTime = currentDateTime.ToString("HH : mm");
        playerSaveInfo.date = formattedDate;
        playerSaveInfo.time = formattedTime;
        int intClearedStage = MapInfo.cur_Stage - 1;
        playerSaveInfo.clearedStage = intClearedStage;
        playerSaveInfo.clearedRooms = InGameManager.clearedRooms;
        playerSaveInfo.playedTime = UIManager.Instance.gameInfoUIController.timeUI.text;
        playerSaveInfo.hpCoreAmount = MapInfo.hpCore;
        playerSaveInfo.dmgCoreAmount = MapInfo.dmgCore;
        playerSaveInfo.speedCoreAmount = MapInfo.speedCore;
        playerSaveInfo.core = MapInfo.core;
        playerSaveInfo.maxHp = staticStat.maxHP;
        playerSaveInfo.attack = staticStat.dmg;
        playerSaveInfo.speed = staticStat.speed;
        playerSaveInfo.defence = staticStat.def;
        playerSaveInfo.duration = staticStat.duration;
        playerSaveInfo.range = staticStat.range;
        playerSaveInfo.fileName = currentDateTime.ToString("yyyMMdd") + currentDateTime.ToString("HHmm");

        SavePPContent(playerSaveInfo);
        InitReportContent(playerSaveInfo);
    }

    public void InitReportContent(PlayerSaveInfo playerInfo)
    {
        reportContent.InitContent(playerInfo);
    }
}
