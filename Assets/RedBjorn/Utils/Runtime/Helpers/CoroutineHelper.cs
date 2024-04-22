using System;
using System.Collections;
using UnityEngine;

namespace RedBjorn.Utils
{
    /// <summary>
    /// Helper gameObject to launch coroutines from non-Monobehaviour classes
    /// </summary>
    public class CoroutineHelper : MonoBehaviour
    {
        static CoroutineHelper CachedInstance;
        static CoroutineHelper Instance
        {
            get
            {
                if (CachedInstance == null)
                {
                    var go = new GameObject("CoroutineLauncher");
                    DontDestroyOnLoad(go);
                    CachedInstance = go.AddComponent<CoroutineHelper>();
                }
                return CachedInstance;
            }
        }

        public static Coroutine Launch(IEnumerator ienum, Action onCompleted = null)
        {
            return Instance.LaunchInternal(ienum, onCompleted);
        }

        public static void Abort(Coroutine coroutine)
        {
            Instance.AbortInternal(coroutine);
        }

        Coroutine LaunchInternal(IEnumerator ienum, Action onCompleted = null)
        {
            return StartCoroutine(WithOnCompleted(ienum, onCompleted));
        }

        void AbortInternal(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }

        IEnumerator WithOnCompleted(IEnumerator ienum, Action onCompleted)
        {
            while (true)
            {
                object current;
                if (!ienum.MoveNext())
                {
                    break;
                }
                current = ienum.Current;
                yield return current;
            }
            onCompleted.SafeInvoke();
        }
    }
}