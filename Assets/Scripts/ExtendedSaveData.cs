using UnityEngine;

[System.Serializable]
public class ExtendedSaveData
{
    public ExtendedSaveData(string _saveFileName, Texture2D _snapshot) => (SaveFileName, Snapshot) = (_saveFileName, _snapshot);

    public string SaveFileName { get; private set; }
    public Texture2D Snapshot { get; private set; }

    public SaveData GetSaveData() => GameManager.Get().GetSaveData(SaveFileName);
}
