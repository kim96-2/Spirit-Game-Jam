using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class PoolObject : MonoBehaviour
    {
        private EntityId _prefabID = EntityId.None;

        public EntityId PrefabID { get { return _prefabID; } set { _prefabID = value; } }

        public Action OnGet;

        public void Get()
        {
            gameObject.SetActive(true);

            OnGet?.Invoke();
        }

        public void Release()
        {
            if(!_prefabID.Equals(EntityId.None))
            {
                //ObjectPoolManager.Instance.Release(this);

                this.transform.position = Vector3.zero;
                this.gameObject.SetActive(false);

                // OnGet = null;

                return;
            }

            Destroy(gameObject);
        }
    }
}
