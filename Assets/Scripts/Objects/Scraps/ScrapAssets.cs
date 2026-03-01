/// Holds reference to all scrap scriptable objects

using UnityEngine;

public class ScrapAssets : MonoBehaviour
{
    public static ScrapAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public ScrapData sparePartsBoxSO;    
}
