// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics.Sprites;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Screens.Play.HUD;
using osuTK;

namespace osu.Game.Skinning
{
    public partial class LegacyAccuracyCounter : GameplayAccuracyCounter, ISerialisableDrawable, IHasSkinDetails
    {
        LocalisableString IHasSkinDetails.VisualName => SkinComponentNameStrings.LegacyAccuracyCounter;
        LocalisableString IHasSkinDetails.ShortName => SkinComponentShortnameStrings.AccuracyCounter;
        ComponentGroup IHasSkinDetails.Group => ComponentGroup.Legacy;

        public bool UsesFixedAnchor { get; set; }

        public LegacyAccuracyCounter()
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;

            Scale = new Vector2(0.6f * 0.96f);
            Margin = new MarginPadding { Vertical = 9, Horizontal = 17 };
        }

        protected sealed override OsuSpriteText CreateSpriteText() => new LegacySpriteText(LegacyFont.Score)
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            FixedWidth = true,
        };
    }
}
