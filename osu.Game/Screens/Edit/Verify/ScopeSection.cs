// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Screens.Edit.Verify
{
    internal partial class ScopeSection : EditorRoundedScreenSettingsSection
    {
        protected override string HeaderText => "Scope";

        [BackgroundDependencyLoader]
        private void load(VerifyScreen verify)
        {
            Flow.Add(new FormEnumDropdown<CheckScope>
            {
                Caption = "Scope",
                HintText = "Select which type of checks to display",
                Current = verify.VerifyChecksScope.GetBoundCopy()
            });
        }
    }
}
