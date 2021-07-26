using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    //DADOS

    public int dayCurrent { get; private set; }

    public int enableChairs { get; private set; }
    public int enableBeds { get; private set; }

    public int campaignDeaths { get; private set; }
    public int campaignRecovered { get; private set; }
    public int dayWins { get; private set; }
    public int finances { get; private set; }

    public int availableFastKits { get; private set; }
    public int availableAccurateKits { get; private set; }

    public int[] availableMedicins { get; private set; }
    public int availableMedicinCommom { get; private set; }
    public int availableMedicinPlasmodiumA { get; private set; }
    public int availableMedicinPlasmodiumB { get; private set; }

    public int equipChairs { get; private set; }
    public int equipBeds { get; private set; }
    public int equipLevel { get; private set; }

    public int financesLevel { get; private set; }

    private bool continuedGame = false;
    public bool isContinued { get { return continuedGame; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad
        GameObject[] managers = GameObject.FindGameObjectsWithTag("Manager");
        if (managers.Length > 2)
            Destroy(managers[1]);
        DontDestroyOnLoad(gameObject);

        if (!continuedGame)
        {
            enableChairs = CampaignManager.instance.inicialChairsAvailable;
            enableBeds = CampaignManager.instance.inicialBedsAvailable;

            equipChairs = 2;
            equipBeds = 3;

            financesLevel = 0;
        }
        else
        {
            Kit.instance.UpdateKitValues(availableFastKits, availableAccurateKits);
        }

        //load medicamentos ao iniciar pela primeira vez 
        AssignMedicinsAvailable();
    }

    private void Start()
    {
        if (!continuedGame)
        {
            availableFastKits = Kit.instance.FastKitTotal;
            availableAccurateKits = Kit.instance.AccurateKitTotal;
            Kit.instance.UpdateKitValues(availableFastKits, availableAccurateKits);
        }
    }

    private void AssignMedicinsAvailable()
    {
        availableMedicins = new int[3];
        availableMedicins[0] = availableMedicinCommom;
        availableMedicins[1] = availableMedicinPlasmodiumA;
        availableMedicins[2] = availableMedicinPlasmodiumB;
    }

    public void AddFinances(int value)
    {
        finances += value;
    }

    public void AddChair()
    {
        enableChairs++;
    }

    public void AddBed()
    {
        enableBeds++;
    }
    
    public void MoveEquipChairs()
    {
        equipChairs += 1;
        equipBeds -= 1;
    }

    public void MoveEquipBeds()
    {
        equipChairs -= 1;
        equipBeds += 1;
    }

    public void AddEquipLevel()
    {
        if (equipLevel < 3)
            equipLevel++;
    }

    public void AddFinancesLevel()
    {
        if (financesLevel < 3)
            financesLevel++;
    }

    public void AddKit(Kit.KitType kitType, int quantity)
    {
        switch (kitType)
        {
            case Kit.KitType.FAST:
                Debug.Log("Antes: " + availableFastKits.ToString());
                availableFastKits += quantity;
                Debug.Log("Depois: " + availableFastKits.ToString());
                break;
            case Kit.KitType.ACCURATE:
                Debug.Log("Antes: " + availableAccurateKits.ToString());
                availableAccurateKits += quantity;
                Debug.Log("Depois: " + availableAccurateKits.ToString());
                break;
        }
        Kit.instance.UpdateKitValues(availableFastKits, availableAccurateKits);
    }

    public void SaveDayValues(int dayCurrent, int campaignDeaths, int campaignRecovered, int dayWins, int finances, 
        int availableFastKits, int availableAccurateKits, int availableMedicinCommom, int availableMedicinPlasmodiumA, int availableMedicinPlasmodiumB)
    {
        this.dayCurrent = dayCurrent;
        this.campaignDeaths += campaignDeaths;
        this.campaignRecovered += campaignRecovered;
        this.dayWins = dayWins;
        AddFinances(finances);
        this.availableFastKits = availableFastKits;
        this.availableAccurateKits = availableAccurateKits;
        this.availableMedicinCommom = availableMedicinCommom;
        this.availableMedicinPlasmodiumA = availableMedicinPlasmodiumA;
        this.availableMedicinPlasmodiumB = availableMedicinPlasmodiumB;

        AssignMedicinsAvailable();

        continuedGame = true;
    }
    
}
