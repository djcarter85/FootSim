namespace FootSim.Table
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TableBuilder<TRow>
    {
        private readonly List<ColumnDefinition> columnDefinitions = new List<ColumnDefinition>();

        public void AddColumn(string header, Alignment alignment, Func<TRow, object> getCellValue)
        {
            this.columnDefinitions.Add(new ColumnDefinition(header, alignment, getCellValue));
        }

        public string Build(IEnumerable<TRow> rows)
        {
            foreach (var columnDefinition in this.columnDefinitions)
            {
                columnDefinition.SetWidth(rows);
            }

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
            private readonly Alignment alignment;
            private readonly Func<TRow, object> getCellValue;

            private int width;

            public ColumnDefinition(string header, Alignment alignment, Func<TRow, object> getCellValue)
            {
                this.header = header;
                this.alignment = alignment;
                this.getCellValue = getCellValue;
            }

            public void SetWidth(IEnumerable<TRow> rows)
            {
                var cellValueLengths = rows.Select(r => this.getCellValue(r).ToString().Length);

                this.width = cellValueLengths.Append(this.header.Length).Max();
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