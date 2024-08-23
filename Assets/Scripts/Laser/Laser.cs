using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private LineRenderer _warningLine;
    [SerializeField] private ParticleSystem _startVFX1;
    [SerializeField] private ParticleSystem _startVFX2;
    [SerializeField] private ParticleSystem _lineSmall;
    [SerializeField] private ParticleSystem _glow;
    [SerializeField] private ParticleSystem _hitVFX;

    private List<ParticleSystem> _allParticles;
    private List<LineRenderer> _allLineRenderers;

    private void Awake()
    {
        // Initialize the lists for easy access
        _allParticles = new List<ParticleSystem>
        {
            _startVFX1,
            _startVFX2,
            _lineSmall,
            _glow,
            _hitVFX
        };

        _allLineRenderers = new List<LineRenderer>
        {
            _laser,
            _warningLine
        };
    }
    private void Start()
    {
        ResetSystems();
    }
    public void ActivateLaser(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the relevant particle systems
        RotateParticleSystem(_startVFX1, angle);
        RotateParticleSystem(_startVFX2, angle);
        RotateParticleSystem(_lineSmall, angle);
        RotateParticleSystem(_glow, angle);

        _hitVFX.transform.position = end;

    }

    private void RotateParticleSystem(ParticleSystem ps, float angle)
    {
        ps.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void PlayParticles()
    {
        foreach (var ps in _allParticles)
        {
            ps.Play();
        }
    }

    public void StopParticles()
    {
        foreach (var ps in _allParticles)
        {
            ps.Stop();
        }
    }

    private void EnableLineRenderers()
    {
        foreach (var lr in _allLineRenderers)
        {
            lr.enabled = true;
        }
    }

    public void DisableLineRenderers()
    {
        foreach (var lr in _allLineRenderers)
        {
            lr.enabled = false;
        }
    }

    public void ResetSystems()
    {
        // Stop all particle systems and disable emission
        foreach (var ps in _allParticles)
        {
            ps.Stop();
            var emission = ps.emission;
            emission.enabled = false;
        }

        // Disable all line renderers
        DisableLineRenderers();
    }
}
