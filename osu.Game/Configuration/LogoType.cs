// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;
using osu.Game.Localisation;

namespace osu.Game.Configuration
{
    public enum LogoType
    {
        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Classic0))]
        Classic0,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Classic1))]
        Classic1,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Classic2))]
        Classic2,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Stable))]
        Stable,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Lazer1))]
        Lazer1,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Lazer2))]
        Lazer2,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Lazer3))]
        Lazer3,

        [LocalisableDescription(typeof(UserInterfaceStrings), nameof(UserInterfaceStrings.Lazer))]
        Lazer,
    }
}
