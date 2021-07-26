using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finance : MonoBehaviour
{
    public static Finance instance;

    [SerializeField] private int inicialMoney;
    [SerializeField] private int moneyPerDay;
    
    [Header("Finances Upgrade")]
    [SerializeField] private int _financeLv1_TenPercent;
    [SerializeField] private int _financeLv2_ThirteenPercent;
    [SerializeField] private int _financeLv3_FourtyAdd;

    public int financeLv1_TenPercent { get { return _financeLv1_TenPercent; } }
    public int financeLv2_ThirteenPercent { get { return _financeLv2_ThirteenPercent; } }
    public int financeLv3_FourtyAdd { get { return _financeLv3_FourtyAdd; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        int newAmount = 0;
        int currentAmount = DataManager.instance.finances;

        if (DataManager.instance.isContinued)
        {
            if (DataManager.instance.financesLevel > 0)
            {
                switch (DataManager.instance.financesLevel)
                {
                    case 1:
                        moneyPerDay += ((moneyPerDay * _financeLv1_TenPercent) / 100);
                        break;
                    case 2:
                        moneyPerDay += ((moneyPerDay * _financeLv1_TenPercent) / 100) + ((moneyPerDay * _financeLv2_ThirteenPercent) / 100);
                        break;
                    case 3:
                        moneyPerDay +=
                            ((moneyPerDay * _financeLv1_TenPercent) / 100) +
                            ((moneyPerDay * _financeLv2_ThirteenPercent) / 100) +
                            _financeLv3_FourtyAdd;
                        break;
                }
            }
            else
            {
                newAmount = currentAmount + moneyPerDay;
            }
        }
        else
        {
            newAmount = currentAmount + inicialMoney;
        }

        Debug.Log("Money per day " + moneyPerDay.ToString());

        DataManager.instance.AddFinances(newAmount);

        PreparationUI.instance.UpdateFinanceTexts();
    }
}
