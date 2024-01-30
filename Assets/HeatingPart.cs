using UnityEngine;
using UnityEngine.UI;

public class HeatingPart : MonoBehaviour
{
    [SerializeField] private Color _offColor = Color.gray;
    [SerializeField] private Color _onColor = Color.red;
    [SerializeField] private Transform HeatArea;
    private bool _isOn;
    public bool IsOn{
        get{return _isOn;}
        set
        {
            _isOn = value;
            GameManager.IsStoveOn = _isOn;
        }
    }
    private Image _image;
    void Start()
    {
        _image = HeatArea.transform.GetComponent<Image>();
        _isOn = false;
        SetColor(_offColor);
    }

    public void OnPowerClick()
    {
        IsOn = !IsOn;
        if(IsOn)
        {
            SetColor(_onColor);
        }
        else
        {
            SetColor(_offColor);
        }
    }
    private void SetColor(Color color)
    {
        _image.color = color;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        return;
    }
}
