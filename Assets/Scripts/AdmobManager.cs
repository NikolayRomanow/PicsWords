using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    [SerializeField] private string rewardId;

    private MoneyManager _moneyManager;
    private RewardedAd _rewardedAd;

    private void Awake()
    {
        MobileAds.Initialize(status => { });
        _moneyManager = FindObjectOfType<MoneyManager>();
    }
    
    private void Start()
    {
        _rewardedAd = new RewardedAd(rewardId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(adRequest);

        _rewardedAd.OnUserEarnedReward += (sender, reward) => { _moneyManager.AddMoney(25); };
    }

    public void ShowRewardedApp()
    {
        if (_rewardedAd.IsLoaded())
            _rewardedAd.Show();
    }
}
