using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacientHealthSystem : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float healTime;
    public float HealTime { get { return healTime; } }
    [SerializeField] private float increseLifetime;

    [SerializeField] private Color healColor;
    [SerializeField] private Slider healthBar;
    public float lifeTimer { get; private set; }
    
    private CampaignManager campaignInstace;
    [HideInInspector] public Pacient pacient;
    private RoundManager roundInstance;

    public bool setedHeal;

    public Slider usingBar;
    [HideInInspector] public bool stopLoseLife;
    [HideInInspector] public int kitType = 2;

    private float medTime;
    [HideInInspector] public bool usingMedicin;
    private bool medicinStateSet;
    public bool endedHealTimer { get; private set; }
    [HideInInspector] public bool cured;

    [SerializeField] private int sortDeathChanceRelease = 50;

    [SerializeField] private GameObject curedText;
    [SerializeField] private Image imgNotification;

    void Start()
    {
        healthBar.maxValue = lifeTime;
        healthBar.value = lifeTime;

        lifeTimer = lifeTime;

        campaignInstace = CampaignManager.instance;
        pacient = GetComponent<Pacient>();
        roundInstance = RoundManager.Instance;
    }
    
    void Update()
    {
        //tempo de vida
        if (!stopLoseLife)
        {
            if (pacient.sector == roundInstance.queue || pacient.sector == roundInstance.chair)
            {
                if (lifeTimer > 0)
                    healthBar.value = lifeTimer -= Time.deltaTime;
                else
                    Death();
            }
            else if (!setedHeal)
            {
                healthBar.maxValue = healTime;
                healthBar.value = 0f;
                healthBar.fillRect.GetComponent<Image>().color = healColor;

                lifeTimer = 0f;

                setedHeal = true;
            }
        }
        
        //cura
        if (setedHeal && !endedHealTimer)
        {
            if (usingMedicin)
            {
                UsingMedicin();
            }
            else
            {
                if (lifeTimer < healTime)
                    healthBar.value = lifeTimer += Time.deltaTime;
                else
                    endedHealTimer = true;
            }
            
            if (InformationUI.Instance.panelInfoGroup.activeSelf == true
                && InformationUI.Instance.npcPrefabRef.GetComponent<Pacient>().characterName == pacient.characterName)
            {
                InformationUI.Instance.BtnMedicinWindowState();
            }
        }
        else if (setedHeal && !cured)
        {
            cured = true;
            InformationUI.Instance.btnRelease.interactable = true;

            if (InformationUI.Instance.panelInfoGroup.activeSelf == true
                && InformationUI.Instance.npcPrefabRef.GetComponent<Pacient>().characterName == pacient.characterName)
                InformationUI.Instance.CureState();

            curedText.SetActive(true);
        }
    }

    public void StartUseMedicin(float medTime)
    {
        float equipReductionTime = 0;

        if (DataManager.instance.equipLevel > 0)
        {
            if (DataManager.instance.equipLevel >= 3)
                equipReductionTime = (DataManager.instance.equipChairs * Equip.instance.timeReduct) + Equip.instance.equipLv1_ReductTime + Equip.instance.equipLv3_ReductTimeMedicin;
            else if (DataManager.instance.equipLevel >= 1)
                equipReductionTime = (DataManager.instance.equipChairs * Equip.instance.timeReduct) + Equip.instance.equipLv1_ReductTime;
        }
        else
        {
            equipReductionTime = DataManager.instance.equipChairs * Equip.instance.timeReduct;
        }

        this.medTime = medTime - equipReductionTime;
        usingMedicin = true;
    }

    private void UsingMedicin()
    {
        if (!medicinStateSet)
        {
            lifeTimer = 0f;
            usingBar.maxValue = medTime;
            usingBar.value = 0f;
            usingBar.gameObject.SetActive(true);
            healthBar.gameObject.SetActive(false);
            medicinStateSet = true;
        }

        if (lifeTimer < medTime)
        {
            usingBar.value = lifeTimer += Time.deltaTime;
            InformationUI.Instance.UseTime("Usando medicamento", (medTime - lifeTimer), pacient.characterName);
        }
        else
        {
            usingBar.gameObject.SetActive(false);
            endedHealTimer = true;

            if (InformationUI.Instance.panelInfoGroup.activeSelf == true
                && InformationUI.Instance.npcPrefabRef.GetComponent<Pacient>().characterName == pacient.characterName)
                InformationUI.Instance.CureState();

            curedText.SetActive(true);
        }
    }

    private void Death()
    {
        //Add mais um óbito ao dia de atendimento
        campaignInstace.AddDeathInDay();
        Debug.Log("Paciente morreu por terminar o tempo");
        //Esconde as informações visuais do npc
        pacient.image.enabled = false;
        healthBar.enabled = false;

        //      <<<<<IMPORTANTE>>>>>>>>  
        //Disparar animação
        //---

        //DEVERÁ SER CHAMADO APÓS ANIMAÇÃO
        DeathAfterAnim();
    }

    public void DeathAfterAnim()
    {
        //Atualizar a situação da fila antes de ser destruído
        roundInstance.RemoveFromSectorPosition(pacient.sector, pacient.currentSectorPosition);

        switch (pacient.sector)
        {
            case "queue":
                roundInstance.UpdateQueueStatus();
                break;
            case "chair":
                roundInstance.UpdateChairStatus();
                break;
            case "bed":
                roundInstance.UpdateBedStatus();
                break;
        }

        //Destruir o gameobject após o fim da animação
        Destroy(gameObject);
    }

    public void Release()
    {
        if (pacient.malariaIndex >= sortDeathChanceRelease && pacient.plasmodiumName != "Sem" && !cured)
        {
            //sorteio
            int sort = Random.Range(1, 11);
            if (sort <= 5)
            {
                //vive
                campaignInstace.AddRecovered();

                //      <<<<<IMPORTANTE>>>>>>>>  
                //CHAMAR A NOTIFICAÇÃO QUE FALA SE O PACIENTE MORREU OU VIVEU
                Debug.Log("Paciente Liberado VIVEU");

                DeathAfterAnim();
            }
            else
            {
                //morre
                campaignInstace.AddDeathInDay();

                //      <<<<<IMPORTANTE>>>>>>>>  
                //CHAMAR A NOTIFICAÇÃO QUE FALA SE O PACIENTE MORREU OU VIVEU
                Debug.Log("Paciente Liberado MORREU");

                DeathAfterAnim();
            }
        }
        else //vive
        {
            campaignInstace.AddRecovered();

            //      <<<<<IMPORTANTE>>>>>>>>  
            //CHAMAR A NOTIFICAÇÃO QUE FALA SE O PACIENTE MORREU OU VIVEU
            Debug.Log("Paciente Liberado VIVEU (liberado direto)");

            DeathAfterAnim();
        }
    }

    public void IncreseLifetime()
    {
        float dif = lifeTime - lifeTimer;
        if (dif > increseLifetime)
        {
            lifeTimer += increseLifetime;
        }
        else
        { 
            lifeTimer += dif;
        }

        //      <<<<<IMPORTANTE>>>>>>>>  
        //disparar VFX da vida aumentando
        Debug.Log("Tempo de Vida Aumentado em: " + increseLifetime.ToString());
    }

    public void ActiveNotification()
    {
        imgNotification.enabled = true;
    }

    public void DisableNotification()
    {
        imgNotification.enabled = false;
    }

}
