using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Purchasing.Extension;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI m_removeAdsPriceText;
    public TextMeshProUGUI m_get1000GemsPriceText;
    public TextMeshProUGUI m_get5000GemsPriceText;
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

        if(m_removeAdsPriceText){
            m_removeAdsPriceText.text = "...";
        }
        if(m_get1000GemsPriceText){
            m_get1000GemsPriceText.text = "...";
        }
        if(m_get5000GemsPriceText){
            m_get5000GemsPriceText.text = "...";
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

    public void OnPurchaseComplete(Product product){
        // Xử lý khi mua thành công theo product id
        string id = product.definition.id;
        Debug.Log($"Purchase Successfully: {id}" );
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription){
        // Xử lý khi mua hàng không thành công
         string id = product.definition.id;
        Debug.Log($"Purchase Failuer: {id}; {failureDescription.message}" );
    }

    public void OnFetchedAds(Product product){
        //Cập nhật Giá UI theo id
        string id = product.definition.id;
        Debug.Log( $"Fetched {id}");
        Debug.Log( $"Price {product.metadata.localizedPriceString}");
        switch (id)
        {
            case "appname_remove_ads":
            m_removeAdsPriceText.text = product.metadata.localizedPriceString;
            break;
            case "appname_get_gems_1":
             m_get1000GemsPriceText.text = product.metadata.localizedPriceString;
            break;
            case "appname_get_gems_2":
             m_get5000GemsPriceText.text = product.metadata.localizedPriceString;
            break;
            default:
            break;
        }
    }


    void Update()
    {

    }

}
