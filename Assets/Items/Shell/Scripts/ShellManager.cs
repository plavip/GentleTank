using UnityEngine;

namespace Item.Shell
{
    public class ShellManager : MonoBehaviour
    {
        public ObjectPool shellExplosionPool;               // ��ը���Գ�

        public float maxDamage = 100f;                      // ����˺�
        public float explosionForce = 100f;                 // ��ը���ĵ�����
        public float maxLifeTime = 2f;                      // ը���������ʱ��
        public float explosionRadius = 5f;                  // ��ը�뾶

        // ��ÿ����Ҫ�õ�����ʱ����������
        private Collider[] colliders;                       // ��ײ������
        private Rigidbody targetRigidbody;                  // Ŀ�����
        private HealthManager targetHealth;                 // Ŀ��Ѫ��

        // �������κ�����
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Flag"))
                return;

            // �ӱ�ը���л�ȡ���󣬲�����λ�ã���ʾ֮
            shellExplosionPool.GetNextObject(transform: transform);

            // ��ȡ��ը��Χ��������ײ��
            colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            for (int i = 0; i < colliders.Length; i++)
            {
                AddForce(colliders[i]);
                TakeDamage(colliders[i]);
            }

            gameObject.SetActive(false);
        }

        // ��һ����ը��,��AI��Ч��NavMeshAgent������ʱ����������䲻��ʵ�֣�
        private void AddForce(Collider collider)
        {
            targetRigidbody = collider.GetComponent<Rigidbody>();
            if (!targetRigidbody)
                return;
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        // ��ȡĿ���Ѫ���������Ѫ����������Ѫ��
        private void TakeDamage(Collider collider)
        {
            targetHealth = collider.GetComponent<HealthManager>();
            if (!targetHealth)
                return;
            targetHealth.SetHealthAmount(-1 * CalculateDamage(targetRigidbody.position));
        }

        // ���ݾ�������˺�
        private float CalculateDamage(Vector3 targetPosition)
        {
            // ���㱬ը���ľ�����Լ��ľ���
            Vector3 explosionToTarget = targetPosition - transform.position;
            float explosionDistance = explosionToTarget.magnitude;

            // ת���ɱ���
            float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

            // ���ݱ��������˺�
            return Mathf.Max(0f, relativeDistance * maxDamage);
        }

    }
}