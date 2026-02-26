// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Rulesets.Edit.Checks.Components;

namespace osu.Game.Screens.Edit.Verify
{
    internal partial class VisibilitySection : EditorRoundedScreenSettingsSection
    {
        private readonly IssueType[] configurableIssueTypes =
        {
            IssueType.Warning,
            IssueType.Error,
            IssueType.Negligible
        };

        private BindableList<IssueType> hiddenIssueTypes;

        protected override string HeaderText => "Visibility";

        [BackgroundDependencyLoader]
        private void load(VerifyScreen verify)
        {
            hiddenIssueTypes = verify.HiddenIssueTypes.GetBoundCopy();

            foreach (IssueType issueType in configurableIssueTypes)
            {
                var checkbox = new FormCheckBox
                {
                    Caption = issueType.ToString(),
                    Current = { Default = !hiddenIssueTypes.Contains(issueType) }
                };

                checkbox.Current.SetDefault();
                checkbox.Current.BindValueChanged(state =>
                {
                    if (!state.NewValue)
                        hiddenIssueTypes.Add(issueType);
                    else
                        hiddenIssueTypes.Remove(issueType);
                });

                Flow.Add(checkbox);
            }
        }
    }
}
