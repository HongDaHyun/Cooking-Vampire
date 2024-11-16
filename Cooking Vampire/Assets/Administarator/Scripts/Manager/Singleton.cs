using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} 인스턴스가 씬에 존재하지 않습니다. 미리 추가해주세요.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.LogWarning($"{typeof(T).Name}의 중복 인스턴스가 발견되어 삭제됩니다.");
            Destroy(gameObject);
        }
    }
}
