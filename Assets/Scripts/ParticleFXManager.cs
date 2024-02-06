using UnityEngine;

public class ParticleFXManager : MonoBehaviour
{
    [SerializeField] private GameObject[] particleFXPrefabs;
    public static ParticleFXManager Instance;

    private void Awake() 
    {
        Instance = this;
    }
    private GameObject RetrieveParticleFX(string name) 
    {
        GameObject prefab = null;
        foreach (GameObject particleFXPrefab in particleFXPrefabs)
        {
            if (particleFXPrefab.name == name)
            {
                prefab = particleFXPrefab;
                break;
            }
        }
        return prefab;
    }
    public GameObject CreateParticleFX(string name, Vector3 position)
    {
        GameObject prefab = RetrieveParticleFX(name);

        if (prefab != null)
        {
            GameObject particleFX = Instantiate(prefab, position, Quaternion.identity);
            Destroy(particleFX, particleFX.GetComponent<ParticleSystem>().main.duration);
            return particleFX;
        }
        else
        {
            Debug.LogError("ParticleFXManager: No prefab found with name " + name);
            return null;
        }
    }

    public GameObject CreateParticleFX(string name, Transform parent, Vector3 relativePosition)
    {
        GameObject prefab = RetrieveParticleFX(name);

        if (prefab != null)
        {
            GameObject particleFX = Instantiate(prefab, parent);
            particleFX.transform.localPosition = relativePosition;
            Destroy(particleFX, particleFX.GetComponent<ParticleSystem>().main.duration);
            return particleFX;
        }
        else
        {
            Debug.LogError("ParticleFXManager: No prefab found with name " + name);
            return null;
        }
    }
}
