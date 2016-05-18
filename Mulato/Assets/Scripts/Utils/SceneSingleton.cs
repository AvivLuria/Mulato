using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// This is a scene singleton - a base class for classes that should only exist once per scene
    /// Correct usage is for class Child to inherit from SceneSingleton<Child>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T g_instance;

        public static T main {
            get {
                //if(g_instance == null)
                //{
                //    return gameObject;
                //}
                return g_instance;
            }
        }

        public virtual void Awake() {
            if (g_instance != null) {
                Debug.LogError("Multiple instances of " + typeof(T).Name + " exist in the same scene");
            }
            g_instance = this as T;
        }

        public virtual void OnDestroy() {
            g_instance = null;
        }
    }
}