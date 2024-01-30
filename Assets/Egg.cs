using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Egg : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform graphic;
    public GameObject m_objectToSpawn;
    private Collider2D hitbox;
    public Image Image {get{return graphic.transform.GetComponent<Image>();}}
    private Vector2 pos;
    private bool clicking = false;
    void Start()
    {
        hitbox = GetComponent<Collider2D>();
        pos = startPosition.position;
        transform.position = pos;
    }
    void Update()
    {
        if(clicking)
        {
            pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
        }
        transform.position = pos;
    }

    public void HandleClick()
    {
        if(!clicking)
        {
            OnClickOnce();
        }
        else
        {
            OnClickAgain();
        }
    }
    private void OnClickOnce()
    {
        hitbox.enabled = false;
        clicking = true;
    }
    private void OnClickAgain()
    {
        hitbox.enabled = true;
        clicking = false;
    }
    private void PlaceOnSkillet()
    {
        //Debug.Log("Destroying Egg");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Skillet"))
        {
            if(GameManager.IsStoveOn)
            {
                PlaceOnSkillet();
            }
        }
    }
}
