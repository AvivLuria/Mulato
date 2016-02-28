using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class DelayedExecute
    {
        public static void ExecuteWithDelay(this MonoBehaviour behavior, float delay, Action action) {
            if (action != null) {
                behavior.StartCoroutine(DelayedExecutionCoroutine(delay, action));
            }
        }

        private static IEnumerator DelayedExecutionCoroutine(float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}



