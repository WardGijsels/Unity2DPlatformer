using UnityEngine;

public class Gem : MonoBehaviour, IItem
{
    [Header("Collect")]
    private Vector3 initialPosition;

    private bool collected = false;

    [Header("GameController")]
    public GameController gameController;

    private void Start()
    {
        initialPosition = transform.position;
        collected = false;
    }

    public void Collect()
    {
        if (!collected)
        {
            if (gameController != null)
            {
                gameController.IncreaseGemCount();
            }
            SoundEffectManager.Play("Gem");
            collected = true;
            gameObject.SetActive(false);
        }
    }

    public void ResetGem()
    {
        gameObject.SetActive(true);
        transform.position = initialPosition;
        collected = false;
    }
}