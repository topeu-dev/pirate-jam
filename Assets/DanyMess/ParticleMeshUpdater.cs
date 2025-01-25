using UnityEngine;

public class ParticleMeshUpdater : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public SkinnedMeshRenderer targetMeshRenderer;

    void Start()
    {
        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
        shape.mesh = targetMeshRenderer.GetComponent<MeshFilter>().mesh;
    }
}
