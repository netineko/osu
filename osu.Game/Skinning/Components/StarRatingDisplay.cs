// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Threading;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Utils;
using osuTK;

namespace osu.Game.Skinning.Components
{
    /// <summary>
    /// A pill that displays the star rating of a beatmap.
    /// </summary>
    public partial class StarRatingDisplay : CompositeDrawable, ISerialisableDrawable, IHasCurrentValue<StarDifficulty>
    {
        public bool UsesFixedAnchor { get; set; }

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.Style))]
        public Bindable<StarRatingDisplaySize> Style { get; } = new Bindable<StarRatingDisplaySize>(StarRatingDisplaySize.Regular);

        [Resolved]
        private IBindable<WorkingBeatmap> beatmap { get; set; } = null!;

        [Resolved]
        private BeatmapDifficultyCache difficultyCache { get; set; } = null!;

        private IBindable<StarDifficulty>? difficultyBindable;
        private CancellationTokenSource? difficultyCancellationSource;

        private readonly Box background;
        private readonly SpriteIcon starIcon;
        private readonly OsuSpriteText starsText;

        private readonly BindableWithCurrent<StarDifficulty> current = new BindableWithCurrent<StarDifficulty>();

        public Bindable<StarDifficulty> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        private readonly Bindable<double> displayedStars = new BindableDouble();

        [Resolved]
        private OsuColour colours { get; set; } = null!;

        private GridContainer styleContainer;

        /// <summary>
        /// Creates a new <see cref="StarRatingDisplay"/> using an already computed <see cref="StarDifficulty"/>.
        /// </summary>
        public StarRatingDisplay()
        {
            AutoSizeAxes = Axes.Both;

            InternalChild = new CircularContainer
            {
                Masking = true,
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    background = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    styleContainer = new GridContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Both,
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize),
                            new Dimension(GridSizeMode.Absolute, 3f),
                            new Dimension(GridSizeMode.AutoSize, minSize: 25f),
                        },
                        RowDimensions = new[] { new Dimension(GridSizeMode.AutoSize) },
                        Content = new[]
                        {
                            new[]
                            {
                                starIcon = new SpriteIcon
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Icon = FontAwesome.Solid.Star,
                                    Size = new Vector2(8f),
                                },
                                Empty(),
                                starsText = new OsuSpriteText
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Margin = new MarginPadding { Bottom = 1.5f },
                                    Spacing = new Vector2(-1.4f),
                                    Font = OsuFont.Torus.With(size: 14.4f, weight: FontWeight.Bold, fixedWidth: true),
                                    Shadow = false,
                                },
                            }
                        }
                    },
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            beatmap.BindValueChanged(b =>
            {
                difficultyCancellationSource?.Cancel();
                difficultyCancellationSource = new CancellationTokenSource();

                difficultyBindable?.UnbindAll();
                difficultyBindable = difficultyCache.GetBindableDifficulty(b.NewValue.BeatmapInfo, difficultyCancellationSource.Token);
                difficultyBindable.BindValueChanged(d => Current.Value = d.NewValue);
            }, true);

            Style.BindValueChanged(b =>
            {
                switch (Style.Value)
                {
                    case StarRatingDisplaySize.Small:
                        styleContainer.Margin = new MarginPadding { Horizontal = 7f };
                        break;

                    case StarRatingDisplaySize.Regular:
                        styleContainer.Margin = new MarginPadding { Horizontal = 8f, Vertical = 2f };
                        break;
                }
            }, true);

            Current.BindValueChanged(c =>
            {
                // Animation roughly matches `StarCounter`'s implementation.
                this.TransformBindableTo(displayedStars, c.NewValue.Stars, 100 + 80 * Math.Abs(c.NewValue.Stars - c.OldValue.Stars), Easing.OutQuint);
            });

            displayedStars.Value = Current.Value.Stars;

            displayedStars.BindValueChanged(s =>
            {
                starsText.Text = s.NewValue < 0 ? "-" : s.NewValue.FormatStarRating();

                background.Colour = colours.ForStarDifficulty(s.NewValue);

                starIcon.Colour = colours.ForStarDifficultyText(s.NewValue);
                starsText.Colour = colours.ForStarDifficultyText(s.NewValue);
            }, true);
        }
    }

    public enum StarRatingDisplaySize
    {
        Small,
        Regular,
    }
}
