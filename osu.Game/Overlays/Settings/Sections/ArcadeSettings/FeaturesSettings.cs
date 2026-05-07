// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Localisation;

namespace osu.Game.Overlays.Settings.Sections.ArcadeSettings
{
    public partial class FeaturesSettings : SettingsSubsection
    {
        protected override LocalisableString Header => @"Features";

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ShowGlobalLeaderboards,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeShowGlobalLeaderboards)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ShowCountryLeaderboards,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeShowCountryLeaderboards)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowTaiko,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowTaiko)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowCTB,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowCTB)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowMania,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowMania)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ShowSupporterFeatures,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeShowSupporterFeatures)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowChat,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowChat)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowSolo,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowSolo)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowMulti,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowMulti)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowOnlineServices,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowOnlineServices)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.AllowTyping,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeAllowTyping)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ShowDigitalKeyboard,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeShowDigitalKeyboard)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ForceLicensedTracks,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeForceLicensedTracks)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.ForceSafeTracks,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeForceSafeTracks)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.FreePlay,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeFreePlay)
            }));
            Add(new SettingsItemV2(new FormCheckBox
            {
                Caption = ArcadeStrings.TimedPlay,
                Current = config.GetBindable<bool>(OsuSetting.ArcadeTimedPlay)
            }));
        }
    }
}
