// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Skinning;

namespace osu.Game.Screens.Play.HUD
{
    public partial class DefaultScoreCounter : GameplayScoreCounter, ISerialisableDrawable, IHasSkinDetails
    {
        LocalisableString IHasSkinDetails.VisualName => SkinComponentNameStrings.DefaultScoreCounter;
        LocalisableString IHasSkinDetails.ShortName => SkinComponentShortnameStrings.ScoreCounter;
        ComponentGroup IHasSkinDetails.Group => ComponentGroup.Triangles;

        public DefaultScoreCounter()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
        }

        public bool UsesFixedAnchor { get; set; }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.BlueLighter;
        }
    }
}
