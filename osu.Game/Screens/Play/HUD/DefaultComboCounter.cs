// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;

namespace osu.Game.Screens.Play.HUD
{
    public partial class DefaultComboCounter : ComboCounter, IHasSkinDetails
    {
        LocalisableString IHasSkinDetails.VisualName => SkinComponentNameStrings.DefaultComboCounter;
        LocalisableString IHasSkinDetails.ShortName => SkinComponentShortnameStrings.ComboCounter;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ScoreProcessor scoreProcessor)
        {
            Colour = colours.BlueLighter;
            Current.BindTo(scoreProcessor.Combo);
        }

        protected override OsuSpriteText CreateSpriteText()
            => base.CreateSpriteText().With(s => s.Font = s.Font.With(size: 20f));

        protected override LocalisableString FormatCount(int count)
        {
            return $@"{count}x";
        }
    }
}
