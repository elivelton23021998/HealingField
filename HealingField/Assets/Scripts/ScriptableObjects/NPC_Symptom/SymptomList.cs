using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("NewSymptomList"), menuName = ("Symptom/SymptomList"), order = 1)]
public class SymptomList : ScriptableObject
{
    [SerializeField]
    private string symptomListName;

    [SerializeField]
    private string[] symptoms;

    public string SymptomListName
    {
        get { return symptomListName; }
    }

    public string[] Symptoms
    {
        get { return symptoms; }
    }

}
