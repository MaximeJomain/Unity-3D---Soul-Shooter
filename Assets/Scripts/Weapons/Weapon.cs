using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public float Damage;

    public virtual void Equip(Character character) {}
}
