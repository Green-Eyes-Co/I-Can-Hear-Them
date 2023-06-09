public class ItemPickup : BaseInteractable
{
    protected void Start()
    {
        GameManager.Instance.TotalItems++;
    }

    public override string GetMouseOverText()
    {
        return "Press E to pick up " + name;
    }

    public override string Interact()
    {
        GameManager.Instance.ItemGot();
        Destroy(gameObject);
        return $"{GameManager.Instance.CurrentItems} / {GameManager.Instance.TotalItems} items got";
    }
}
