﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroInstall.Publish.WinForms.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ZeroInstall.Publish.WinForms.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter the GnuPG passphrase for {0}.
        /// </summary>
        internal static string AskForPassphrase {
            get {
                return ResourceManager.GetString("AskForPassphrase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter GnuPG passphrase.
        /// </summary>
        internal static string AskForPassphraseTitle {
            get {
                return ResourceManager.GetString("AskForPassphraseTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (auto detect).
        /// </summary>
        internal static string AutoDetect {
            get {
                return ResourceManager.GetString("AutoDetect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Discared
        ///Discard unsaved changes.
        /// </summary>
        internal static string DiscardChanges {
            get {
                return ResourceManager.GetString("DiscardChanges", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Downloading image for preview....
        /// </summary>
        internal static string Downloading_image_for_preview {
            get {
                return ResourceManager.GetString("Downloading_image_for_preview", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The feed has not the right format..
        /// </summary>
        internal static string FeedNotValid {
            get {
                return ResourceManager.GetString("FeedNotValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to a full description, which can be several paragraphs long.
        /// </summary>
        internal static string HintTextMultiline {
            get {
                return ResourceManager.GetString("HintTextMultiline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to a short one-line description.
        /// </summary>
        internal static string HintTextSinlgeLine {
            get {
                return ResourceManager.GetString("HintTextSinlgeLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The image format is not supported by ZeroInstall..
        /// </summary>
        internal static string ImageFormatNotSupported {
            get {
                return ResourceManager.GetString("ImageFormatNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Save
        ///Save changes.
        /// </summary>
        internal static string SaveChanges {
            get {
                return ResourceManager.GetString("SaveChanges", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attention, unsaved changes will be deleted.
        ///Do you like to save the changes?.
        /// </summary>
        internal static string SaveQuestion {
            get {
                return ResourceManager.GetString("SaveQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wrong passphrase entered.
        ///Please retry entering the GnuPG passphrase for {0}.
        /// </summary>
        internal static string WrongPassphrase {
            get {
                return ResourceManager.GetString("WrongPassphrase", resourceCulture);
            }
        }
    }
}
