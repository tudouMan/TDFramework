
#define REQUIRE_LISTENER
 
using System;
using System.Collections.Generic;
using UnityEngine;


namespace TDFramework.EventSystem
{
    public class CEvent
    {
        public Enum type;
        public object data;

        public Enum GetEventId()
        {
            return type;
        }

    }
    public static class EventCenter
    {

        //Disable the unused variable warning
#pragma warning disable 0414
        //Ensures that the MessengerHelper will be created automatically upon start of the game.
        //	static private MessengerHelper mMessengerHelper = ( new GameObject("MessengerHelper") ).AddComponent< MessengerHelper >();
#pragma warning restore 0414

        static public Dictionary<Enum, Delegate> mEventTable = new Dictionary<Enum, Delegate>();
        //Message handlers that should never be removed, regardless of calling Cleanup
        static public List<Enum> mPermanentMessages = new List<Enum>();


        //Marks a certain message as permanent.
        static public void MarkAsPermanent(Enum eventType)
        {
#if LOG_ALL_MESSAGES
		XDebug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif

            mPermanentMessages.Add(eventType);
        }


        static public void Cleanup()
        {
#if LOG_ALL_MESSAGES
		XDebug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

            List<Enum> messagesToRemove = new List<Enum>();

            foreach (KeyValuePair<Enum, Delegate> pair in mEventTable)
            {
                bool wasFound = false;

                foreach (Enum message in mPermanentMessages)
                {
                    if (pair.Key == message)
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (Enum message in messagesToRemove)
            {
                mEventTable.Remove(message);
            }
        }

        static public void PrEGameEventEventTable()
        {

            foreach (KeyValuePair<Enum, Delegate> pair in mEventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

        }

        static public void OnListenerAdding(Enum eventType, Delegate listenerBeingAdded)
        {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		XDebug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

            if (!mEventTable.ContainsKey(eventType))
            {
                mEventTable.Add(eventType, null);
            }

            Delegate d = mEventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
            }
        }

        static public void OnListenerRemoving(Enum eventType, Delegate listenerBeingRemoved)
        {
#if LOG_ALL_MESSAGES
		XDebug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

            if (mEventTable.ContainsKey(eventType))
            {
                Delegate d = mEventTable[eventType];

                if (d == null)
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
            }
        }

        static public void OnListenerRemoved(Enum eventType)
        {
            if (mEventTable[eventType] == null)
            {
                mEventTable.Remove(eventType);
            }
        }

        static public void OnBroadcasting(Enum eventType)
        {
#if REQUIRE_LISTENER
            if (!mEventTable.ContainsKey(eventType))
            {
            }
#endif
        }

        static public BroadcastException CreateBroadcastSignatureException(Enum eventType)
        {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }

        public class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        public class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }

        //No parameters
        static public void AddListener(Enum eventType, CallbackE handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE)mEventTable[eventType] + handler;
        }

        //Single parameter
        static public void AddListener<T>(Enum eventType, CallbackE<T> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T>)mEventTable[eventType] + handler;
        }

        //Two parameters
        static public void AddListener<T, U>(Enum eventType, CallbackE<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U>)mEventTable[eventType] + handler;
        }

        //Three parameters
        static public void AddListener<T, U, V>(Enum eventType, CallbackE<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V>)mEventTable[eventType] + handler;
        }

        //Four parameters
        static public void AddListener<T, U, V, X>(Enum eventType, CallbackE<T, U, V, X> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X>)mEventTable[eventType] + handler;
        }

        //Five parameters
        static public void AddListener<T, U, V, X, Y>(Enum eventType, CallbackE<T, U, V, X, Y> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y>)mEventTable[eventType] + handler;
        }
        //Six parameters
        static public void AddListener<T, U, V, X, Y, Z>(Enum eventType, CallbackE<T, U, V, X, Y, Z> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y, Z>)mEventTable[eventType] + handler;
        }



        /// <summary>
        /// 直接移除所有触发事件，只要事件不是静态的就会被回收
        /// </summary>
        /// <param name="eventType"></param>
        static public void RemoveAllListener(Enum eventType)
        {
            if (mEventTable.ContainsKey(eventType))
            {
                mEventTable.Remove(eventType);
            }
        }


        //No parameters
        static public void RemoveListener(Enum eventType, CallbackE handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Single parameter
        static public void RemoveListener<T>(Enum eventType, CallbackE<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Two parameters
        static public void RemoveListener<T, U>(Enum eventType, CallbackE<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Three parameters
        static public void RemoveListener<T, U, V>(Enum eventType, CallbackE<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Four parameters
        static public void RemoveListener<T, U, V, X>(Enum eventType, CallbackE<T, U, V, X> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Five parameters
        static public void RemoveListener<T, U, V, X, Y>(Enum eventType, CallbackE<T, U, V, X, Y> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Six parameters
        static public void RemoveListener<T, U, V, X, Y, Z>(Enum eventType, CallbackE<T, U, V, X, Y, Z> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y, Z>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }



        static public void Broadcast(Enum eventType)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE callback = d as CallbackE;

                if (callback != null)
                {
                    callback();
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        static public void SendEvent(CEvent evt)
        {
            Broadcast<CEvent>(evt.GetEventId(), evt);
        }

        //Single parameter
        static public void Broadcast<T>(Enum eventType, T arg1)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE<T> callback = d as CallbackE<T>;

                if (callback != null)
                {
                    callback(arg1);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Two parameters
        static public void Broadcast<T, U>(Enum eventType, T arg1, U arg2)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE<T, U> callback = d as CallbackE<T, U>;

                if (callback != null)
                {
                    callback(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Three parameters
        static public void Broadcast<T, U, V>(Enum eventType, T arg1, U arg2, V arg3)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE<T, U, V> callback = d as CallbackE<T, U, V>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Four parameters
        static public void Broadcast<T, U, V, X>(Enum eventType, T arg1, U arg2, V arg3, X arg4)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE<T, U, V, X> callback = d as CallbackE<T, U, V, X>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Five parameters
        static public void Broadcast<T, U, V, X, Y>(Enum eventType, T arg1, U arg2, V arg3, X arg4, Y arg5)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE<T, U, V, X, Y> callback = d as CallbackE<T, U, V, X, Y>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Six parameters
        static public void Broadcast<T, U, V, X, Y, Z>(Enum eventType, T arg1, U arg2, V arg3, X arg4, Y arg5, Z arg6)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
        XDebug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallbackE<T, U, V, X, Y, Z> callback = d as CallbackE<T, U, V, X, Y, Z>;

                if (callback != null)
                {
                    callback(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }
    }

}

