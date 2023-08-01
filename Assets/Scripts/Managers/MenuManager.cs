using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    GoogleAds ads;
    void Start()
    {
        ads = GameObject.FindObjectOfType<GoogleAds>();
        if (ads == null)
        {
            GameObject obj = new GameObject("GoogleAds");
            ads = obj.AddComponent<GoogleAds>();
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
        if(isError){
            /// Xử lý nếu có lỗi
        }
        // Xử lý chuyển màn hình ở đây
        ads.LoadInterstitialAd();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = true;
    }
    
    // Update is called once per frame
    public void ShowAdsToNewScene()
    {
        if(ads != null){
            ads.ShowInterstitialAd();
        }
    }

    void Update()
    {

    }

}
