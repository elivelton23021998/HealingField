using UnityEngine;

[CreateAssetMenu(fileName = ("NewSymptomMasterData"), menuName = ("Symptom/SymptomSettings"), order = 0)]
public class SymptomMasterData : ScriptableObject
{
    [SerializeField] private CharacterNamesList charactersNameList;

    [SerializeField] private string[] plasmodium;
    
    [SerializeField] private float[] plasmodium1_SymptomChance;
    [SerializeField] private float[] plasmodium2_SymptomChance;
    [SerializeField] private float[] plasmodium3_SymptomChance;

    [SerializeField] private SymptomList[] symptomsList;

    [SerializeField] private float[] malariaIndex;
    
    #region Setters and Getters
    public CharacterNamesList CharacterNames { get { return charactersNameList; } }

    public string[] PlasmodiumName { get { return plasmodium; } }

    public float[] Plasmodium1_SymptomChance { get { return plasmodium1_SymptomChance; } }
    public float[] Plasmodium2_SymptomChance { get { return plasmodium2_SymptomChance; } }
    public float[] Plasmodium3_SymptomChance { get { return plasmodium3_SymptomChance; } }

    public SymptomList[] SymptomsList { get { return symptomsList; } }

    public float[] MalariaIndex { get { return malariaIndex; } }
    #endregion
}
