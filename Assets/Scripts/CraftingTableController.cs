using UnityEngine;

public class CraftingTableController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingUI;

    public void Interact()
    {
        if (craftingUI.activeSelf == false)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        craftingUI.SetActive(!craftingUI.activeSelf);
    }
}
