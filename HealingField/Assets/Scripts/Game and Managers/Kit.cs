using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kit : MonoBehaviour
{
    public static Kit instance;

    [SerializeField] private GameObject kitPrefab;

    [Header("Fast Kit")]
    [SerializeField] private int fastKitTotal;
    [SerializeField] private float useTimeFastKit;

    [Header("Accurate Kit")]
    [SerializeField] private int accurateKitTotal;
    [SerializeField] private float useTimeAccurateKit;

    public int FastKitTotal { get { return fastKitTotal; } }
    public int AccurateKitTotal { get { return accurateKitTotal; } }

    public enum KitType{ FAST , ACCURATE }
    
    public int currentKitFast { get; private set; }
    public int currentKitAccurate { get; private set; }
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public void KitFast(GameObject pacient)
    {
        UseKit(pacient, KitType.FAST);
    }

    public void KitAccurate(GameObject pacient)
    {
        UseKit(pacient, KitType.ACCURATE);
    }

    private void UseKit(GameObject pacient, KitType kitType)
    {
        switch (kitType)
        {
            case KitType.FAST:
                //mostra estado de contaminação
                NewKitInstance(pacient, useTimeFastKit, KitType.FAST);
                currentKitFast--;
                break;
            case KitType.ACCURATE:
                //mostra estado de contaminaçã0 e plasmodium
                NewKitInstance(pacient, useTimeAccurateKit, KitType.ACCURATE);
                currentKitAccurate--;
                break;
        }
    }

    private void NewKitInstance(GameObject pacient, float useTime, KitType kType)
    {
        GameObject instance = Instantiate(kitPrefab, pacient.transform);
        KitBehaviour kitBehaviour = instance.GetComponent<KitBehaviour>();
        kitBehaviour.Initialize(pacient, useTime, (int)kType);
    }

    public void UpdateKitValues(int fastKits, int accurateKits)
    {
        currentKitFast = fastKits;
        currentKitAccurate = accurateKits;
    }
}
