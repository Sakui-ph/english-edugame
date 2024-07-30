using System;
using AUDIO_SYSTEM;
using CARD_GAME;
using UnityEngine;

// todo: this thing has to subscribe to all the events that has to do with HP
public class CardGamePlayer
{
    public int currentHP = 0;
    public int maxHP = 5;
    public delegate void CardGameHealthEvent(int healthAmount);
    public event CardGameHealthEvent HealthChanged;

    public void ChangeHealth(int amount)
    {
        currentHP += amount;
        if (currentHP >= maxHP) 
            currentHP = maxHP;
        if (currentHP <= 0)
            currentHP = 0;

        Debug.Log($"Health changed to {amount}");
        
        UpdateHealth();
    }

    private void UpdateHealth() => HealthChanged?.Invoke(currentHP);
}
