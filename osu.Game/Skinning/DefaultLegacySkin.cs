// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using JetBrains.Annotations;
using osu.Framework.IO.Stores;
using osu.Game.Extensions;
using osu.Game.IO;
using osuTK.Graphics;

namespace osu.Game.Skinning
{
    public class DefaultLegacySkin : LegacySkin
    {
        public static SkinInfo CreateInfo() => new SkinInfo
        {
            ID = Skinning.SkinInfo.CLASSIC_SKIN, // this is temporary until database storage is decided upon.
            Name = "osu!stream \"classic\" (2012)",
            Creator = "team osu!stream",
            Protected = true,
            InstantiationInfo = typeof(DefaultLegacySkin).GetInvariantInstantiationInfo()
        };

        public DefaultLegacySkin(IStorageResourceProvider resources)
            : this(CreateInfo(), resources)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        public DefaultLegacySkin(SkinInfo skin, IStorageResourceProvider resources)
            : base(
                skin,
                resources,
                new NamespacedResourceStore<byte[]>(resources.Resources, "Skins/Legacy")
            )
        {
            Configuration.CustomColours["SliderBall"] = new Color4(255, 255, 255, 255);
            Configuration.CustomComboColours = new List<Color4>
            {
                // N-TODO: rip these colors precisely, i colorpicked from web and its off
                new Color4(254,227,0,255), //yellow 4
                new Color4(254,0,161,255), //pink 1
                new Color4(0,169,255,255), //blue 2
                new Color4(254,169,14,255) //orange 3
            };

            Configuration.ConfigDictionary[nameof(SkinConfiguration.LegacySetting.AllowSliderBallTint)] = @"false";

            Configuration.LegacyVersion = 2.7m;
        }
    }
}
