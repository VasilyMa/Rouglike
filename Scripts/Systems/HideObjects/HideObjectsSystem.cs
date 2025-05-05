using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Client {
    sealed class HideObjectsSystem : IEcsRunSystem 
    {
        readonly EcsPoolInject<TransformComponent> _transfromPool = default;
        readonly EcsPoolInject<ShowThroughComponent> _ShowThroughComponent = default;
        readonly EcsFilterInject<Inc<ShowThroughComponent>> _filter;
        public void Run (IEcsSystems systems)
        {
            foreach(var entity in _filter.Value)
            {
                ref var _showThrough = ref _ShowThroughComponent.Value.Get(entity);
                Vector3 playerPosition = _transfromPool.Value.Get(entity).Transform.position;
                Vector3 cameraPosition = Camera.main.transform.position;
                Vector3 direction = (playerPosition - cameraPosition).normalized;
                Ray ray = new Ray(cameraPosition, direction);
                var hits = Physics.RaycastAll(ray, Vector3.Distance(playerPosition, cameraPosition)).ToList();
                foreach (RaycastHit hit in hits)
                {
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null && (renderer.gameObject.layer == LayerMask.NameToLayer("Obstacle") || renderer.gameObject.layer == LayerMask.NameToLayer("Wall")))
                    {
                        Transparency(ref renderer, 0.5f);
                    }
                }
                List<RaycastHit> filteredList = new();
                foreach (var hideObject in _showThrough.HideObjects)
                {
                    bool flag = false;
                    foreach (var hit in hits)
                    {
                        if (hit.collider == hideObject.collider)
                        {
                            flag = true;
                        }
                    }
                    if (!flag)
                    {
                        filteredList.Add(hideObject);
                    }
                }
                foreach (var showObject in filteredList)
                {
                    Renderer renderer = showObject.collider.GetComponent<Renderer>();
                    if (renderer != null && (renderer.gameObject.layer == LayerMask.NameToLayer("Obstacle") || renderer.gameObject.layer == LayerMask.NameToLayer("Wall")))
                    {

                        Transparency(ref renderer, 1f);
                    }
                }
                _showThrough.HideObjects = hits;
            }
        }
        public void Transparency(ref Renderer renderer, float value)
        {
            Material material = renderer.material;
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.renderQueue = 3000;
            material.color = new Color(material.color.r, material.color.g, material.color.b, value);
            renderer.material = material;
        }
    }
}