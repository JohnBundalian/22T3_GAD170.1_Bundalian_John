namespace JohnBundalian
{
    public enum StatModType
    {
        Flat,
        PercentAdd,
        PercentMult,
    }

    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;
        // Source variable.
        public readonly object Source;

        public StatModifier(float value, StatModType type, int order)
        {
            Value = value;
            Type = type;
            Order = order;
        }

        public StatModifier(float value, StatModType Type) : this (value, Type, (int)type) { }
    }
}


