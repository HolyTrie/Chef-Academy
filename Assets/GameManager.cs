using UnityEditor.SearchService;
using UnityEngine;
//this class auto-adds itself to the game as a singleton - https://gist.github.com/kurtdekker/775bb97614047072f7004d6fb9ccce30
public sealed class GameManager : MonoBehaviour 
{
    // !!!!!! DO NOT PUT THIS IN ANY SCENE; this code auto-instantiates itself once.
	// note for this project - canvas is for ui only and wont work with physics, src: https://forum.unity.com/threads/ui-image-box-collider.296278/
    private static GameManager _Instance;
	public static GameManager Instance
	{
		get
		{
			if (!_Instance)
			{
				_Instance = new GameObject().AddComponent<GameManager>();
				_Instance.name = _Instance.GetType().ToString(); // name it for easy recognition
				DontDestroyOnLoad(_Instance.gameObject); // mark root as DontDestroyOnLoad();
			}
			return _Instance;
		}
	}
    private static bool _isStoveOn = false;
    public static bool IsStoveOn
	{
		get
		{
			return _isStoveOn;
		} 
		set
		{
			_isStoveOn = value;
			//Debug.Log("IsStoveOn = "+value);
		}
	}

	private static bool _clicking;
    public static bool Clicking { get{return _clicking;} internal set{_clicking = value;} }
}
