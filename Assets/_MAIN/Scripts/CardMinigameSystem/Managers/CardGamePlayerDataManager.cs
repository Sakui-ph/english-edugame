using System;
using AUDIO_SYSTEM;
using UnityEngine;

public class CardGamePlayerDataManager : MonoBehaviour
{
    public static CardGamePlayerDataManager instance;

    public int currentHP = 0;
    public int initialHP = 3;
    public int maxHP = 5;

    public event Action<int> HealthUpdate;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        }
        else
            DestroyImmediate(this);
    }

    public void ReduceHP(int amount = 1)
    {
        if (currentHP > 0)
        {
            AudioManager.instance.PlaySoundEffect("IncorrectSound");
            currentHP -= amount;
            HealthUpdate?.Invoke(currentHP);
        }

        if (currentHP <= 0)
            GameSystem.instance.ResetLevel();
    }

    public void IncreaseHP(int amount = 1)
    {
        if (currentHP != maxHP)
        {   
            currentHP += amount;
            HealthUpdate?.Invoke(currentHP);
        }
    }

    public void Initialize(int startingHP)
    {
        initialHP = startingHP;
        currentHP = initialHP;
        HealthUpdate?.Invoke(currentHP);
    }


}
