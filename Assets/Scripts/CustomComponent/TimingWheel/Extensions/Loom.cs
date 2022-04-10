﻿using System;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions
{
    public class Loom
    {
        public static Loom Instance { get; } = new Loom(Thread.CurrentThread.ManagedThreadId);
        private readonly int m_ThreadId;

        private readonly ConcurrentQueue<Action> m_Queue = new ConcurrentQueue<Action>();

        private Action m_Action;

        private Loom(int threadId)
        {
            this.m_ThreadId = threadId;
        }

        public void Update()
        {
            while (true)
            {
                if (!this.m_Queue.TryDequeue(out m_Action))
                {
                    return;
                }
            
                try
                {
                    m_Action();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
        public void Post(Action action)
        {
            if (Thread.CurrentThread.ManagedThreadId == this.m_ThreadId)
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }

                return;
            }

            this.m_Queue.Enqueue(action);
        }
		
        public void PostNext(Action action)
        {
            this.m_Queue.Enqueue(action);
        }
    }
}