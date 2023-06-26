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
        public void Play(string animationName, float speed = 1f, float? normalizedTime = 0f)
        {
            MeshRenderer.GetPropertyBlock(_propertyBlock);
            MeshAnimation.Play(_propertyBlock, animationName, speed, normalizedTime);
            MeshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}