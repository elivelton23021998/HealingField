using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationUI : MonoBehaviour
{
    public static PreparationUI instance;

    [SerializeField] private GameObject grpPreparationMasterGroup;

    [SerializeField] private GameObject grpOverview;
    [SerializeField] private GameObject grpUpgradeChangeButtons;
    [SerializeField] private GameObject[] grpUpgradeGroups;
    [SerializeField] private GameObject grpShop;
    [SerializeField] private GameObject grpMoneyDisplay;
    private enum UpgradeGroup { SECTORS, EQUIP, FINANCES }

    [Header("Overview")]
    [SerializeField] private Text txtObjective;
    [SerializeField] private Text txtTarget;
    [SerializeField] private Button btnOverview;
    [SerializeField] private Button btnUpgrades;

    [Header("Money Display")]
    [SerializeField] private Text txtMoney;

    [Header("Upgrade Sector")]
    [SerializeField] private Text txtChairValue;
    [SerializeField] private Text txtBedValue;
    [SerializeField] private Text txtCurrentChairs;
    [SerializeField] private Text txtCurrentBeds;
    [SerializeField] private Button btnAddChair;
    [SerializeField] private Button btnAddBed;

    [Header("Equip Alocation")]
    [SerializeField] private Text txtEquipChairs;
    [SerializeField] private Text txtEquipBeds;
    [SerializeField] private Button btnMoveChairs;
    [SerializeField] private Button btnMoveBeds;

    [Header("Equip Upgrade")]
    [SerializeField] private Sprite starSprite;
    [SerializeField] private string[] equipDescriptionLvlUp;
    [SerializeField] private Image[] stars;
    [SerializeField] private Text txtEquipUpgradeValue;
    [SerializeField] private Text txtEquipDescriptionLvlUp;
    [SerializeField] private Button btnUpgradeEquip;

    [Header("Finances Upgrade")]
    [SerializeField] private string[] financeDescriptionLvlUp;
    [SerializeField] private Image[] starsFinance;
    [SerializeField] private Text txtFinanceUpgradeValue;
    [SerializeField] private Text txtFinanceDescriptionLvlUp;
    [SerializeField] private Button btnUpgradeFinance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdatePreparationUI();
    }

    private void UpdatePreparationUI()
    {
        UpdateTarget(CampaignManager.instance.dayObjective, CampaignManager.instance.dayTarget);
        
        UpdateEquip();

        UpdateEquipUpgrade();

        UpdateFinanceUpgrade();

        UpdateFinanceTexts();
    }

    private void UpdateTarget(int objective, int target)
    {
        txtObjective.text = objective.ToString();
        txtTarget.text = target.ToString();
    }
    
    public void UpdateFinanceTexts()
    {
        txtMoney.text = DataManager.instance.finances.ToString();
        txtChairValue.text = UpgradeSystem.instance.newChairCost.ToString();
        txtBedValue.text = UpgradeSystem.instance.newBedCost.ToString();
        txtCurrentChairs.text = DataManager.instance.enableChairs.ToString();
        txtCurrentBeds.text = DataManager.instance.enableBeds.ToString();

        if (DataManager.instance.enableChairs >= CampaignManager.instance.totalChairs)
            btnAddChair.interactable = false;
        if (DataManager.instance.enableBeds >= CampaignManager.instance.totalBeds)
            btnAddBed.interactable = false;
    }

    public void UpdateEquip()
    {
        txtEquipChairs.text = DataManager.instance.equipChairs.ToString();
        txtEquipBeds.text = DataManager.instance.equipBeds.ToString();

        if (DataManager.instance.equipBeds > 0)
            btnMoveChairs.interactable = true;
        else
            btnMoveChairs.interactable = false;

        if (DataManager.instance.equipChairs > 0)
            btnMoveBeds.interactable = true;
        else
            btnMoveBeds.interactable = false;
    }

    public void UpdateEquipUpgrade()
    {
        if (DataManager.instance.equipLevel >= 3)
            btnUpgradeEquip.interactable = false;

        switch (DataManager.instance.equipLevel)
        {
            case 0:
                txtEquipDescriptionLvlUp.text = equipDescriptionLvlUp[0];
                break;
            case 1:
                txtEquipDescriptionLvlUp.text = equipDescriptionLvlUp[1];
                stars[0].sprite = starSprite;
                break;
            case 2:
                txtEquipDescriptionLvlUp.text = equipDescriptionLvlUp[2];
                //stars[0].sprite = starSprite;
                stars[1].sprite = starSprite;
                break;
            case 3:
                txtEquipDescriptionLvlUp.text = "Nível máximo.";
                for (int i = 0; i < stars.Length; i++)
                {
                    stars[i].sprite = starSprite;
                }
                break;
        }

        txtEquipUpgradeValue.text = UpgradeSystem.instance.equipUpgradeCost.ToString();
        UpdateFinanceTexts();
    }

    public void UpdateFinanceUpgrade()
    {
        if (DataManager.instance.financesLevel >= 3)
            btnUpgradeFinance.interactable = false;

        switch (DataManager.instance.financesLevel)
        {
            case 0:
                txtFinanceDescriptionLvlUp.text = financeDescriptionLvlUp[0];
                break;
            case 1:
                txtFinanceDescriptionLvlUp.text = financeDescriptionLvlUp[1];
                starsFinance[0].sprite = starSprite;
                break;
            case 2:
                txtFinanceDescriptionLvlUp.text = financeDescriptionLvlUp[2];
                starsFinance[1].sprite = starSprite;
                break;
            case 3:
                txtFinanceDescriptionLvlUp.text = "Nível máximo.";
                for (int i = 0; i < starsFinance.Length; i++)
                {
                    starsFinance[i].sprite = starSprite;
                }
                break;
        }

        txtFinanceUpgradeValue.text = UpgradeSystem.instance.financeUpCost.ToString();
        UpdateFinanceTexts();
    }

    // <<< BOTÕES >>>
    public void OpenPreparationWindow()
    {
        grpOverview.SetActive(true);

        grpPreparationMasterGroup.SetActive(true);
    }

    public void ClosePreparationWindow()
    {
        //Ao inves desativar eu chamei uma animacao dele fechando pela propria unity
        grpPreparationMasterGroup.SetActive(false);

        grpOverview.SetActive(false);
        grpUpgradeChangeButtons.SetActive(false);
        for (int i = 0; i < grpUpgradeGroups.Length; i++)
        {
            grpUpgradeGroups[i].SetActive(false);
        }
        grpMoneyDisplay.SetActive(false);
        grpShop.SetActive(false);
    }

    public void OpenUpgradeGroup()
    {
        grpOverview.SetActive(false);
        grpShop.SetActive(false);

        grpUpgradeGroups[(int)UpgradeGroup.SECTORS].SetActive(true);
        grpUpgradeChangeButtons.SetActive(true);
        grpMoneyDisplay.SetActive(true);
    }

    public void NextUpgradeGroup()
    {
        int activeGroup = 0;

        for (int i = 0; i < grpUpgradeGroups.Length; i++)
        {
            if (grpUpgradeGroups[i].activeSelf == true)
            {
                activeGroup = i;
                break;
            }
        }

        grpUpgradeGroups[activeGroup].SetActive(false);

        if (activeGroup == grpUpgradeGroups.Length - 1)
            grpUpgradeGroups[0].SetActive(true);
        else
            grpUpgradeGroups[activeGroup + 1].SetActive(true);
    }

    public void OpenShop()
    {
        grpOverview.SetActive(false);
        grpUpgradeChangeButtons.SetActive(false);
        for (int i = 0; i < grpUpgradeGroups.Length; i++)
        {
            grpUpgradeGroups[i].SetActive(false);
        }
        grpMoneyDisplay.SetActive(false);

        grpShop.SetActive(true);
    }
    
    public void PreviousUpgradeGroup()
    {
        int activeGroup = 0;

        for (int i = 0; i < grpUpgradeGroups.Length; i++)
        {
            if (grpUpgradeGroups[i].activeSelf == true)
            {
                activeGroup = i;
                break;
            }
        }

        grpUpgradeGroups[activeGroup].SetActive(false);
        
        if (activeGroup == 0)
            grpUpgradeGroups[grpUpgradeGroups.Length - 1].SetActive(true);
        else
            grpUpgradeGroups[activeGroup - 1].SetActive(true);
    }

    public void OpenOverviewGroup()
    {
        grpShop.SetActive(false);
        grpUpgradeChangeButtons.SetActive(false);
        for (int i = 0; i < grpUpgradeGroups.Length; i++)
        {
            grpUpgradeGroups[i].SetActive(false);
        }
        grpMoneyDisplay.SetActive(false);

        grpOverview.SetActive(true);
    }

    public void UpgradeChair()
    {
        UpgradeSystem.instance.UpChair();
    }

    public void UpgradeBed()
    {
        UpgradeSystem.instance.UpBed();
    }

    public void EquipForChair()
    {
        DataManager.instance.MoveEquipChairs();
        UpdateEquip();
    }

    public void EquipForBed()
    {
        DataManager.instance.MoveEquipBeds();
        UpdateEquip();
    }

    public void UpgradeEquip()
    {
        UpgradeSystem.instance.UpEquip();

    }

    public void UpgradeFinances()
    {
        UpgradeSystem.instance.UpFinances();
        UpdateFinanceUpgrade();
    }

}
