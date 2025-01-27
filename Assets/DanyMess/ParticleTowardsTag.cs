using UnityEngine;

public class ParticleTowardsTag : MonoBehaviour
{
    public string targetTag = "Demon"; // ��� ����
    public ParticleSystem particleSystem; // ������ �� Particle System
    public float searchRadius = 50f; // ������ ������ ����
    public float particleSpeed = 5f; // �������� ������

    private ParticleSystem.Particle[] particles;

    void Start()
    {
        if (!particleSystem)
            particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // ������� ��������� ����
        GameObject target = FindClosestTarget();

        if (target)
        {
            Vector3 targetPosition = target.transform.position;

            // ��������� ����������� ������ � ����
            UpdateParticleDirections(targetPosition);
        }
    }

    void UpdateParticleDirections(Vector3 targetPosition)
    {
        // ������������� ������� ������
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }

        // �������� ������� �������
        int particleCount = particleSystem.GetParticles(particles);

        // �������� ����������� ������ �������
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 direction = (targetPosition - particles[i].position).normalized;
            particles[i].velocity = direction * particleSpeed; // ������������� ����� ��������
        }

        // ��������� ��������� ������� � ������� ������
        particleSystem.SetParticles(particles, particleCount);
    }

    GameObject FindClosestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance && distance <= searchRadius)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}
