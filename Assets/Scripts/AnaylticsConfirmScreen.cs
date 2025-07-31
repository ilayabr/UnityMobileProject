using UnityEngine;

public class AnaylticsConfirmScreen : MonoBehaviour
{
    public void Deny()
    {
        gameObject.SetActive(false);
    }
    public void Confirm()
    {
        PlayerPrefs.SetInt("data-consent", 1);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }
}
