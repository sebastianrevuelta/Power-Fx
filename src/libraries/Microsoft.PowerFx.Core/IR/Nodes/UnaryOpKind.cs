﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.PowerFx.Core.IR.Nodes
{
    internal enum UnaryOpKind
    {
        // Value operations
        Negate,
        Percent,

        // Coercion operations
        BooleanToNumber,
        OptionSetToNumber,
        DateToNumber,
        TimeToNumber,
        DateTimeToNumber,
        CurrencyToNumber,

        BlobToHyperlink,
        ImageToHyperlink,
        MediaToHyperlink,
        TextToHyperlink,

        SingleColumnRecordToLargeImage,
        ImageToLargeImage,
        LargeImageToImage,
        TextToImage,

        TextToMedia,
        TextToBlob,

        NumberToText,
        BooleanToText,
        OptionSetToText,
        ViewToText,
        CurrencyToText,
        GUIDToText,
        ImageToText,
        MediaToText,
        BlobToText,
        PenImageToText,

        TextToGUID,

        NumberToCurrency,
        TextToCurrency,
        BooleanToCurrency,

        NumberToBoolean,
        TextToBoolean,
        OptionSetToBoolean,
        CurrencyToBoolean,

        OptionSetToColor,

        RecordToRecord, // See field mappings
        TableToTable,
        RecordToTable,

        NumberToDateTime,
        NumberToDate,
        NumberToTime,
        TextToDateTime,
        TextToDate,
        TextToTime,

        DateTimeToTime,
        DateToTime,
        TimeToDate,
        DateTimeToDate,
        TimeToDateTime,
        DateToDateTime,

        BooleanToOptionSet,
        AggregateToDataEntity,

        // Argument pre-processesor in IR Phase.

        /// <summary>
        /// Used for pre-processing function arguments from blank to empty string.
        /// All Interpreter(backed) must implement this.
        /// </summary>
        BlankToEmptyString,
    }
}
