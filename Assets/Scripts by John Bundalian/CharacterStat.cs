using System;
using System.Collections.Generic;

namespace JohnBundalian
{
    public class CharacterStat
    {
        public float BaseValue;

        public float Value {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;

            }
        }

        // StatModifier variable.
        private bool isDirty = true;
        private float _value;

        private readonly List<StatModifier> statModifiers;

        public CharacterStat(float baseValue)
        {
            BaseValue = baseValue;
            statModifiers = new List<StatModifier>();
        }

        public void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        public bool RemoveModifier(StatModifier mod)
        {
            isDirty = true;
            return statModifiers.Remove(mod);
        }

        // Allows Modifiers to compare in order so that multipiers of talents and armour are in linear equation.
        private int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; 
            // if (a.Order == b.Order).
        }

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i <statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                    else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            // 12.0001f != 12f
            return (float)Math.Round(finalValue, 4);
        }
    }
}