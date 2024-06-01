using UnityEngine;

class EnabledController : MonoBehaviour
{
    [SerializeField] private GameObject _controlledObject;

    public void DisableObject()
    {
        _controlledObject.SetActive(false);
    }

    public void EnableObject()
    {
        _controlledObject.SetActive(true);
    }
}