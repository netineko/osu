// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Localisation;

namespace osu.Game.Overlays.Settings.Sections.ArcadeSettings
{
    public partial class ServiceSettings : SettingsSubsection
    {
        protected override LocalisableString Header => @"Service";

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ServiceMode,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeServiceMode)
            }));
        }
    }
}
