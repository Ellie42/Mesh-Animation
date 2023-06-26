using UnityEngine.Serialization;

namespace CodeWriter.MeshAnimation
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using TriInspector;

    [DrawWithTriInspector]
    [CreateAssetMenu(menuName = "Mesh Animation")]
    public class MeshAnimationAsset : ScriptableObject
    {
        [FormerlySerializedAs("skin")]
        [InfoBox("$" + nameof(GetValidationMessage), TriMessageType.Error, visibleIf: nameof(IsInvalid))]
        [SerializeField]
        public GameObject Skin = default;

        [FormerlySerializedAs("shader")]
        [Required]
        [SerializeField]
        public Shader Shader = default;

        [FormerlySerializedAs("materialPreset")] [SerializeField]
        public Material MaterialPreset = default;

        [FormerlySerializedAs("npotBakedTexture")] [SerializeField]
        public bool NpotBakedTexture = false;

        [FormerlySerializedAs("linearColorSpace")] [SerializeField]
        public bool LinearColorSpace = false;

        [FormerlySerializedAs("animationClips")]
        [PropertySpace]
        [Required]
        [SerializeField]
        [ListDrawerSettings(AlwaysExpanded = true)]
        public AnimationClip[] AnimationClips = new AnimationClip[0];

        [FormerlySerializedAs("extraMaterials")]
        [Required]
        [SerializeField]
        [TableList]
        public List<ExtraMaterial> ExtraMaterials = new List<ExtraMaterial>();

        public Texture2D BakedTexture => bakedTexture;
        public Material BakedMaterial => bakedMaterial;

        [ReadOnly]
        [SerializeField]
        internal Texture2D bakedTexture = default;

        [ReadOnly]
        [SerializeField]
        internal Material bakedMaterial = default;

        [TableList(HideAddButton = true, HideRemoveButton = true)]
        [ReadOnly]
        [SerializeField]
        internal List<ExtraMaterialData> extraMaterialData = new List<ExtraMaterialData>();

        [TableList(AlwaysExpanded = true, HideAddButton = true, HideRemoveButton = true)]
        [ReadOnly]
        [SerializeField]
        internal List<AnimationData> animationData = new List<AnimationData>();

        [Serializable]
        public class ExtraMaterial
        {
            [Required]
            public string name;

            public Material preset;
        }

        [Serializable]
        internal class ExtraMaterialData
        {
            public string name;
            public Material material;
        }

        [Serializable]
        internal class AnimationData
        {
            public string name;
            public float startFrame;
            public float lengthFrames;
            public float lengthSeconds;
            public bool looping;
        }

        public bool IsInvalid => GetValidationMessage() != null;

        public string GetValidationMessage()
        {
            if (Skin == null) return "Skin is required";

            if (AnimationClips.Length == 0) return "No animation clips";

            foreach (var clip in AnimationClips)
            {
                if (clip == null) return "Animation clip is null";
                if (clip.legacy) return "Legacy Animation clips not supported";
            }

            if (Shader == null) return "shader is null";
            if (Skin == null) return "skin is null";

            var skinnedMeshRenderer = Skin.GetComponentInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer == null) return "skin.GetComponentInChildren<SkinnedMeshRenderer>() == null";

            var skinAnimator = Skin.GetComponent<Animator>();
            if (skinAnimator == null) return "skin.GetComponent<Animator>() == null";
            if (skinAnimator.runtimeAnimatorController == null)
                return "skin.GetComponent<Animator>().runtimeAnimatorController == null";

            return null;
        }

        public AnimationClip GetAnimationClip(string name)
        {
            foreach (var clip in AnimationClips)
            {
                if (clip.name == name)
                {
                    return clip;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            LinearColorSpace = UnityEditor.PlayerSettings.colorSpace == ColorSpace.Linear;
        }

        [DisableIf(nameof(IsInvalid))]
        [PropertySpace(10)]
        [Button(ButtonSizes.Large, Name = "Bake")]
        public void Bake()
        {
            MeshAnimationBaker.Bake(this);
        }

        [PropertySpace(5)]
        [Button(ButtonSizes.Small, Name = "Clear baked data")]
        public void Clear()
        {
            MeshAnimationBaker.Clear(this);
        }
#endif
    }
}