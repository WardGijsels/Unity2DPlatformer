using System;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour, IItem
{
    public static event Action<float> onSpeedBoost;

    public float speedMultiplier = 1.5f;

    public void Collect()
    {
        onSpeedBoost.Invoke(speedMultiplier);
        Destroy(gameObject);
    }
}