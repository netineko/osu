// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Configuration;
using osu.Game.Screens.Play.HUD;
using osuTK;

namespace osu.Game.Skinning
{
    public partial class LegacySongProgress : SongProgress
    {
        private CircularProgress circularProgress = null!;

        // Legacy song progress doesn't support interaction for now.
        public override bool HandleNonPositionalInput => false;
        public override bool HandlePositionalInput => false;

        [SettingSource("Border thickness")]
        public new BindableFloat BorderThickness { get; set; } = new BindableFloat(2)
        {
            MinValue = 0,
            MaxValue = 5,
            Precision = 0.1f
        };

        [SettingSource("Dot size")]
        public BindableFloat DotSize { get; set; } = new BindableFloat(4)
        {
            MinValue = 0,
            MaxValue = 20,
            Precision = 0.1f
        };

        [SettingSource("Border colour")]
        public new BindableColour4 BorderColour { get; set; } = new BindableColour4(Colour4.FromHex("#FFFFFF"));

        [SettingSource("Normal progress colour")]
        public BindableColour4 NormalColour { get; set; } = new BindableColour4(Colour4.FromHex("#FFFFFF"));

        [SettingSource("Intro progress colour")]
        public BindableColour4 IntroColour { get; set; } = new BindableColour4(Colour4.FromHex("#C7FF2F"));

        public LegacySongProgress()
        {
            // User shouldn't be able to adjust width/height of this as `CircularProgress` doesn't
            // handle stretched cases well.
            AutoSizeAxes = Axes.Both;
        }

        private CircularContainer border = null!;
        private Circle dot = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.95f),
                    Child = circularProgress = new CircularProgress
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.6f,
                    },
                },
                border = new CircularContainer
                {
                    Size = new Vector2(33),
                    Masking = true,
                    BorderColour = BorderColour.Value,
                    BorderThickness = BorderThickness.Value,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        AlwaysPresent = true,
                        Alpha = 0,
                    }
                },
                dot = new Circle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = BorderColour.Value,
                    Size = new Vector2(DotSize.Value),
                }
            };

            BorderThickness.BindValueChanged(b => border.BorderThickness = b.NewValue);
            DotSize.BindValueChanged(b => dot.Size = new Vector2(b.NewValue));
            BorderColour.BindValueChanged(b => border.BorderColour = b.NewValue);
            BorderColour.BindValueChanged(b => dot.Colour = b.NewValue);
        }

        protected override void UpdateProgress(double progress, bool isIntro)
        {
            if (isIntro)
            {
                circularProgress.Scale = new Vector2(-1, 1);
                circularProgress.Anchor = Anchor.TopRight;
                circularProgress.Colour = IntroColour?.Value ?? new Colour4(199, 255, 47, 153);
                circularProgress.Progress = 1 - progress;
            }
            else
            {
                circularProgress.Scale = new Vector2(1);
                circularProgress.Anchor = Anchor.TopLeft;
                circularProgress.Colour = NormalColour?.Value ?? new Colour4(255, 255, 255, 153);
                circularProgress.Progress = progress;
            }
        }
    }
}
