using UnityEngine;

public class ParticleTowardsTag : MonoBehaviour
{
    public string targetTag = "Demon"; // Тег цели
    public ParticleSystem particleSystem; // Ссылка на Particle System
    public float searchRadius = 50f; // Радиус поиска цели
    public float particleSpeed = 5f; // Скорость частиц

    private ParticleSystem.Particle[] particles;

    void Start()
    {
        if (!particleSystem)
            particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Находим ближайшую цель
        GameObject target = FindClosestTarget();

        if (target)
        {
            Vector3 targetPosition = target.transform.position;

            // Обновляем направление частиц к цели
            UpdateParticleDirections(targetPosition);
        }
    }

    void UpdateParticleDirections(Vector3 targetPosition)
    {
        // Инициализация массива частиц
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
        {
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }

        // Получаем текущие частицы
        int particleCount = particleSystem.GetParticles(particles);

        // Изменяем направление каждой частицы
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 direction = (targetPosition - particles[i].position).normalized;
            particles[i].velocity = direction * particleSpeed; // Устанавливаем новую скорость
        }

        // Применяем изменения обратно к системе частиц
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
