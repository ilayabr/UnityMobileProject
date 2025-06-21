using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSaveCell : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private TextMeshProUGUI saveName;
    public void Setup(ExtendedSaveData data)
    {
        image.texture = data.Snapshot;
        saveName.text = data.SaveFileName;
    }
}
