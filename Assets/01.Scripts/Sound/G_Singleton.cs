// 실행 순서를 최대한 앞당겨서 Inst보다 먼저 Awake 보장
using UnityEngine;

[DefaultExecutionOrder(-100)]
// Scene이 넘어가도 유지되는 직관적인 제네릭 싱글턴
public class G_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;
    private static bool appIsClosing = false;

    public static T Inst
    {
        get
        {
            // 앱 종료 중일 때의 예외 처리 (에디터 내 고스트 객체 생성 방지)
            if (appIsClosing)
                return null;

            // Unity null 대응 (Destroy된 객체 포함)
            if (m_Instance == null)
            {
                // 1. 씬에 이미 인스턴스가 존재하는지 검색   
                m_Instance = FindFirstObjectByType<T>();

                // 2. 씬에 없다면 새로 생성
                if (m_Instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    m_Instance = go.AddComponent<T>();
                }
            }

            return m_Instance;
        }
    }

    protected virtual void Awake()
    {
        // 인스턴스가 나 자신이 아니라면 중복 생성된 것이므로 파괴
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 최초 생성 시 셋업
        m_Instance = this as T;

        // DontDestroyOnLoad는 최상위 오브젝트에서만 작동하므로 부모를 해제
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }

        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnMyAppQuit() { }

    private void OnApplicationQuit()
    {
        OnMyAppQuit();
        appIsClosing = true;
    }
}