// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Configuration;
using osu.Game.Localisation.SkinComponents;
using osu.Game.Overlays.Settings;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Skinning.Components
{
    public partial class BoxElement : CompositeDrawable, ISerialisableDrawable
    {
        public bool UsesFixedAnchor { get; set; }

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.CornerRadius), nameof(SkinnableComponentStrings.CornerRadiusDescription),
            SettingControlType = typeof(SettingsPercentageSlider<float>))]
        public new BindableFloat CornerRadius { get; } = new BindableFloat(0.25f)
        {
            MinValue = 0,
            MaxValue = 0.5f,
            Precision = 0.01f
        };

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.ShearAmount),
            SettingControlType = typeof(SettingsPercentageSlider<float>))]
        public BindableFloat ShearAmount { get; } = new BindableFloat(0)
        {
            MinValue = 0,
            MaxValue = 0.5f,
            Precision = 0.01f
        };

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.Colour1))]
        public BindableColour4 Colour1 { get; } = new BindableColour4(Colour4.White);

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.Opacity1))]
        public BindableFloat Opacity1 { get; } = new BindableFloat(1)
        {
            MinValue = 0,
            MaxValue = 1,
            Precision = 0.01f
        };

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.Colour2))]
        public BindableColour4 Colour2 { get; } = new BindableColour4(Colour4.White);

        [SettingSource(typeof(SkinnableComponentStrings), nameof(SkinnableComponentStrings.Opacity2))]
        public BindableFloat Opacity2 { get; } = new BindableFloat(1)
        {
            MinValue = 0,
            MaxValue = 1,
            Precision = 0.01f
        };

        public BoxElement()
        {
            Size = new Vector2(400, 80);

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.White,
                    RelativeSizeAxes = Axes.Both,
                },
            };

            Masking = true;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Colour1.BindValueChanged(_ => Colour = ColourInfo.GradientHorizontal(Colour1.Value.Opacity(Opacity1.Value), Colour2.Value.Opacity(Opacity2.Value)), true);
            Colour2.BindValueChanged(_ => Colour = ColourInfo.GradientHorizontal(Colour1.Value.Opacity(Opacity1.Value), Colour2.Value.Opacity(Opacity2.Value)), true);
            Opacity1.BindValueChanged(_ => Colour = ColourInfo.GradientHorizontal(Colour1.Value.Opacity(Opacity1.Value), Colour2.Value.Opacity(Opacity2.Value)), true);
            Opacity2.BindValueChanged(_ => Colour = ColourInfo.GradientHorizontal(Colour1.Value.Opacity(Opacity1.Value), Colour2.Value.Opacity(Opacity2.Value)), true);

            ShearAmount.BindValueChanged(_ => Shear = new Vector2(ShearAmount.Value, 0));
        }

        protected override void Update()
        {
            base.Update();

            base.CornerRadius = CornerRadius.Value * Math.Min(DrawWidth, DrawHeight);
        }
    }
}
