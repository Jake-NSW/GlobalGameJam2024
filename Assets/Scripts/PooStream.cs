using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Controls the visual intensity of the characters poo stream.
/// </summary>
public class PooStream : MonoBehaviour
{
    [field: SerializeField] public bool IsOn { get; private set; } = false;
    [field: SerializeField] public Color PooColor { get; private set; }
    [field: SerializeField, Range(0, 100)] public int PooViolence { get; private set; } = 0;
    
    [SerializeField, Tooltip("How scaled the particles can be compared to their starting scale")]
    private Vector2 pooSizeMinMax = new Vector2(0.5f, 1.5f);
    [SerializeField, Tooltip("How long the vortex particles can be compared to their starting scale")] 
    private Vector2 pooVelocityMinMax = new Vector2(1f, 3f);
    
    [Header("Shaking Controls")]
    [SerializeField] private Transform[] pooVortexTransforms;
    [SerializeField] private bool isVortexShaking = true;
    [SerializeField] private Vector3 pooViolenceBoxSize = new Vector3(0.25f, 0.25f, 0.25f);
    [SerializeField] private float pooShakingIntensity = 0.5f;
    
    private ParticleSystem[] _particleSystems;

    // Represents how far the poo vortex is from the parent.
    private Vector3 _pooVortexLocalPositionOffset;
    private Vector3 _pooVortexNextLocalPosition;
    
    private Vector3[] _initialScales;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void Start()
    {
        _pooVortexLocalPositionOffset = pooVortexTransforms[0].localPosition;
        _pooVortexNextLocalPosition = GetRandomPointInPooBox();
        UpdateColor(PooColor);
        
        _initialScales = new Vector3[_particleSystems.Length];
        for (var i = 0; i < _particleSystems.Length; i++)
        {
            _initialScales[i] = _particleSystems[i].transform.localScale;
        }
    }
    
    public void TurnOff()
    {
        IsOn = false;
        foreach (var ps in _particleSystems)
        {
            ps.gameObject.SetActive(false);
            
        }
    }
    
    public void TurnOn()
    {
        IsOn = true;
        foreach (var ps in _particleSystems)
        {
            ps.gameObject.SetActive(true);
        }
    }


    public void UpdateViolence(int violence)
    {
        PooViolence = violence;

        if (violence <= 0)
        {
            foreach (var ps in _particleSystems)
                ps.Stop();
            
            isVortexShaking = false;
            return;
        }


        foreach (var ps in _particleSystems)
        {
            // only play it if it's not playing
            if (!ps.isPlaying)
                ps.Play();
        }
        isVortexShaking = true;
        UpdateParticleScale();
    }

    private void UpdateParticleScale()
    {
        var violenceScaleFactor = PooViolence / 100f;
        var newSize = Vector3.Lerp(
            _initialScales[0],
            _initialScales[1],
            violenceScaleFactor);

        // increase all the particles sizes.
        for (var i = 0; i < _particleSystems.Length; i++)
        {
            _particleSystems[i].transform.localScale = _initialScales[i] * violenceScaleFactor;
        }
        
        
        var newLength = Mathf.Lerp(pooSizeMinMax.x, pooSizeMinMax.y, violenceScaleFactor);
        var newLengthVector = new Vector3(1, newLength, 1);
        
        foreach (var t in pooVortexTransforms)
        {
            var newScale = Vector3.Scale(t.localScale, newLengthVector);
            t.localScale = newScale;
        }
    }
    

    public void UpdateColor(Color color)
    {
        //update the main color of the particle system
        foreach (var ps in _particleSystems)
        {
            var main = ps.main;
            main.startColor = color;
        }
    }

    /// <summary>
    /// Get a random local position within the poo box.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPointInPooBox()
    {
        
        // get a random position within the bounds
        var scaledBounds = pooViolenceBoxSize * (PooViolence / 100f);

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
                Time.deltaTime * PooViolence * pooShakingIntensity);
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

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(PooStream))]
public class PooStreamEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var pooStream = (PooStream) target;

        if (GUILayout.Button("Update Color"))
        {
            pooStream.UpdateColor(pooStream.PooColor);
        }
    }
}
#endif