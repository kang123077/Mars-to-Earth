using System;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameManager : Singleton<InGameManager>
{
    public static int clearedRooms = 0; // 클리어한 룸 수를 저장하는 변수
    public static int clearedBossRoom = 0;
    public GameObject cardUICon;
    public GameObject reinforceCardUICon;
    public CardInfo cardInfo;
    public ReinforceInfo reinforceInfo;
    public List<Skill.Skill> inGameSkill;

    // "ca-app-pub-3459570317181089/8456868389"; 실제광고
    // Test= "ca-app-pub-3940256099942544/5224354917"; 테스트
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3459570317181089/8456868389";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitId = "unused";
#endif

    public RewardedAd rewardedAd;

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
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            MapInfo.isRevive = true;
            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            MapInfo.isRevive = true;
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

    protected override void Awake()
    {
        base.Awake();
        LoadRewardedAd();
        // 스킬들을 리스트로 만듦
        inGameSkill = ResourceManager.Instance.skills.ToList();
    }

    private void Start()
    {
        // CardUI 부모 객체를 끔
        cardUICon.SetActive(false);
        reinforceCardUICon.SetActive(false);
        InitInGame();
    }

    private void TriggerEvent()
    {
        MapInfo.pauseRequest++;
        AudioManager.Instance.PlayEffect(1);
        AudioManager.Instance.PauseSource();
        UIManager.Instance.aimImage.gameObject.SetActive(false);
        cardUICon.SetActive(true);
        cardInfo.CardInit();
    }

    // 맵 클리어 시 발동 함수
    public void OnRoomCleared()
    {
        clearedRooms++;
        if (clearedRooms % 1 == 0 && !CombatUI.fullCheck == true)
        {
            TriggerEvent();
        }
        else if (clearedRooms % 1 == 0 && CombatUI.fullCheck == true)
        {
            ReinforceEvent();
        }
    }

    private void ReinforceEvent()
    {
        reinforceCardUICon.SetActive(true);
        reinforceInfo.ReinforceAdd();
    }

    public void OnBossCleared()
    {
        clearedBossRoom++;
        Vector3 point = SpawnManager.Instance.playerTransform.position;
        point.y = 0.8f;

        for (ushort i = 0; i < 8; i++)
        {
            SpawnManager.Instance.DropItem(point + Vector3.right *
                SpawnManager.rand.Next(-8, 8) + Vector3.forward * SpawnManager.rand.Next(-8, 8), EnemyPool.Boss);
        }
        Character.staticStat.ResetValues();
    }

    public void InitInGame()
    {
        MapManager.Instance.GenerateMapCall();
        foreach (PathController path in MapManager.Instance.paths)
        {
            path.InitPath();
        }
        SpawnManager.Instance.InitSpawn();
    }
}
