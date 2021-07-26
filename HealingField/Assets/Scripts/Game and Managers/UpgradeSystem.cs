using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem instance;

    [SerializeField] private int _newChairCost;
    public int newChairCost { get { return _newChairCost; } }
    [SerializeField] private int _newBedCost;
    public int newBedCost { get { return _newBedCost; } }

    [SerializeField] private int _equipUpCost;
    public int equipUpgradeCost { get { return _equipUpCost; } }

    [SerializeField] private int _financeUpCost;
    public int financeUpCost { get { return _financeUpCost; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpChair()
    {
        if (DataManager.instance.finances >= _newChairCost)
        {
            DataManager.instance.AddChair();
            DataManager.instance.AddFinances(- _newChairCost);
            PreparationUI.instance.UpdateFinanceTexts();
        }
    }

    public void UpBed()
    {
        if (DataManager.instance.finances >= _newBedCost)
        {
            DataManager.instance.AddBed();
            DataManager.instance.AddFinances(- _newBedCost);
            PreparationUI.instance.UpdateFinanceTexts();
        }
    }

    public void UpEquip()
    {
        if (DataManager.instance.equipLevel < 3 && DataManager.instance.finances >= _equipUpCost)
        {
            DataManager.instance.AddEquipLevel();
            DataManager.instance.AddFinances(-_equipUpCost);
            PreparationUI.instance.UpdateEquipUpgrade();
        }
    }

    public void UpFinances()
    {
        if (DataManager.instance.financesLevel < 3 && DataManager.instance.finances >= _financeUpCost)
        {
            DataManager.instance.AddFinancesLevel();
            DataManager.instance.AddFinances(-_financeUpCost);
            PreparationUI.instance.UpdateFinanceUpgrade();
            PreparationUI.instance.UpdateFinanceTexts();
        }
    }

}
