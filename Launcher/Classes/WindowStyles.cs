// Project: Launcher
// File: WindowStyles.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace Launcher.Classes
{
    [Flags]
    public enum WindowStyles : long
    {
        Border = 0x800000L,
        Caption = 0xc00000L,
        Child = 0x40000000L,
        ChildWindow = 0x40000000L,
        ClipChildren = 0x2000000L,
        ClipSiblings = 0x4000000L,
        Disabled = 0x8000000L,
        DlgFrame = 0x400000L,
        ExAcceptFiles = 0x10L,
        ExAppWindow = 0x40000L,
        ExClientEdge = 0x200L,
        ExComposited = 0x2000000L,
        ExContexthelp = 0x400L,
        ExControlParent = 0x10000L,
        ExDlgModalFrame = 1L,
        ExLayered = 0x80000L,
        ExLayoutRtl = 0x400000L,
        ExLeft = 0L,
        ExLeftScrollbar = 0x4000L,
        ExLtrReading = 0L,
        ExMdiChild = 0x40L,
        ExNoActivate = 0x4000000L,
        ExNoInheritLayout = 0x100000L,
        ExNoParentNotify = 4L,
        ExOverlappedWindow = 0x300L,
        ExPaletteWindow = 0x188L,
        ExRight = 0x1000L,
        ExRightScrollbar = 0L,
        ExRtlReading = 0x2000L,
        ExStaticEdge = 0x20000L,
        ExToolWindow = 0x80L,
        ExTopMost = 8L,
        ExTransparent = 0x20L,
        ExWindowEdge = 0x100L,
        Group = 0x20000L,
        HScroll = 0x100000L,
        Iconic = 0x20000000L,
        Maximize = 0x1000000L,
        MaximizeBox = 0x10000L,
        Minimize = 0x20000000L,
        MinimizeBox = 0x20000L,
        OverLapped = 0L,
        OverLappedWindow = 0xcf0000L,
        Popup = 0x80000000L,
        PopupWindow = 0x80880000L,
        SizeBox = 0x40000L,
        SysMenu = 0x80000L,
        TabStop = 0x10000L,
        ThickFrame = 0x40000L,
        Tiled = 0L,
        TiledWindow = 0xcf0000L,
        Visible = 0x10000000L,
        VScroll = 0x200000L
    }
}