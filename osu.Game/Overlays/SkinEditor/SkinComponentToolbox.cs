// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osu.Framework.Logging;
using osu.Framework.Threading;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Localisation;
using osu.Game.Rulesets;
using osu.Game.Screens.Edit.Components;
using osu.Game.Skinning;
using osuTK;

namespace osu.Game.Overlays.SkinEditor
{
    public partial class SkinComponentToolbox : EditorSidebarSection
    {
        public Action<Type>? RequestPlacement;

        private readonly SkinnableContainer target;

        private readonly RulesetInfo? ruleset;

        private FillFlowContainer fill = null!;

        public Bindable<bool> ExpandsOnHover = new Bindable<bool>(true);

        public ToolboxComponentGroup ArgonGroup = null!;
        public ToolboxComponentGroup TrianglesGroup = null!;
        public ToolboxComponentGroup LegacyGroup = null!;

        /// <summary>
        /// Create a new component toolbox for the specified taget.
        /// </summary>
        /// <param name="target">The target. This is mainly used as a dependency source to find candidate components.</param>
        /// <param name="ruleset">A ruleset to filter components by. If null, only components which are not ruleset-specific will be included.</param>
        public SkinComponentToolbox(SkinnableContainer target, RulesetInfo? ruleset)
            : base(ruleset == null ? SkinEditorStrings.Components : LocalisableString.Interpolate($"{SkinEditorStrings.Components} ({ruleset.Name})"))
        {
            this.target = target;
            this.ruleset = ruleset;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Child = fill = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(EditorSidebar.PADDING)
            };

            reloadComponents();
        }

        private void reloadComponents()
        {
            fill.Clear();

            // we dont want the groups showing in the rulset categories (mostly because theyre so small) so we exclude them here
            if (ruleset == null)
            {
                fill.Add(ArgonGroup = new ToolboxComponentGroup("Argon components"));
                fill.Add(TrianglesGroup = new ToolboxComponentGroup("Triangles components"));
                fill.Add(LegacyGroup = new ToolboxComponentGroup("Custom components"));

                fill.Add(new Container { Height = 5 });

                LinkFlowContainer linkFlow;
                LegacyGroup.Fill.Add(new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    CornerRadius = 5,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.FromHex("#796718"),
                        },
                        linkFlow = new LinkFlowContainer
                        {
                            Padding = new MarginPadding(10),
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                        },
                    }
                });
                linkFlow.AddText("These components require textures to be externally added to the skin! See ");
                linkFlow.AddLink("this wiki page", "https://osu.ppy.sh/wiki/en/Skinning");
                linkFlow.AddText(" for some of the required files.");
            }

            var skinnableTypes = SerialisedDrawableInfo.GetAllAvailableDrawables(ruleset);
            foreach (var type in skinnableTypes)
                attemptAddComponent(type);
        }

        private void attemptAddComponent(Type type)
        {
            try
            {
                Drawable instance = (Drawable)Activator.CreateInstance(type)!;

                if (!((ISerialisableDrawable)instance).IsEditable) return;

                IHasSkinDetails? detailedComponent = null;
                try
                {
                    detailedComponent = (IHasSkinDetails)instance;
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Skin component {type} is missing the SkinComponent base class");
                }

                if (detailedComponent?.Group == ComponentGroup.Argon)
                {
                    ArgonGroup.Fill.Add(new ToolboxComponentButton(instance, target, this, detailedComponent)
                    {
                        RequestPlacement = t => RequestPlacement?.Invoke(t),
                        Expanding = contractOtherButtons,
                    });
                }
                else if (detailedComponent?.Group == ComponentGroup.Triangles)
                {
                    TrianglesGroup.Fill.Add(new ToolboxComponentButton(instance, target, this, detailedComponent)
                    {
                        RequestPlacement = t => RequestPlacement?.Invoke(t),
                        Expanding = contractOtherButtons,
                    });
                }
                else if (detailedComponent?.Group == ComponentGroup.Legacy)
                {
                    LegacyGroup.Fill.Add(new ToolboxComponentButton(instance, target, this, detailedComponent)
                    {
                        RequestPlacement = t => RequestPlacement?.Invoke(t),
                        Expanding = contractOtherButtons,
                    });
                }
                else
                {
                    fill.Add(new ToolboxComponentButton(instance, target, this, detailedComponent)
                    {
                        RequestPlacement = t => RequestPlacement?.Invoke(t),
                        Expanding = contractOtherButtons,
                    });
                }
            }
            catch (DependencyNotRegisteredException)
            {
                // This loading code relies on try-catching any dependency injection errors to know which components are valid for the current target screen.
                // If a screen can't provide the required dependencies, a skinnable component should not be displayed in the list.
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Skin component {type} could not be loaded in the editor component list due to an error");
            }
        }

        private void contractOtherButtons(ToolboxComponentButton obj)
        {
            foreach (var b in fill.OfType<ToolboxComponentButton>())
            {
                if (b == obj)
                    continue;

                b.Contract();
            }
        }

        public partial class ToolboxComponentButton : OsuButton
        {
            public Action<Type>? RequestPlacement;
            public Action<ToolboxComponentButton>? Expanding;

            private readonly Drawable component;
            private readonly IHasSkinDetails? detailedComponent;
            private readonly CompositeDrawable? dependencySource;
            private readonly SkinComponentToolbox toolbox;

            private Container innerContainer = null!;
            private Container background = null!;
            private OsuSpriteText text = null!;

            private ScheduledDelegate? expandContractAction;

            private const float contracted_size = 90;
            private const float expanded_size = 150;
            private const float text_height = 20;

            public ToolboxComponentButton(Drawable component, CompositeDrawable? dependencySource, SkinComponentToolbox toolbox, IHasSkinDetails? detailedComponent = null)
            {
                this.component = component;
                this.detailedComponent = detailedComponent;
                this.dependencySource = dependencySource;
                this.toolbox = toolbox;

                Enabled.Value = true;

                RelativeSizeAxes = Axes.X;
                Height = contracted_size;
            }

            private const double animation_duration = 400;

            protected override bool OnHover(HoverEvent e)
            {
                if (toolbox.ExpandsOnHover.Value)
                {
                    expandContractAction?.Cancel();
                    expandContractAction = Scheduler.AddDelayed(() =>
                    {
                        this.ResizeHeightTo(expanded_size, animation_duration, Easing.OutQuint);
                        background.ResizeHeightTo(expanded_size - text_height * 1.5f, animation_duration, Easing.OutQuint);
                        text.ScaleTo(1.1f, animation_duration, Easing.OutQuint);
                        text.TransformTo(nameof(Margin), new MarginPadding(6), animation_duration, Easing.OutQuint);
                        Expanding?.Invoke(this);
                    }, 100);
                }

                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);

                if (toolbox.ExpandsOnHover.Value)
                {
                    expandContractAction?.Cancel();
                    // If no other component is selected for too long, force a contract.
                    // Otherwise we will generally contract when Contract() is called from outside.
                    expandContractAction = Scheduler.AddDelayed(Contract, 200);
                }
            }

            public void Contract()
            {
                // Cheap debouncing to avoid stacking animations.
                // The only place this is nulled is at the end of this method.
                if (expandContractAction == null)
                    return;

                this.ResizeHeightTo(contracted_size, animation_duration, Easing.OutQuint);
                background.ResizeHeightTo(contracted_size - text_height, animation_duration, Easing.OutQuint);
                text.ScaleTo(0.9f, animation_duration, Easing.OutQuint);
                text.TransformTo(nameof(Margin), new MarginPadding(4), animation_duration, Easing.OutQuint);

                expandContractAction?.Cancel();
                expandContractAction = null;
            }

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                BackgroundColour = colourProvider.Background3;

                AddRange(new Drawable[]
                {
                    background = new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        CornerRadius = 5,
                        Masking = true,
                        Height = Height - text_height,
                        BorderThickness = 2,
                        BorderColour = ColourInfo.GradientVertical(colourProvider.Background2, colourProvider.Background1),
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = colourProvider.Background2
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Padding = new MarginPadding(10),
                                Masking = true,
                                Child = innerContainer = new DependencyBorrowingContainer(dependencySource)
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Child = component
                                },
                            },
                        },
                    },
                    text = new OsuSpriteText
                    {
                        Text = detailedComponent?.VisualName ?? component.GetType().Name,
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                        Margin = new MarginPadding(4),
                        Scale = new Vector2(0.9f),
                    },
                });

                // adjust provided component to fit / display in a known state.
                component.Anchor = Anchor.Centre;
                component.Origin = Anchor.Centre;
            }

            protected override void UpdateAfterChildren()
            {
                base.UpdateAfterChildren();

                if (component.DrawSize != Vector2.Zero)
                {
                    float bestScale = Math.Min(
                        innerContainer.DrawWidth / component.DrawWidth,
                        innerContainer.DrawHeight / component.DrawHeight);

                    innerContainer.Scale = new Vector2(bestScale);
                }
            }

            protected override bool OnClick(ClickEvent e)
            {
                RequestPlacement?.Invoke(component.GetType());
                return true;
            }
        }

        public partial class ToolboxComponentGroup : ClickableContainer
        {
            public FillFlowContainer Fill = null!;
            private Container fillBackground = null!;
            private Box background = null!;
            private LocalisableString groupName = @"Unknown group";

            private bool expanded = false;

            public ToolboxComponentGroup(string GroupName)
            {
                groupName = GroupName;
                Action = () => { };
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;
            }

            private const double animation_duration = 300;

            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                AddRange(new Drawable[]
                {
                    fillBackground = new Container
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        RelativeSizeAxes = Axes.X,
                        CornerRadius = 10,
                        Y = 40,
                        Masking = true,
                        Children = new Drawable[]
                        {
                            background = new Box
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                RelativeSizeAxes = Axes.Both,
                                Colour = colourProvider.Background6,
                            },
                            Fill = new FillFlowContainer
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Scale = new Vector2(1, 1),
                                Padding = new MarginPadding(5) { Top = 15 },
                                Spacing = new Vector2(EditorSidebar.PADDING),
                                Direction = FillDirection.Vertical,
                            },
                        }
                    },
                    new Button(groupName, colourProvider, expanded, expanding, Fill, fillBackground),
                });
            }

            private bool expanding = false;

            private partial class Button : Container
            {
                private bool expanded = false;
                private bool expanding = false;
                private FillFlowContainer fill = null!;
                private Container fillBackground = null!;
                private OverlayColourProvider colourProvider = null!;
                private Box background = null!;
                private OsuSpriteText text = null!;
                private SpriteIcon chevron = null!;

                public Button(LocalisableString groupName, OverlayColourProvider colourProvider, bool expanded, bool expanding, FillFlowContainer fill, Container fillBackground)
                {
                    this.expanded = expanded;
                    this.expanding = expanding;
                    this.fill = fill;
                    this.fillBackground = fillBackground;
                    this.colourProvider = colourProvider;

                    RelativeSizeAxes = Axes.X;
                    Height = 50;
                    CornerRadius = 5;
                    Masking = true;
                    Children = new Drawable[]
                    {
                        background = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = colourProvider.Background3,
                        },
                        text = new OsuSpriteText
                        {
                            Text = groupName,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Margin = new MarginPadding(10),
                            Font = OsuFont.GetFont(Typeface.Torus, size: 18, weight: FontWeight.SemiBold)
                        },
                        chevron = new SpriteIcon
                        {
                            Icon = FontAwesome.Solid.ChevronDown,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Size = new Vector2(15),
                            Margin = new MarginPadding(15)
                        }
                    };
                }

                protected override bool OnHover(HoverEvent e)
                {
                    //text.ScaleTo(1.3f, animation_duration, Easing.OutQuint);
                    text.TransformSpacingTo(new Vector2(0.8f, 0), animation_duration, Easing.OutQuint);
                    background.FadeColour(colourProvider.Colour3, 200, Easing.OutQuint);

                    return base.OnHover(e);
                }

                protected override void OnHoverLost(HoverLostEvent e)
                {
                    //text.ScaleTo(1.2f, animation_duration, Easing.OutQuint);
                    text.TransformSpacingTo(new Vector2(0, 0), animation_duration, Easing.OutQuint);
                    background.FadeColour(colourProvider.Background3, 500, Easing.OutQuint);

                    base.OnHoverLost(e);
                }
                protected override bool OnClick(ClickEvent e)
                {
                    if (expanded) expanded = false;
                    else expanded = true;

                    if (expanded)
                    {
                        chevron.ScaleTo(new Vector2(1, -1), 300, Easing.OutQuint);
                        expanding = true;
                        fill.FadeInFromZero(300, Easing.OutQuint);
                        fillBackground.ResizeHeightTo(fill.Height, 300, Easing.InOutQuint);
                        Scheduler.AddDelayed(() => expanding = false, 300);
                    }
                    else
                    {
                        chevron.ScaleTo(new Vector2(1, 1), 300, Easing.OutQuint);
                        fill.FadeOutFromOne(300, Easing.OutQuint);
                        fillBackground.ResizeHeightTo(0f, 300, Easing.OutQuint);
                    }
                    return true;
                }

                protected override void Update()
                {
                    if (expanded && !expanding)
                    {
                        fillBackground.ResizeHeightTo(fill.Height);
                    }
                    base.Update();
                }
            }
        }

        private partial class DependencyBorrowingContainer : Container
        {
            protected override bool ShouldBeConsideredForInput(Drawable child) => false;

            public override bool PropagateNonPositionalInputSubTree => false;

            private readonly CompositeDrawable? donor;

            public DependencyBorrowingContainer(CompositeDrawable? donor)
            {
                this.donor = donor;
            }

            protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            {
                var baseDependencies = base.CreateChildDependencies(parent);
                if (donor == null)
                    return baseDependencies;

                var dependencies = new DependencyContainer(donor.Dependencies);
                // inject `SkinEditor` again *on top* of the borrowed dependencies.
                // this is designed to let components know when they are being displayed in the context of the skin editor
                // via attempting to resolve `SkinEditor`.
                dependencies.CacheAs(baseDependencies.Get<SkinEditor>());
                return dependencies;
            }
        }
    }
}
