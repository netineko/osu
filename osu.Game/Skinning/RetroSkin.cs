// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game.Extensions;
using osu.Game.IO;
using osu.Game.Screens.Play.HUD;
using osu.Game.Screens.Play.HUD.HitErrorMeters;
using osuTK.Graphics;

namespace osu.Game.Skinning
{
    /// <summary>
    /// A skin that looks like osu!stable as it was around 2008.
    /// </summary>
    /// <remarks>
    /// "Around 2008" was chosen as the cutoff for this skin because that's when the look of core gameplay settled into its final design (until <see cref="DefaultLegacySkin"/>). Skin elements from later versions of osu! were preferred as long as they only fixed bugs or applied minor tweaks to 2008 elements.
    /// </remarks>
    public class RetroSkin : LegacySkin
    {
        public static SkinInfo CreateInfo() => new SkinInfo
        {
            ID = Skinning.SkinInfo.RETRO_SKIN,
            Name = "osu! \"retro\" (2008)",
            Creator = "team osu!",
            Protected = true,
            InstantiationInfo = typeof(RetroSkin).GetInvariantInstantiationInfo(),
        };

        public RetroSkin(IStorageResourceProvider resources)
            : this(CreateInfo(), resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public RetroSkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(
                skin,
                resources,
                new NamespacedResourceStore<byte[]>(resources.Resources, "Skins/Retro")
            )
        {
            Configuration.ConfigDictionary[@"SliderBallFlip"] = "0";
            Configuration.ConfigDictionary[@"SliderBallFrames"] = "10";
            Configuration.ConfigDictionary[@"AllowSliderBallTint"] = "0";
            Configuration.ConfigDictionary[@"CursorTrailRotate"] = "0";
            Configuration.ConfigDictionary[@"Version"] = "1";

            Configuration.CustomComboColours =
            [
                new Color4(255, 150, 0, 255),
                new Color4(5, 240, 5, 255),
                new Color4(5, 5, 240, 255),
                new Color4(240, 5, 5, 255)
            ];

            Configuration.ConfigDictionary[@"HitCircleOverlap"] = "3";
            Configuration.ConfigDictionary[@"ScoreOverlap"] = "3";
            Configuration.ConfigDictionary[@"ComboOverlap"] = "3";
        }

        public override Drawable? GetDrawableComponent(ISkinComponentLookup lookup)
        {
            switch (lookup)
            {
                case GlobalSkinnableContainerLookup containerLookup:
                    switch (containerLookup.Lookup)
                    {
                        case GlobalSkinnableContainers.MainHUDComponents:
                            return new DefaultSkinComponentsContainer(container =>
                            {
                                var score = container.OfType<LegacyScoreCounter>().FirstOrDefault();
                                var accuracy = container.OfType<GameplayAccuracyCounter>().FirstOrDefault();

                                if (score != null && accuracy != null)
                                {
                                    accuracy.Y = container.ToLocalSpace(score.ScreenSpaceDrawQuad.BottomRight).Y;
                                }

                                var songProgress = container.OfType<LegacySongProgress>().FirstOrDefault();

                                if (songProgress != null && accuracy != null)
                                {
                                    songProgress.Anchor = Anchor.TopRight;
                                    songProgress.Origin = Anchor.CentreRight;
                                    songProgress.X = -accuracy.ScreenSpaceDeltaToParentSpace(accuracy.ScreenSpaceDrawQuad.Size).X - 18;
                                    songProgress.Y = container.ToLocalSpace(accuracy.ScreenSpaceDrawQuad.TopLeft).Y + (accuracy.ScreenSpaceDeltaToParentSpace(accuracy.ScreenSpaceDrawQuad.Size).Y / 2);
                                    songProgress.DotSize.Value = 0;
                                }

                                var hitError = container.OfType<HitErrorMeter>().FirstOrDefault();

                                if (hitError != null)
                                {
                                    hitError.Anchor = Anchor.BottomCentre;
                                    hitError.Origin = Anchor.CentreLeft;
                                    hitError.Rotation = -90;
                                }

                                foreach (var d in container.OfType<ISerialisableDrawable>())
                                    d.UsesFixedAnchor = true;
                            })
                            {
                                Children = new Drawable[]
                                {
                                    new LegacyScoreCounter(),
                                    new LegacyAccuracyCounter(),
                                    new LegacySongProgress(),
                                    new LegacyHealthDisplay(),
                                    new BarHitErrorMeter(),
                                }
                            };
                    }

                    return null;
            }
            return base.GetDrawableComponent(lookup);
        }

        public override Texture? GetTexture(string componentName, WrapMode wrapModeS, WrapMode wrapModeT)
        {
            // Retro taiko hit explosions use osu textures
            if (componentName.StartsWith("taiko-hit", StringComparison.Ordinal))
                componentName = componentName.Substring(6);

            // Retro taiko slider has no fail variant, but it needs to exist to avoid displaying nothing
            if (componentName == "taiko-slider-fail")
                componentName = "taiko-slider";

            return base.GetTexture(componentName, wrapModeS, wrapModeT);
        }
    }
}
