//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework;

namespace UGFExtensions.Hotfix
{
    public static partial class ReferencePool
    {
        private sealed class ReferenceCollection
        {
            private readonly Queue<IReference> m_References;

            public ReferenceCollection(Type referenceType)
            {
                m_References = new Queue<IReference>();
                ReferenceType = referenceType;
                UsingReferenceCount = 0;
                AcquireReferenceCount = 0;
                ReleaseReferenceCount = 0;
                AddReferenceCount = 0;
                RemoveReferenceCount = 0;
            }

            public Type ReferenceType { get; }

            public int UnusedReferenceCount => m_References.Count;

            public int UsingReferenceCount { get; private set; }

            public int AcquireReferenceCount { get; private set; }

            public int ReleaseReferenceCount { get; private set; }

            public int AddReferenceCount { get; private set; }

            public int RemoveReferenceCount { get; private set; }

            public T Acquire<T>() where T : class, IReference, new()
            {
                if (typeof(T) != ReferenceType) throw new GameFrameworkException("Type is invalid.");

                UsingReferenceCount++;
                AcquireReferenceCount++;

                if (m_References.Count > 0) return (T) m_References.Dequeue();


                AddReferenceCount++;
                return new T();
            }

            public IReference Acquire()
            {
                UsingReferenceCount++;
                AcquireReferenceCount++;

                if (m_References.Count > 0) return m_References.Dequeue();


                AddReferenceCount++;
                return (IReference) Activator.CreateInstance(ReferenceType);
            }

            public void Release(IReference reference)
            {
                reference.Clear();

                if (m_References.Contains(reference))
                    throw new GameFrameworkException("The reference has been released.");

                m_References.Enqueue(reference);


                ReleaseReferenceCount++;
                UsingReferenceCount--;
            }

            public void Add<T>(int count) where T : class, IReference, new()
            {
                if (typeof(T) != ReferenceType) throw new GameFrameworkException("Type is invalid.");


                AddReferenceCount += count;
                while (count-- > 0) m_References.Enqueue(new T());
            }

            public void Add(int count)
            {
                AddReferenceCount += count;
                while (count-- > 0) m_References.Enqueue((IReference) Activator.CreateInstance(ReferenceType));
            }

            public void Remove(int count)
            {
                if (count > m_References.Count) count = m_References.Count;

                RemoveReferenceCount += count;
                while (count-- > 0) m_References.Dequeue();
            }

            public void RemoveAll()
            {
                RemoveReferenceCount += m_References.Count;
                m_References.Clear();
            }
        }
    }
}