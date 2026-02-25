// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osuTK.Graphics;

namespace osu.Game.Graphics.UserInterfaceV2
{
    public partial class FormColourPicker : CompositeDrawable, IHasCurrentValue<Colour4>, IFormControl
    {
        public Bindable<Colour4> Current
        {
            get => current.Current;
            set => current.Current = value;
        }

        private readonly BindableWithCurrent<Colour4> current = new BindableWithCurrent<Colour4>();

        /// <summary>
        /// Caption describing this colour picker, displayed on the left of it.
        /// </summary>
        public LocalisableString Caption { get; init; }

        /// <summary>
        /// Hint text containing an extended description of this colour picker, displayed in a tooltip when hovering the caption.
        /// </summary>
        public LocalisableString HintText { get; init; }

        [Resolved]
        private OverlayColourProvider colourProvider { get; set; } = null!;

        private FormControlBackground background = null!;
        private FormFieldCaption caption = null!;
        private Button button = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Masking = true,
                CornerRadius = 5,
                CornerExponent = 2.5f,
                Children = new Drawable[]
                {
                    background = new FormControlBackground(),
                    new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Padding = new MarginPadding
                        {
                            Left = 9,
                            Right = 5,
                            Vertical = 5,
                        },
                        Children = new Drawable[]
                        {
                            new Container
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.X,
                                Width = 0.5f,
                                AutoSizeAxes = Axes.Y,
                                Padding = new MarginPadding { Right = Width / 2 + 5 },
                                Children = new Drawable[]
                                {
                                    caption = new FormFieldCaption
                                    {
                                        Caption = Caption,
                                        TooltipText = HintText,
                                    },
                                },
                            },
                            button = new Button
                            {
                                Current = Current,
                                Text = Current.Value.ToHex(),
                                Anchor = Anchor.CentreRight,
                                Origin = Anchor.CentreRight,
                                RelativeSizeAxes = Axes.X,
                                Width = 0.5f,
                                BackgroundColour = Current.Value,
                                TextColour = OsuColour.ForegroundTextColourFor(Current.Value)
                            }
                        },
                    },
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Current.BindValueChanged(_ =>
            {
                button.Current = Current;
                button.BackgroundColour = Current.Value;
                button.Text = Current.Value.ToHex();
                button.TextColour = OsuColour.ForegroundTextColourFor(Current.Value);
            });
            updateState();
        }

        protected override bool OnHover(HoverEvent e)
        {
            updateState();
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            updateState();
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (!IsDisabled)
            {
                background.Flash();
                button.TriggerClick();
            }

            return true;
        }

        private void updateState()
        {
            caption.Colour = IsDisabled ? colourProvider.Background1 : colourProvider.Content2;

            if (IsDisabled)
                background.VisualStyle = VisualStyle.Disabled;
            else if (IsHovered)
                background.VisualStyle = VisualStyle.Hovered;
            else
                background.VisualStyle = VisualStyle.Normal;

            // TODO: Support BackgroundColour?
        }

        public IEnumerable<LocalisableString> FilterTerms => Caption.Yield();
        public event Action? ValueChanged;
        public bool IsDefault => Current.IsDefault;
        public void SetDefault() => Current.SetDefault();
        public bool IsDisabled => Current.Disabled;
        public float MainDrawHeight => DrawHeight;

        public partial class Button : ClickableContainer, IHasPopover
        {
            public Bindable<Colour4> Current { get; set; } = new Bindable<Colour4>();

            public Color4 BackgroundColour
            {
                get => background.Colour; set => background.Colour = value;
            }

            public Color4 TextColour
            {
                get => spriteText.Colour; set => spriteText.Colour = value;
            }

            public LocalisableString Text
            {
                get => spriteText.Text; set => spriteText.Text = value;
            }

            private Box background = new Box
            {
                RelativeSizeAxes = Axes.X,
                Height = 40,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };
            private OsuSpriteText spriteText = new OsuSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = OsuFont.GetFont(weight: FontWeight.Bold)
            };

            [BackgroundDependencyLoader]
            private void load()
            {
                CornerRadius = 4;
                Masking = true;
                AutoSizeAxes = Axes.Y;
                Action = this.ShowPopover;

                Add(background);
                Add(spriteText);
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                Content.CornerRadius = 4;

                updateState();
            }

            protected override bool OnHover(HoverEvent e)
            {
                updateState();
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                updateState();
                base.OnHoverLost(e);
            }

            protected override bool OnClick(ClickEvent e)
            {
                if (!IsDisabled) Action.Invoke();
                return true;
            }

            public Popover GetPopover() => new ColourPickerPopover
            {
                Current = { BindTarget = Current }
            };

            private void updateState()
            {
                Alpha = IsDisabled ? 0.5f : 1;
            }

            public bool IsDisabled => Current.Disabled;
        }

        private partial class ColourPickerPopover : OsuPopover, IHasCurrentValue<Colour4>
        {
            public Bindable<Colour4> Current
            {
                get => current.Current;
                set => current.Current = value;
            }

            private readonly BindableWithCurrent<Colour4> current = new BindableWithCurrent<Colour4>();

            public ColourPickerPopover()
                : base(false)
            {
            }

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                Child = new OsuColourPicker
                {
                    Current = { BindTarget = Current }
                };

                Body.BorderThickness = 2;
                Body.BorderColour = colourProvider.Highlight1;
                Content.Padding = new MarginPadding(2);
            }
        }
    }
}
