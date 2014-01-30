// FFXIVAPP.Plugin.Radar
// Radar.xaml.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Radar.Helpers;
using FFXIVAPP.Plugin.Radar.Properties;
using FFXIVAPP.Plugin.Radar.ViewModels;
using NLog;

namespace FFXIVAPP.Plugin.Radar.Controls
{
    /// <summary>
    ///     Interaction logic for Radar.xaml
    /// </summary>
    public partial class Radar
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Radar Declarations

        public bool IsRendered { get; set; }

        #endregion

        #region DrawContext Declaratoins

        private CultureInfo _cultureInfo = CultureInfo.InvariantCulture;
        private FlowDirection _flowDirection = FlowDirection.LeftToRight;
        private Typeface _typeface = new Typeface("Verdana");

        #endregion

        public Radar View;

        public Radar()
        {
            View = this;
            InitializeComponent();
            if (IsRendered)
            {
                return;
            }
            IsRendered = true;
        }

        public void Refresh()
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var user = XIVInfoViewModel.Instance.CurrentUser;

            var origin = new Coordinate
            {
                X = (float) (ActualWidth / 2),
                Y = (float) (ActualHeight / 2)
            };

            var scale = ((float) (ActualHeight / 2.0f) / 125.0f);
            var angle = Math.Abs(user.Heading * (180 / Math.PI) - 180);

            if (Settings.Default.RadarCompassMode)
            {
                var drawingGroup = new DrawingGroup
                {
                    Transform = new RotateTransform
                    {
                        Angle = angle,
                        CenterX = origin.X,
                        CenterY = origin.Y
                    }
                };
                drawingGroup.Children.Add(new ImageDrawing(RadarIconHelper.RadarHeading, new Rect(origin.X - 64, origin.Y - 128, 128, 128)));
                drawingContext.DrawDrawing(drawingGroup);
            }
            else
            {
                drawingContext.DrawImage(RadarIconHelper.RadarHeading, new Rect(new Point(origin.X - 64, origin.Y - 128), new Size(128, 128)));
            }

            var sb = new StringBuilder();

            var npcEntites = new List<ActorEntity>(XIVInfoViewModel.Instance.CurrentNPCs.ToList());
            var monsterEntites = new List<ActorEntity>(XIVInfoViewModel.Instance.CurrentMonsters.ToList());
            var pcEntites = new List<ActorEntity>(XIVInfoViewModel.Instance.CurrentPCs.ToList());

            #region Resolve PCs

            if (Settings.Default.PCShow)
            {
                foreach (var actorEntity in pcEntites)
                {
                    sb.Clear();
                    var opacityLevel = (actorEntity.Coordinate.Z / XIVInfoViewModel.Instance.CurrentUser.Coordinate.Z);
                    var fsModifier = ResolveFontSize(opacityLevel);
                    opacityLevel = opacityLevel < 0.5 ? 0.5 : opacityLevel > 1 ? 1 : opacityLevel;
                    drawingContext.PushOpacity(opacityLevel);
                    try
                    {
                        if (!actorEntity.IsValid || user == null)
                        {
                            continue;
                        }
                        if (actorEntity.ID == user.ID)
                        {
                            continue;
                        }
                        Coordinate screen;
                        if (Settings.Default.RadarCompassMode)
                        {
                            var coord = user.Coordinate.Subtract(actorEntity.Coordinate)
                                            .Scale(scale);
                            screen = new Coordinate(-coord.X, 0, -coord.Y).Add(origin);
                        }
                        else
                        {
                            screen = user.Coordinate.Subtract(actorEntity.Coordinate)
                                         .Rotate2D(user.Heading)
                                         .Scale(scale)
                                         .Add(origin);
                        }
                        screen = screen.Add(-8, -8, 0);
                        if (Settings.Default.PCShowName)
                        {
                            sb.Append(actorEntity.Name);
                        }
                        if (Settings.Default.PCShowHPPercent)
                        {
                            sb.AppendFormat(" {0:P0}", actorEntity.HPPercent);
                        }
                        if (Settings.Default.PCShowDistance)
                        {
                            sb.AppendFormat(" {0:N2}", XIVInfoViewModel.Instance.CurrentUser.GetDistanceTo(actorEntity));
                        }
                        var useJob = Settings.Default.PCShowJob;
                        if (Settings.Default.PCShowJob)
                        {
                            #region Get Job Icons

                            switch (actorEntity.Job)
                            {
                                case Actor.Job.ACN:
                                    drawingContext.DrawImage(RadarIconHelper.Arcanist, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.ALC:
                                    drawingContext.DrawImage(RadarIconHelper.Alchemist, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.ARC:
                                    drawingContext.DrawImage(RadarIconHelper.Archer, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.ARM:
                                    drawingContext.DrawImage(RadarIconHelper.Armorer, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.BLM:
                                    drawingContext.DrawImage(RadarIconHelper.Blackmage, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.BRD:
                                    drawingContext.DrawImage(RadarIconHelper.Bard, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.BSM:
                                    drawingContext.DrawImage(RadarIconHelper.Blacksmith, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.BTN:
                                    drawingContext.DrawImage(RadarIconHelper.Botanist, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.CNJ:
                                    drawingContext.DrawImage(RadarIconHelper.Conjurer, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.CPT:
                                    drawingContext.DrawImage(RadarIconHelper.Carpenter, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.CUL:
                                    drawingContext.DrawImage(RadarIconHelper.Culinarian, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.Chocobo:
                                    drawingContext.DrawImage(RadarIconHelper.Chocobo, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.DRG:
                                    drawingContext.DrawImage(RadarIconHelper.Dragoon, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.FSH:
                                    drawingContext.DrawImage(RadarIconHelper.Fisher, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.GLD:
                                    drawingContext.DrawImage(RadarIconHelper.Gladiator, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.GSM:
                                    drawingContext.DrawImage(RadarIconHelper.Goldsmith, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.LNC:
                                    drawingContext.DrawImage(RadarIconHelper.Leatherworker, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.LTW:
                                    drawingContext.DrawImage(RadarIconHelper.Leatherworker, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.MIN:
                                    drawingContext.DrawImage(RadarIconHelper.Miner, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.MNK:
                                    drawingContext.DrawImage(RadarIconHelper.Monk, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.MRD:
                                    drawingContext.DrawImage(RadarIconHelper.Marauder, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.PGL:
                                    drawingContext.DrawImage(RadarIconHelper.Pugilist, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.PLD:
                                    drawingContext.DrawImage(RadarIconHelper.Paladin, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.Pet:
                                    drawingContext.DrawImage(RadarIconHelper.Sheep, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.SCH:
                                    drawingContext.DrawImage(RadarIconHelper.Scholar, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.Unknown:
                                    useJob = false;
                                    break;
                                case Actor.Job.WAR:
                                    drawingContext.DrawImage(RadarIconHelper.Warrior, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.WHM:
                                    drawingContext.DrawImage(RadarIconHelper.Whitemage, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                                case Actor.Job.WVR:
                                    drawingContext.DrawImage(RadarIconHelper.Weaver, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    break;
                            }

                            #endregion
                        }
                        if (!useJob)
                        {
                            var imageSource = actorEntity.HPCurrent > 0 ? RadarIconHelper.Player : RadarIconHelper.Skull;
                            drawingContext.DrawImage(imageSource, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                        }
                        if (Settings.Default.PCShowName || Settings.Default.PCShowHPPercent)
                        {
                            var label = new FormattedText(sb.ToString(), _cultureInfo, _flowDirection, _typeface, 12 + fsModifier, Brushes.White);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, ex.Message);
                    }
                    drawingContext.Pop();
                }
            }

            #endregion

            #region Resolve Monsters

            if (Settings.Default.MonsterShow)
            {
                foreach (var actorEntity in monsterEntites)
                {
                    sb.Clear();
                    var opacityLevel = (actorEntity.Coordinate.Z / XIVInfoViewModel.Instance.CurrentUser.Coordinate.Z);
                    var fsModifier = ResolveFontSize(opacityLevel);
                    opacityLevel = opacityLevel < 0.5 ? 0.5 : opacityLevel > 1 ? 1 : opacityLevel;
                    drawingContext.PushOpacity(opacityLevel);
                    try
                    {
                        if (!actorEntity.IsValid || user == null)
                        {
                            continue;
                        }
                        if (actorEntity.ID == user.ID)
                        {
                            continue;
                        }
                        Coordinate screen;
                        if (Settings.Default.RadarCompassMode)
                        {
                            var coord = user.Coordinate.Subtract(actorEntity.Coordinate)
                                            .Scale(scale);
                            screen = new Coordinate(-coord.X, 0, -coord.Y).Add(origin);
                        }
                        else
                        {
                            screen = user.Coordinate.Subtract(actorEntity.Coordinate)
                                         .Rotate2D(user.Heading)
                                         .Scale(scale)
                                         .Add(origin);
                        }
                        screen = screen.Add(-8, -8, 0);
                        ImageSource actorIcon = null;
                        var brush = Brushes.Red;
                        switch (actorEntity.IsFate)
                        {
                            case true:
                                actorIcon = RadarIconHelper.Fate;
                                break;
                            case false:
                                actorIcon = actorEntity.IsClaimed ? RadarIconHelper.MonsterClaimed : RadarIconHelper.Monster;
                                break;
                        }
                        if (actorEntity.HPCurrent > 0)
                        {
                            if (actorIcon != null)
                            {
                                drawingContext.DrawImage(actorIcon, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                            }
                        }
                        else
                        {
                            drawingContext.DrawImage(RadarIconHelper.Skull, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                        }
                        if (Settings.Default.MonsterShowName)
                        {
                            sb.Append(actorEntity.Name);
                        }
                        if (Settings.Default.MonsterShowHPPercent)
                        {
                            sb.AppendFormat(" {0:P0}", actorEntity.HPPercent);
                        }
                        if (Settings.Default.MonsterShowDistance)
                        {
                            sb.AppendFormat(" {0:N2}", XIVInfoViewModel.Instance.CurrentUser.GetDistanceTo(actorEntity));
                        }
                        if (Settings.Default.MonsterShowName || Settings.Default.MonsterShowHPPercent)
                        {
                            var label = new FormattedText(sb.ToString(), _cultureInfo, _flowDirection, _typeface, 12 + fsModifier, brush);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, ex.Message);
                    }
                    drawingContext.Pop();
                }
            }

            #endregion

            #region Resolve NPCs, Gathering & Other

            foreach (var actorEntity in npcEntites)
            {
                switch (actorEntity.Type)
                {
                    case Actor.Type.NPC:

                        #region Resolve NPCs

                        if (Settings.Default.NPCShow)
                        {
                            sb.Clear();
                            var opacityLevel = (actorEntity.Coordinate.Z / XIVInfoViewModel.Instance.CurrentUser.Coordinate.Z);
                            var fsModifier = ResolveFontSize(opacityLevel);
                            opacityLevel = opacityLevel < 0.5 ? 0.5 : opacityLevel > 1 ? 1 : opacityLevel;
                            drawingContext.PushOpacity(opacityLevel);
                            try
                            {
                                if (!actorEntity.IsValid || user == null)
                                {
                                    continue;
                                }
                                if (actorEntity.ID == user.ID)
                                {
                                    continue;
                                }
                                Coordinate screen;
                                if (Settings.Default.RadarCompassMode)
                                {
                                    var coord = user.Coordinate.Subtract(actorEntity.Coordinate)
                                                    .Scale(scale);
                                    screen = new Coordinate(-coord.X, 0, -coord.Y).Add(origin);
                                }
                                else
                                {
                                    screen = user.Coordinate.Subtract(actorEntity.Coordinate)
                                                 .Rotate2D(user.Heading)
                                                 .Scale(scale)
                                                 .Add(origin);
                                }
                                screen = screen.Add(-8, -8, 0);
                                var brush = Brushes.LimeGreen;
                                if (Settings.Default.NPCShowName)
                                {
                                    sb.Append(actorEntity.Name);
                                }
                                if (Settings.Default.NPCShowHPPercent)
                                {
                                    sb.AppendFormat(" {0:P0}", actorEntity.HPPercent);
                                }
                                if (Settings.Default.NPCShowDistance)
                                {
                                    sb.AppendFormat(" {0:N2}", XIVInfoViewModel.Instance.CurrentUser.GetDistanceTo(actorEntity));
                                }
                                var actorIcon = RadarIconHelper.NPC;
                                if (actorEntity.HPCurrent > 0)
                                {
                                    drawingContext.DrawImage(actorIcon, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                }
                                else
                                {
                                    drawingContext.DrawImage(RadarIconHelper.Skull, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                }
                                if (Settings.Default.NPCShowName || Settings.Default.NPCShowHPPercent)
                                {
                                    var label = new FormattedText(sb.ToString(), _cultureInfo, _flowDirection, _typeface, 12 + fsModifier, brush);
                                    drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                                }
                            }
                            catch (Exception ex)
                            {
                                Logging.Log(Logger, ex.Message);
                            }
                            drawingContext.Pop();
                        }

                        #endregion

                        break;
                    case Actor.Type.Gathering:

                        #region Resolve Gathering

                        if (Settings.Default.GatheringShow)
                        {
                            sb.Clear();
                            var opacityLevel = (actorEntity.Coordinate.Z / XIVInfoViewModel.Instance.CurrentUser.Coordinate.Z);
                            var fsModifier = ResolveFontSize(opacityLevel);
                            opacityLevel = opacityLevel < 0.5 ? 0.5 : opacityLevel > 1 ? 1 : opacityLevel;
                            drawingContext.PushOpacity(opacityLevel);
                            try
                            {
                                if (!actorEntity.IsValid || user == null)
                                {
                                    continue;
                                }
                                if (actorEntity.ID == user.ID)
                                {
                                    continue;
                                }
                                Coordinate screen;
                                if (Settings.Default.RadarCompassMode)
                                {
                                    var coord = user.Coordinate.Subtract(actorEntity.Coordinate)
                                                    .Scale(scale);
                                    screen = new Coordinate(-coord.X, 0, -coord.Y).Add(origin);
                                }
                                else
                                {
                                    screen = user.Coordinate.Subtract(actorEntity.Coordinate)
                                                 .Rotate2D(user.Heading)
                                                 .Scale(scale)
                                                 .Add(origin);
                                }
                                screen = screen.Add(-8, -8, 0);
                                var brush = Brushes.Orange;
                                if (Settings.Default.GatheringShowName)
                                {
                                    sb.Append(actorEntity.Name);
                                }
                                if (Settings.Default.GatheringShowHPPercent)
                                {
                                    sb.AppendFormat(" {0:P0}", actorEntity.HPPercent);
                                }
                                if (Settings.Default.GatheringShowDistance)
                                {
                                    sb.AppendFormat(" {0:N2}", XIVInfoViewModel.Instance.CurrentUser.GetDistanceTo(actorEntity));
                                }
                                var actorIcon = RadarIconHelper.Wood;
                                if (actorEntity.HPCurrent > 0)
                                {
                                    drawingContext.DrawImage(actorIcon, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                }
                                else
                                {
                                    drawingContext.DrawImage(RadarIconHelper.Skull, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                }
                                if (Settings.Default.GatheringShowName || Settings.Default.GatheringShowHPPercent)
                                {
                                    var label = new FormattedText(sb.ToString(), _cultureInfo, _flowDirection, _typeface, 12 + fsModifier, brush);
                                    drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                                }
                            }
                            catch (Exception ex)
                            {
                                Logging.Log(Logger, ex.Message);
                            }
                            drawingContext.Pop();
                        }

                        #endregion

                        break;
                    default:

                        #region Resolve Other

                        if (Settings.Default.OtherShow)
                        {
                            sb.Clear();
                            var opacityLevel = (actorEntity.Coordinate.Z / XIVInfoViewModel.Instance.CurrentUser.Coordinate.Z);
                            var fsModifier = ResolveFontSize(opacityLevel);
                            opacityLevel = opacityLevel < 0.5 ? 0.5 : opacityLevel > 1 ? 1 : opacityLevel;
                            drawingContext.PushOpacity(opacityLevel);
                            try
                            {
                                if (!actorEntity.IsValid || user == null)
                                {
                                    continue;
                                }
                                if (actorEntity.ID == user.ID)
                                {
                                    continue;
                                }
                                Coordinate screen;
                                if (Settings.Default.RadarCompassMode)
                                {
                                    var coord = user.Coordinate.Subtract(actorEntity.Coordinate)
                                                    .Scale(scale);
                                    screen = new Coordinate(-coord.X, 0, -coord.Y).Add(origin);
                                }
                                else
                                {
                                    screen = user.Coordinate.Subtract(actorEntity.Coordinate)
                                                 .Rotate2D(user.Heading)
                                                 .Scale(scale);
                                }
                                screen = screen.Add(-8, -8, 0);
                                ImageSource actorIcon;
                                var brush = Brushes.Yellow;
                                switch (actorEntity.Type)
                                {
                                    case Actor.Type.Aetheryte:
                                        actorIcon = RadarIconHelper.Crystal;
                                        break;
                                    case Actor.Type.Minion:
                                        actorIcon = RadarIconHelper.Sheep;
                                        break;
                                    default:
                                        actorIcon = RadarIconHelper.NPC;
                                        break;
                                }
                                if (actorEntity.HPCurrent > 0 || actorEntity.Type == Actor.Type.Aetheryte)
                                {
                                    if (actorIcon != null)
                                    {
                                        drawingContext.DrawImage(actorIcon, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                    }
                                }
                                else
                                {
                                    drawingContext.DrawImage(RadarIconHelper.Skull, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                                }
                                if (Settings.Default.OtherShowName)
                                {
                                    sb.Append(actorEntity.Name);
                                }
                                if (Settings.Default.OtherShowHPPercent)
                                {
                                    sb.AppendFormat(" {0:P0}", actorEntity.HPPercent);
                                }
                                if (Settings.Default.OtherShowDistance)
                                {
                                    sb.AppendFormat(" {0:N2}", XIVInfoViewModel.Instance.CurrentUser.GetDistanceTo(actorEntity));
                                }
                                if (Settings.Default.OtherShowName || Settings.Default.OtherShowHPPercent)
                                {
                                    var label = new FormattedText(sb.ToString(), _cultureInfo, _flowDirection, _typeface, 12 + fsModifier, brush);
                                    drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                                }
                            }
                            catch (Exception ex)
                            {
                                Logging.Log(Logger, ex.Message);
                            }
                            drawingContext.Pop();
                        }

                        #endregion

                        break;
                }
            }

            #endregion
        }

        private double ResolveFontSize(double opacityLevel)
        {
            var difference = opacityLevel - 1;
            double fsModifier;
            if (difference > 0)
            {
                fsModifier = (difference >= 20 ? 20 : difference) / 10;
            }
            else
            {
                difference = Math.Abs(difference);
                fsModifier = -((difference >= 20 ? 20 : difference) / 10);
            }
            return fsModifier;
        }
    }
}
