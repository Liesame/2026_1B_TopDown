using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Scriptable Objects/Achievement")]
public class Achievement : ScriptableObject
{
    public string achievementName;
    public string description;
    public int index; // SaveData의 배열 인덱스와 매칭
}

