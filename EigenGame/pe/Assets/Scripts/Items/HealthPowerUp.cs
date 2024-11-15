using System;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour, IItem
{
    [Header("Health logic")]
    public int healthAmount = 1;

    public static event Action<int> onHealthPickUp;

    public void Collect()
    {
        onHealthPickUp?.Invoke(healthAmount);
        Destroy(gameObject);
    }
}