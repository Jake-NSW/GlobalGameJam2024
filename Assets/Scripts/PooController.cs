using UnityEngine;

/// <summary>
/// Controls the visual intensity of the characters poo stream.
/// </summary>
public class PooController : MonoBehaviour
{
    private readonly Vector3 _pooViolenceBoxSize = new Vector3(0.25f, 0.25f, 0.25f);
    [field: SerializeField] public Color PooColor { get; private set; }
    [field: SerializeField, Range(0, 100)] public int PooViolence { get; private set; } = 0;
    [SerializeField] private Transform[] pooVortexTransforms;

    private ParticleSystem[] _particleSystems;
    private Vector3[] _particleSystemInitialScale;
    private Vector3 _pooVortexNextPosition;
    private Vector3 _pooVortexInitialPosition;

    private void Start()
    {
        _pooVortexNextPosition = transform.position;
        _particleSystems = GetComponentsInChildren<ParticleSystem>();
        _particleSystemInitialScale = new Vector3[_particleSystems.Length];
        for (var i = 0; i < _particleSystems.Length; i++)
        {
            _particleSystemInitialScale[i] = _particleSystems[i].transform.localScale;
        }

        _pooVortexInitialPosition = pooVortexTransforms[0].position;
    }


    public void UpdateViolence(int violence)
    {
        PooViolence = violence;

        // update the particle systems scale
        for (var i = 0; i < _particleSystems.Length; i++)
        {
            _particleSystems[i].transform.localScale = _particleSystemInitialScale[i] * (PooViolence / 100f);
        }
    }

    public void UpdateColor(Color color)
    {
        PooColor = color;
        foreach (var ps in _particleSystems)
        {
            var main = ps.main;
            main.startColor = color;
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
        foreach (var t in pooVortexTransforms)
        {
            if (Vector3.Distance(t.position, _pooVortexNextPosition) < 0.1f)
            {
                _pooVortexNextPosition = GetRandomPointInPooBox();
            }

            t.position = Vector3.Lerp(t.position, _pooVortexNextPosition, Time.deltaTime * PooViolence * 0.5f);
        }
    }
}