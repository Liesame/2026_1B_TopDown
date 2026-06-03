using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [SerializeField] Damege data;

    public int Damage()
    {
        return data.Damage;
    }
}
