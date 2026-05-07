// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Localisation
{
    public static class ArcadeStrings
    {
        private const string prefix = @"osu.Game.Resources.Localisation.ArcadeStrings";

        /// <summary>
        /// "Service mode"
        /// </summary>
        public static LocalisableString ServiceMode => new TranslatableString(getKey(@"service_mode"), @"Service mode");

        /// <summary>
        /// "Global leaderboards"
        /// </summary>
        public static LocalisableString ShowGlobalLeaderboards => new TranslatableString(getKey(@"show_global_leaderboards"), @"Global leaderboards");

        /// <summary>
        /// "Country leaderboards"
        /// </summary>
        public static LocalisableString ShowCountryLeaderboards => new TranslatableString(getKey(@"show_country_leaderboards"), @"Country leaderboards");

        /// <summary>
        /// "osu!taiko"
        /// </summary>
        public static LocalisableString AllowTaiko => new TranslatableString(getKey(@"allow_taiko"), @"osu!taiko");

        /// <summary>
        /// "osu!catch"
        /// </summary>
        public static LocalisableString AllowCTB => new TranslatableString(getKey(@"allow_ctb"), @"osu!catch");

        /// <summary>
        /// "osu!mania"
        /// </summary>
        public static LocalisableString AllowMania => new TranslatableString(getKey(@"allow_mania"), @"osu!mania");

        /// <summary>
        /// "osu! supporter features"
        /// </summary>
        public static LocalisableString ShowSupporterFeatures => new TranslatableString(getKey(@"show_supporter_features"), @"osu! supporter features");

        /// <summary>
        /// "Local chat"
        /// </summary>
        public static LocalisableString AllowChat => new TranslatableString(getKey(@"allow_chat"), @"Local chat");

        /// <summary>
        /// "Local multiplayer"
        /// </summary>
        public static LocalisableString AllowMulti => new TranslatableString(getKey(@"allow_multi"), @"Local multiplayer");

        /// <summary>
        /// "Solo gameplay"
        /// </summary>
        public static LocalisableString AllowSolo => new TranslatableString(getKey(@"allow_solo"), @"Solo gameplay");

        /// <summary>
        /// "Online services"
        /// </summary>
        public static LocalisableString AllowOnlineServices => new TranslatableString(getKey(@"allow_online_services"), @"Online services");

        /// <summary>
        /// "Allow typing"
        /// </summary>
        public static LocalisableString AllowTyping => new TranslatableString(getKey(@"allow_typing"), @"Allow typing");

        /// <summary>
        /// "Use digital keyboard"
        /// </summary>
        public static LocalisableString ShowDigitalKeyboard => new TranslatableString(getKey(@"show_digital_keyboard"), @"Use digital keyboard");

        /// <summary>
        /// "Enforce licensed tracks"
        /// </summary>
        public static LocalisableString ForceLicensedTracks => new TranslatableString(getKey(@"force_licensed_tracks"), @"Enforce licensed tracks");

        /// <summary>
        /// "Enforce SFW tracks"
        /// </summary>
        public static LocalisableString ForceSafeTracks => new TranslatableString(getKey(@"force_safe_tracks"), @"Enforce SFW tracks");

        /// <summary>
        /// "Free play"
        /// </summary>
        public static LocalisableString FreePlay => new TranslatableString(getKey(@"free_play"), @"Free play");

        /// <summary>
        /// "Timed play"
        /// </summary>
        public static LocalisableString TimedPlay => new TranslatableString(getKey(@"timed_play"), @"Timed play");

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
