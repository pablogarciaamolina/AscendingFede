using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FedeHealth : MonoBehaviour
{
    public interface IHealth
    {
        void TakeDamage(int damage);
        void Heal(int heal);
        void SetMaxHealth(int maxHealth);
        int GetHealth();
    }
}
