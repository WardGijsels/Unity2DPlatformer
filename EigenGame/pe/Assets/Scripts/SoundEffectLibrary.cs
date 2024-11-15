using System.Collections.Generic;
using UnityEngine;

public class SoundEffectLibrary : MonoBehaviour
{
    // Lijst van geluids effectgroepen, die via de Inspector kan worden ingesteld
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;

    // Dictionary om geluidseffecten op naam te kunnen opslaan
    private Dictionary<string, AudioClip> soundDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (SoundEffectGroup group in soundEffectGroups)
        {
            // Voeg de audioClip toe aan de dictionary met de naam als key
            soundDictionary[group.name] = group.audioClip;
        }
    }

    public AudioClip GetClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            return soundDictionary[name];
        }
        return null;
    }
}

//Dictionary kan niet gebruikt worden in de Unity Inspector
//Hierom wordt een nieuwe klasse aangemaakt
[System.Serializable]
public struct SoundEffectGroup
{
    public string name;
    public AudioClip audioClip;
}