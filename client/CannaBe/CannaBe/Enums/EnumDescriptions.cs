﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CannaBe.Enums
{
    [AttributeUsage(AttributeTargets.All)]
    public class EnumDescriptions : DescriptionAttribute
    {

        public EnumDescriptions(string q1, string q2)
        {
            this.q1 = q1;
            this.q2 = q2;
        }

        public string q1 { get; set; }
        public string q2 { get; set; }
    }

    public static class Extensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
