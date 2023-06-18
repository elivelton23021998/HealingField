using UnityEngine;

public class KitBehaviour : MonoBehaviour
{
    private bool startTimer;
    private float timer;
    private float useTime;
    private PacientHealthSystem pacientHealthSystem;

    public bool used;

    public void Initialize(GameObject pacient, float useTime, int kitType)
    {
        this.useTime = useTime;
        pacientHealthSystem = pacient.GetComponent<PacientHealthSystem>();

        pacientHealthSystem.kitType = kitType;

        StartUse();
    }

    private void StartUse()
    {
        float equipReductionTime = 0;
        if (DataManager.instance.equipLevel > 0)
        {
            if (DataManager.instance.equipLevel >= 2)
                equipReductionTime = (DataManager.instance.equipChairs * Equip.instance.timeReduct) + Equip.instance.equipLv2_ReductTimekit + Equip.instance.equipLv1_ReductTime;
            else if (DataManager.instance.equipLevel >= 1)
                equipReductionTime = (DataManager.instance.equipChairs * Equip.instance.timeReduct) + Equip.instance.equipLv1_ReductTime;
        }
        else
        {
            equipReductionTime = (DataManager.instance.equipChairs * Equip.instance.timeReduct);
        }
        
        pacientHealthSystem.usingBar.gameObject.SetActive(true);
        pacientHealthSystem.usingBar.value = useTime - equipReductionTime;
        pacientHealthSystem.usingBar.maxValue = useTime - equipReductionTime;

        timer = useTime - equipReductionTime;
        
        pacientHealthSystem.stopLoseLife = true;

        startTimer = true;
    }

    private void Update()
    {
        if (startTimer && timer > 0)
        {
            pacientHealthSystem.usingBar.value = timer -= Time.deltaTime;
            InformationUI.Instance.UseTime("Usando kit", timer, pacientHealthSystem.pacient.characterName);
        }
        else if (!used)
        {
            Debug.LogWarning("Kit Behavior: Utilizado.");
            used = true;
            if (InformationUI.Instance.panelInfoGroup.activeSelf == true &&
                InformationUI.Instance.characterName == pacientHealthSystem.gameObject.GetComponent<Pacient>().characterName)
            {
                InformationUI.Instance.btnRelease.interactable = true;
                InformationUI.Instance.ChairsWindow();
            }
        
            pacientHealthSystem.usingBar.gameObject.SetActive(false);
            pacientHealthSystem.stopLoseLife = false;
            pacientHealthSystem.ActiveNotification();
        }
    }

}
