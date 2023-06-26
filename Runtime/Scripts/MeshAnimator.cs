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

        private MaterialPropertyBlock _propertyBlock;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();

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

            MeshRenderer.GetPropertyBlock(_propertyBlock);
            MeshAnimation.Play(_propertyBlock, animationClip, speed, normalizedTime);
            MeshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}