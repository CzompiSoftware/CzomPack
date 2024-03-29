﻿using System.Reflection;

namespace CzomPack.Extensions
{
    public static class AssemblyExtensions
    {

        public static bool IsDebugBuild(this Assembly assembly) => assembly.GetBuildConfiguration().ToLower() == "release" || assembly.GetBuildConfiguration().ToLower() == "stable";

        public static string GetBuildConfiguration(this Assembly assembly) => assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration;

    }
}
