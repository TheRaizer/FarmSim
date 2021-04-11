using UnityEngine;

/// <class name="Singleton">
///     <summary>
///         Base class for any singleton.
///     </summary>
/// </class>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static readonly object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            // while running whats in the lock no other thread can execute it.
            lock (m_Lock)
            {
                // if there is no instance
                if (m_Instance == null)
                {
                    // try and find an instance
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // if no instance is found
                    if (m_Instance == null)
                    {
                        // create a single instance
                        GameObject singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }

    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}
