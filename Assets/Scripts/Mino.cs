using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering.Universal;

public class Mino : MonoBehaviour
{
    public MinoState state;
    [SerializeField]private MeshRenderer _render;
    private Material _mat;
    [SerializeField]private Color _colorFirst = new Color(1, 0, 0), _colorSecond = new Color(0, 0, 1),
        _colorGhost = new(.8f, .8f, .8f);
    [SerializeField] private float _intensity = 3.17f, _threshold = 0;
    public MinoState State {  get { return state; } }
    public int Index;
    public void Initialize()
    {
        _mat = _render.sharedMaterial;
        SetState(MinoState.Empty);
    }

    private void SetMinoMat(float intensity = 1, float threshold = 0, Color color = default)
    {
        if (_mat.enableInstancing)
        {
            MaterialPropertyBlock props = new();
            props.SetColor("_color", color);
            props.SetFloat("_fresnelIntensity", intensity);
            props.SetFloat("_thresh", threshold);
            _render.SetPropertyBlock(props);
        }
        else
        {
            _mat.SetColor("_color", color);
            _mat.SetFloat("_fresnelIntensity", intensity);
            _mat.SetFloat("_thresh", threshold);
        }
    }

    public void SetState(MinoState ms)
    {
        state = ms;
        switch (ms)
        {
            case MinoState.Empty:   SetMinoMat(1, 1); break;
            case MinoState.Ghost:   SetMinoMat(_intensity, _threshold, _colorGhost); break;
            case MinoState.First:   SetMinoMat(_intensity, _threshold, _colorFirst); break;
            case MinoState.Second:  SetMinoMat(_intensity, _threshold, _colorSecond); break;
            default: break;
        }
    }
    private void Start()
    {
        Initialize();
    }
    private void Update()
    {
        //SetState(state);
        
    }
}

public enum MinoState
{
    Empty,
    Ghost,
    First,
    Second,
}
