using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private float kitFastBuyValue;
    [SerializeField] private float kitAccurateBuyValue;

    [SerializeField] private int kitFastQuantityToBuy;
    [SerializeField] private int kitAccurateQuantityToBuy;

    [Header("UI Shop")]
    [SerializeField] private GameObject grpConfirmation;
    [SerializeField] private GameObject grpBuyConfirmation;
    [SerializeField] private GameObject grpBuyConfirmed;

    [SerializeField] private Text txtQuantityFastKit;
    [SerializeField] private Text txtQuantityAccurateKit;
    [SerializeField] private Text txtBuyValueFastKit;
    [SerializeField] private Text txtBuyValueAccurateKit;

    private bool isBuyingKitFast;
    private bool isBuyingKitAccurate;

    private void Start()
    {
        txtQuantityFastKit.text = kitFastQuantityToBuy.ToString();
        txtQuantityAccurateKit.text = kitAccurateQuantityToBuy.ToString();
        txtBuyValueFastKit.text = "R$ " + kitFastBuyValue.ToString();
        txtBuyValueAccurateKit.text = "R$ " + kitAccurateBuyValue.ToString();
    }
    
    private void ShowConfirmationGroup(bool state)
    {
        grpConfirmation.SetActive(state);
        grpBuyConfirmation.SetActive(state);
    }

    public void BuyKitFast()
    {
        isBuyingKitFast = true;
        ShowConfirmationGroup(true);
    }

    public void BuyKitAccurate()
    {
        isBuyingKitAccurate = true;
        ShowConfirmationGroup(true);
    }

    public void BuyConfirmation()
    {
        if (isBuyingKitFast)
        {
            DataManager.instance.AddKit(Kit.KitType.FAST, kitFastQuantityToBuy);
            isBuyingKitFast = false;
            grpBuyConfirmation.SetActive(false);
            grpBuyConfirmed.SetActive(true);
        }
        else if (isBuyingKitAccurate)
        {
            DataManager.instance.AddKit(Kit.KitType.ACCURATE, kitAccurateQuantityToBuy);
            isBuyingKitAccurate = false;
            grpBuyConfirmation.SetActive(false);
            grpBuyConfirmed.SetActive(true);
        }
    }

    public void BuyDenied()
    {
        isBuyingKitFast = false;
        isBuyingKitAccurate = false;
        ShowConfirmationGroup(false);
    }

}
