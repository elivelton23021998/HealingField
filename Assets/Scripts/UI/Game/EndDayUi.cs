using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndDayUi : MonoBehaviour
{

    public static EndDayUi instance;

    [SerializeField] private GameObject grpEndDayMasterGroup;
    [SerializeField] private GameObject grpEndCampaignMasterGroup;

    [SerializeField] private Text txtRecovereds;
    [SerializeField] private Text txtDeads;
    [SerializeField] private Text txtTotalPacients;
    [SerializeField] private Text txtObjectiveReached;
    [SerializeField] private Text txtTargetReached;
    [SerializeField] private Text txtMinObjective;
    [SerializeField] private Text txtTargetValue;
    [SerializeField] private Text txtFinancesValue;
    [SerializeField] private Text txtBonusValue;

    [SerializeField] private Button btnEndDay;

    [SerializeField] private Image dayResult;
    [SerializeField] private Sprite winImage;
    [SerializeField] private Sprite loseImage;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    [Header("End Campaign")]
    [SerializeField] private Text txtRecovered;
    [SerializeField] private Text txtDeaths;
    [SerializeField] private Text txtTotal;
    [SerializeField] private Image imgResult;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateEndDayUI(int recovered, int deads, int totalPacients, bool objctiveReached, bool targetReached, int objectiveValue, int targetValue, int financesValue, int bonusValue, bool dayResult)
    {
        txtRecovereds.text = "";
        txtDeads.text = "";

        txtRecovereds.text = recovered.ToString();
        txtDeads.text = deads.ToString();
        txtTotalPacients.text = totalPacients.ToString();

        if (objctiveReached)
        {
            txtObjectiveReached.color = Color.green;
            txtObjectiveReached.text = "SIM";
        }
        else
        {
            txtObjectiveReached.color = Color.red;
            txtObjectiveReached.text = "NÃO";
        }

        if (targetReached)
        {
            txtTargetReached.color = Color.green;
            txtTargetReached.text = "SIM";
        }
        else
        {
            txtTargetReached.color = Color.red;
            txtTargetReached.text = "NÃO";
        }

        txtMinObjective.text = objectiveValue.ToString();
        txtTargetValue.text = targetValue.ToString();
        txtFinancesValue.text = financesValue.ToString();
        txtBonusValue.text = bonusValue.ToString();

        if (dayResult)
        {
            this.dayResult.sprite = winImage;
            winSound.Play();
        }
        else
        {
            this.dayResult.sprite = loseImage;
            loseSound.Play();//coloquei pra chamar o som de vitoria e derrota baseado no desenho.
        }
    }

    public void EndDay()
    {
        CampaignManager.instance.EndDay();
        grpEndDayMasterGroup.SetActive(true);
    }

    public void UpdateEndCampaign()
    {
        int total = 0;
        for (int i = 0; i < CampaignManager.instance.npcPerRound.Length; i++)
        {
            total += CampaignManager.instance.npcPerRound[i];
        }
        int recovered = DataManager.instance.campaignRecovered;
        int deaths = DataManager.instance.campaignDeaths;
        
        txtRecovered.text = recovered.ToString();
        txtDeaths.text = deaths.ToString();
        txtTotal.text = total.ToString();

        if (CampaignManager.instance.EndCampaign())
            imgResult.sprite = winImage;
            
        else
            imgResult.sprite = loseImage;
    }


    public void BtnEndDay()
    {
        if (CampaignManager.instance.currentDay > CampaignManager.instance.rounds)
        {
            grpEndDayMasterGroup.SetActive(false);

            UpdateEndCampaign();

            grpEndCampaignMasterGroup.SetActive(true);
        }
        else
        {
            CampaignManager.instance.UpdateDayState(CampaignManager.instance.currentDay, DataManager.instance.isContinued);
            SceneManager.LoadScene("PreparationPhase");
        }
    }

    public void BtnEndCampaign()
    {
        Destroy(DataManager.instance.gameObject);
        SceneManager.LoadScene("Menu");
    }

}
