namespace FootSim.Table
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TableBuilder<TRow>
    {
        private readonly List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

        public void AddColumn(string header, int width, Alignment alignment, Func<TRow, object> getCellValue)
        {
            this.columnDefinitions.Add(new ColumnDefinition(header, width, alignment, getCellValue));
        }

        public string Build(IEnumerable<TRow> rows)
        {
            return rows.Select(this.CreateRow)
                .Prepend(this.CreateHeader())
                .Join(Environment.NewLine);
        }

        private string CreateHeader()
        {
            return this.columnDefinitions
                .Select(cd => cd.GetHeaderValue())
                .Join(" ");
        }

        private string CreateRow(TRow row)
        {
            return this.columnDefinitions
                .Select(cd => cd.GetCellValue(row))
                .Join(" ");
        }

        private class ColumnDefinition
        {
            private readonly string header;
            private readonly int width;
            private readonly Alignment alignment;
            private readonly Func<TRow, object> getCellValue;

            public ColumnDefinition(string header, int width, Alignment alignment, Func<TRow, object> getCellValue)
            {
                this.header = header;
                this.width = width;
                this.alignment = alignment;
                this.getCellValue = getCellValue;
            }

            public string GetHeaderValue()
            {
                return this.Pad(this.header);
            }

            public string GetCellValue(TRow row)
            {
                return this.Pad(this.getCellValue(row).ToString());
            }

            private string Pad(string value)
            {
                switch (this.alignment)
                {
                    case Alignment.Left:
                        return value.PadRight(this.width);
                    case Alignment.Right:
                        return value.PadLeft(this.width);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(this.alignment));
                }
            }
        }
    }
}