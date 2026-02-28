// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Localisation;

namespace osu.Game.Skinning
{
    /// <summary>
    /// Gives skin components their names.
    /// </summary>
    public interface IHasSkinDetails
    {
        /// <summary>
        /// The visual name of this component used in the skin editor.
        /// </summary>
        public LocalisableString VisualName { get; }

        /// <summary>
        /// The shortened name of this component used in the skin editor.
        /// </summary>
        public LocalisableString ShortName { get; }

        /// <summary>
        /// The group this component is part of in the skin editor.
        /// </summary>
        public ComponentGroup Group { get; }
    }
}
