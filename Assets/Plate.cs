using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    private GameObject obj;
    [SerializeField] private float dishJudgeDelay = 1f;
    [SerializeField][Range(0,1)] private float acceptableCookedPercentage = 0.9f;
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
        var col = obj.GetComponent<Image>().color;
        Debug.Log(col);
        bool cooked = (col.r >= acceptableCookedPercentage) && (col.g >= acceptableCookedPercentage) && (col.b >= acceptableCookedPercentage);
        // comparing by obj.GetComponent<Image>().color == color.white produced undesired result.
        string msg = "It's cooked to perfection! Well done!";
        if(!cooked)
        {
            msg = "It's not cooked perfectly, but you can try again!";
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
