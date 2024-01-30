using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    private GameObject obj;
    [SerializeField] private float dishJudgeDelay = 1f;
    private Collider2D _collider;
    private bool Clicking{get{return GameManager.Clicking;} set{GameManager.Clicking = value;}}

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }
    private void Update() 
    {
        if(Clicking)
        {
            _collider.enabled = false;
        }
        else
        {
            _collider.enabled = true;
        }
    }

    IEnumerator JudgeItem()
    {
        
        yield return new WaitForSeconds(dishJudgeDelay);
        bool cooked = obj.GetComponent<Image>().color == Color.white;
        string msg = new("It's not cooked perfectly, but you can try again!");
        if(cooked)
        {
            msg = "It's cooked to perfection! Well done!";
        }
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SceneController>().DisplaySceneEndMsg(msg);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("Skillet"))
        {
            var skillet = other.gameObject.GetComponent<Skillet>(); 
            if(skillet.HasObject)
            {
                obj = skillet.ChangeObjectOwner(this);
                skillet.ResetPosition();
                GameManager.IsStoveOn = false;
                StartCoroutine(JudgeItem());
            }
        }
    }
}
