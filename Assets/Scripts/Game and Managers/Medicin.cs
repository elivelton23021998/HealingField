using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicin : MonoBehaviour
{
    public static Medicin instance;
    
    public List<MedicinObject> medicins;

    public enum MedicinType { COMMOM, PLASMODIUM_A, PLASMODIUM_B }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateMedicinValues(DataManager.instance.isContinued, DataManager.instance.availableMedicins);
    }

    public void UpdateMedicinValues(bool continued, int[] loadValues)
    {
        if (continued)
        {
            for (int i = 0; i < medicins.Count; i++)
            {
                medicins[i].currentMedicinValue = loadValues[i];
            }
        }
        else
        {
            for (int i = 0; i < medicins.Count; i++)
            {
                medicins[i].currentMedicinValue = medicins[i].totalMedicin;
            }
        }
    }

    public void UseMedicinCommom(GameObject npc)
    {
        UseMedicin(MedicinType.COMMOM, npc);
        medicins[(int)MedicinType.COMMOM].currentMedicinValue--;
    }

    public void UseMedicinA(GameObject npc)
    {
        UseMedicin(MedicinType.PLASMODIUM_A, npc);
        medicins[(int)MedicinType.PLASMODIUM_A].currentMedicinValue--;
    }

    public void UseMedicinB(GameObject npc)
    {
        UseMedicin(MedicinType.PLASMODIUM_B, npc);
        medicins[(int)MedicinType.PLASMODIUM_B].currentMedicinValue--;
    }

    private void UseMedicin(MedicinType type, GameObject npc)
    {
        Pacient pacient = npc.gameObject.GetComponent<Pacient>();

        float passTime = 0;

        switch (pacient.plasmodiumName)
        {
            case "Sem":
                switch (type)
                {
                    case MedicinType.COMMOM:
                        passTime = medicins[(int)MedicinType.COMMOM].useTimeMedicin;
                        break;
                    case MedicinType.PLASMODIUM_A:
                        passTime = medicins[(int)MedicinType.PLASMODIUM_A].useTimeWrongMedicin;
                        break;
                    case MedicinType.PLASMODIUM_B:
                        passTime = medicins[(int)MedicinType.PLASMODIUM_B].useTimeWrongMedicin;
                        break;
                }
                break;
            case "Vivax":
                switch (type)
                {
                    case MedicinType.COMMOM:
                        passTime = medicins[(int)MedicinType.COMMOM].useTimeWrongMedicin;
                        break;
                    case MedicinType.PLASMODIUM_A:
                        passTime = medicins[(int)MedicinType.PLASMODIUM_A].useTimeMedicin;
                        break;
                    case MedicinType.PLASMODIUM_B:
                        passTime = medicins[(int)MedicinType.PLASMODIUM_B].useTimeWrongMedicin;
                        break;
                }
                break;
            case "Falciparum":
                switch (type)
                {
                    case MedicinType.COMMOM:
                        passTime = medicins[(int)MedicinType.COMMOM].useTimeWrongMedicin;
                        break;
                    case MedicinType.PLASMODIUM_A:
                        passTime = medicins[(int)MedicinType.PLASMODIUM_A].useTimeWrongMedicin;
                        break;
                    case MedicinType.PLASMODIUM_B:
                        passTime = medicins[(int)MedicinType.PLASMODIUM_B].useTimeMedicin;
                        break;
                }
                break;
        }

        npc.gameObject.GetComponent<PacientHealthSystem>().StartUseMedicin(passTime);
    }
}

[System.Serializable]
public class MedicinObject
{
    public string medicinName;
    public int totalMedicin;
    public int currentMedicinValue;
    public float useTimeMedicin;
    public float useTimeWrongMedicin;
}