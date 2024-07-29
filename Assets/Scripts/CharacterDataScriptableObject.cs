using UnityEngine;

[CreateAssetMenu(fileName = "haracterDataScriptableObject", menuName = "ScriptableObjects/haracterDataScriptableObject", order = 1)]
public class CharacterDataScriptableObject : ScriptableObject
{
    public string characterName;
    public int maxHealth = 3;
}

