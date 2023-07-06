namespace CodeWriter.MeshAnimation
{
    using UnityEngine;

    public static class MeshAnimationAssetExtensions
    {
        private static readonly int AnimationTimeProp = Shader.PropertyToID("_AnimTime");
        private static readonly int AnimationLoopProp = Shader.PropertyToID("_AnimLoop");

        public static void Play(this MeshAnimationAsset asset,
            MeshRenderer renderer,
            AnimationClip animationClip,
            float speed = 1f,
            float? normalizedTime = 0f)
        {
            MeshAnimationAsset.AnimationData data = null;

            foreach (var animationData in asset.animationData)
            {
                if (animationData.name != animationClip.name)
                {
                    continue;
                }

                data = animationData;
                break;
            }

            if (data == null)
            {
                return;
            }

            var start = data.startFrame;
            var length = data.lengthFrames;
            var s = speed / Mathf.Max(data.lengthSeconds, 0.01f);
            var material = renderer.material;
            var time = normalizedTime.HasValue
                ? Time.timeSinceLevelLoad - Mathf.Clamp01(normalizedTime.Value) / s
                : material.GetVector(AnimationTimeProp).z;

            material.SetFloat(AnimationLoopProp, data.looping ? 1 : 0);
            material.SetVector(AnimationTimeProp, new Vector4(start, length, s, time));
        }
    }
}