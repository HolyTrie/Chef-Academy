using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    [SerializeField] private RectTransform gameOverScreen;
    public RectTransform GameOverScreen { get; }
    private void Start()
    {
        HideSceneEndMsg();
        //DisplaySceneEndMsg(null);
    }
    public void ResetCurrScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNewScene(int _buildIndex)
    {
        SceneManager.LoadScene(_buildIndex);
    }
    public void DisplaySceneEndMsg(string msg)
    {
        msg ??= "You have been juged, and found worthy!"; //if msg is null then assign this string to it.
        gameOverScreen.gameObject.GetComponent<TextMeshProUGUI>().text = msg; // for some reason using TexTMeshProGUI worked. https://stackoverflow.com/questions/47761666/textmeshpro-null-reference-exception
        gameOverScreen.gameObject.SetActive(true);
    }
    public void HideSceneEndMsg()
    {
        gameOverScreen.gameObject.SetActive(false);
    }
}
