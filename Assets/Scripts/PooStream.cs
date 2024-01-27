using UnityEngine;

/// <summary>
/// Controls the visual intensity of the characters poo stream.
/// </summary>
public class PooStream : MonoBehaviour
{
    private readonly Vector3 _pooViolenceBoxSize = new Vector3(0.25f, 0.25f, 0.25f);
    [field: SerializeField] public Color PooColor { get; private set; }
    [field: SerializeField, Range(0, 100)] public int PooViolence { get; private set; } = 0;
    [SerializeField] private Transform[] pooVortexTransforms;

    [SerializeField] private bool isVortexShaking = true;
    private ParticleSystem[] _particleSystems;

    // Represents how far the poo vortex is from the parent.
    private Vector3 _pooVortexLocalPositionOffset;
    private Vector3 _pooVortexNextLocalPosition;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void Start()
    {
        _pooVortexLocalPositionOffset = pooVortexTransforms[0].localPosition;
        _pooVortexNextLocalPosition = GetRandomPointInPooBox();
    }


    public void UpdateViolence(int violence)
    {
        PooViolence = violence;
    }
    

    public void UpdateColor(Color color)
    {
        return;
    }

    /// <summary>
    /// Get a random local position within the poo box.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPointInPooBox()
    {
        
        // get a random position within the bounds
        var scaledBounds = _pooViolenceBoxSize * (PooViolence / 100f);

        var randomPosition = new Vector3(
            Random.Range(-scaledBounds.x / 2, scaledBounds.x / 2),
            Random.Range(-scaledBounds.y / 2, scaledBounds.y / 2),
            Random.Range(-scaledBounds.z / 2, scaledBounds.z / 2)
        );

        return randomPosition + _pooVortexLocalPositionOffset;
    }

    private void Update()
    {
        if (!isVortexShaking) return;
        
        foreach (var t in pooVortexTransforms)
        {
            if (Vector3.Distance(t.localPosition, _pooVortexNextLocalPosition) < 0.1f)
            {
                _pooVortexNextLocalPosition = GetRandomPointInPooBox();
            }
            // world position change
            t.localPosition = Vector3.Lerp(
                t.localPosition,
                _pooVortexNextLocalPosition,
                Time.deltaTime * PooViolence * 0.5f);
        }
    }

    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        if(_particleSystems == null) return;
        UpdateViolence(PooViolence);
        UpdateColor(PooColor);
    }
}