using UnityEngine;

public class PinSpawner : MonoBehaviour
{
    [Header("Commons")]
    // ÇÉ ÇÁ¸®ÆÕ
    [SerializeField]
    private GameObject pinPrefab;

    public void SpawnThrowablePin(Vector3 position)
    {
        // ÇÉ ¿ÀºêÁ§Æ® »ý¼º
        Instantiate(pinPrefab, position, Quaternion.identity);
    }
}
