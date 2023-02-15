using UnityEngine;
using Util;

public class ColorController : MonoBehaviour
{
    [SerializeField] private Material _material;
    private MeshRenderer _renderer;
    
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = _material;
    }

    public void RandomizeColor()
    {
        _material.color = new Color(Rng.NextF(1f), Rng.NextF(1f), Rng.NextF(1f));
    }
    
}
