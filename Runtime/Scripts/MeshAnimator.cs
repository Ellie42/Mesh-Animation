using System;
using UnityEngine.Serialization;

namespace CodeWriter.MeshAnimation
{
    using JetBrains.Annotations;
    using UnityEngine;
    using TriInspector;

    [DrawWithTriInspector]
    public class MeshAnimator : MonoBehaviour
    {
        [FormerlySerializedAs("meshRenderer")]
        [Required]
        [SerializeField]
        public MeshRenderer MeshRenderer = default;

        [FormerlySerializedAs("meshAnimation")]
        [Required]
        [SerializeField]
        public MeshAnimationAsset MeshAnimation = default;

        private void Awake()
        {
            MeshCache.GenerateSecondaryUv(this.MeshRenderer.GetComponent<MeshFilter>().sharedMesh);
        }

        [PublicAPI]
        public void Play(AnimationClip animationClip, float speed = 1f, float? normalizedTime = 0f)
        {
            if (!MeshAnimation.Contains(animationClip))
            {
                Debug.LogError($"Animation clip {animationClip.name} not found in {MeshAnimation.name} asset");
                return;
            }

            MeshAnimation.Play(MeshRenderer, animationClip, speed, normalizedTime);
        }

        public void SetProgress(float progress01)
        {
            MeshRenderer.material.SetVector("_AnimState", new Vector4(progress01, 0, 0, 0));
        }
    }
}