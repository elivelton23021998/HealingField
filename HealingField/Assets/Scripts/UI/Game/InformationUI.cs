using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationUI : MonoBehaviour
{
    public static InformationUI Instance { get; set; }

    [Header("Smoke effect")]
    [SerializeField] private Transform tela;
    [SerializeField] private GameObject puff;
    
    //Variáveis temporárias do NPC
    [HideInInspector] public GameObject npcPrefabRef;
    [HideInInspector] public string characterName;
    private string plasmodiumName;
    private string contamined;
    private string malariaIndex;
    private string complaint;
    private string[] symptoms;

    //Sprites dos plasmodium
    [SerializeField] private Sprite[] plasmodiumsSprites;
    private enum PlasmodiumSprite { NONE, VIVAX, FALCIPARUM, UNDEFINED}

    //não me lembro
    public GameObject panelInfoGroup;
    public Button btnRelease;
    
    [Header("Groups")]
    [SerializeField] private GameObject grpCommomInfo;
    [SerializeField] private GameObject grpQueueInfo;
    [SerializeField] private GameObject grpChairsInfo;
    [SerializeField] private GameObject grpChairsInfo_grpSymptoms;
    [SerializeField] private GameObject grpChairsInfo_grpChairUsedKit;
    [SerializeField] private GameObject grpChairsInfo_grpBtnsChairs;
    [SerializeField] private GameObject grpKits;
    [SerializeField] private GameObject grpMalariaIndex;
    [SerializeField] private GameObject grpUseTime;
    [SerializeField] private GameObject grpBedsInfo;
    [SerializeField] private GameObject grpBedsInfo_grpMedicins;
    [SerializeField] private GameObject grpPopUpRelease;
    [SerializeField] private GameObject grpCured;

    [Header("Commom Info")]
    [SerializeField] private Image imgPacient;
    [SerializeField] private Text lblPacientName;
    [SerializeField] private Text lblTitle;

    [Header("Queue Info")]
    [SerializeField] private Text txtComplaint;
    [SerializeField] private Button btnSendToChairs;
    
    [Header("Chairs Info")]
    [SerializeField] private Text[] txtSymptom;
    [SerializeField] private Image imgPlasmodium;
    [SerializeField] private Text txtPlasmodiumName;
    [SerializeField] private Text txtContamined;
    [SerializeField] private Button btnUseKit;
    [SerializeField] private Button btnSendToBeds;

    [Header("Kit Info")]
    [SerializeField] private Text txtKitFastQuantity;
    [SerializeField] private Text txtKitAccurateQuantity;
    [SerializeField] private Button btnKitFast;
    [SerializeField] private Button btnKitAccurate;
    [SerializeField] private Button btnBackChairsInfo;

    [Header("Malaria Index")]
    [SerializeField] private Slider barMalariaIndex;
    [SerializeField] private Text txtMalariaIndex;
    
    [Header("Using Something")]
    [SerializeField] private Text lblTitleUse;
    [SerializeField] private Text txtTime;

    [Header("Medicins Info")]
    [SerializeField] private Text txtMedValueCommom;
    [SerializeField] private Text txtMedValuePlasmA;
    [SerializeField] private Text txtMedValuePlasmB;
    [SerializeField] private Button[] btnUseMedicin;
    public Button btnMedicinWindow;
    [SerializeField] private Button btnBackToBedInfo;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    //Método chamado ao clicar no paciente
    public void OpenInformationUI(GameObject npc, string sectorStatus, string npcName, string plasmodiumName, string isContamined, string malariaIndex, string complaint, string[] symptoms)
    {
        SetNpcInfo(npc, npcName, plasmodiumName, isContamined, malariaIndex, complaint, symptoms);

        switch (sectorStatus)
        {
            case "queue":
                QueueWindow();
                break;
            case "chair":
                ChairsWindow();
                break;
            case "bed":
                BedsWindow();
                break;
        }

        OpenInfoWindow();
    }
    
    private void QueueWindow()
    {
        btnRelease.interactable = true;

        CommomInfo("Queixa do Paciente");
        txtComplaint.text = complaint;
        
        grpQueueInfo.SetActive(true);
    }

    public void ChairsWindow()
    {
        if (npcPrefabRef.GetComponent<Pacient>().usingKit)
        {
            btnRelease.interactable = false;
            btnUseKit.interactable = false;
            grpMalariaIndex.SetActive(false);
            grpChairsInfo_grpBtnsChairs.SetActive(true);

            if (npcPrefabRef.GetComponentInChildren<KitBehaviour>().used)
            {
                btnRelease.interactable = true;

                CommomInfo("Diagnóstico");

                grpChairsInfo_grpSymptoms.SetActive(false);
                grpUseTime.SetActive(false);

                btnSendToBeds.interactable = true;

                SetUsedKit();
                grpChairsInfo_grpChairUsedKit.SetActive(true);
            }
            else
            {
                btnRelease.interactable = false;

                CommomInfo("Sintomas");
                SetSymptoms();
                grpChairsInfo_grpSymptoms.SetActive(true);
                grpChairsInfo_grpChairUsedKit.SetActive(false);

                btnSendToBeds.interactable = false;

                //o use time é atualizado por quem o utiliza
                grpUseTime.SetActive(true);
            }
        }
        else
        {
            btnRelease.interactable = true;

            CommomInfo("Sintomas");
            grpChairsInfo_grpBtnsChairs.SetActive(true);

            btnSendToBeds.interactable = true;

            SetSymptoms();
            grpChairsInfo_grpSymptoms.SetActive(true);

            SetMalariaIndexBar();
            grpMalariaIndex.SetActive(true);

            btnUseKit.interactable = true;
        }

        grpChairsInfo.SetActive(true);
    }

    public void BedsWindow()
    {
        btnMedicinWindow.interactable = true;

        if (!npcPrefabRef.GetComponent<PacientHealthSystem>().endedHealTimer)
        {
            if (npcPrefabRef.GetComponent<Pacient>().usingKit)
            {
                CommomInfo("Diagnóstico");

                SetUsedKit();
                grpChairsInfo_grpChairUsedKit.SetActive(true);
            }
            else
            {
                CommomInfo("Sintomas");

                SetSymptoms();
                grpChairsInfo_grpSymptoms.SetActive(true);
            }
        }
        else
        {
            CureState();
        }

        BtnMedicinWindowState();

        if (!npcPrefabRef.GetComponent<PacientHealthSystem>().cured)
            btnRelease.interactable = false;
        else
            btnRelease.interactable = true;
        
        grpBedsInfo.gameObject.SetActive(true);
    }

    public void BtnMedicinWindowState()
    {
        PacientHealthSystem thisPacientHealthSystem = npcPrefabRef.GetComponent<PacientHealthSystem>();

        if (thisPacientHealthSystem.usingMedicin && !thisPacientHealthSystem.endedHealTimer)
        {
            btnMedicinWindow.interactable = false;

            grpUseTime.SetActive(true);
        }
        else if (thisPacientHealthSystem.endedHealTimer)  //após ser curado
        {
            btnMedicinWindow.interactable = false;

            grpUseTime.SetActive(false);

            CureState();
        }
        else if (thisPacientHealthSystem.setedHeal) //não permite uso do medicamento caso o tempo de cura seja menor que o tempo de uso do medicamento
        {
            float remainingTime = thisPacientHealthSystem.HealTime - thisPacientHealthSystem.lifeTimer;
            int smallestUseTimeIndex = 0;
            for (int i = 0; i < Medicin.instance.medicins.Count; i++)
            {
                if (i != 0)
                {
                    if (Medicin.instance.medicins[i - 1].useTimeMedicin > Medicin.instance.medicins[i].useTimeMedicin
                        || Medicin.instance.medicins[i - 1].useTimeWrongMedicin > Medicin.instance.medicins[i].useTimeWrongMedicin)
                    {
                        if (Medicin.instance.medicins[smallestUseTimeIndex].useTimeMedicin < Medicin.instance.medicins[i - 1].useTimeMedicin
                        || Medicin.instance.medicins[smallestUseTimeIndex].useTimeWrongMedicin < Medicin.instance.medicins[i - 1].useTimeWrongMedicin)
                            smallestUseTimeIndex = i - 1;
                    }
                    else
                    {
                        smallestUseTimeIndex = i;
                    }
                }
            }

            if (remainingTime < Medicin.instance.medicins[smallestUseTimeIndex].useTimeMedicin
                || remainingTime < Medicin.instance.medicins[smallestUseTimeIndex].useTimeWrongMedicin)
            {
                btnMedicinWindow.interactable = false;
            }
        }
    }

    //atualiza as info comuns
    private void CommomInfo(string title) //Aloca a informação comum
    {
        imgPacient.sprite = npcPrefabRef.GetComponent<Pacient>().npcImage;
        imgPacient.SetNativeSize();
        lblPacientName.text = characterName;
        lblTitle.text = title;

        grpCommomInfo.SetActive(true);
    }

    //Estado de Cura
    public void CureState()
    {
        grpChairsInfo_grpChairUsedKit.SetActive(false);
        grpChairsInfo_grpSymptoms.SetActive(false);

        CommomInfo("Estado do Paciente");
        grpCured.SetActive(true);
    }

    //Atualiza os txt de sintomas na tela
    private void SetSymptoms()
    {
        for (int i = 0; i < symptoms.Length; i++)
        {
            txtSymptom[i].text = symptoms[i];
        }
    }

    //Atualizar o Use Time
    public void UseTime(string titleUse, float time, string pacientName)
    {
        if (panelInfoGroup.activeSelf == true
                && npcPrefabRef.GetComponent<Pacient>().characterName == pacientName)
        {
            lblTitleUse.text = titleUse;
            txtTime.text = time.ToString("00") + " s";
        }
    }

    private void SetMalariaIndexBar()
    {
        barMalariaIndex.maxValue = 100f;
        barMalariaIndex.value = float.Parse(malariaIndex);
        txtMalariaIndex.text = malariaIndex + "%";
    }

    private void SetUsedKit()
    {
        if ((int)Kit.KitType.ACCURATE == npcPrefabRef.GetComponent<PacientHealthSystem>().kitType) //kit preciso
        {
            txtPlasmodiumName.text = plasmodiumName;

            if (plasmodiumName == "Vivax")
                imgPlasmodium.sprite = plasmodiumsSprites[(int)PlasmodiumSprite.VIVAX];
            else if (plasmodiumName == "Falciparum")
                imgPlasmodium.sprite = plasmodiumsSprites[(int)PlasmodiumSprite.FALCIPARUM];
            else
                imgPlasmodium.sprite = plasmodiumsSprites[(int)PlasmodiumSprite.NONE];

            imgPlasmodium.gameObject.SetActive(true);
            txtPlasmodiumName.gameObject.SetActive(true);
        }
        else //kit rápido
        {
            imgPlasmodium.sprite = plasmodiumsSprites[(int)PlasmodiumSprite.UNDEFINED];
            txtPlasmodiumName.text = "Indefinido";

            imgPlasmodium.gameObject.SetActive(true);
            txtPlasmodiumName.gameObject.SetActive(true);
        }

        if (contamined == "Sem")
            txtContamined.text = "Não contaminado";
        else
            txtContamined.text = "Contaminado";
    }
    
    //<<<  AÇÕES DO BOTÕES  >>>
    public void OpenInfoWindow()
    {
        panelInfoGroup.SetActive(true);
    }

    public void CloseInfoWindow()
    {
        grpCommomInfo.SetActive(false);
        grpQueueInfo.SetActive(false);
        grpChairsInfo.SetActive(false);
        grpChairsInfo_grpSymptoms.SetActive(false);
        grpChairsInfo_grpChairUsedKit.SetActive(false);
        grpKits.SetActive(false);
        grpMalariaIndex.SetActive(false);
        grpUseTime.SetActive(false);
        grpBedsInfo.SetActive(false);
        grpBedsInfo_grpMedicins.SetActive(false);
        grpPopUpRelease.SetActive(false);
        grpCured.SetActive(false);

        panelInfoGroup.SetActive(false);
    }

    private void SetNpcInfo(GameObject npc, string characterName, string plasmodiumName, string isContamined, string malariaIndex, string complaint, string[] symptoms)
    {
        npcPrefabRef = npc;

        this.characterName = characterName;
        this.plasmodiumName = plasmodiumName;
        contamined = isContamined;
        this.malariaIndex = malariaIndex;
        this.complaint = complaint;
        this.symptoms = new string[symptoms.Length];
        for (int i = 0; i < symptoms.Length; i++)
        {
            this.symptoms[i] = symptoms[i];
        }
    }

    public void SendToChair()
    {
        Some();
        RoundManager.Instance.ChairAdd(npcPrefabRef);
    }
    
    public void SendToBed()
    {
        npcPrefabRef.GetComponent<PacientHealthSystem>().DisableNotification();

        GameObject temp = Instantiate(puff, npcPrefabRef.transform.position, npcPrefabRef.transform.rotation);
        temp.transform.SetParent(tela);

        RoundManager.Instance.BedAdd(npcPrefabRef);
    }

    public void ReleaseBtn()
    {
        //chamar método de liberar
        Some();
        npcPrefabRef.GetComponent<PacientHealthSystem>().Release();
        //fechar ficha do paciente

        grpPopUpRelease.SetActive(false);
        CloseInfoWindow();
        //      <<<<<IMPORTANTE>>>>>>>>  
        //visual: feedback de paciente liberado
    }

    public void OpenKitsWindow()
    {  
        if (Kit.instance.currentKitFast == 0)
        {
            btnKitFast.interactable = false;
        }

        if (Kit.instance.currentKitAccurate == 0)
        {
            btnKitAccurate.interactable = false;
        }

        txtKitFastQuantity.text = Kit.instance.currentKitFast.ToString("00");
        txtKitAccurateQuantity.text = Kit.instance.currentKitAccurate.ToString("00");

        grpChairsInfo_grpSymptoms.SetActive(false);
        grpChairsInfo_grpBtnsChairs.SetActive(false);

        grpKits.SetActive(true);
    }

    public void CloseKitsWindow()
    {
        grpKits.SetActive(false);
        ChairsWindow();
    }

    //KITS
    public void FastKit()
    {
        Kit.instance.KitFast(npcPrefabRef);
        btnRelease.interactable = false;
        npcPrefabRef.GetComponent<Pacient>().usingKit = true;

        CloseKitsWindow();
    }

    public void AccurateKit()
    {
        Kit.instance.KitAccurate(npcPrefabRef);
        btnRelease.interactable = false;
        npcPrefabRef.GetComponent<Pacient>().usingKit = true;

        CloseKitsWindow();
    }

    //Leitos
    public void OpenMedicinWindow()
    {
        CommomInfo("Medicamentos");

        grpChairsInfo_grpChairUsedKit.SetActive(false);
        grpChairsInfo_grpSymptoms.SetActive(false);

        txtMedValueCommom.text = Medicin.instance.medicins[(int)Medicin.MedicinType.COMMOM].currentMedicinValue.ToString();
        txtMedValuePlasmA.text = Medicin.instance.medicins[(int)Medicin.MedicinType.PLASMODIUM_A].currentMedicinValue.ToString();
        txtMedValuePlasmB.text = Medicin.instance.medicins[(int)Medicin.MedicinType.PLASMODIUM_B].currentMedicinValue.ToString();

        for (int i = 0; i < btnUseMedicin.Length; i++)
        {
            if (Medicin.instance.medicins[i].currentMedicinValue <= 0)
                btnUseMedicin[i].interactable = false;
        }
        
        grpBedsInfo_grpMedicins.SetActive(true);
    }

    public void CloseMedicinWindow()
    {
        grpBedsInfo_grpMedicins.SetActive(false);
        BedsWindow();
    }

    public void UseMedicinCommom()
    {
        Medicin.instance.UseMedicinCommom(npcPrefabRef);
        grpUseTime.SetActive(true);
        CloseMedicinWindow();
    }

    public void UseMedicinPlasmA()
    {
        Medicin.instance.UseMedicinA(npcPrefabRef);
        grpUseTime.SetActive(true);
        CloseMedicinWindow();
    }

    public void UseMedicinPlasmB()
    {
        Medicin.instance.UseMedicinB(npcPrefabRef);
        grpUseTime.SetActive(true);
        CloseMedicinWindow();
    }

    //Pop up Liberação
    public void OpenPopUpRelease()
    {
        Pacient pacient = npcPrefabRef.GetComponent<Pacient>();
        if (pacient.sector != "bed" && pacient.sector == "queue" || pacient.sector != "bed" && !pacient.usingKit)
        {
            grpPopUpRelease.SetActive(true);
        }
        else
        {
            ReleaseBtn();
        }
    }

    private void Some()
    {
        GameObject temp = Instantiate(puff, npcPrefabRef.transform.position, npcPrefabRef.transform.rotation);
        temp.transform.SetParent(tela);
    }

}
