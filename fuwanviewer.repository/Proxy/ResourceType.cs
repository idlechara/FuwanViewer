using System;
using System.Collections.Generic;

namespace FuwanViewer.Repository.Proxy
{
    internal enum ResourceType
    {
        Cover,
        MainArt,
        Screenshoot1,
        Screenshoot2,
        Screenshoot3,
        Screenshoot4,
        Screenshoot5,
        Screenshoot6,
        Screenshoot7,
        Screenshoot8
    }

    internal static class ResourceTypeFactory
    {
        public static List<ResourceType> GetScreenshootResources()
        {
            return new List<ResourceType>()
            {
                ResourceType.Screenshoot1,
                ResourceType.Screenshoot2,
                ResourceType.Screenshoot3,
                ResourceType.Screenshoot4,
                ResourceType.Screenshoot5,
                ResourceType.Screenshoot6,
                ResourceType.Screenshoot7,
                ResourceType.Screenshoot8,
            };
        }
    }

    internal static class EnumExtensions
    {
        internal static bool IsScreenshootResource(this Enum value)
        {
            if (value.GetType() != typeof(ResourceType))
                return false;

            ResourceType resource = (ResourceType)value;
            return ResourceTypeFactory.GetScreenshootResources().Contains(resource);
        }
    }
}
