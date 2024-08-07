using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private ParticleSystem _laser_particle;
    [SerializeField] private ParticleSystem _laser_beam;

    [SerializeField] private ParticleSystem _laser_particle_end;
    [SerializeField] private ParticleSystem _laser_beam_end;

    public void Start()
    {
        LoadComponent();
    }
    private void LoadComponent()
    {
        _laser.enabled = false;
        _laser_particle.Stop();
        _laser_beam.Stop();
        _laser_particle_end.Stop();
        _laser_beam_end.Stop();
    }
    public void EnableLaser()
    {
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {   
        _laser_beam.Play();
        yield return new WaitForSeconds(1f);
        _laser.enabled = true;
        _laser_particle.Play();
        _laser_particle_end.Play();
        _laser_beam_end.Play();
        yield return new WaitForSeconds(2f);
        LoadComponent();
        yield return null;
    }

}
