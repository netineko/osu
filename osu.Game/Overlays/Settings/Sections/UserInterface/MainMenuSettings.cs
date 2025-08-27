// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Localisation;
using osu.Game.Online.API;
using osu.Game.Online.API.Requests.Responses;

namespace osu.Game.Overlays.Settings.Sections.UserInterface
{
    public partial class MainMenuSettings : SettingsSubsection
    {
        protected override LocalisableString Header => UserInterfaceStrings.MainMenuHeader;

        private IBindable<APIUser> user;
        private IBindable<SeasonalBackgroundMode> seasonalBackground;

        private SettingsEnumDropdown<BackgroundSource> backgroundSourceDropdown;
        private SettingsEnumDropdown<BackgroundType> backgroundTypeDropdown;

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config, IAPIProvider api)
        {
            user = api.LocalUser.GetBoundCopy();
            seasonalBackground = config.GetBindable<SeasonalBackgroundMode>(OsuSetting.SeasonalBackgroundMode);

            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = UserInterfaceStrings.ShowMenuTips,
                    Current = config.GetBindable<bool>(OsuSetting.MenuTips)
                },
                new SettingsCheckbox
                {
                    Keywords = new[] { "intro", "welcome" },
                    LabelText = UserInterfaceStrings.InterfaceVoices,
                    Current = config.GetBindable<bool>(OsuSetting.MenuVoice)
                },
                new SettingsCheckbox
                {
                    Keywords = new[] { "intro", "welcome" },
                    LabelText = UserInterfaceStrings.OsuMusicTheme,
                    Current = config.GetBindable<bool>(OsuSetting.MenuMusic)
                },
                new SettingsEnumDropdown<IntroSequence>
                {
                    LabelText = UserInterfaceStrings.IntroSequence,
                    Current = config.GetBindable<IntroSequence>(OsuSetting.IntroSequence),
                },
                new SettingsEnumDropdown<LogoType>
                {
                    LabelText = UserInterfaceStrings.LogoType,
                    Current = config.GetBindable<LogoType>(OsuSetting.LogoType),
                },
                backgroundSourceDropdown = new SettingsEnumDropdown<BackgroundSource>
                {
                    LabelText = UserInterfaceStrings.BackgroundSource,
                    Current = config.GetBindable<BackgroundSource>(OsuSetting.MenuBackgroundSource),
                },
                new SettingsEnumDropdown<SeasonalBackgroundMode>
                {
                    LabelText = UserInterfaceStrings.SeasonalBackgrounds,
                    Current = config.GetBindable<SeasonalBackgroundMode>(OsuSetting.SeasonalBackgroundMode),
                },
                backgroundTypeDropdown = new SettingsEnumDropdown<BackgroundType> // Consider merging this setting with BackgroundSource
                {
                    LabelText = UserInterfaceStrings.BackgroundType,
                    Current = config.GetBindable<BackgroundType>(OsuSetting.BackgroundType),
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            user.BindValueChanged(u =>
            {
                if (u.NewValue?.IsSupporter != true)
                    backgroundSourceDropdown.SetNoticeText(UserInterfaceStrings.NotSupporterNote, true);
                else
                    backgroundSourceDropdown.ClearNoticeText();
            }, true);
            seasonalBackground.BindValueChanged(u =>
            {
                if (u.NewValue != SeasonalBackgroundMode.Always)
                    backgroundTypeDropdown.Current.Disabled = false;
                else
                    backgroundTypeDropdown.Current.Disabled = true;
            }, true);
        }
    }
}
