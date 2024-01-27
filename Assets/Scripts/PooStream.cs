using DefaultNamespace;
using UnityEngine;

/// <summary>
/// Controls the visual intensity of the characters poo stream.
/// </summary>
public class PooStream : MonoBehaviour
{
    private readonly Vector3 _pooViolenceBoxSize = new Vector3(0.25f, 0.25f, 0.25f);
    [field: SerializeField] public Color PooColor { get; private set; }
    [field: SerializeField, Range(0, 100)] public int PooViolence { get; private set; } = 0;
    [field:SerializeField, Range(0, 100)] public int PooVelocity { get; private set; } = 100;
    [SerializeField] private Transform[] pooVortexTransforms;

    [SerializeField] private bool isVortexShaking = true;

    private PooParticle[] _pooParticles;
    private Vector3 _pooVortexInitialPosition;
    private Vector3 _pooVortexNextPosition;

    private bool _isPlayingDebug = false;

    private void Start()
    {
        _pooParticles = GetComponentsInChildren<PooParticle>();
        _pooVortexInitialPosition = pooVortexTransforms[0].position;
        _pooVortexNextPosition = GetRandomPointInPooBox();
        _isPlayingDebug = false;
    }


    public void UpdateViolence(int violence)
    {
        PooViolence = violence;

        foreach (var pp in _pooParticles)
        {
            pp.UpdateViolenceScale(violence);
        }
    }
    
    public void UpdateVelocity(int velocity)
    {
        PooVelocity = velocity;
        foreach (var pp in _pooParticles)
        {
            pp.UpdateVelocityScale(velocity);
        }
    }

    public void UpdateColor(Color color)
    {
        PooColor = color;
        foreach (var pp in _pooParticles)
        {
            pp.UpdateColor(color);
        }
    }

    private Vector3 GetRandomPointInPooBox()
    {
        
        // get a random position within the bounds
        var scaledBounds = _pooViolenceBoxSize * (PooViolence / 100f);

        var randomPosition = new Vector3(
            Random.Range(-scaledBounds.x / 2, scaledBounds.x / 2),
            Random.Range(-scaledBounds.y / 2, scaledBounds.y / 2),
            Random.Range(-scaledBounds.z / 2, scaledBounds.z / 2)
        );

        return randomPosition + _pooVortexInitialPosition;
    }

    private void Update()
    {
        if (!isVortexShaking) return;
        
        foreach (var t in pooVortexTransforms)
        {
            if (Vector3.Distance(t.position, _pooVortexNextPosition) < 0.1f)
            {
                _pooVortexNextPosition = GetRandomPointInPooBox();
            }
            
            // world position change
            Transform parent;
            var worldCurrentPosition = t.position + (parent = t.parent).position;
            var worldNextPosition = _pooVortexNextPosition + parent.position;

            t.position = Vector3.Lerp(t.position, _pooVortexNextPosition, Time.deltaTime * PooViolence * 0.5f);
        }
    }

    private void OnValidate()
    {
        if(!_isPlayingDebug) return;
        
        UpdateViolence(PooViolence);
        UpdateVelocity(PooVelocity);
        UpdateColor(PooColor);
    }
}