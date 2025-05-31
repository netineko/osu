// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Osu.UI.Cursor;
using osu.Game.Skinning;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Osu.Skinning.Legacy
{
    public partial class LegacyCursor : SkinnableCursor
    {
        public static readonly int REVOLUTION_DURATION = 10000;

        private const float pressed_scale = 1.3f;
        private const float released_scale = 1f;

        private readonly ISkin skin;
        private bool spin;

        public LegacyCursor(ISkin skin)
        {
            this.skin = skin;
            Size = new Vector2(50);

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            bool centre = skin.GetConfig<OsuSkinConfiguration, bool>(OsuSkinConfiguration.CursorCentre)?.Value ?? true;
            spin = skin.GetConfig<OsuSkinConfiguration, bool>(OsuSkinConfiguration.CursorRotate)?.Value ?? true;

            InternalChildren = new[]
            {
                ExpandTarget = new NonPlayfieldSprite
                {
                    Texture = skin.GetTexture("cursor"),
                    Anchor = Anchor.Centre,
                    Origin = centre ? Anchor.Centre : Anchor.TopLeft,
                    Colour = new Color4(255, 52, 21, 255),
                    //Colour = new Color4(116, 218, 64, 255),
                },
                ExpandTarget2 = new NonPlayfieldSprite
                {
                    Blending = BlendingParameters.Inherit,
                    Texture = skin.GetTexture("cursormiddle"),
                    Anchor = Anchor.Centre,
                    Origin = centre ? Anchor.Centre : Anchor.TopLeft,
                    Colour = new Color4(255, 141, 123, 180),
                    //Colour = new Color4(200, 255, 172, 180), // N-TODO: make this multiplicative or whatever it is
                },
            };
        }

        protected override void LoadComplete()
        {
            if (spin)
                ExpandTarget.Spin(REVOLUTION_DURATION, RotationDirection.Clockwise);
        }

        public override void Expand()
        {
            ExpandTarget?.ScaleTo(released_scale)
                        .ScaleTo(pressed_scale, 100, Easing.Out);
            ExpandTarget2?.ScaleTo(released_scale)
                        .ScaleTo(pressed_scale, 100, Easing.Out);
        }

        public override void Contract()
        {
            ExpandTarget?.ScaleTo(released_scale, 100, Easing.Out);
            ExpandTarget2?.ScaleTo(released_scale, 100, Easing.Out);
        }
    }
}
