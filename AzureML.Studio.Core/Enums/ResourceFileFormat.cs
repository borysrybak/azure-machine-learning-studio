using System.ComponentModel;

namespace AzureML.Studio.Core.Enums
{
    public enum ResourceFileFormat
    {
        [Description("GenericCSV")]
        GenericCSV,

        [Description("GenericCSVNoHeader")]
        GenericCSVNoHeader,

        [Description("GenericTSV")]
        GenericTSV,

        [Description("GenericTSVNoHeader")]
        GenericTSVNoHeader,

        [Description("ARFF")]
        ARFF,

        [Description("Zip")]
        Zip,

        [Description("RData")]
        RData,

        [Description("PlainText")]
        PlainText
    }
}
