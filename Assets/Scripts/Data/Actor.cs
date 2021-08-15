﻿using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class Actor : GameEntity
    {
        public int Health { get; private set; }
        public int Armor { get; private set; }

        [JsonConstructor]
        public Actor(int id, Properties properties, int health, int armor) : base(id, properties)
        {
            Health = health;
            Armor = armor;
        }

        public Actor(int health, IContext context) : base(context)
        {
            Health = health;
        }

        public void TryDealDamage(int damage, out int totalDamage, out int healthDamage)
        {
            int armorDamage = Mathf.Min(damage, Armor);
            Armor -= armorDamage;
            healthDamage = damage - armorDamage;
            healthDamage = Mathf.Min(healthDamage, Health);
            totalDamage = healthDamage + armorDamage;

            Health -= healthDamage;
        }

        public void GainArmor(int amount)
        {
            Armor += amount;
        }

        public void ResetArmor()
        {
            Armor = 0;
        }
    }
}