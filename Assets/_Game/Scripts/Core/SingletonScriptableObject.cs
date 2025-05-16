using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>(typeof(T).Name);
                if (instance == null)
                {
                    Debug.LogError($"SingletonScriptableObject: {typeof(T).Name} not found in Resources folder.");
                }
            }
            return instance;
        }
    }
}
