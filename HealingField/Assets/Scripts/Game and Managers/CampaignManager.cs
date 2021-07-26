using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignManager : MonoBehaviour
{
    public static CampaignManager instance;

    //VALORES DA CAMPANHA
    [Header("Campanha")]
    [SerializeField] private int _rounds;
    [SerializeField] private int[] _npcPerRound;

    public int rounds { get { return _rounds; } }
    public int[] npcPerRound { get { return _npcPerRound; } }
    
    private int winDays;

    //VALORES DIÁRIOS
    public int currentDay { get; private set; }
    private int deathInDay;
    private int recoveredInDay;

    public int dayTarget { get; private set; }
    public int dayObjective { get; private set; }

    //VALORES GANHOS NA CAMPANHA
    [Header("Gains")]
    [SerializeField] private int gainForEachRecovered;
    [SerializeField] private int gainForEachRecoveredBeyondObjective;
    [SerializeField] private int gainToReachObjective;
    [SerializeField] private int gainToReachTarget;
    [SerializeField] private int gainToReachWin;

    [Header("Chair and Beds Inicial Values")]
    public int inicialChairsAvailable;
    public int inicialBedsAvailable;
    [SerializeField] private int _totalChairs;
    [SerializeField] private int _totalBeds;

    public int totalChairs { get { return _totalChairs; } }
    public int totalBeds { get { return _totalBeds; } }
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        currentDay = 0;
    }

    private void Start()
    {
        UpdateDayState(DataManager.instance.dayCurrent, DataManager.instance.isContinued);
    }

    //defini o valor das variáveis a serem utilizadas
    public void UpdateDayState(int day, bool isContinued)
    {
        deathInDay = 0;
        recoveredInDay = 0;

        if (isContinued)
            currentDay = day;
        else
            currentDay = 1;

        Debug.Log("Current day (UpdateDayState): " + currentDay.ToString());

        dayTarget = DayTarget(_npcPerRound[currentDay - 1]);
        dayObjective = DayObjective(dayTarget);
    }

    private int DayTarget(int pacientsInDay)
    {
        int target = (pacientsInDay * 80) / 100;

        return target;
    }

    private int DayObjective(int target)
    {
        int objective = target - 2;

        if (objective < 1)
            objective = 1;

        return objective;
    }
    
    private void AddDay()
    {
        currentDay++;
    }

    private void AddWinDay()
    {
        winDays++;
    }

    public void AddDeathInDay()
    {
        deathInDay++;
    }

    public void AddRecovered()
    {
        recoveredInDay++;
    }
    
    //  <<<< FIM DO DIA  >>>>

    //faz os pacientes ainda na triagem receber o mesmo tratamento que os pacientes liberados
    private void PacientsInChairsSector()
    {
        for (int i = 0; i < RoundManager.Instance.queueOccupation.Length; i++)
        {
            if (RoundManager.Instance.queueOccupation[i] == true)
            {
                PacientHealthSystem pacientHealthSystem;
                pacientHealthSystem = RoundManager.Instance.queueGridRect.queueSlots[i].GetComponentInChildren<PacientHealthSystem>();
                pacientHealthSystem.Release();
            }
        }
        for (int i = 0; i < RoundManager.Instance.chairsOccupation.Length; i++)
        {
            if (RoundManager.Instance.chairsOccupation[i] == true)
            {
                PacientHealthSystem pacientHealthSystem;
                pacientHealthSystem = RoundManager.Instance.instaceChair.chairSlots[i].GetComponentInChildren<PacientHealthSystem>();
                pacientHealthSystem.Release();
            }
        }
    }

    public void EndDay()
    {
        PacientsInChairsSector();

        Debug.Log("Day " + currentDay.ToString());

        if (currentDay <= rounds)
        {
            bool objectiveDone = false;
            bool targetDone = false;

            int financesReceived = 0;
            int bonus = 0;
            int totalFinanceReceived = 0;
            
            int eachRecoveredBeyondObjective = 0;
            int toReachObjective = 0;
            int toReachTarget = 0;
            int toReachWin = 0;

            if (recoveredInDay >= dayObjective)
            {
                int beyond = dayObjective - recoveredInDay;
                if (eachRecoveredBeyondObjective > 0)
                    eachRecoveredBeyondObjective = 0;
                else
                    eachRecoveredBeyondObjective *= -1;

                eachRecoveredBeyondObjective = gainForEachRecoveredBeyondObjective * beyond;
                toReachObjective = gainToReachObjective;
                toReachWin = gainToReachWin;

                AddWinDay();
                objectiveDone = true;
            }

            if (recoveredInDay >= dayTarget)
            {
                toReachTarget = gainToReachTarget;
                targetDone = true;
            }

            bonus = toReachObjective + toReachTarget + toReachWin;
            financesReceived = (recoveredInDay * gainForEachRecovered);
            totalFinanceReceived = financesReceived + bonus;

            //Debug.Log("Recuperados: " + recoveredInDay.ToString());
            //Debug.Log("Mortos: " + deathInDay.ToString());
            Debug.Log("Objetctive (Margen) " + dayObjective.ToString());
            Debug.Log("Target (meta)" + dayTarget.ToString());

            EndDayUi.instance.UpdateEndDayUI(recoveredInDay, deathInDay, _npcPerRound[currentDay - 1], targetDone, objectiveDone, dayObjective, dayTarget, financesReceived, bonus, objectiveDone);
            
            AddDay();

            DataManager.instance.SaveDayValues (
                currentDay, deathInDay, recoveredInDay, winDays, totalFinanceReceived, 
                Kit.instance.currentKitFast, Kit.instance.currentKitAccurate, 
                Medicin.instance.medicins[(int)Medicin.MedicinType.COMMOM].currentMedicinValue, 
                Medicin.instance.medicins[(int)Medicin.MedicinType.PLASMODIUM_A].currentMedicinValue, 
                Medicin.instance.medicins[(int)Medicin.MedicinType.PLASMODIUM_B].currentMedicinValue
                );
        }
    }

    public bool EndCampaign()
    {
        int total = 0;
        for (int i = 0; i < CampaignManager.instance.npcPerRound.Length; i++)
        {
            total += CampaignManager.instance.npcPerRound[i];
        }

        if (DataManager.instance.dayWins >= 4 || DataManager.instance.campaignRecovered > ((total * 60) / 100))
            return true;
        else
            return false;
    }

}
