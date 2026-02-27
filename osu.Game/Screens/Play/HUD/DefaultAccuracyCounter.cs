// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Skinning;

namespace osu.Game.Screens.Play.HUD
{
    public partial class DefaultAccuracyCounter : GameplayAccuracyCounter, ISerialisableDrawable, IHasSkinDetails
    {
        LocalisableString IHasSkinDetails.VisualName => SkinComponentNameStrings.DefaultAccuracyCounter;
        LocalisableString IHasSkinDetails.ShortName => SkinComponentShortnameStrings.AccuracyCounter;

        public bool UsesFixedAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.BlueLighter;
        }
    }
}
