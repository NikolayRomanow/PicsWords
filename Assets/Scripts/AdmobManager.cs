using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    private const string _rewardID = "ca-app-pub-4476600945726730/7367746483";
    private RewardedAd _rewardedAd;
    
    private MoneyManager _moneyManager;

    private void Awake()
    {
        MobileAds.Initialize(status => { });
        
        _moneyManager = FindObjectOfType<MoneyManager>();
    }
    
    private void Start()
    {
        _rewardedAd = new RewardedAd(_rewardID);

        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(adRequest);
    }

    public void ShowRewardedApp()
    {
        _rewardedAd.Show();
        _moneyManager.AddMoney(25);
    }
}
