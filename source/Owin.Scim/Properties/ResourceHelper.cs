﻿namespace Owin.Scim.Properties
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class ResourceHelper
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Owin.Scim.Properties.Resources", typeof(Resources).GetTypeInfo().Assembly);
        
        /// <summary>
        /// The type of the property at path '{0}' could not be determined.
        /// </summary>
        internal static string FormatCannotDeterminePropertyType(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CannotDeterminePropertyType"), p0);
        }
        
        /// <summary>
        /// The property at '{0}' could not be read.
        /// </summary>
        internal static string FormatCannotReadProperty(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CannotReadProperty"), p0);
        }

        /// <summary>
        /// The property at path '{0}' could not be updated.
        /// </summary>
        internal static string FormatCannotUpdateProperty(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CannotUpdateProperty"), p0);
        }
        
        /// <summary>
        /// The key '{0}' was not found.
        /// </summary>
        internal static string FormatDictionaryKeyNotFound(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("DictionaryKeyNotFound"), p0);
        }
        
        /// <summary>
        /// For operation '{0}' on array property at path '{1}', the index is larger than the array size.
        /// </summary>
        internal static string FormatInvalidIndexForArrayProperty(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidIndexForArrayProperty"), p0, p1);
        }
        
        /// <summary>
        /// The type '{0}' was malformed and could not be parsed.
        /// </summary>
        internal static string FormatInvalidJsonPatchDocument(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidJsonPatchDocument"), p0);
        }
        
        /// <summary>
        /// For operation '{0}', the provided path is invalid for array property at path '{1}'.
        /// </summary>
        internal static string FormatInvalidPathForArrayProperty(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidPathForArrayProperty"), p0, p1);
        }
        
        /// <summary>
        /// The provided string '{0}' is an invalid path.
        /// </summary>
        internal static string FormatInvalidValueForPath(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidValueForPath"), p0);
        }
        
        /// <summary>
        /// The value '{0}' is invalid for property at path '{1}'.
        /// </summary>
        internal static string FormatInvalidValueForProperty(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("InvalidValueForProperty"), p0, p1);
        }

        /// <summary>
        /// For operation '{0}' on array property at path '{1}', the index is negative.
        /// </summary>
        internal static string FormatNegativeIndexForArrayProperty(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("NegativeIndexForArrayProperty"), p0, p1);
        }
        
        /// <summary>
        /// '{0}' must be of type '{1}'.
        /// </summary>
        internal static string FormatParameterMustMatchType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ParameterMustMatchType"), p0, p1);
        }

        /// <summary>
        /// The property at path '{0}' could not be added.
        /// </summary>
        internal static string FormatPropertyCannotBeAdded(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyCannotBeAdded"), p0);
        }
        
        /// <summary>
        /// The property at path '{0}' could not be removed.
        /// </summary>
        internal static string FormatPropertyCannotBeRemoved(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyCannotBeRemoved"), p0);
        }

        /// <summary>
        /// Property does not exist at path '{0}'.
        /// </summary>
        internal static string FormatPropertyDoesNotExist(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("PropertyDoesNotExist"), p0);
        }
        
        /// <summary>
        /// The test operation is not supported.
        /// </summary>
        internal static string FormatTestOperationNotSupported()
        {
            return GetString("TestOperationNotSupported");
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}