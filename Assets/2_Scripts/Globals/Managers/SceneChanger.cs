
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

    [SerializeField] GameObject _loadCanvas;
    [SerializeField] Image _loadingSlider;
    float _target;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

  
    public async void LoadAsyncSceneByName(string sceneName)
    {
        _target = 0;
        _loadingSlider.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loadCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loadCanvas.SetActive(false);
    }

    public async void LoadAsyncSceneByIndex(int sceneIndex)
    {
        _target = 0;
        _loadingSlider.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneIndex);
        scene.allowSceneActivation = false;

        _loadCanvas.SetActive(true);

        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        }
        while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _loadCanvas.SetActive(false);
    }

    private void Update()
    {
        _loadingSlider.fillAmount = Mathf.MoveTowards(_loadingSlider.fillAmount, _target, 3 * Time.deltaTime);
    }


}
