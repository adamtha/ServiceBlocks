﻿using System;

namespace ServiceBlocks.DistributedCache.Common
{
    public class CacheValueWrapper<T>
    {
        public CacheValueWrapper()
        {
            SetNotFound();
        }

        private CacheValueState _state;
        public T Value { get; set; }

        public CacheValueState State
        {
            get
            {
                if (Expires.HasValue && Expires.Value < DateTime.UtcNow)
                    return CacheValueState.Expired;
                return _state;
            }
            set { _state = value; }
        }

        public DateTime LastModified { get; set; }
        public DateTime? Expires { get; set; }

        private void SetTimeStamps(TimeSpan? ttl = null)
        {
            LastModified = DateTime.UtcNow;
            if (ttl.HasValue)
                Expires = DateTime.UtcNow.Add(ttl.Value);
        }

        public void SetValue(T value, TimeSpan? ttl = null)
        {
            Value = value;
            State = CacheValueState.Exists;
            SetTimeStamps(ttl);
        }
        public void SetMissing()
        {
            Value = default(T);
            State = CacheValueState.Missing;
            SetTimeStamps();
        }
        public void SetNotFound(TimeSpan? ttl = null)
        {
            Value = default(T);
            State = CacheValueState.NotFound;
            SetTimeStamps(ttl);
        }

        public static CacheValueWrapper<T> CreateNotFound(TimeSpan? ttl = null)
        {
            var value = new CacheValueWrapper<T>();
            value.SetNotFound(ttl);
            return value;
        }
        public static CacheValueWrapper<T> CreateMissing()
        {
            var value = new CacheValueWrapper<T>();
            value.SetMissing();
            return value;
        }
    }
}
