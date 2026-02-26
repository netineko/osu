// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterfaceV2;

namespace osu.Game.Screens.Edit.Verify
{
    internal partial class InterpretationSection : EditorRoundedScreenSettingsSection
    {
        protected override string HeaderText => "Interpretation";

        [BackgroundDependencyLoader]
        private void load(VerifyScreen verify)
        {
            Flow.Add(new FormEnumDropdown<DifficultyRating>
            {
                Caption = "Interpretation",
                HintText = "Affects checks that depend on difficulty level",
                Current = verify.InterpretedDifficulty.GetBoundCopy()
            });
        }
    }
}
