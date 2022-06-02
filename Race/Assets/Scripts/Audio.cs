using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string _createdTag;
    private void Awake()
    {
        GameObject obj = GameObject.FindWithTag(this._createdTag);
        if (obj != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.tag = this._createdTag;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
