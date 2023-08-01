using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayGameManager : MonoBehaviour
{

    public TextMeshProUGUI m_gemsText;

    int gems = 0;

    GoogleAds ads;

    enum FunctionAds { none, addGems, backToHome }
    FunctionAds functionAds = FunctionAds.none;


    // Start is called before the first frame update
    void Start()
    {
        ads = GameObject.FindObjectOfType<GoogleAds>();
        if (ads == null)
        {
            GameObject obj = new GameObject("GoogleAds");
            ads = obj.AddComponent<GoogleAds>();
        }

        if (m_gemsText)
        {
            m_gemsText.text = "Gems: " + gems.ToString();
        }

    }
    void OnEnable()
    {
        GoogleAds.OnAdCLose += OnCloseAds;
    }
    void OnDisable()
    {
        GoogleAds.OnAdCLose -= OnCloseAds;
    }

    void OnCloseAds(bool isError)
    {
        switch (functionAds)
        {
            case FunctionAds.addGems:
                if (isError)
                {
                    // Không add gem khi ko xem dc quảng cáo
                    break;
                }
                //Tải lại quảng cáo đã show
                ads.LoadRewardedAd();
                //Add gem khi xem quảng cáo ok
                AddGem();
                break;
            case FunctionAds.backToHome:
                //Tải lại quảng cáo đã show
                ads.LoadInterstitialAd();
                // Back về home kể cả khi lỗi quảng cáo
                BackToHome();
                break;
            default:
                break;
        }
        functionAds = FunctionAds.none;
    }


    public void ShowAdToAddGem() {
        if(ads){
            functionAds = FunctionAds.addGems;
            ads.ShowRewardedAd();
        }
    }
    public void ShowAdToBackHome() {
         if(ads){
            functionAds = FunctionAds.backToHome;
            ads.ShowInterstitialAd();
        }
     }

    void AddGem(){
        gems++;
         if (m_gemsText)
        {
            m_gemsText.text = "Gems: " + gems.ToString();
        }
    }

    void BackToHome(){
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        asyncOperation.allowSceneActivation = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
