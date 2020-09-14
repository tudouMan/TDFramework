
#define REQUIRE_LISTENER
 
using System;
using System.Collections.Generic;
using UnityEngine;


namespace TDFramework.EventSystem
{
    public class CEvent
    {
        public int type;
        public object data;

        public int GetEventId()
        {
            return type;
        }

    }


    public  class EventCenter
    {

        //Disable the unused variable warning
#pragma warning disable 0414
        //Ensures that the MessengerHelper will be created automatically upon start of the game.
        //	static private MessengerHelper mMessengerHelper = ( new GameObject("MessengerHelper") ).AddComponent< MessengerHelper >();
#pragma warning restore 0414

         public static  Dictionary<int, Delegate> mEventTable = new Dictionary<int, Delegate>();
        //Message handlers that should never be removed, regardless of calling Cleanup
         public static  List<int> mPermanentMessages = new List<int>();


        //Marks a certain message as permanent.
        public static void MarkAsPermanent(int eventType)
        {
#if LOG_ALL_MESSAGES
		XDebug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif

            mPermanentMessages.Add(eventType);
        }


        public static void Cleanup()
        {
#if LOG_ALL_MESSAGES
		XDebug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

            List<int> messagesToRemove = new List<int>();

            foreach (KeyValuePair<int, Delegate> pair in mEventTable)
            {
                bool wasFound = false;

                foreach (int message in mPermanentMessages)
                {
                    if (pair.Key==message)
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (int message in messagesToRemove)
            {
                mEventTable.Remove(message);
            }
        }

         public static void PrEGameEventEventTable()
        {

            foreach (KeyValuePair<int, Delegate> pair in mEventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

        }

         public static void OnListenerAdding(int eventType, Delegate listenerBeingAdded)
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

         public static void OnListenerRemoving(int eventType, Delegate listenerBeingRemoved)
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

         public static void OnListenerRemoved(int eventType)
        {
            if (mEventTable[eventType] == null)
            {
                mEventTable.Remove(eventType);
            }
        }

         public static void OnBroadcasting(int eventType)
        {
#if REQUIRE_LISTENER
            if (!mEventTable.ContainsKey(eventType))
            {
            }
#endif
        }

         public static BroadcastException CreateBroadcastSignatureException(int eventType)
        {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }

        public  class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        public  class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }

        //No parameters
         public static  void AddListener(int eventType, CallbackE handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE)mEventTable[eventType] + handler;
        }

        //Single parameter
         public static void AddListener<T>(int eventType, CallbackE<T> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T>)mEventTable[eventType] + handler;
        }

        //Two parameters
         public static void AddListener<T, U>(int eventType, CallbackE<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U>)mEventTable[eventType] + handler;
        }

        //Three parameters
         public static void AddListener<T, U, V>(int eventType, CallbackE<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V>)mEventTable[eventType] + handler;
        }

        //Four parameters
         public static void AddListener<T, U, V, X>(int eventType, CallbackE<T, U, V, X> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X>)mEventTable[eventType] + handler;
        }

        //Five parameters
         public static void AddListener<T, U, V, X, Y>(int eventType, CallbackE<T, U, V, X, Y> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y>)mEventTable[eventType] + handler;
        }
        //Six parameters
         public static void AddListener<T, U, V, X, Y, Z>(int eventType, CallbackE<T, U, V, X, Y, Z> handler)
        {
            OnListenerAdding(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y, Z>)mEventTable[eventType] + handler;
        }



        /// <summary>
        /// 直接移除所有触发事件，只要事件不是静态的就会被回收
        /// </summary>
        /// <param name="eventType"></param>
         public static void RemoveAllListener(int eventType)
        {
            if (mEventTable.ContainsKey(eventType))
            {
                mEventTable.Remove(eventType);
            }
        }


        //No parameters
         public static void RemoveListener(int eventType, CallbackE handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Single parameter
         public static void RemoveListener<T>(int eventType, CallbackE<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Two parameters
        public static void RemoveListener<T, U>(int eventType, CallbackE<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Three parameters
        public static void RemoveListener<T, U, V>(int eventType, CallbackE<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Four parameters
        public static void RemoveListener<T, U, V, X>(int eventType, CallbackE<T, U, V, X> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Five parameters
        public static void RemoveListener<T, U, V, X, Y>(int eventType, CallbackE<T, U, V, X, Y> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Six parameters
         public static void RemoveListener<T, U, V, X, Y, Z>(int eventType, CallbackE<T, U, V, X, Y, Z> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallbackE<T, U, V, X, Y, Z>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }



         public static void Broadcast(int eventType)
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

         public static void SendEvent(CEvent evt)
        {
            Broadcast<CEvent>(evt.GetEventId(), evt);
        }

        //Single parameter
         public static void Broadcast<T>(int eventType, T arg1)
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
         public static void Broadcast<T, U>(int eventType, T arg1, U arg2)
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
        public static void Broadcast<T, U, V>(int eventType, T arg1, U arg2, V arg3)
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
        public static void Broadcast<T, U, V, X>(int eventType, T arg1, U arg2, V arg3, X arg4)
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
         public static void Broadcast<T, U, V, X, Y>(int eventType, T arg1, U arg2, V arg3, X arg4, Y arg5)
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
         public static void Broadcast<T, U, V, X, Y, Z>(int eventType, T arg1, U arg2, V arg3, X arg4, Y arg5, Z arg6)
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

