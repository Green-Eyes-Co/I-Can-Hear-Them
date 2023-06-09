using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] float interactDistance = 1f;
    [SerializeField] LayerMask interactionLayerMask;
    [SerializeField] TMP_Text interactText;
    [SerializeField] GameObject interactDot;
    [SerializeField] float messageTextDuration = 1f;

    IInteractable currentInteractable;
    bool isShowingMessage;

    void Update()
    {
        var showText = false;
        currentInteractable = null;

        if (!isShowingMessage)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out var hit, interactDistance, interactionLayerMask))
            {
                IInteractable interactable;
                if (hit.transform.TryGetComponent(out interactable))
                {
                    showText = true;
                    currentInteractable = interactable;

                    //interactText.text = interactable.GetMouseOverText();
                }
            }
            interactDot.SetActive(showText);
            //interactText.gameObject.SetActive(showText);
        }
    }

    void OnInteract()
    {
        string result;
        if (currentInteractable == null)
        {
            return;
        }

        result = currentInteractable.Interact();

        if (!string.IsNullOrEmpty(result))
        {
            StartCoroutine(ShowMessageCoroutine(result));
        }
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        isShowingMessage = true;
        interactDot.SetActive(false);

        interactText.text = message;
        interactText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageTextDuration);
        interactText.gameObject.SetActive(false);

        isShowingMessage = false;
        interactDot.SetActive(true);
    }
}
