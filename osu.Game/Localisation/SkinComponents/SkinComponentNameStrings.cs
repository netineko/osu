// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Localisation.SkinComponents
{
    public static class SkinComponentNameStrings
    {
        private const string prefix = @"osu.Game.Resources.Localisation.SkinComponents.SkinComponentNameStrings";

        #region Gameplay

        /// <summary>
        /// "Argon - Accuracy counter"
        /// </summary>
        public static LocalisableString ArgonAccuracyCounter => new TranslatableString(getKey(@"argon_accuracy_counter"), @"Argon - Accuracy counter");

        /// <summary>
        /// "Triangles - Accuracy counter"
        /// </summary>
        public static LocalisableString DefaultAccuracyCounter => new TranslatableString(getKey(@"triangles_accuracy_counter"), @"Triangles - Accuracy counter");

        /// <summary>
        /// "Legacy - Accuracy counter"
        /// </summary>
        public static LocalisableString LegacyAccuracyCounter => new TranslatableString(getKey(@"legacy_accuracy_counter"), @"Legacy - Accuracy counter");



        /// <summary>
        /// "Argon - Score counter"
        /// </summary>
        public static LocalisableString ArgonScoreCounter => new TranslatableString(getKey(@"argon_score_counter"), @"Argon - Score counter");

        /// <summary>
        /// "Triangles - Score counter"
        /// </summary>
        public static LocalisableString DefaultScoreCounter => new TranslatableString(getKey(@"triangles_score_counter"), @"Triangles - Score counter");

        /// <summary>
        /// "Legacy - Score counter"
        /// </summary>
        public static LocalisableString LegacyScoreCounter => new TranslatableString(getKey(@"legacy_score_counter"), @"Legacy - Score counter");



        /// <summary>
        /// "Argon - Combo counter"
        /// </summary>
        public static LocalisableString ArgonComboCounter => new TranslatableString(getKey(@"argon_combo_counter"), @"Argon - Combo counter");

        /// <summary>
        /// "Triangles - Combo counter"
        /// </summary>
        public static LocalisableString DefaultComboCounter => new TranslatableString(getKey(@"triangles_combo_counter"), @"Triangles - Combo counter");

        /// <summary>
        /// "Legacy - Combo counter"
        /// </summary>
        public static LocalisableString LegacyDefaultComboCounter => new TranslatableString(getKey(@"legacy_combo_counter"), @"Legacy - Combo counter");



        /// <summary>
        /// "Triangles - Longest combo"
        /// </summary>
        public static LocalisableString LongestComboCounter => new TranslatableString(getKey(@"triangles_longest_combo"), @"Triangles - Longest combo");



        /// <summary>
        /// "Argon - Health bar"
        /// </summary>
        public static LocalisableString ArgonHealthDisplay => new TranslatableString(getKey(@"argon_health_bar"), @"Argon - Health bar");

        /// <summary>
        /// "Triangles - Health bar"
        /// </summary>
        public static LocalisableString DefaultHealthDisplay => new TranslatableString(getKey(@"triangles_health_bar"), @"Triangles - Health bar");

        /// <summary>
        /// "Legacy - Health bar"
        /// </summary>
        public static LocalisableString LegacyHealthDisplay => new TranslatableString(getKey(@"legacy_health_bar"), @"Legacy - Health bar");



        /// <summary>
        /// "Argon - Performance points counter"
        /// </summary>
        public static LocalisableString ArgonPerformancePointsCounter => new TranslatableString(getKey(@"argon_performance_points_counter"), @"Argon - Performance points counter");

        /// <summary>
        /// "Triangles - Performance points counter"
        /// </summary>
        public static LocalisableString TrianglesPerformancePointsCounter => new TranslatableString(getKey(@"triangles_performance_points_counter"), @"Triangles - Performance points counter");

        /// <summary>
        /// "Legacy - Performance points counter"
        /// </summary>
        public static LocalisableString LegacyPerformancePointsCounter => new TranslatableString(getKey(@"legacy_performance_points_counter"), @"Legacy - Performance points counter");



        /// <summary>
        /// "Argon - Song progress bar"
        /// </summary>
        public static LocalisableString ArgonSongProgress => new TranslatableString(getKey(@"argon_progress_bar"), @"Argon - Song progress bar");

        /// <summary>
        /// "Triangles - Song progress bar"
        /// </summary>
        public static LocalisableString DefaultSongProgress => new TranslatableString(getKey(@"triangles_progress_bar"), @"Triangles - Song progress bar");

        /// <summary>
        /// "Song progress pie"
        /// </summary>
        public static LocalisableString LegacySongProgress => new TranslatableString(getKey(@"song_progress_pie"), @"Song progress pie");



        /// <summary>
        /// "Argon - Unstable rate counter"
        /// </summary>
        public static LocalisableString ArgonUnstableRateCounter => new TranslatableString(getKey(@"argon_unstable_rate_counter"), @"Argon - Unstable rate counter");

        /// <summary>
        /// "Triangles - Unstable rate counter"
        /// </summary>
        public static LocalisableString TrianglesUnstableRateCounter => new TranslatableString(getKey(@"triangles_ur_counter"), @"Triangles - Unstable rate counter");



        /// <summary>
        /// "Argon - Judgement counter"
        /// </summary>
        public static LocalisableString ArgonJudgementCounter => new TranslatableString(getKey(@"argon_judgement_counter"), @"Argon - Judgement counter");

        /// <summary>
        /// "Triangles - Judgement counter"
        /// </summary>
        public static LocalisableString JudgementCounterDisplay => new TranslatableString(getKey(@"triangles_judgement_counter"), @"Triangles - Judgement counter");



        /// <summary>
        /// "Argon - Key counter"
        /// </summary>
        public static LocalisableString ArgonKeyCounter => new TranslatableString(getKey(@"argon_key_counter"), @"Argon - Key counter");

        /// <summary>
        /// "Triangles - Key counter"
        /// </summary>
        public static LocalisableString DefaultKeyCounterDisplay => new TranslatableString(getKey(@"triangles_key_counter"), @"Triangles - Key counter");

        /// <summary>
        /// "Legacy - Key counter"
        /// </summary>
        public static LocalisableString LegacyKeyCounterDisplay => new TranslatableString(getKey(@"legacy_key_counter"), @"Legacy - Key counter");



        /// <summary>
        /// "Triangles - Clicks per second counter"
        /// </summary>
        public static LocalisableString ClicksPerSecondCounter => new TranslatableString(getKey(@"triangles_cps_counter"), @"Triangles - Clicks per second counter");



        /// <summary>
        /// "Triangles - BPM counter"
        /// </summary>
        public static LocalisableString BPMCounter => new TranslatableString(getKey(@"triangles_bpm_counter"), @"Triangles - BPM counter");

        #endregion



        #region Other Components

        /// <summary>
        /// "Accuracy meter (Bar)"
        /// </summary>
        public static LocalisableString BarHitErrorMeter => new TranslatableString(getKey(@"bar_accuracy_meter"), @"Accuracy meter (Bar)");

        /// <summary>
        /// "Accuracy meter (Colours)"
        /// </summary>
        public static LocalisableString ColourHitErrorMeter => new TranslatableString(getKey(@"colour_accuracy_meter"), @"Accuracy meter (Colours)");

        /// <summary>
        /// "Rank display"
        /// </summary>
        public static LocalisableString DefaultRankDisplay => new TranslatableString(getKey(@"rank_display"), @"Rank display");

        /// <summary>
        /// "Legacy - Rank display"
        /// </summary>
        public static LocalisableString LegacyRankDisplay => new TranslatableString(getKey(@"legacy_rank_display"), @"Legacy - Rank display");

        /// <summary>
        /// "Leaderboard"
        /// </summary>
        public static LocalisableString DrawableGameplayLeaderboard => new TranslatableString(getKey(@"gameplay_leaderboard"), @"Leaderboard");

        /// <summary>
        /// "Mod list"
        /// </summary>
        public static LocalisableString SkinnableModDisplay => new TranslatableString(getKey(@"mod_list"), @"Mod list");

        /// <summary>
        /// "Spectator list"
        /// </summary>
        public static LocalisableString SpectatorList => new TranslatableString(getKey(@"spectator_list"), @"Spectator list");

        /// <summary>
        /// "Beatmap attribute text"
        /// </summary>
        public static LocalisableString BeatmapAttributeText => new TranslatableString(getKey(@"beatmap_attribute_text"), @"Beatmap attribute text");

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
        /// "Player avatar"
        /// </summary>
        public static LocalisableString PlayerAvatar => new TranslatableString(getKey(@"player_avatar"), @"Player avatar");

        /// <summary>
        /// "Player name"
        /// </summary>
        public static LocalisableString PlayerName => new TranslatableString(getKey(@"player_name"), @"Player name");

        /// <summary>
        /// "Player flag"
        /// </summary>
        public static LocalisableString PlayerFlag => new TranslatableString(getKey(@"player_flag"), @"Player flag");

        /// <summary>
        /// "Player's team flag"
        /// </summary>
        public static LocalisableString PlayerTeamFlag => new TranslatableString(getKey(@"player_team_flag"), @"Player's team flag");

        /// <summary>
        /// "Big black box (Experimental)"
        /// </summary>
        public static LocalisableString BigBlackBox => new TranslatableString(getKey(@"big_black_box_experimental"), @"Big black box (Experimental)");

        #endregion



        #region osu! Ruleset

        /// <summary>
        /// "Aim error meter (osu!)"
        /// </summary>
        public static LocalisableString AimErrorMeter => new TranslatableString(getKey(@"aim_error_meter"), @"Aim error meter (osu!)");

        #endregion



        #region osu!mania Ruleset

        /// <summary>
        /// "Argon - Combo counter (osu!mania)"
        /// </summary>
        public static LocalisableString ArgonManiaComboCounter => new TranslatableString(getKey(@"argon_mania_combo_counter"), @"Argon - Combo counter (osu!mania)");

        /// <summary>
        /// "Legacy - Combo counter (osu!mania)"
        /// </summary>
        public static LocalisableString LegacyManiaComboCounter => new TranslatableString(getKey(@"legacy_mania_combo_counter"), @"Legacy - Combo counter (osu!mania)");

        #endregion

        private static string getKey(string key) => $@"{prefix}:{key}";
    }
}
