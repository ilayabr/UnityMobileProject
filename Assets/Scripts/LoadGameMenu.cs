using UnityEngine;

public class LoadGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameSaveCellPrefab;
    [SerializeField] private Transform gameSaveCellsParent;

    void Start()
    {
        var allSaves = GameManager.Get().GetAllExtSaves();

        foreach (var save in allSaves)
        {
            var gameSaveCell = Instantiate(gameSaveCellPrefab, gameSaveCellsParent).GetComponent<GameSaveCell>();
            gameSaveCell.Setup(save);
        }
    }
}
