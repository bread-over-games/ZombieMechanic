/// this holds references to scriptable objects for backpacks

using UnityEngine;

public class BackpackAssets : MonoBehaviour
{
    public static BackpackAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public BackpackData smallBackpackSO;
}
