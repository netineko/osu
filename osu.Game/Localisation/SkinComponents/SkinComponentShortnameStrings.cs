// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Localisation.SkinComponents
{
    public static class SkinComponentShortnameStrings
    {
        private const string prefix = @"osu.Game.Resources.Localisation.SkinComponents.SkinComponentShortnameStrings";

        #region Gameplay

        /// <summary>
        /// "Accuracy counter"
        /// </summary>
        public static LocalisableString AccuracyCounter => new TranslatableString(getKey(@"accuracy_counter"), @"Accuracy counter");

        /// <summary>
        /// "Score counter"
        /// </summary>
        public static LocalisableString ScoreCounter => new TranslatableString(getKey(@"score_counter"), @"Score counter");

        /// <summary>
        /// "Combo counter"
        /// </summary>
        public static LocalisableString ComboCounter => new TranslatableString(getKey(@"combo_counter"), @"Combo counter");

        /// <summary>
        /// "Longest combo"
        /// </summary>
        public static LocalisableString LongestComboCounter => new TranslatableString(getKey(@"longest_combo"), @"Longest combo");

        /// <summary>
        /// "Health bar"
        /// </summary>
        public static LocalisableString HealthDisplay => new TranslatableString(getKey(@"health_bar"), @"Health bar");

        /// <summary>
        /// "PP counter"
        /// </summary>
        public static LocalisableString PerformancePointsCounter => new TranslatableString(getKey(@"pp_counter"), @"PP counter");

        /// <summary>
        /// "Song progress bar"
        /// </summary>
        public static LocalisableString SongProgressBar => new TranslatableString(getKey(@"song_progress_bar"), @"Song progress bar");

        /// <summary>
        /// "Song progress pie"
        /// </summary>
        public static LocalisableString SongProgressPie => new TranslatableString(getKey(@"song_progress_pie"), @"Song progress pie");

        /// <summary>
        /// "UR counter"
        /// </summary>
        public static LocalisableString UnstableRateCounter => new TranslatableString(getKey(@"ur_counter"), @"UR counter");

        /// <summary>
        /// "Judgement counter"
        /// </summary>
        public static LocalisableString JudgementCounter => new TranslatableString(getKey(@"judgement_counter"), @"Judgement counter");

        /// <summary>
        /// "Key counter"
        /// </summary>
        public static LocalisableString KeyCounter => new TranslatableString(getKey(@"key_counter"), @"Key counter");

        /// <summary>
        /// "CPS counter"
        /// </summary>
        public static LocalisableString ClicksPerSecondCounter => new TranslatableString(getKey(@"cps_counter"), @"CPS counter");

        /// <summary>
        /// "BPM counter"
        /// </summary>
        public static LocalisableString BPMCounter => new TranslatableString(getKey(@"bpm_counter"), @"BPM counter");

        #endregion



        #region Other Components

        /// <summary>
        /// "Accuracy meter"
        /// </summary>
        public static LocalisableString HitErrorMeter => new TranslatableString(getKey(@"accuracy_meter"), @"Accuracy meter");

        /// <summary>
        /// "Rank display"
        /// </summary>
        public static LocalisableString RankDisplay => new TranslatableString(getKey(@"rank_display"), @"Rank display");

        /// <summary>
        /// "Leaderboard"
        /// </summary>
        public static LocalisableString Leaderboard => new TranslatableString(getKey(@"leaderboard"), @"Leaderboard");

        /// <summary>
        /// "Mods"
        /// </summary>
        public static LocalisableString ModDisplay => new TranslatableString(getKey(@"mods"), @"Mods");

        /// <summary>
        /// "Spectators"
        /// </summary>
        public static LocalisableString SpectatorList => new TranslatableString(getKey(@"spectators"), @"Spectators");

        /// <summary>
        /// "Attribute text"
        /// </summary>
        public static LocalisableString BeatmapAttributeText => new TranslatableString(getKey(@"attribute_text"), @"Attribute text");

        #endregion



        #region Basic Elements

        /// <summary>
        /// "Box"
        /// </summary>
        public static LocalisableString BoxElement => new TranslatableString(getKey(@"box"), @"Box");

        /// <summary>
        /// "Text"
        /// </summary>
        public static LocalisableString TextElement => new TranslatableString(getKey(@"text"), @"Text");

        /// <summary>
        /// "Sprite"
        /// </summary>
        public static LocalisableString SkinnableSprite => new TranslatableString(getKey(@"sprite"), @"Sprite");

        #endregion



        #region Cosmetic Components

        /// <summary>
        /// "Sheared wedge"
        /// </summary>
        public static LocalisableString ArgonWedgePiece => new TranslatableString(getKey(@"sheared_wedge"), @"Sheared wedge");

        /// <summary>
        /// "Avatar"
        /// </summary>
        public static LocalisableString PlayerAvatar => new TranslatableString(getKey(@"player_avatar"), @"Avatar");

        /// <summary>
        /// "Username"
        /// </summary>
        public static LocalisableString PlayerName => new TranslatableString(getKey(@"player_name"), @"Username");

        /// <summary>
        /// "Flag"
        /// </summary>
        public static LocalisableString PlayerFlag => new TranslatableString(getKey(@"player_flag"), @"Flag");

        /// <summary>
        /// "Team flag"
        /// </summary>
        public static LocalisableString PlayerTeamFlag => new TranslatableString(getKey(@"player_team_flag"), @"Team flag");

        /// <summary>
        /// "The big black"
        /// </summary>
        public static LocalisableString BigBlackBox => new TranslatableString(getKey(@"big_black_box_experimental"), @"The big black"); // whos afraid of the big black- O_O

        #endregion



        #region osu! Ruleset

        /// <summary>
        /// "Aim error meter"
        /// </summary>
        public static LocalisableString AimErrorMeter => new TranslatableString(getKey(@"aim_error_meter"), @"Aim error meter");

        #endregion

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
