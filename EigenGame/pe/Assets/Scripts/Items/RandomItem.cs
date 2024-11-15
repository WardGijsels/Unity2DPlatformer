using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour, IItem
{
    [Header("Item Pool")]
    public List<GameObject> itemPrefabs; // List van mogelijke items

    public void Collect()
    {
        if (itemPrefabs.Count == 0)
        {
            Debug.LogWarning("No items set in the item pool!");
            return;
        }

        // Kies een item uit de lijst
        int randomIndex = Random.Range(0, itemPrefabs.Count);
        GameObject randomItem = itemPrefabs[randomIndex];

        // Instantieer het random item op dezelfde positie en rotatie
        GameObject spawnedItem = Instantiate(randomItem, transform.position, transform.rotation);

        // Destroy het gameObject zodat er geen meerdere items gemaakt worden
        Destroy(gameObject);
    }
}