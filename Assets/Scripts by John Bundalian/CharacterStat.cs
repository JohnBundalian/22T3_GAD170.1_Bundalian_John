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
        }

        public bool RemoveModifier(StatModifier mod)
        {
            isDirty = true;
            return statModifiers.Remove(mod);
        }

        private float CalculateFinalValue()
        {
            float finalValue = BaseValue;

            for (int i = 0; i <statModifiers.Count; i++)
            {
                finalValue += statModifiers[i].Value;
            }

            // 12.0001f != 12f
            return (float)Math.Round(finalValue, 4);
        }
    }
}