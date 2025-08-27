// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Diagnostics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Input.StateChanges;
using osu.Framework.Utils;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Configuration;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Game.Screens.Menu
{
    /// <summary>
    /// osu! logo and its attachments (pulsing, visualiser etc.)
    /// </summary>
    public partial class OsuLogo : BeatSyncedContainer
    {
        private const double transition_length = 300;

        /// <summary>
        /// The osu! logo sprite has a shadow included in its texture.
        /// This adjustment vector is used to match the precise edge of the border of the logo.
        /// </summary>
        public static readonly Vector2 SCALE_ADJUST = new Vector2(0.94f);

        private Bindable<LogoType> logoType;
        private readonly Sprite logo;
        private readonly Sprite logoOld;
        private readonly Sprite logoBump;
        private int bumpType = 2;
        private bool legacyRipple = false;
        private bool legacyFlash = false;
        private readonly CircularContainer logoBumpContainer;
        private readonly CircularContainer logoContainer;
        private readonly Container logoBounceContainer;
        private readonly Container logoBeatContainer;
        private readonly Container logoAmplitudeContainer;
        private readonly Container logoHoverContainer;
        private readonly MenuLogoVisualisation visualizer;
        private readonly ClassicMenuLogoVisualisation visualizerOld;

        private readonly IntroSequence intro;

        private Sample sampleClick;
        private SampleChannel sampleClickChannel;

        protected virtual MenuLogoVisualisation CreateMenuLogoVisualisation() => new MenuLogoVisualisation();
        protected virtual ClassicMenuLogoVisualisation CreateClassicMenuLogoVisualisation() => new ClassicMenuLogoVisualisation();

        protected virtual double BeatSampleVariance => 0.1;

        protected Sample SampleBeat;
        protected Sample SampleDownbeat;

        private readonly Container colourAndTriangles;
        private readonly TrianglesV2 triangles;
        private readonly Triangles trianglesOld;
        private readonly Box background;

        /// <summary>
        /// Return value decides whether the logo should play its own sample for the click action.
        /// </summary>
        public Func<bool> Action;

        /// <summary>
        /// The size of the logo Sprite with respect to the scale of its hover and bounce containers.
        /// </summary>
        /// <remarks>Does not account for the scale of this <see cref="OsuLogo"/></remarks>
        public float SizeForFlow => logo == null ? 0 : logo.DrawSize.X * logo.Scale.X * logoBounceContainer.Scale.X * logoHoverContainer.Scale.X;

        public bool IsTracking { get; set; }

        private readonly Sprite ripple;

        private readonly Container rippleContainer;

        public bool Triangles
        {
            set => colourAndTriangles.FadeTo(value ? 1 : 0, transition_length, Easing.OutQuint);
        }

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => logoContainer.ReceivePositionalInputAt(screenSpacePos);

        public bool Ripple
        {
            get => rippleContainer.Alpha > 0;
            set => rippleContainer.FadeTo(value ? 1 : 0, transition_length, Easing.OutQuint);
        }

        private const float visualizer_default_alpha = 0.5f;

        private readonly Box flashLayer;

        private readonly Container impactContainer;

        private const double early_activation = 60;

        private const float triangles_paused_velocity = 0.5f;

        public override bool IsPresent => base.IsPresent || Scheduler.HasPendingTasks;

        public OsuLogo()
        {
            EarlyActivationMilliseconds = early_activation;

            Origin = Anchor.Centre;

            AutoSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                intro = new IntroSequence
                {
                    RelativeSizeAxes = Axes.Both,
                },
                logoHoverContainer = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        logoBounceContainer = new Container
                        {
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                rippleContainer = new Container
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    RelativeSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        ripple = new Sprite
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Blending = BlendingParameters.Additive,
                                            Alpha = 0
                                        }
                                    }
                                },
                                logoAmplitudeContainer = new Container
                                {
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        logoBeatContainer = new Container
                                        {
                                            AutoSizeAxes = Axes.Both,
                                            Children = new Drawable[]
                                            {
                                                visualizer = CreateMenuLogoVisualisation().With(v =>
                                                {
                                                    v.RelativeSizeAxes = Axes.Both;
                                                    v.Origin = Anchor.Centre;
                                                    v.Anchor = Anchor.Centre;
                                                    v.Alpha = visualizer_default_alpha;
                                                    v.Size = SCALE_ADJUST;
                                                }),
                                                visualizerOld = CreateClassicMenuLogoVisualisation().With(v =>
                                                {
                                                    v.RelativeSizeAxes = Axes.Both;
                                                    v.Origin = Anchor.Centre;
                                                    v.Anchor = Anchor.Centre;
                                                    v.Alpha = visualizer_default_alpha;
                                                    v.Size = SCALE_ADJUST;
                                                }),
                                                LogoElements = new Container
                                                {
                                                    AutoSizeAxes = Axes.Both,
                                                    Children = new Drawable[]
                                                    {
                                                        logoOld = new Sprite
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                        },
                                                        logoBumpContainer = new CircularContainer
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Children = new Drawable[]
                                                            {
                                                                logoBump = new Sprite
                                                                {
                                                                    Anchor = Anchor.Centre,
                                                                    Origin = Anchor.Centre,
                                                                    Alpha = 0.3F,
                                                                },
                                                            }
                                                        },
                                                        logoContainer = new CircularContainer
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Scale = SCALE_ADJUST,
                                                            Masking = true,
                                                            Children = new Drawable[]
                                                            {
                                                                colourAndTriangles = new Container
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Anchor = Anchor.Centre,
                                                                    Origin = Anchor.Centre,
                                                                    Children = new Drawable[]
                                                                    {
                                                                        background = new Box
                                                                        {
                                                                            RelativeSizeAxes = Axes.Both,
                                                                            Colour = ColourInfo.GradientVertical(Color4Extensions.FromHex(@"ff66ab"), Color4Extensions.FromHex(@"cc5289")),
                                                                        },
                                                                        triangles = new TrianglesV2
                                                                        {
                                                                            Anchor = Anchor.Centre,
                                                                            Origin = Anchor.Centre,
                                                                            Thickness = 0.009f,
                                                                            ScaleAdjust = 3,
                                                                            SpawnRatio = 1.4f,
                                                                            Colour = ColourInfo.GradientVertical(Color4Extensions.FromHex(@"ff66ab"), Color4Extensions.FromHex(@"b6346f")),
                                                                            RelativeSizeAxes = Axes.Both,
                                                                        },
                                                                        trianglesOld = new Triangles
                                                                        {
                                                                            Alpha = 0,
                                                                            TriangleScale = 4,
                                                                            ColourLight = Color4Extensions.FromHex(@"ff7db7"),
                                                                            ColourDark = Color4Extensions.FromHex(@"de5b95"),
                                                                            RelativeSizeAxes = Axes.Both,
                                                                        },
                                                                    }
                                                                },
                                                                flashLayer = new Box
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Blending = BlendingParameters.Additive,
                                                                    Colour = Color4.White,
                                                                    Alpha = 0,
                                                                },
                                                            },
                                                        },
                                                        logo = new Sprite
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                        },
                                                    }
                                                },
                                                impactContainer = new CircularContainer
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Alpha = 0,
                                                    BorderColour = Color4.White,
                                                    RelativeSizeAxes = Axes.Both,
                                                    BorderThickness = 10,
                                                    Masking = true,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            AlwaysPresent = true,
                                                            Alpha = 0,
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        public Container LogoElements { get; private set; }

        /// <summary>
        /// Schedule a new external animation. Handled queueing and finishing previous animations in a sane way.
        /// </summary>
        /// <param name="action">The animation to be performed</param>
        /// <param name="waitForPrevious">If true, the new animation is delayed until all previous transforms finish. If false, existing transformed are cleared.</param>
        public void AppendAnimatingAction(Action action, bool waitForPrevious)
        {
            void runnableAction()
            {
                if (waitForPrevious)
                    this.DelayUntilTransformsFinished().Schedule(action);
                else
                {
                    ClearTransforms();
                    action();
                }
            }

            if (IsLoaded)
                runnableAction();
            else
                Schedule(runnableAction);
        }
        private TextureStore textures;
        [BackgroundDependencyLoader]
        private void load(TextureStore textures, AudioManager audio, OsuConfigManager config)
        {
            this.textures = textures;
            logoType = config.GetBindable<LogoType>(OsuSetting.LogoType);
            sampleClick = audio.Samples.Get(@"Menu/osu-logo-select");

            SampleBeat = audio.Samples.Get(@"Menu/osu-logo-heartbeat");
            SampleDownbeat = audio.Samples.Get(@"Menu/osu-logo-downbeat");

            logo.Texture = textures.Get(@"Menu/logo/latest");
            updateType();
        }
        private void updateType()
        {
            switch (logoType.Value)
            {
                case LogoType.Lazer:
                    {
                        logo.Texture = textures.Get(@"Menu/logo/latest");
                        ripple.Texture = textures.Get(@"Menu/logo/latest");
                        background.Colour = ColourInfo.GradientVertical(Color4Extensions.FromHex(@"ff66ab"), Color4Extensions.FromHex(@"cc5289"));
                        visualizer.Scale = new Vector2(1);
                        visualizerOld.Scale = new Vector2(0);
                        trianglesOld.Alpha = 0;
                        triangles.Alpha = 1;
                        logo.Alpha = 1;
                        background.Alpha = 1;
                        bumpType = 2;
                        break;
                    }
                case LogoType.Classic0:
                    {
                        logoOld.Texture = textures.Get(@"Menu/logo/0");
                        logoBump.Texture = textures.Get(@"Menu/logo/0");
                        ripple.Texture = textures.Get(@"Menu/logo-ripple-0");
                        triangles.Alpha = 0;
                        trianglesOld.Alpha = 0;
                        background.Alpha = 0;
                        logo.Alpha = 0;
                        flashLayer.Scale = new Vector2(0);
                        visualizer.Scale = new Vector2(0);
                        visualizerOld.Scale = new Vector2(0);
                        bumpType = 0;
                        //legacyRipple = true; // NETI-TODO: implement this properly before it gets enabled
                        break;
                    }
                case LogoType.Classic1:
                    {
                        logoOld.Texture = textures.Get(@"Menu/logo/1");
                        logoBump.Texture = textures.Get(@"Menu/logo/1");
                        ripple.Texture = textures.Get(@"Menu/logo-ripple");
                        triangles.Alpha = 0;
                        trianglesOld.Alpha = 0;
                        background.Alpha = 0;
                        logo.Alpha = 0;
                        visualizer.Scale = new Vector2(0);
                        visualizerOld.Scale = new Vector2(0);
                        bumpType = 0;
                        //legacyRipple = true; // NETI-TODO: implement this properly before it gets enabled
                        break;
                    }
                case LogoType.Classic2:
                    {
                        logoOld.Texture = textures.Get(@"Menu/logo/2");
                        logoBump.Texture = textures.Get(@"Menu/logo/2");
                        ripple.Texture = textures.Get(@"Menu/logo-ripple");
                        triangles.Alpha = 0;
                        trianglesOld.Alpha = 0;
                        background.Alpha = 0;
                        logo.Alpha = 0;
                        //visualizer.Scale = new Vector2(0);
                        bumpType = 1;
                        //legacyRipple = true; // NETI-TODO: implement this properly before it gets enabled
                        legacyFlash = true;
                        visualizer.Scale = new Vector2(0);
                        visualizerOld.Scale = new Vector2(1);
                        break;
                    }
                case LogoType.Stable:
                    {
                        logoOld.Texture = textures.Get(@"Menu/logo/3");
                        logoBump.Texture = textures.Get(@"Menu/logo/3");
                        ripple.Texture = textures.Get(@"Menu/logo/3");
                        triangles.Alpha = 0;
                        trianglesOld.Alpha = 0;
                        background.Alpha = 0;
                        logo.Alpha = 0;
                        visualizer.Scale = new Vector2(1);
                        visualizerOld.Scale = new Vector2(0);
                        legacyFlash = true;
                        bumpType = 1;
                        break;
                    }
                case LogoType.Lazer1:
                    {
                        logo.Texture = textures.Get(@"Menu/logo/4");
                        ripple.Texture = textures.Get(@"Menu/logo/4");
                        trianglesOld.Alpha = 1;
                        triangles.Alpha = 0;
                        logo.Alpha = 1;
                        background.Alpha = 1;
                        background.Colour = Color4Extensions.FromHex(@"e967a1");
                        visualizer.Scale = new Vector2(1);
                        visualizerOld.Scale = new Vector2(0);
                        legacyFlash = true;
                        bumpType = 2;
                        break;
                    }
                case LogoType.Lazer2:
                    {
                        logo.Texture = textures.Get(@"Menu/logo/5");
                        ripple.Texture = textures.Get(@"Menu/logo/5");
                        visualizer.Scale = new Vector2(1);
                        visualizerOld.Scale = new Vector2(0);
                        background.Colour = ColourInfo.GradientVertical(Color4Extensions.FromHex(@"ff66ab"), Color4Extensions.FromHex(@"cc5289"));
                        trianglesOld.Alpha = 0;
                        triangles.Alpha = 1;
                        logo.Alpha = 1;
                        background.Alpha = 1;
                        bumpType = 2;
                        break;
                    }
                case LogoType.Lazer3:
                    {
                        logo.Texture = textures.Get(@"Menu/logo/6");
                        ripple.Texture = textures.Get(@"Menu/logo/6");
                        visualizer.Scale = new Vector2(1);
                        visualizerOld.Scale = new Vector2(0);
                        background.Colour = ColourInfo.GradientVertical(Color4Extensions.FromHex(@"ff66ab"), Color4Extensions.FromHex(@"cc5289"));
                        trianglesOld.Alpha = 0;
                        triangles.Alpha = 1;
                        logo.Alpha = 1;
                        background.Alpha = 1;
                        bumpType = 2;
                        break;
                    }
            }
        }

        private int lastBeatIndex;

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, ChannelAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            updateType();

            lastBeatIndex = beatIndex;

            double beatLength = timingPoint.BeatLength;

            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            if (beatIndex < 0) return;

            if (Action != null && IsHovered)
            {
                this.Delay(early_activation).Schedule(() =>
                {
                    if (beatIndex % timingPoint.TimeSignature.Numerator == 0)
                    {
                        SampleDownbeat?.Play();
                    }
                    else
                    {
                        var channel = SampleBeat.GetChannel();

                        channel.Frequency.Value = 1 - BeatSampleVariance / 2 + RNG.NextDouble(BeatSampleVariance);
                        channel.Play();
                    }
                });
            }

            if (bumpType == 0)
            {
                logoOld
                    .ScaleTo(1 + 0.05f * amplitudeAdjust, 0, Easing.Out).Then()
                    .ScaleTo(1, beatLength, Easing.Out);
                logoBump
                    .ScaleTo(1 + 0.05f * amplitudeAdjust, 0, Easing.Out).Then()
                    .ScaleTo(1.1f, beatLength, Easing.Out);
                logoBumpContainer
                    .FadeOutFromOne(beatLength, Easing.Out);
            }
            else if (bumpType == 1)
            {
                logoOld
                    .ScaleTo(1 - 0.03f, beatLength / 30).Then()
                    .ScaleTo(1, beatLength - beatLength / 30);
                logoBump
                    .ScaleTo(1 + 0.03f, beatLength / 30).Then()
                    .ScaleTo(1f, beatLength - beatLength / 30);
                logoBump.Alpha = 0.15F;
            }
            else
            {
                logoBeatContainer
                    .ScaleTo(1 - 0.02f * amplitudeAdjust, early_activation, Easing.Out).Then()
                    .ScaleTo(1, beatLength * 2, Easing.OutQuint);
            }

            ripple.ClearTransforms();
            if (!legacyRipple)
            {
                ripple
                    .ScaleTo(logoAmplitudeContainer.Scale)
                    .ScaleTo(logoAmplitudeContainer.Scale * (1 + 0.04f * amplitudeAdjust), beatLength, Easing.OutQuint)
                    .FadeTo(0.15f * amplitudeAdjust).FadeOut(beatLength, Easing.OutQuint);
            }
            else
            {
                ripple
                    .ScaleTo(1)
                    .ScaleTo(1.4F, beatLength*2, Easing.OutQuint)
                    .FadeOutFromOne(beatLength*2, Easing.OutQuint);
            }


            if (effectPoint.KiaiMode && flashLayer.Alpha < 0.4f && bumpType != 0)
            {
                flashLayer.ClearTransforms();
                if (!legacyFlash)
                {
                    flashLayer
                        .FadeTo(0.2f * amplitudeAdjust, early_activation, Easing.Out).Then()
                        .FadeOut(beatLength);
                }
                else
                {
                    flashLayer
                        .FadeTo(0, early_activation).Then()
                        .FadeTo(0.05f * amplitudeAdjust, beatLength);
                }

                visualizer.ClearTransforms();
                visualizer
                    .FadeTo(visualizer_default_alpha * 1.8f * amplitudeAdjust, early_activation, Easing.Out).Then()
                    .FadeTo(visualizer_default_alpha, beatLength);
            }
            else
                flashLayer.FadeTo(0, beatLength);

            this.Delay(early_activation).Schedule(() =>
            {
                triangles.Velocity += amplitudeAdjust * (effectPoint.KiaiMode ? 6 : 3);
            });
        }

        public void PlayIntro()
        {
            const double length = 3150;
            const double fade = 200;

            logoHoverContainer.FadeOut().Delay(length).FadeIn(fade);
            intro.Show();
            intro.Start(length);
            intro.Delay(length + fade).FadeOut();
        }

        [Resolved]
        private MusicController musicController { get; set; }

        protected override void Update()
        {
            base.Update();

            const float scale_adjust_cutoff = 0.4f;

            if (bumpType != 0)
            {
                if (musicController.CurrentTrack.IsRunning)
                {
                    float maxAmplitude = lastBeatIndex >= 0 ? musicController.CurrentTrack.CurrentAmplitudes.Maximum : 0;
                    logoAmplitudeContainer.Scale = new Vector2((float)Interpolation.Damp(logoAmplitudeContainer.Scale.X, 1 - Math.Max(0, maxAmplitude - scale_adjust_cutoff) * 0.04f, 0.9f, Time.Elapsed));

                    triangles.Velocity = (float)Interpolation.Damp(triangles.Velocity, triangles_paused_velocity * (IsKiaiTime ? 4 : 2), 0.995f, Time.Elapsed);
                }
                else
                {
                    triangles.Velocity = (float)Interpolation.Damp(triangles.Velocity, triangles_paused_velocity, 0.9f, Time.Elapsed);
                }
            }
        }

        public override bool HandlePositionalInput => base.HandlePositionalInput && Alpha > 0.2f;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button != MouseButton.Left) return true;

            logoBounceContainer.ScaleTo(0.9f, 1000, Easing.Out);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            // HORRIBLE HACK
            // This is here so that on mobile, the logo can correctly progress from main menu to song select v2 when held.
            // Once the temporary solution of holding the logo to access song select v2 is removed, this should be too.
            // Without this, the long-press-to-right-click flow intercepts the hold and converts it to a right click which would not trigger the logo
            // and therefore not progress to song select.
            if (e.Button == MouseButton.Right && e.CurrentState.Mouse.LastSource is ISourcedFromTouch)
                triggerClick();
            // END OF HORRIBLE HACK

            if (e.Button != MouseButton.Left) return;

            logoBounceContainer.ScaleTo(1f, 500, Easing.OutElastic);
        }

        protected override bool OnClick(ClickEvent e)
        {
            triggerClick();
            return true;
        }

        private void triggerClick()
        {
            flashLayer.ClearTransforms();
            flashLayer.Alpha = 0.4f;
            flashLayer.FadeOut(1500, Easing.OutExpo);

            if (Action?.Invoke() == true)
            {
                StopSamplePlayback();
                sampleClickChannel = sampleClick.GetChannel();
                sampleClickChannel.Play();
            }
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Action != null)
                logoHoverContainer.ScaleTo(1.1f, 500, Easing.OutElastic);

            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            logoHoverContainer.ScaleTo(1, 500, Easing.OutElastic);
        }

        public void Impact()
        {
            impactContainer.FadeOutFromOne(250, Easing.In);
            impactContainer.ScaleTo(SCALE_ADJUST);
            impactContainer.ScaleTo(1.12f, 250);
        }

        public override bool DragBlocksClick => false;

        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
        {
            Vector2 change = e.MousePosition - e.MouseDownPosition;

            // Diminish the drag distance as we go further to simulate "rubber band" feeling.
            change *= change.Length <= 0 ? 0 : MathF.Pow(change.Length, 0.6f) / change.Length;

            logoBounceContainer.MoveTo(change);
        }

        protected override void OnDragEnd(DragEndEvent e)
        {
            logoBounceContainer.MoveTo(Vector2.Zero, 800, Easing.OutElastic);
            base.OnDragEnd(e);
        }

        private Container defaultProxyTarget;
        private Container currentProxyTarget;
        private Drawable proxy;

        public void StopSamplePlayback() => sampleClickChannel?.Stop();

        public IDisposable ProxyToContainer(Container c)
        {
            if (currentProxyTarget != null)
                throw new InvalidOperationException("Previous proxy usage was not returned");

            if (defaultProxyTarget == null)
                throw new InvalidOperationException($"{nameof(SetupDefaultContainer)} must be called first");

            currentProxyTarget = c;

            defaultProxyTarget.Remove(proxy, false);
            currentProxyTarget.Add(proxy);

            return new InvokeOnDisposal(returnProxy);

            void returnProxy()
            {
                Debug.Assert(currentProxyTarget != null);
                Debug.Assert(defaultProxyTarget != null);

                currentProxyTarget.Remove(proxy, false);
                currentProxyTarget = null;

                defaultProxyTarget.Add(proxy);
            }
        }

        public void SetupDefaultContainer(Container container)
        {
            defaultProxyTarget = container;

            defaultProxyTarget.Add(this);
            defaultProxyTarget.Add(proxy = CreateProxy());
        }

        public void ChangeAnchor(Anchor anchor)
        {
            var previousAnchor = AnchorPosition;
            Anchor = anchor;
            Position -= AnchorPosition - previousAnchor;
        }
    }
}
