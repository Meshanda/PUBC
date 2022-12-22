using Unity.FPS.Game;
using Unity.Netcode;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class ProjectileChargeParameters : NetworkBehaviour
    {
        public MinMaxFloat Damage;
        public MinMaxFloat Radius;
        public MinMaxFloat Speed;
        public MinMaxFloat GravityDownAcceleration;
        public MinMaxFloat AreaOfEffectDistance;

        ProjectileBase m_ProjectileBase;

        void OnEnable()
        {
            m_ProjectileBase = GetComponent<ProjectileBase>();
            DebugUtility.HandleErrorIfNullGetComponent<ProjectileBase, ProjectileChargeParameters>(m_ProjectileBase,
                this, gameObject);

            m_ProjectileBase.OnShoot += OnShootPCP;
        }

        void OnShootPCP()
        {
            if (!IsOwner)
                return;
            
            // Apply the parameters based on projectile charge
            ProjectileStandard proj = GetComponent<ProjectileStandard>();
            if (proj)
            {
                proj.Damage = Damage.GetValueFromRatio(m_ProjectileBase.InitialCharge);
                proj.Radius = Radius.GetValueFromRatio(m_ProjectileBase.InitialCharge);
                proj.Speed = Speed.GetValueFromRatio(m_ProjectileBase.InitialCharge);
                proj.GravityDownAcceleration =
                    GravityDownAcceleration.GetValueFromRatio(m_ProjectileBase.InitialCharge);
            }
        }
    }
}