using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class NextSceneTrigger : MonoBehaviour
{
    // Si se deja vacío, carga la siguiente escena de la lista de Build Profiles
    [SerializeField] private string sceneName;

    private bool loading;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (loading || other.GetComponentInParent<PlayerController>() == null) return;

        if (!string.IsNullOrEmpty(sceneName))
        {
            loading = true;
            SceneManager.LoadScene(sceneName);
            return;
        }

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("No hay más escenas en Build Profiles → Scene List.");
            return;
        }

        loading = true;
        SceneManager.LoadScene(nextIndex);
    }
}
