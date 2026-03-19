using UnityEngine;

public class AntibioticsAssets : MonoBehaviour
{
    public static AntibioticsAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public AntibioticsData bsAntibioticsSO;
}
