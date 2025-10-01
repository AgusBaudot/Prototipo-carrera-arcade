using UnityEngine;
using UnityEditor;

public static class DeletePersonalBest
{
    private const string highScoreKey = "High score";
    
    [MenuItem("Dev tools/Reset personal best")]
    private static void ResetPersonalBest()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            PlayerPrefs.DeleteKey(highScoreKey);
            PlayerPrefs.Save();
        }
    }
}