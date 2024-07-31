using System;
using System.Runtime.InteropServices;
using AUDIO_SYSTEM;
using CARD_GAME;
using JetBrains.Annotations;
using UnityEngine;

// todo: this thing has to subscribe to all the events that has to do with HP
public class CardGamePlayer
{
    private int _currentHP;
    public int CurrentHP {
        get
        {
            return _currentHP;
        }
        set
        {
            if (value >= maxHP)
                _currentHP = maxHP;
            else if (value <= 0) {
                _currentHP = 0;
                Debug.Log("Death");
            }
            else
            {
                _currentHP = value;
            }
        }
    }
    public int maxHP = 5;
    public delegate void CardGameHealthEvent(int healthAmount);
    public event CardGameHealthEvent HealthChanged;

    public void ChangeHealth(int amount)
    {
        CurrentHP += amount;
        UpdateHealth();
    }

    public void SetHealth(int amount)
    {
        CurrentHP = amount;
        UpdateHealth();
    }

    private void UpdateHealth() => HealthChanged?.Invoke(CurrentHP);
}
