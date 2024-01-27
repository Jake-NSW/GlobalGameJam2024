using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooStream : MonoBehaviour
{
    [field:SerializeField] public Color pooColor { get; private set; }
    [field:SerializeField] public float PooViolence { get; private set; } = 0f;
    [SerializeField] private Vector3 pooViolenceBoxSize = new Vector3(0f, 0f, 0f);
    [SerializeField] private Transform[] pooStreamTransforms;
    private ParticleSystem[] particleSystems;
    private Vector3 _nextPosition;

    private void Start()
    {
        _nextPosition = transform.position;
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }
    

    public void UpdateViolence(float violence)
    {
        PooViolence = violence;
    }
    
    public void UpdateColor(Color color)
    {
        pooColor = color;
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.startColor = color;
        }
    }

    private Vector3 GetRandomPointInPooBox()
{
    // get a random position within the bounds
    var randomPosition = new Vector3(
        Random.Range(-pooViolenceBoxSize.x / 2, pooViolenceBoxSize.x / 2),
        Random.Range(-pooViolenceBoxSize.y / 2, pooViolenceBoxSize.y / 2),
        Random.Range(-pooViolenceBoxSize.z / 2, pooViolenceBoxSize.z / 2)
    );

    return randomPosition;
}

private void Update()
{
    for(var i = 0; i < pooStreamTransforms.Length; i++)
    {
        var t = pooStreamTransforms[i];
        if(Vector3.Distance(t.position, _nextPosition) < 0.1f)
        {
            _nextPosition = GetRandomPointInPooBox();
        }

        t.position = Vector3.Lerp(t.position, _nextPosition, Time.deltaTime * PooViolence);
    }
}


}
