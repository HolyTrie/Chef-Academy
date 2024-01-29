using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PolygonCollider2D))]
public class Skillet : MonoBehaviour
{
    private GameObject curr_obj = null;
    public bool HasObject{get{return curr_obj != null;}}
    private Image ObjImage{get{return curr_obj.GetComponent<Image>();}}
    private bool Clicking{get{return GameManager.Clicking;} set{GameManager.Clicking = value;}}
    private void PlaceNewObject(GameObject obj, Vector2 pos)
    {
        if(curr_obj!=null)
        {
            Destroy(curr_obj);
        }
        curr_obj = Instantiate(obj,pos,quaternion.identity,transform); // also sets skillet as the parent of the object in the scene hierarchy.
        ObjImage.color = Color.grey;
        _edgeCollider.enabled = true;
    }
    private void RemoveCurrObject()
    {
        Destroy(curr_obj);
    }
    private void IncreaseColours(float value)
    {
        value *= Time.deltaTime;
        var col = ObjImage.color;
        col.r += value;
        col.g += value;
        col.b += value;
        ObjImage.color = col;
    }
    [SerializeField] float cookingPower = 1f;
    [SerializeField] Transform StartPosition;
    [SerializeField] Transform heatImage;
    [SerializeField] Transform handleRef;
    private Image heatImg;
    private Collider2D basket;
    private Vector2 pos;
    private bool heat = false;
    private bool onStove = false;
    public bool Heat
    {
        get{return heat;}
        set
        {
            if(value) { heatImg.enabled = true; }
            else { heatImg.enabled = false;  }
            heat = value;
        }
    }
    private Vector2[] _polygonColliderPoints;
    private EdgeCollider2D _edgeCollider;
    private void Awake() {
        /* cool hack to build edge colliders for 'trapping' RigidBody2D objects within the edge,
        *  by building an appropriate polygon collider in the editor and inheriting its points.
        *  @source - https://pressstart.vip/tutorials/2019/08/1/99/rigidbodies-inside-colliders.html
        */
        PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
        _polygonColliderPoints = poly.points;
        _edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        _edgeCollider.points = _polygonColliderPoints;
        _edgeCollider.enabled = false; // start disabled in our use-case.
        poly.enabled = false; // we disable this instead just in case
    }
    void Start()
    {
        pos = StartPosition.position;
        transform.position = pos;
        basket = GetComponent<Collider2D>();
        basket.enabled = false;
        heatImg = heatImage.transform.GetComponent<Image>();
        Heat = false;
    }
    void Update()
    {
        if(Clicking)
        {
            pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
        }
        if(onStove)
        {
            Heat = GameManager.IsStoveOn;
        }
        transform.position = pos;

        if(curr_obj != null)
        {
            IncreaseColours(cookingPower);
        }
    }
    public void HandleClick()
    {
        if(!Clicking)
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
        Heat = false;
        onStove = false;
        basket.enabled = false;
        Clicking = true;
    }
    private void OnClickAgain()
    {
        basket.enabled = true;
        Clicking = false;
    }

    private void AttachToHeat(Collider2D other)
    { 
        Heat = true;
        onStove = true;
        pos = other.transform.position + handleRef.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Skillet trigger with "+other.transform.name);
        if(other.gameObject.CompareTag("Heat"))
        {
            if(GameManager.IsStoveOn)
            {
                Debug.Log("Attaching to Heat Source!");
                AttachToHeat(other);
            }
        }
        if(other.gameObject.CompareTag("Egg"))
        {
            if(GameManager.IsStoveOn)
            {
                Debug.Log("Attaching Egg object to skillet!");
                var obj = other.transform.GetComponent<Egg>().m_objectToSpawn;
                AttachToSkillet(obj,other.transform.position);
            }
        }
    }

    private void AttachToSkillet(GameObject objectToSpawn, Vector2 pos)
    {
        PlaceNewObject(objectToSpawn,pos);
    }

    internal GameObject ChangeObjectOwner(MonoBehaviour new_owner)
    {
        curr_obj.transform.SetParent(new_owner.transform);
        var temp = curr_obj;
        curr_obj = null;
        return temp;
    }

    internal void ResetPosition()
    {
        pos = StartPosition.position;
    }
}
