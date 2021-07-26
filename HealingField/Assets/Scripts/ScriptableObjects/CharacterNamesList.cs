using UnityEngine;

[CreateAssetMenu(fileName = ("Character Names List"))]
public class CharacterNamesList : ScriptableObject
{
    [SerializeField] private string[] _names;

    public string[] names { get { return _names; } }
}
