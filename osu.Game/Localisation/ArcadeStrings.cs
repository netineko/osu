// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Localisation
{
    public static class ArcadeStrings
    {
        private const string prefix = @"osu.Game.Resources.Localisation.ArcadeStrings";

        /// <summary>
        /// "Service Mode"
        /// </summary>
        public static LocalisableString ServiceMode => new TranslatableString(getKey(@"service_mode"), @"Service Mode");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
