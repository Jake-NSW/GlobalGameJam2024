using System;
using UnityEngine;

namespace Jam
{
    public readonly struct TimeSince : IEquatable<TimeSince>, IEquatable<float>
    {
        private TimeSince(float time)
        {
            m_Time = Time.time - time;
        }

        private readonly float m_Time;

        public static implicit operator TimeSpan(TimeSince ts) => TimeSpan.FromSeconds(ts);
        public static implicit operator float(TimeSince ts) => Time.time - ts.m_Time;
        public static implicit operator TimeSince(float ts) => new TimeSince(ts);

        public override bool Equals(object obj)
        {
            if (obj is float value)
            {
                return value.Equals(Time.time - m_Time);
            }

            return obj is TimeSince other && Equals(other);
        }

        public bool Equals(TimeSince other)
        {
            return m_Time.Equals(other.m_Time);
        }

        public bool Equals(float other)
        {
            return (Time.time - m_Time).Equals(other);
        }

        public override string ToString()
        {
            return $"since:{Time.time - m_Time}";
        }

        public override int GetHashCode()
        {
            return m_Time.GetHashCode();
        }
    }
}
