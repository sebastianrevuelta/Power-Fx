// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.PowerFx.Core.Functions;
using Microsoft.PowerFx.Core.Utils;

namespace Microsoft.PowerFx.Core.Types.Enums
{
    /// <summary>
    /// Static class used to store built in Power Fx enums.
    /// </summary>
    internal sealed class EnumStoreBuilder
    {
        internal static IReadOnlyDictionary<string, DType> DefaultEnums2 { get; } =
            new Dictionary<string, DType>(StringComparer.InvariantCultureIgnoreCase)
            {
                {
                    EnumConstants.BorderStyleEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "None", new EquatableObject("none") }, { "Dashed", new EquatableObject("dashed") }, { "Solid", new EquatableObject("solid") },
                        { "Dotted", new EquatableObject("dotted") }
                    })
                },
                {
                    EnumConstants.ColorEnumString,
                    DType.CreateEnum(DType.Color, ((Dictionary<string, uint>)ColorTable.InvariantNameToHexMap).ToDictionary(kvp => kvp.Key, kvp => new EquatableObject((double)kvp.Value)))
                },
                {
                    EnumConstants.DateTimeFormatEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "LongDate", new EquatableObject("'longdate'") }, { "ShortDate", new EquatableObject("'shortdate'") }, { "LongTime", new EquatableObject("'longtime'") },
                        { "ShortTime", new EquatableObject("'shorttime'") }, { "LongTime24", new EquatableObject("'longtime24'") },
                        { "ShortTime24", new EquatableObject("'shorttime24'") }, { "LongDateTime", new EquatableObject("'longdatetime'") },
                        { "ShortDateTime", new EquatableObject("'shortdatetime'") }, { "LongDateTime24", new EquatableObject("'longdatetime24'") },
                        { "ShortDateTime24", new EquatableObject("'shortdatetime24'") }, { "UTC", new EquatableObject("utc") }
                    })
                },
                {
                    EnumConstants.StartOfWeekEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Sunday", new EquatableObject(1d) }, { "Monday", new EquatableObject(2d) }, { "MondayZero", new EquatableObject(3d) },
                        { "Tuesday", new EquatableObject(12d) }, { "Wednesday", new EquatableObject(13d) }, { "Thursday", new EquatableObject(14d) },
                        { "Friday", new EquatableObject(15d) }, { "Saturday", new EquatableObject(16d) }
                    })
                },
                {
                    EnumConstants.DirectionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Start", new EquatableObject("start") }, { "End", new EquatableObject("end") }
                    })
                },
                {
                    EnumConstants.DisplayModeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Edit", new EquatableObject("edit") }, { "View", new EquatableObject("view") }, { "Disabled", new EquatableObject("disabled") }
                    })
                },
                {
                    EnumConstants.LayoutModeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Manual", new EquatableObject("manual") }, { "Auto", new EquatableObject("auto") }
                    })
                },
                {
                    EnumConstants.LayoutAlignItemsEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Start", new EquatableObject("flex-start") }, { "Center", new EquatableObject("center") }, { "End", new EquatableObject("flex-end") },
                        { "Stretch", new EquatableObject("stretch") }
                    })
                },
                {
                    EnumConstants.AlignInContainerEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Start", new EquatableObject("flex-start") }, { "Center", new EquatableObject("center") }, { "End", new EquatableObject("flex-end") },
                        { "Stretch", new EquatableObject("stretch") }, { "SetByContainer", new EquatableObject("auto") }
                    })
                },
                {
                    EnumConstants.LayoutJustifyContentEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Start", new EquatableObject("flex-start") }, { "Center", new EquatableObject("center") }, { "End", new EquatableObject("flex-end") },
                        { "SpaceBetween", new EquatableObject("space-between") }
                    })
                },
                {
                    EnumConstants.LayoutOverflowEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Hide", new EquatableObject("hidden") }, { "Scroll", new EquatableObject("auto") }
                    })
                },
                {
                    EnumConstants.FontEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Segoe UI", new EquatableObject("'Segoe UI', 'Open Sans', sans-serif") }, { "Arial", new EquatableObject("Arial, sans-serif") },
                        { "Lato Hairline", new EquatableObject("'Lato Hairline', sans-serif") }, { "Lato", new EquatableObject("'Lato', sans-serif") },
                        { "Lato Light", new EquatableObject("'Lato Light', sans-serif") }, { "Courier New", new EquatableObject("'Courier New', monospace") },
                        { "Georgia", new EquatableObject("Georgia, serif") }, { "Dancing Script", new EquatableObject("'Dancing Script', sans-serif") },
                        { "Lato Black", new EquatableObject("'Lato Black', sans-serif") }, { "Verdana", new EquatableObject("Verdana, sans-serif") },
                        { "Open Sans", new EquatableObject("'Open Sans', sans-serif") }, { "Open Sans Condensed", new EquatableObject("'Open Sans Condensed', sans-serif") },
                        { "Great Vibes", new EquatableObject("'Great Vibes', sans-serif") }, { "Patrick Hand", new EquatableObject("'Patrick Hand', sans-serif") }
                    })
                },
                {
                    EnumConstants.FontWeightEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Normal", new EquatableObject("normal") }, { "Semibold", new EquatableObject("600") }, { "Bold", new EquatableObject("bold") },
                        { "Lighter", new EquatableObject("lighter") }
                    })
                },
                {
                    EnumConstants.ImagePositionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Fill", new EquatableObject("fill") }, { "Fit", new EquatableObject("fit") }, { "Stretch", new EquatableObject("stretch") },
                        { "Tile", new EquatableObject("tile") }, { "Center", new EquatableObject("center") }
                    })
                },
                {
                    EnumConstants.LayoutEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Horizontal", new EquatableObject("horizontal") }, { "Vertical", new EquatableObject("vertical") }
                    })
                },
                {
                    EnumConstants.LayoutDirectionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Horizontal", new EquatableObject("row") }, { "Vertical", new EquatableObject("column") }
                    })
                },
                {
                    EnumConstants.TextPositionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Left", new EquatableObject("left") }, { "Right", new EquatableObject("right") }
                    })
                },
                {
                    EnumConstants.TextModeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "SingleLine", new EquatableObject("singleline") }, { "Password", new EquatableObject("password") }, { "MultiLine", new EquatableObject("multiline") }
                    })
                },
                {
                    EnumConstants.TextFormatEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Text", new EquatableObject("text") }, { "Number", new EquatableObject("number") }
                    })
                },
                {
                    EnumConstants.VirtualKeyboardModeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Auto", new EquatableObject("auto") }, { "Numeric", new EquatableObject("numeric") }, { "Text", new EquatableObject("text") }
                    })
                },
                {
                    EnumConstants.TeamsThemeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Default", new EquatableObject("default") }, { "Dark", new EquatableObject("dark") }, { "Contrast", new EquatableObject("contrast") }
                    })
                },
                {
                    EnumConstants.ThemesEnumString,
                    DType.CreateEnum(DType.Color, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Vivid", new EquatableObject(8573268208d) }, { "Eco", new EquatableObject(8577703760d) }, { "Harvest", new EquatableObject(8588214850d) },
                        { "Dust", new EquatableObject(8580980564d) }, { "Awakening", new EquatableObject(8575207804d) }
                    })
                },
                {
                    EnumConstants.PenModeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Draw", new EquatableObject("draw") }, { "Erase", new EquatableObject("erase") }
                    })
                },
                {
                    EnumConstants.RemoveFlagsEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "First", new EquatableObject("first") }, { "All", new EquatableObject("all") }
                    })
                },
                {
                    EnumConstants.ScreenTransitionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Fade", new EquatableObject("fade") }, { "Cover", new EquatableObject("cover") }, { "UnCover", new EquatableObject("uncover") },
                        { "CoverRight", new EquatableObject("coverright") }, { "UnCoverRight", new EquatableObject("uncoverright") }, { "None", new EquatableObject("none") }
                    })
                },
                {
                    LanguageConstants.SortOrderEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Ascending", new EquatableObject("ascending") }, { "Descending", new EquatableObject("descending") }
                    })
                },
                {
                    EnumConstants.AlignEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Left", new EquatableObject("left") }, { "Right", new EquatableObject("right") }, { "Center", new EquatableObject("center") },
                        { "Justify", new EquatableObject("justify") }
                    })
                },
                {
                    EnumConstants.VerticalAlignEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Top", new EquatableObject("top") }, { "Middle", new EquatableObject("middle") }, { "Bottom", new EquatableObject("bottom") }
                    })
                },
                {
                    EnumConstants.TransitionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Push", new EquatableObject("push") }, { "Pop", new EquatableObject("pop") }, { "None", new EquatableObject("none") }
                    })
                },
                {
                    EnumConstants.TimeUnitEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Years", new EquatableObject("years") }, { "Quarters", new EquatableObject("quarters") }, { "Months", new EquatableObject("months") },
                        { "Days", new EquatableObject("days") }, { "Hours", new EquatableObject("hours") }, { "Minutes", new EquatableObject("minutes") },
                        { "Seconds", new EquatableObject("seconds") }, { "Milliseconds", new EquatableObject("milliseconds") }
                    })
                },
                {
                    EnumConstants.OverflowEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Hidden", new EquatableObject("hidden") }, { "Scroll", new EquatableObject("scroll") }
                    })
                },
                {
                    EnumConstants.MapStyleEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Road", new EquatableObject("road") }, { "Aerial", new EquatableObject("aerial") }, { "Auto", new EquatableObject("auto") }
                    })
                },
                {
                    EnumConstants.GridStyleEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "All", new EquatableObject("all") }, { "None", new EquatableObject("none") }, { "XOnly", new EquatableObject("xonly") },
                        { "YOnly", new EquatableObject("yonly") }
                    })
                },
                {
                    EnumConstants.LabelPositionEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Inside", new EquatableObject("inside") }, { "Outside", new EquatableObject("outside") }
                    })
                },
                {
                    EnumConstants.DataSourceInfoEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "DisplayName", new EquatableObject("displayname") }, { "Required", new EquatableObject("required") }, { "MaxLength", new EquatableObject("maxlength") },
                        { "MinLength", new EquatableObject("minlength") }, { "MaxValue", new EquatableObject("maxvalue") }, { "MinValue", new EquatableObject("minvalue") },
                        { "AllowedValues", new EquatableObject("allowedvalues") }, { "EditPermission", new EquatableObject("editpermission") },
                        { "ReadPermission", new EquatableObject("readpermission") }, { "CreatePermission", new EquatableObject("createpermission") },
                        { "DeletePermission", new EquatableObject("deletepermission") }
                    })
                },
                {
                    EnumConstants.RecordInfoEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "EditPermission", new EquatableObject("editpermission") }, { "ReadPermission", new EquatableObject("readpermission") },
                        { "DeletePermission", new EquatableObject("deletepermission") }
                    })
                },
                {
                    EnumConstants.StateEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "NoChange", new EquatableObject(1d) }, { "Added", new EquatableObject(2d) }, { "Updated", new EquatableObject(4d) },
                        { "Deleted", new EquatableObject(8d) }, { "All", new EquatableObject(4294967295d) }
                    })
                },
                {
                    EnumConstants.ErrorStateEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "NoError", new EquatableObject(1d) }, { "DataSourceError", new EquatableObject(2d) }, { "All", new EquatableObject(4294967295d) }
                    })
                },
                {
                    EnumConstants.ErrorSeverityEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "NoError", new EquatableObject(0d) }, { "Warning", new EquatableObject(1d) }, { "Moderate", new EquatableObject(2d) },
                        { "Severe", new EquatableObject(3d) }
                    })
                },
                {
                    EnumConstants.ErrorKindEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "None", new EquatableObject(0d) }, { "Sync", new EquatableObject(1d) }, { "MissingRequired", new EquatableObject(2d) },
                        { "CreatePermission", new EquatableObject(3d) }, { "EditPermissions", new EquatableObject(4d) }, { "DeletePermissions", new EquatableObject(5d) },
                        { "Conflict", new EquatableObject(6d) }, { "NotFound", new EquatableObject(7d) }, { "ConstraintViolated", new EquatableObject(8d) },
                        { "GeneratedValue", new EquatableObject(9d) }, { "ReadOnlyValue", new EquatableObject(10d) }, { "Validation", new EquatableObject(11d) },
                        { "Unknown", new EquatableObject(12d) }, { "Div0", new EquatableObject(13d) }, { "BadLanguageCode", new EquatableObject(14d) },
                        { "BadRegex", new EquatableObject(15d) }, { "InvalidFunctionUsage", new EquatableObject(16d) }, { "FileNotFound", new EquatableObject(17d) },
                        { "AnalysisError", new EquatableObject(18d) }, { "ReadPermission", new EquatableObject(19d) }, { "NotSupported", new EquatableObject(20d) },
                        { "InsufficientMemory", new EquatableObject(21d) }, { "QuotaExceeded", new EquatableObject(22d) }, { "Network", new EquatableObject(23d) },
                        { "Numeric", new EquatableObject(24d) }, { "InvalidArgument", new EquatableObject(25d) }, { "Internal", new EquatableObject(26d) },
                        { "NotApplicable", new EquatableObject(27d) }, { "Custom", new EquatableObject(1000d) }
                    })
                },
                {
                    EnumConstants.ZoomEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "FitWidth", new EquatableObject(-1d) }, { "FitHeight", new EquatableObject(-2d) }, { "FitBoth", new EquatableObject(-3d) }
                    })
                },
                {
                    EnumConstants.FormModeEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Edit", new EquatableObject(0d) }, { "New", new EquatableObject(1d) }, { "View", new EquatableObject(2d) }
                    })
                },
                {
                    EnumConstants.PDFPasswordStateEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "NoPasswordNeeded", new EquatableObject(0d) }, { "PasswordNeeded", new EquatableObject(1d) }, { "IncorrectPassword", new EquatableObject(2d) }
                    })
                },
                {
                    EnumConstants.DateTimeZoneEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Local", new EquatableObject("local") }, { "UTC", new EquatableObject("utc") }
                    })
                },
                {
                    EnumConstants.BarcodeTypeEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Auto", new EquatableObject("any") }, { "Aztec", new EquatableObject("aztec") }, { "Codabar", new EquatableObject("codabar") },
                        { "Code128", new EquatableObject("code128") }, { "Code39", new EquatableObject("code39") }, { "Code93", new EquatableObject("code93") },
                        { "DataMatrix", new EquatableObject("dataMatrix") }, { "Ean", new EquatableObject("ean") }, { "I2of5", new EquatableObject("i2of5") },
                        { "Pdf417", new EquatableObject("pdf417") }, { "QRCode", new EquatableObject("qrCode") }, { "Rss14", new EquatableObject("rss14") },
                        { "RssExpanded", new EquatableObject("rssExpanded") }, { "Upc", new EquatableObject("upc") }
                    })
                },
                {
                    EnumConstants.ImageRotationEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "None", new EquatableObject("none") }, { "Rotate90", new EquatableObject("rotate90") }, { "Rotate180", new EquatableObject("rotate180") },
                        { "Rotate270", new EquatableObject("rotate270") }
                    })
                },
                {
                    EnumConstants.MatchEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Email", new EquatableObject(".+@.+\\.[^\\.]{2,}") }, { "Letter", new EquatableObject("\\p{L}") }, { "MultipleLetters", new EquatableObject("\\p{L}+") },
                        { "OptionalLetters", new EquatableObject("\\p{L}*") }, { "Digit", new EquatableObject("\\d") }, { "MultipleDigits", new EquatableObject("\\d+") },
                        { "OptionalDigits", new EquatableObject("\\d*") }, { "Any", new EquatableObject(".") }, { "Hyphen", new EquatableObject("\\-") },
                        { "Period", new EquatableObject("\\.") }, { "Comma", new EquatableObject(",") }, { "LeftParen", new EquatableObject("\\(") },
                        { "RightParen", new EquatableObject("\\)") }, { "Space", new EquatableObject("\\s") }, { "MultipleSpaces", new EquatableObject("\\s+") },
                        { "OptionalSpaces", new EquatableObject("\\s*") }, { "NonSpace", new EquatableObject("\\S") }, { "MultipleNonSpaces", new EquatableObject("\\S+") },
                        { "OptionalNonSpaces", new EquatableObject("\\S*") }
                    })
                },
                {
                    EnumConstants.MatchOptionsEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "BeginsWith", new EquatableObject("^c") }, { "EndsWith", new EquatableObject("$c") }, { "Contains", new EquatableObject("c") },
                        { "Complete", new EquatableObject("^c$") }, { "IgnoreCase", new EquatableObject("i") }, { "Multiline", new EquatableObject("m") }
                    })
                },
                {
                    EnumConstants.JSONFormatEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Compact", new EquatableObject(string.Empty) }, { "IndentFour", new EquatableObject("4") }, { "IgnoreBinaryData", new EquatableObject("G") },
                        { "IncludeBinaryData", new EquatableObject("B") }, { "IgnoreUnsupportedTypes", new EquatableObject("I") }
                    })
                },
                {
                    EnumConstants.TraceOptionsEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "None", new EquatableObject("none") }, { "IgnoreUnsupportedTypes", new EquatableObject("I") }
                    })
                },
                {
                    EnumConstants.EntityFormPatternEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "None", new EquatableObject("none") }, { "Details", new EquatableObject("details") }, { "List", new EquatableObject("list") },
                        { "CardList", new EquatableObject("cardlist") }
                    })
                },
                {
                    EnumConstants.ListItemTemplateEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Single", new EquatableObject("single") }, { "Double", new EquatableObject("double") }, { "Person", new EquatableObject("person") }
                    })
                },
                {
                    EnumConstants.LoadingSpinnerEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Controls", new EquatableObject(0d) }, { "Data", new EquatableObject(1d) }, { "None", new EquatableObject(2d) }
                    })
                },
                {
                    EnumConstants.NotificationTypeEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Error", new EquatableObject(0d) }, { "Warning", new EquatableObject(1d) }, { "Success", new EquatableObject(2d) },
                        { "Information", new EquatableObject(3d) }
                    })
                },
                {
                    EnumConstants.LiveEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Off", new EquatableObject("off") }, { "Polite", new EquatableObject("polite") }, { "Assertive", new EquatableObject("assertive") }
                    })
                },
                {
                    EnumConstants.TextRoleEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Default", new EquatableObject("default") }, { "Heading1", new EquatableObject("heading1") }, { "Heading2", new EquatableObject("heading2") },
                        { "Heading3", new EquatableObject("heading3") }, { "Heading4", new EquatableObject("heading4") }
                    })
                },
                {
                    EnumConstants.ScreenSizeEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Small", new EquatableObject(1d) }, { "Medium", new EquatableObject(2d) }, { "Large", new EquatableObject(3d) },
                        { "ExtraLarge", new EquatableObject(4d) }
                    })
                },
                {
                    EnumConstants.IconEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Add", new EquatableObject("builtinicon:Add") }, { "Cancel", new EquatableObject("builtinicon:Cancel") },
                        { "CancelBadge", new EquatableObject("builtinicon:CancelBadge") }, { "Edit", new EquatableObject("builtinicon:Edit") },
                        { "Check", new EquatableObject("builtinicon:Check") }, { "CheckBadge", new EquatableObject("builtinicon:CheckBadge") },
                        { "Search", new EquatableObject("builtinicon:Search") }, { "Filter", new EquatableObject("builtinicon:Filter") },
                        { "FilterFlat", new EquatableObject("builtinicon:FilterFlat") }, { "FilterFlatFilled", new EquatableObject("builtinicon:FilterFlatFilled") },
                        { "Sort", new EquatableObject("builtinicon:Sort") }, { "Reload", new EquatableObject("builtinicon:Reload") },
                        { "Trash", new EquatableObject("builtinicon:Trash") }, { "Save", new EquatableObject("builtinicon:Save") },
                        { "Download", new EquatableObject("builtinicon:Download") }, { "Copy", new EquatableObject("builtinicon:Copy") },
                        { "LikeDislike", new EquatableObject("builtinicon:LikeDislike") }, { "Crop", new EquatableObject("builtinicon:Crop") },
                        { "Pin", new EquatableObject("builtinicon:Pin") }, { "ClearDrawing", new EquatableObject("builtinicon:ClearDrawing") },
                        { "ExpandView", new EquatableObject("builtinicon:ExpandView") }, { "CollapseView", new EquatableObject("builtinicon:CollapseView") },
                        { "Draw", new EquatableObject("builtinicon:Draw") }, { "Compose", new EquatableObject("builtinicon:Compose") },
                        { "Erase", new EquatableObject("builtinicon:Erase") }, { "Message", new EquatableObject("builtinicon:Message") },
                        { "Post", new EquatableObject("builtinicon:Post") }, { "AddDocument", new EquatableObject("builtinicon:AddDocument") },
                        { "AddLibrary", new EquatableObject("builtinicon:AddLibrary") }, { "Home", new EquatableObject("builtinicon:Home") },
                        { "Hamburger", new EquatableObject("builtinicon:Hamburger") }, { "Settings", new EquatableObject("builtinicon:Settings") },
                        { "More", new EquatableObject("builtinicon:More") }, { "Waffle", new EquatableObject("builtinicon:Waffle") },
                        { "ChevronLeft", new EquatableObject("builtinicon:ChevronLeft") }, { "ChevronRight", new EquatableObject("builtinicon:ChevronRight") },
                        { "ChevronUp", new EquatableObject("builtinicon:ChevronUp") }, { "ChevronDown", new EquatableObject("builtinicon:ChevronDown") },
                        { "NextArrow", new EquatableObject("builtinicon:NextArrow") }, { "BackArrow", new EquatableObject("builtinicon:BackArrow") },
                        { "ArrowDown", new EquatableObject("builtinicon:ArrowDown") }, { "ArrowUp", new EquatableObject("builtinicon:ArrowUp") },
                        { "ArrowLeft", new EquatableObject("builtinicon:ArrowLeft") }, { "ArrowRight", new EquatableObject("builtinicon:ArrowRight") },
                        { "Camera", new EquatableObject("builtinicon:Camera") }, { "Document", new EquatableObject("builtinicon:Document") },
                        { "DockCheckProperties", new EquatableObject("builtinicon:DockCheckProperties") }, { "Folder", new EquatableObject("builtinicon:Folder") },
                        { "Journal", new EquatableObject("builtinicon:Journal") }, { "ForkKnife", new EquatableObject("builtinicon:ForkKnife") },
                        { "Transportation", new EquatableObject("builtinicon:Transportation") }, { "Airplane", new EquatableObject("builtinicon:Airplane") },
                        { "Bus", new EquatableObject("builtinicon:Bus") }, { "Cars", new EquatableObject("builtinicon:Cars") },
                        { "Money", new EquatableObject("builtinicon:Money") }, { "Currency", new EquatableObject("builtinicon:Currency") },
                        { "AddToCalendar", new EquatableObject("builtinicon:AddToCalendar") }, { "CalendarBlank", new EquatableObject("builtinicon:CalendarBlank") },
                        { "OfficeBuilding", new EquatableObject("builtinicon:OfficeBuilding") }, { "PaperClip", new EquatableObject("builtinicon:PaperClip") },
                        { "Newspaper", new EquatableObject("builtinicon:Newspaper") }, { "Lock", new EquatableObject("builtinicon:Lock") },
                        { "Waypoint", new EquatableObject("builtinicon:Waypoint") }, { "Location", new EquatableObject("builtinicon:Location") },
                        { "DocumentPDF", new EquatableObject("builtinicon:DocumentPDF") }, { "Bell", new EquatableObject("builtinicon:Bell") },
                        { "ShoppingCart", new EquatableObject("builtinicon:ShoppingCart") }, { "Phone", new EquatableObject("builtinicon:Phone") },
                        { "PhoneHangUp", new EquatableObject("builtinicon:PhoneHangUp") }, { "Mobile", new EquatableObject("builtinicon:Mobile") },
                        { "Laptop", new EquatableObject("builtinicon:Laptop") }, { "ComputerDesktop", new EquatableObject("builtinicon:ComputerDesktop") },
                        { "Devices", new EquatableObject("builtinicon:Devices") }, { "Controller", new EquatableObject("builtinicon:Controller") },
                        { "Tools", new EquatableObject("builtinicon:Tools") }, { "ToolsWrench", new EquatableObject("builtinicon:ToolsWrench") },
                        { "Mail", new EquatableObject("builtinicon:Mail") }, { "Send", new EquatableObject("builtinicon:Send") },
                        { "Clock", new EquatableObject("builtinicon:Clock") }, { "ListWatchlistRemind", new EquatableObject("builtinicon:ListWatchlistRemind") },
                        { "LogJournal", new EquatableObject("builtinicon:LogJournal") }, { "Note", new EquatableObject("builtinicon:Note") },
                        { "PhotosPictures", new EquatableObject("builtinicon:PhotosPictures") },
                        { "RadarActivityMonitor", new EquatableObject("builtinicon:RadarActivityMonitor") }, { "Tablet", new EquatableObject("builtinicon:Tablet") },
                        { "Tag", new EquatableObject("builtinicon:Tag") }, { "CameraAperture", new EquatableObject("builtinicon:CameraAperture") },
                        { "ColorPicker", new EquatableObject("builtinicon:ColorPicker") }, { "DetailList", new EquatableObject("builtinicon:DetailList") },
                        { "DocumentWithContent", new EquatableObject("builtinicon:DocumentWithContent") },
                        { "ListScrollEmpty", new EquatableObject("builtinicon:ListScrollEmpty") },
                        { "ListScrollWatchlist", new EquatableObject("builtinicon:ListScrollWatchlist") }, { "OptionsList", new EquatableObject("builtinicon:OptionsList") },
                        { "People", new EquatableObject("builtinicon:People") }, { "Person", new EquatableObject("builtinicon:Person") },
                        { "EmojiFrown", new EquatableObject("builtinicon:EmojiFrown") }, { "EmojiSmile", new EquatableObject("builtinicon:EmojiSmile") },
                        { "EmojiSad", new EquatableObject("builtinicon:EmojiSad") }, { "EmojiNeutral", new EquatableObject("builtinicon:EmojiNeutral") },
                        { "EmojiHappy", new EquatableObject("builtinicon:EmojiHappy") }, { "Warning", new EquatableObject("builtinicon:Warning") },
                        { "Information", new EquatableObject("builtinicon:Information") }, { "Database", new EquatableObject("builtinicon:Database") },
                        { "Weather", new EquatableObject("builtinicon:Weather") }, { "TrendingHashtag", new EquatableObject("builtinicon:TrendingHashtag") },
                        { "TrendingUpwards", new EquatableObject("builtinicon:TrendingUpwards") }, { "Items", new EquatableObject("builtinicon:Items") },
                        { "LevelsLayersItems", new EquatableObject("builtinicon:LevelsLayersItems") }, { "Trending", new EquatableObject("builtinicon:Trending") },
                        { "LineWeight", new EquatableObject("builtinicon:LineWeight") }, { "Printing3D", new EquatableObject("builtinicon:Printing3D") },
                        { "Import", new EquatableObject("builtinicon:Import") }, { "Export", new EquatableObject("builtinicon:Export") },
                        { "QuestionMark", new EquatableObject("builtinicon:QuestionMark") }, { "Help", new EquatableObject("builtinicon:Help") },
                        { "ThumbsDown", new EquatableObject("builtinicon:ThumbsDown") }, { "ThumbsUp", new EquatableObject("builtinicon:ThumbsUp") },
                        { "ThumbsDownFilled", new EquatableObject("builtinicon:ThumbsDownFilled") }, { "ThumbsUpFilled", new EquatableObject("builtinicon:ThumbsUpFilled") },
                        { "Undo", new EquatableObject("builtinicon:Undo") }, { "Redo", new EquatableObject("builtinicon:Redo") },
                        { "ZoomIn", new EquatableObject("builtinicon:ZoomIn") }, { "ZoomOut", new EquatableObject("builtinicon:ZoomOut") },
                        { "OpenInNewWindow", new EquatableObject("builtinicon:OpenInNewWindow") }, { "Share", new EquatableObject("builtinicon:Share") },
                        { "Publish", new EquatableObject("builtinicon:Publish") }, { "Link", new EquatableObject("builtinicon:Link") },
                        { "Sync", new EquatableObject("builtinicon:Sync") }, { "View", new EquatableObject("builtinicon:View") },
                        { "Hide", new EquatableObject("builtinicon:Hide") }, { "Bookmark", new EquatableObject("builtinicon:Bookmark") },
                        { "BookmarkFilled", new EquatableObject("builtinicon:BookmarkFilled") }, { "Reset", new EquatableObject("builtinicon:Reset") },
                        { "Blocked", new EquatableObject("builtinicon:Blocked") }, { "DockLeft", new EquatableObject("builtinicon:DockLeft") },
                        { "DockRight", new EquatableObject("builtinicon:DockRight") }, { "LightningBolt", new EquatableObject("builtinicon:LightningBolt") },
                        { "HorizontalLine", new EquatableObject("builtinicon:HorizontalLine") }, { "VerticalLine", new EquatableObject("builtinicon:VerticalLine") },
                        { "Ribbon", new EquatableObject("builtinicon:Ribbon") }, { "Diamond", new EquatableObject("builtinicon:Diamond") },
                        { "Alarm", new EquatableObject("builtinicon:Alarm") }, { "History", new EquatableObject("builtinicon:History") },
                        { "Heart", new EquatableObject("builtinicon:Heart") }, { "Print", new EquatableObject("builtinicon:Print") },
                        { "Error", new EquatableObject("builtinicon:Error") }, { "Flag", new EquatableObject("builtinicon:Flag") },
                        { "Notebook", new EquatableObject("builtinicon:Notebook") }, { "Bug", new EquatableObject("builtinicon:Bug") },
                        { "Microphone", new EquatableObject("builtinicon:Microphone") }, { "Video", new EquatableObject("builtinicon:Video") },
                        { "Shop", new EquatableObject("builtinicon:Shop") }, { "Phonebook", new EquatableObject("builtinicon:Phonebook") },
                        { "Enhance", new EquatableObject("builtinicon:Enhance") }, { "Unlock", new EquatableObject("builtinicon:Unlock") },
                        { "Calculator", new EquatableObject("builtinicon:Calculator") }, { "Support", new EquatableObject("builtinicon:Support") },
                        { "Lightbulb", new EquatableObject("builtinicon:Lightbulb") }, { "Key", new EquatableObject("builtinicon:Key") },
                        { "Scan", new EquatableObject("builtinicon:Scan") }, { "Hospital", new EquatableObject("builtinicon:Hospital") },
                        { "Health", new EquatableObject("builtinicon:Health") }, { "Medical", new EquatableObject("builtinicon:Medical") },
                        { "Manufacture", new EquatableObject("builtinicon:Manufacture") }, { "Train", new EquatableObject("builtinicon:Train") },
                        { "Globe", new EquatableObject("builtinicon:Globe") }, { "HalfFilledCircle", new EquatableObject("builtinicon:HalfFilledCircle") },
                        { "Tray", new EquatableObject("builtinicon:Tray") }, { "AddUser", new EquatableObject("builtinicon:AddUser") },
                        { "Text", new EquatableObject("builtinicon:Text") }, { "Shirt", new EquatableObject("builtinicon:Shirt") },
                        { "Signal", new EquatableObject("builtinicon:Signal") }, { "Cut", new EquatableObject("builtinicon:Cut") },
                        { "Paste", new EquatableObject("builtinicon:Paste") }, { "Leave", new EquatableObject("builtinicon:Leave") }
                    })
                },
                {
                    EnumConstants.LaunchTargetEnumString,
                    DType.CreateEnum(DType.String, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "New", new EquatableObject("_blank") }, { "Replace", new EquatableObject("_self") }
                    })
                },
                {
                    EnumConstants.TraceSeverityEnumString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Information", new EquatableObject(3d) }, { "Warning", new EquatableObject(1d) }, { "Error", new EquatableObject(0d) },
                        { "Critical", new EquatableObject(-1d) }
                    })
                },
                {
                    EnumConstants.SelectedStateString,
                    DType.CreateEnum(DType.Number, new Dictionary<string, EquatableObject>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        { "Edit", new EquatableObject(0d) }, { "New", new EquatableObject(1d) }, { "View", new EquatableObject(2d) }
                    })
                }
            };

        internal static IReadOnlyDictionary<string, string> DefaultEnums { get; } = DefaultEnums2.ToDictionary(e => e.Key, e => e.Value.ToString());

        private readonly Dictionary<string, DType> _workingEnums = new Dictionary<string, DType>();

        private ImmutableList<EnumSymbol> _enumSymbols;

        private Dictionary<string, DType> _enumTypes;

        #region Internal methods
        internal EnumStoreBuilder WithRequiredEnums(TexlFunctionSet functions)
        {
            var missingEnums = new Dictionary<string, DType>();

            foreach (var name in functions.Enums)
            {
                if (!_workingEnums.ContainsKey(name) && !missingEnums.ContainsKey(name))
                {
                    missingEnums.Add(name, DefaultEnums2[name]);
                }
            }

            foreach (var missingEnum in missingEnums)
            {
                _workingEnums.Add(missingEnum.Key, missingEnum.Value);
            }

            return this;
        }

        internal EnumStoreBuilder WithDefaultEnums()
        {
            foreach (var defaultEnum in DefaultEnums2)
            {
                if (!_workingEnums.ContainsKey(defaultEnum.Key))
                {
                    _workingEnums.Add(defaultEnum.Key, defaultEnum.Value);
                }
            }

            return this;
        }

        internal EnumStore Build()
        {
            return EnumStore.Build(new List<EnumSymbol>(EnumSymbols));
        }
        #endregion

        private Dictionary<string, DType> RegenerateEnumTypes()
        {            
            return _workingEnums;
        }

        private IEnumerable<(DName name, DType typeSpec)> Enums()
        {
            CollectionUtils.EnsureInstanceCreated(ref _enumTypes, () =>
            {
                return RegenerateEnumTypes();
            });

            var list = ImmutableList.CreateBuilder<(DName name, DType typeSpec)>();

            foreach (var enumSpec in _workingEnums)
            {
                Contracts.Assert(DName.IsValidDName(enumSpec.Key));

                var name = new DName(enumSpec.Key);
                list.Add((name, _enumTypes[enumSpec.Key]));
            }

            return list.ToImmutable();
        }

        private IEnumerable<EnumSymbol> EnumSymbols => CollectionUtils.EnsureInstanceCreated(ref _enumSymbols, () => RegenerateEnumSymbols());

        private ImmutableList<EnumSymbol> RegenerateEnumSymbols()
        {
            var list = ImmutableList.CreateBuilder<EnumSymbol>();

            foreach (var (name, typeSpec) in Enums())
            {
                list.Add(new EnumSymbol(name, typeSpec));
            }

            return list.ToImmutable();
        }
    }
}
