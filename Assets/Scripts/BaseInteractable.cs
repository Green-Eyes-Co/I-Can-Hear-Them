using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    protected virtual void Awake()
    {
        //gameObject.layer = (int)Layers.Interactable;
        //foreach (var child in GetComponentsInChildren<Transform>())
        //{
        //    child.gameObject.layer = (int)Layers.Interactable;
        //}
    }

    public virtual string GetMouseOverText()
    {
        return "Press E to interact.";
    }

    public virtual string Interact()
    {
        Debug.LogWarning($"No interaction for this interactable ({name})");
        return null;
    }
}
