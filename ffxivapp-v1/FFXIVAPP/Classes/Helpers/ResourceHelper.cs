// FFXIVAPP
// ResourceHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Linq.Expressions;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class ResourceHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public static string StringR(string key)
        {
            return (string) MainWindow.View.FindResource(key);
        }

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            var expressionBody = (MemberExpression) memberExpression.Body;
            return expressionBody.Member.Name;
        }
    }
}