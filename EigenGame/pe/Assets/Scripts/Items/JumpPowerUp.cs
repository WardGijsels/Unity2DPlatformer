using System;
using UnityEngine;

public class JumpPowerUp : MonoBehaviour, IItem
{
    public static event Action<float> onJumpBoost;

    public float jumpPower;

    public void Collect()
    {
        onJumpBoost.Invoke(jumpPower);
        Destroy(gameObject);
    }
}