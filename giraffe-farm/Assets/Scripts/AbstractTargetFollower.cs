using System;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
    public abstract class AbstractTargetFollower : MonoBehaviour
    {
        [SerializeField] protected Transform m_Target;            // The target object to follow
        [SerializeField] private bool m_AutoTargetPlayer = true;  // Whether the rig should automatically target the player.

        protected Rigidbody targetRigidbody;

        protected virtual void Start()
        {
            // if auto targeting is used, find the object tagged "Player"
            // any class inheriting from this should call base.Start() to perform this action!
            if (m_AutoTargetPlayer)
            {
                FindAndTargetPlayer();
            }
            if (m_Target == null) return;
            
        }

        public void ManualUpdate(bool initialRun)
        {
            if (m_AutoTargetPlayer && (m_Target == null || !m_Target.gameObject.activeSelf))
            {
                FindAndTargetPlayer();
            }
            
			FollowTarget(Time.deltaTime, initialRun);
        }

        protected abstract void FollowTarget(float deltaTime, bool initialRun);


        public void FindAndTargetPlayer()
        {
            // auto target an object tagged player, if no target has been assigned
            var targetObj = GameObject.FindGameObjectWithTag("Player");
            if (targetObj)
            {
                SetTarget(targetObj.transform);
				targetRigidbody = m_Target.GetComponent<Rigidbody>();
            }
        }


        public virtual void SetTarget(Transform newTransform)
        {
            m_Target = newTransform;
        }


        public Transform Target
        {
            get { return m_Target; }
        }
    }
}
