//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UGFExtensions.Hotfix
{
    public sealed partial class EventPool<T> where T : HotfixGameEventArgs
    {
        /// <summary>
        ///     事件结点。
        /// </summary>
        private sealed class Event
        {
            public Event(object sender, T e)
            {
                Sender = sender;
                EventArgs = e;
            }

            public object Sender { get; }

            public T EventArgs { get; }
        }
    }
}