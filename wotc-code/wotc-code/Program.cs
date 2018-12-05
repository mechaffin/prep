using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace wotc_code
{
    class Program
    {
        static void Main(string[] args)
        {
            AttributeKey key = AttributeKey.Power;
            MyLayeredAttributes o = new MyLayeredAttributes();
            o.SetBaseAttribute(key, 3);
            o.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = key, Layer = 10, Modification = 2, Operation = EffectOperation.Multiply });
            o.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = key, Layer = 1, Modification = 3, Operation = EffectOperation.Add });
            o.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = key, Layer = 10, Modification = 1, Operation = EffectOperation.Add });
            Console.WriteLine(o.GetCurrentAttribute(key)); // ((3 + 3) * 2) + 1 => (6 * 2) + 1 => 13

            o.SetBaseAttribute(key, 5);
            Console.WriteLine(o.GetCurrentAttribute(key)); // ((5 + 3) * 2) + 1 => (8 * 2) + 1 => 17

            o.ClearLayeredEffects();
            Console.WriteLine(o.GetCurrentAttribute(key)); // with no effects => 5

            Console.ReadLine();
        }
    }

    internal class SortedLayeredEffects : IEnumerable<LayeredEffectDefinition>
    {
        private AttributeKey _key;
        private SortedSet<LayeredEffectDefinition> _effects;

        //There are a lot of ways of doing locking. I haven't worked at all with most 
        //of them so I would need to do further research on the topic.
        private readonly object _lock = new object();

        public SortedLayeredEffects(AttributeKey key)
        {
            if (key == AttributeKey.Invalid) throw new ApplicationException($"Attribute key not valid.");
            _key = key;
            _effects = new SortedSet<LayeredEffectDefinition>(LayeredEffectComparer.Instance);
        }

        public void Add(LayeredEffectDefinition effect)
        {
            if (effect.Attribute != _key)
                throw new ApplicationException($"Incorrect attribute for this instance. Should be {_key}.");
            if (effect.Operation == EffectOperation.Invalid)
                throw new ApplicationException($"Incorrect attribute for this instance. Should be {_key}.");

            if (effect.Operation == EffectOperation.Set)
            {
                //An optimization could be made here to remove any effects where layer <= effect.Layer
            }

            //Locking to maintain order.
            lock (_lock)
            {
                _effects.Add(effect);
            }
        }

        public int GetCurrentValue(int baseVal)
        {
            int output = baseVal;

            //Locking this so the layered effects cannot change out from under us while trying to calculate the current value.
            lock (_lock)
            {
                foreach (LayeredEffectDefinition effect in this)
                {
                    //Console.WriteLine(effect); //used for debugging to see the order
                    output = ApplyEffect(output, effect);
                }
            }
            return output;
        }

        #region Private Implementation
        private int ApplyEffect(int currentValue, LayeredEffectDefinition effect)
        {
            switch (effect.Operation)
            {
                case EffectOperation.Set: return effect.Modification;
                case EffectOperation.Add: return currentValue + effect.Modification;
                case EffectOperation.Subtract: return currentValue - effect.Modification;
                case EffectOperation.Multiply: return currentValue * effect.Modification;
                case EffectOperation.BitwiseOr: return currentValue | effect.Modification;
                case EffectOperation.BitwiseAnd: return currentValue & effect.Modification;
                case EffectOperation.BitwiseXor: return currentValue ^ effect.Modification;
                default:
                    //Do nothing. Alternatively, an exception could be thrown. 
                    //However, when trying to apply effects is not the time to slow down for exceptions. 
                    //It would be better to catch them as they are added.
                    return currentValue;
            }
        }
        #endregion

        #region IEnumerable<T> Implemenation
        public IEnumerator<LayeredEffectDefinition> GetEnumerator()
        {
            return ((IEnumerable<LayeredEffectDefinition>)_effects).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<LayeredEffectDefinition>)_effects).GetEnumerator();
        }
        #endregion
    }

    internal class LayeredEffectComparer : Comparer<LayeredEffectDefinition>
    {
        public static LayeredEffectComparer Instance { get; } = new LayeredEffectComparer();

        public override int Compare(LayeredEffectDefinition x, LayeredEffectDefinition y)
        {
            if (x.Layer.CompareTo(y.Layer) != 0)
                return x.Layer.CompareTo(y.Layer);

            //If they are the same layer, then they stay in the order they came in.
            return 1;
        }
    }

    public class MyLayeredAttributes : ILayeredAttributes
    {
        ConcurrentDictionary<AttributeKey, int> _baseVal = new ConcurrentDictionary<AttributeKey, int>();
        ConcurrentDictionary<AttributeKey, SortedLayeredEffects> _effectsByAttr = new ConcurrentDictionary<AttributeKey, SortedLayeredEffects>();

        public MyLayeredAttributes()
        {
            InitBaseAttributes();
        }


        #region Private Implementation
        private void InitBaseAttributes()
        {
            foreach (var key in Enum.GetValues(typeof(AttributeKey)))
            {
                _baseVal.AddOrUpdate((AttributeKey)key, 0, (k, v) => 0); //always update to 0;
            }
        }
        #endregion

        #region Public Interface
        public void SetBaseAttribute(AttributeKey key, int value)
        {
            _baseVal.AddOrUpdate(key, value, (k, v) => value);
        }

        public int GetCurrentAttribute(AttributeKey key)
        {
            return _effectsByAttr.GetOrAdd(key, (k) => new SortedLayeredEffects(k)).GetCurrentValue(_baseVal[key]);
        }

        public void AddLayeredEffect(LayeredEffectDefinition effect)
        {
            _effectsByAttr.GetOrAdd(effect.Attribute, (k) => new SortedLayeredEffects(k)).Add(effect);
        }

        public void ClearLayeredEffects()
        {
            _effectsByAttr.Clear();
        }
        #endregion
    }
}
