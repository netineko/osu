// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Development;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Overlays.Settings.Sections.ArcadeSettings;

namespace osu.Game.Overlays.Settings.Sections
{
    public partial class ArcadeSection : SettingsSection
    {
        public override LocalisableString Header => @"Arcade";

        public override Drawable CreateIcon() => new SpriteIcon
        {
            Icon = OsuIcon.Debug
        };

        public ArcadeSection()
        {
            Add(new ServiceSettings());
        }
    }
}
