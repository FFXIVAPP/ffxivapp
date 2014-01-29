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
                        var useJob = Settings.Default.PCShowJob;
                        if (Settings.Default.PCShowJob)
                        {
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
                        }
                        if (!useJob)
                        {
                            var imageSource = actorEntity.HPCurrent > 0 ? RadarIconHelper.Player : RadarIconHelper.Skull;
                            drawingContext.DrawImage(imageSource, new Rect(new Point(screen.X, screen.Y), new Size(16, 16)));
                        }
                        if (Settings.Default.PCShowName || Settings.Default.PCShowHPPercent)
                        {
                            var label = new FormattedText(sb.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12, Brushes.White);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger,ex.Message);
                    }
                }
            }

            #endregion

            #region Resolve Monsters

            if (Settings.Default.MonsterShow)
            {
                foreach (var actorEntity in monsterEntites)
                {
                    sb.Clear();
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
                        if (Settings.Default.MonsterShowName || Settings.Default.MonsterShowHPPercent)
                        {
                            var label = new FormattedText(sb.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12, brush);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, ex.Message);
                    }
                }
            }

            #endregion

            #region Resolve NPCs

            if (Settings.Default.NPCShow)
            {
                foreach (var actorEntity in npcEntites)
                {
                    switch (actorEntity.Type)
                    {
                        case Actor.Type.NPC:
                            break;
                        default:
                            continue;
                    }
                    sb.Clear();
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
                            var label = new FormattedText(sb.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12, brush);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, ex.Message);
                    }
                }
            }

            #endregion

            #region Resolve Gathering

            if (Settings.Default.GatheringShow)
            {
                foreach (var actorEntity in npcEntites)
                {
                    switch (actorEntity.Type)
                    {
                        case Actor.Type.Gathering:
                            break;
                        default:
                            continue;
                    }
                    sb.Clear();
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
                            var label = new FormattedText(sb.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12, brush);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, ex.Message);
                    }
                }
            }

            #endregion

            #region Resolve Other

            if (Settings.Default.OtherShow)
            {
                foreach (var actorEntity in npcEntites)
                {
                    switch (actorEntity.Type)
                    {
                        case Actor.Type.NPC:
                        case Actor.Type.Gathering:
                            continue;
                    }
                    sb.Clear();
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
                        if (Settings.Default.OtherShowName)
                        {
                            sb.Append(actorEntity.Name);
                        }
                        if (Settings.Default.OtherShowHPPercent)
                        {
                            sb.AppendFormat(" {0:P0}", actorEntity.HPPercent);
                        }
                        if (Settings.Default.OtherShowName || Settings.Default.OtherShowHPPercent)
                        {
                            var label = new FormattedText(sb.ToString(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Verdana"), 12, brush);
                            drawingContext.DrawText(label, new Point(screen.X + 20, screen.Y));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(Logger, ex.Message);
                    }
                }
            }

            #endregion
        }
    }
}
