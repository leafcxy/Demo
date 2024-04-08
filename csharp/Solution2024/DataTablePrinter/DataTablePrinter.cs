using System;
using System.Collections.Generic;
using System.Data;
//using System.Linq;

namespace DataTablePrinter
{
    /// <summary>
    /// Enumerates the border flags of the text represented by a <see cref="DataTable"/> which is to be drawn on a
    /// pretty printed string.
    /// </summary>
    [Flags]
    public enum Border
    {
        /// <summary>
        /// No border.
        /// </summary>
        None = 0,

        /// <summary>
        /// Bottom border.
        /// </summary>
        Bottom = 1,

        /// <summary>
        /// Left border.
        /// </summary>
        Left = 2,

        /// <summary>
        /// Right border.
        /// </summary>
        Right = 4,

        /// <summary>
        /// Top border.
        /// </summary>
        Top = 8,

        /// <summary>
        /// All borders.
        /// </summary>
        All = 15,
    }

    /// <summary>
    /// An extension class providing <see cref="char"/> utility methods for pretty printing to a string.
    /// </summary>
    internal static class CharExtensions
    {
        /// <summary>
        /// Draws a border on a canvas given a bounding box specified by coordinates.
        /// </summary>
        ///
        /// <param name="canvas">
        /// The canvas to draw on which is assumed to be large enough.
        /// </param>
        ///
        /// <param name="x1">
        /// The x coordinate of the beginning of the bounding box to draw.
        /// </param>
        ///
        /// <param name="y1">
        /// The y coordinate of the beginning of the bounding box to draw.
        /// </param>
        ///
        /// <param name="x2">
        /// The x coordinate of the end of the bounding box to draw.
        /// </param>
        ///
        /// <param name="y2">
        /// The y coordinate of the end of the bounding box to draw.
        /// </param>
        ///
        /// <param name="border">
        /// The border flags which dictate which border to draw.
        /// </param>
        internal static void DrawBorder(this char[,] canvas, int x1, int y1, int x2, int y2, Border border)
        {
            if (border.HasFlag(Border.Top))
            {
                canvas.DrawLine(x1, y1, x2, y1);
            }

            if (border.HasFlag(Border.Bottom))
            {
                canvas.DrawLine(x1, y2, x2, y2);
            }

            if (border.HasFlag(Border.Left))
            {
                canvas.DrawLine(x1, y1, x1, y2);
            }

            if (border.HasFlag(Border.Right))
            {
                canvas.DrawLine(x2, y1, x2, y2);
            }
        }

        /// <summary>
        /// Draws a line on a canvas given a beginning and an end coordinate.
        /// </summary>
        ///
        /// <param name="canvas">
        /// The canvas to draw on which is assumed to be large enough.
        /// </param>
        ///
        /// <param name="x1">
        /// The x coordinate of the beginning of the line to draw.
        /// </param>
        ///
        /// <param name="y1">
        /// The y coordinate of the beginning of the line to draw.
        /// </param>
        ///
        /// <param name="x2">
        /// The x coordinate of the end of the line to draw.
        /// </param>
        ///
        /// <param name="y2">
        /// The y coordinate of the end of the line to draw.
        /// </param>
        ///
        /// <remarks>
        /// If this line crosses any other line drawn on the canvas then a '+' is inserted on the crossing boundary.
        /// </remarks>
        ///
        /// <exception cref="ArgumentException">
        /// Thrown when the line is neither horizontal nor vertical.
        /// </exception>
        internal static void DrawLine(this char[,] canvas, int x1, int y1, int x2, int y2)
        {
            if (x1 != x2 && y1 != y2)
            {
                throw new ArgumentException("Cannot draw non-horizontal or non-vertical lines");
            }

            if (x1 == x2)
            {
                if (y1 > y2)
                {
                    Utilities.Swap(ref y1, ref y2);
                }

                for (var y = y1; y <= y2; ++y)
                {
                    canvas[y, x1] = (y == y1 || y == y2 || canvas[y, x1] == '-' || canvas[y, x1] == '+') ? '+' : '|';
                }
            }
            else
            {
                if (x1 > x2)
                {
                    Utilities.Swap(ref x1, ref x2);
                }

                for (var x = x1; x <= x2; ++x)
                {
                    canvas[y1, x] = (x == x1 || x == x2 || canvas[y1, x] == '|' || canvas[y1, x] == '+') ? '+' : '-';
                }
            }
        }

        /// <summary>
        /// Draws a string on a canvas with alignment specified.
        /// </summary>
        ///
        /// <param name="canvas">
        /// The canvas to draw on which is assumed to be large enough.
        /// </param>
        ///
        /// <param name="x1">
        /// The x coordinate of the beginning of the string to draw.
        /// </param>
        ///
        /// <param name="y1">
        /// The y coordinate of the beginning of the string to draw.
        /// </param>
        ///
        /// <param name="x2">
        /// The x coordinate of the end of the string to draw.
        /// </param>
        ///
        /// <param name="y2">
        /// The y coordinate of the end of the string to draw.
        /// </param>
        ///
        /// <param name="text">
        /// The text to draw.
        /// </param>
        ///
        /// <param name="alignment">
        /// The alignment of the <paramref name="text"/> to draw.
        /// </param>
        ///
        /// <remarks>
        /// If the text cannot be contained within the bounding box specified by the coordinates then either nothing is
        /// drawn or the input string is truncated and '..' is added to the end.
        /// </remarks>
        internal static void DrawText(this char[,] canvas, int x1, int y1, int x2, int y2, string text, TextAlignment alignment)
        {
            // Truncate the text if it will not fit in the text box bounds
            if (text.Length > x2 - x1 + 1)
            {
                if (x2 - x1 >= 1)
                {
                    text = text.Substring(0, x2 - x1 - 1) + "..";
                }
                else
                {
                    return;
                }
            }

            // Update the coordinates based the text alignment
            switch (alignment)
            {
                case TextAlignment.Center:
                    {
                        y1 = (y2 + y1) / 2;
                        x1 += (x2 - x1 + 1 - text.Length) / 2;

                        break;
                    }

                case TextAlignment.Left:
                    {
                        y1 = (y2 + y1) / 2;

                        break;
                    }

                case TextAlignment.Right:
                    {
                        y1 = (y2 + y1) / 2;
                        x1 = x2 - text.Length + 1;

                        break;
                    }

                default:
                    {
                        throw new InvalidOperationException("Unreachable case reached");
                    }
            }

            for (var x = x1; x < x1 + text.Length; ++x)
            {
                canvas[y1, x] = text[x - x1];
            }
        }
    }

    /// <summary>
    /// An extension class providing <see cref="DataColumn"/> utility methods for pretty printing to a string.
    /// </summary>
    public static class DataColumnExtensions
    {
        /// <summary>
        /// Gets the border around the data area of the column.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <returns>
        /// The border around the data area of the column.
        /// </returns>
        public static Border GetDataBorder(this DataColumn column)
        {
            return column.GetExtendedProperty("DataBorder", Border.All);
        }

        /// <summary>
        /// Sets the border around the data area of the column.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetDataBorder(this DataColumn column, Border value)
        {
            column.SetExtendedProperty("DataBorder", value);
        }

        /// <summary>
        /// Gets the text alignment of the data in this column.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <returns>
        /// Gets the data alignment.
        /// </returns>
        public static TextAlignment GetDataAlignment(this DataColumn column)
        {
            return column.GetExtendedProperty("DataAlignment", TextAlignment.Left);
        }

        /// <summary>
        /// Sets the text alignment of the data in this column.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetDataAlignment(this DataColumn column, TextAlignment value)
        {
            column.SetExtendedProperty("DataAlignment", value);
        }

        /// <summary>
        /// Gets the formatting method which given a column and row formats the data of the cell into a string. This
        /// API can be used for arbitrary formatting of induvidual data cells.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <returns>
        /// The method which formats the data cell.
        /// </returns>
        public static Func<DataColumn, DataRow, string> GetDataTextFormat(this DataColumn column)
        {
            return column.GetExtendedProperty<Func<DataColumn, DataRow, string>>("DataTextFormat", (c, r) => string.Format("{0}", r[c]));
        }

        /// <summary>
        /// Sets the formatting method which given a column and row formats the data of the cell into a string. This
        /// API can be used for arbitrary formatting of induvidual data cells.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The method used to format the data cell which will be called during pretty printing of the table.
        /// </param>
        public static void SetDataTextFormat(this DataColumn column, Func<DataColumn, DataRow, string> value)
        {
            column.SetExtendedProperty("DataTextFormat", value);
        }

        /// <summary>
        /// Gets the border around the column header area which displays the column names.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <returns>
        /// The border around the column header area.
        /// </returns>
        public static Border GetHeaderBorder(this DataColumn column)
        {
            return column.GetExtendedProperty("HeaderBorder", Border.All);
        }

        /// <summary>
        /// Sets the border around the column header area which displays the column names.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetHeaderBorder(this DataColumn column, Border value)
        {
            column.SetExtendedProperty("HeaderBorder", value);
        }

        /// <summary>
        /// Gets the <see cref="DataColumn.ColumnName"/> text alignment.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <returns>
        /// Gets the column name text alignment.
        /// </returns>
        public static TextAlignment GetColumnNameAlignment(this DataColumn column)
        {
            return column.GetExtendedProperty("ColumnNameAlignment", TextAlignment.Center);
        }

        /// <summary>
        /// Sets the <see cref="DataColumn.ColumnName"/> text alignment.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetColumnNameAlignment(this DataColumn column, TextAlignment value)
        {
            column.SetExtendedProperty("ColumnNameAlignment", value);
        }

        /// <summary>
        /// Gets whether to show the <see cref="DataTable.TableName"/>.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <returns>
        /// <c>true</c> if the column name is to be shown; <c>false</c> otherwise.
        /// </returns>
        public static bool GetShowColumnName(this DataColumn column)
        {
            return column.GetExtendedProperty("ShowColumnName", true);
        }

        /// <summary>
        /// Sets whether to show the <see cref="DataTable.TableName"/>.
        /// </summary>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetShowColumnName(this DataColumn column, bool value)
        {
            column.SetExtendedProperty("ShowColumnName", value);
        }

        /// <summary>
        /// Gets the width (in characters) of this column as it would appear on the pretty printed table.
        /// </summary>
        ///
        /// <param name="column">
        /// The input column.
        /// </param>
        ///
        /// <returns>
        /// The width (in characters) of this column which is retrieved either by user defined value or the aggregate
        /// maximum width of any row in the table.
        /// </returns>
        public static int GetWidth(this DataColumn column)
        {
            if (column.ExtendedProperties.ContainsKey("Width"))
            {
                return (int)column.ExtendedProperties["Width"];
            }
            else
            {
                var columnNameLength = 1;

                if (column.GetShowColumnName())
                {
                    columnNameLength = column.ColumnName.Length;
                }

                // Linq.Max cannot handle empty sequences
                if (column.Table.Rows.Count == 0)
                {
                    return columnNameLength + 2;
                }
                else
                {
                    //return Math.Max(columnNameLength, column.Table.Rows.Cast<DataRow>().Max(r => column.GetDataTextFormat().Invoke(column, r).Length)) + 2;
                    return GetMaxColumnNameLength(columnNameLength, column);
                }
            }
        }

        private static int GetMaxColumnNameLength(int columnNameLength,DataColumn column)
        {
            int maxLength = columnNameLength;
            foreach (DataRow row in column.Table.Rows)
            {
                string cellValue = column.GetDataTextFormat().Invoke(column, row);
                if (cellValue.Length > maxLength)
                {
                    maxLength = cellValue.Length;
                }
            }
            return maxLength + 2;
        }

        /// <summary>
        /// Sets the width (in characters) of this column as it would appear on the pretty printed table.
        /// </summary>
        ///
        /// <param name="column">
        /// The input column.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set which will be clamped to be at least 1.
        /// </param>
        public static void SetWidth(this DataColumn column, int value)
        {
            value = Math.Max(1, value);

            column.SetExtendedProperty("Width", value);
        }

        /// <summary>
        /// Gets the beginning X coordinate of the data area of this column.
        /// </summary>
        ///
        /// <param name="column">
        /// The input column.
        /// </param>
        ///
        /// <returns>
        /// The X coordinate of the beginning of the data area.
        /// </returns>
        internal static int GetDataX1(this DataColumn column)
        {
            var columnIndex = column.Table.Columns.IndexOf(column);

            //return column.Table.Columns.Cast<DataColumn>().Take(columnIndex).Aggregate(0, (a, c) => a + c.GetWidth() + 1);
            return GetWidthSum(column, columnIndex);
        }

        private static int GetWidthSum(DataColumn column, int columnIndex)
        {
            int sum = 0;
            for (int i = 0; i < columnIndex; i++)
            {
                sum += column.Table.Columns[i].GetWidth() + 1;
            }
            return sum;
        }

        /// <summary>
        /// Gets the end X coordinate of the data area of this column.
        /// </summary>
        ///
        /// <param name="column">
        /// The input column.
        /// </param>
        ///
        /// <returns>
        /// The X coordinate of the end of the data area.
        /// </returns>
        internal static int GetDataX2(this DataColumn column)
        {
            return column.GetDataX1() + 1 + column.GetWidth();
        }

        /// <summary>
        /// Gets the beginning Y coordinate of the data area of this column.
        /// </summary>
        ///
        /// <param name="column">
        /// The input column.
        /// </param>
        ///
        /// <returns>
        /// The Y coordinate of the beginning of the data area.
        /// </returns>
        internal static int GetDataY1(this DataColumn column)
        {
            // Account for the top border
            var y1 = 1;

            // Account for the title and a rule following the title
            if (column.Table.GetShowTableName())
            {
                y1 += 2;
            }

            // Account for the header and a rule following the header
            if (column.Table.GetShowColumnHeader())
            {
                y1 += 2;
            }

            return y1;
        }

        /// <summary>
        /// Gets the end Y coordinate of the data area of this column.
        /// </summary>
        ///
        /// <param name="column">
        /// The input column.
        /// </param>
        ///
        /// <returns>
        /// The Y coordinate of the end of the data area.
        /// </returns>
        internal static int GetDataY2(this DataColumn column)
        {
            //return column.GetDataY1() + column.Table.Rows.Cast<DataRow>().Aggregate(0, (a, r) => a + r.GetHeight());
            return column.GetDataY1() + GetHeightSum(column);
        }

        private static int GetHeightSum(DataColumn column)
        {
            int sum = 0;
            foreach (DataRow row in column.Table.Rows)
            {
                sum += row.GetHeight();
            }
            return sum;
        }

        /// <summary>
        /// Gets an extended property from the <see cref="DataColumn"/> with a default value if it does not exist.
        /// </summary>
        ///
        /// <typeparam name="T">
        /// The type of the value to get.
        /// </typeparam>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="property">
        /// The extended property to get.
        /// </param>
        ///
        /// <param name="defaultValue">
        /// The default value to return if the extended property does not exist.
        /// </param>
        ///
        /// <returns>
        /// The value of the extended property if it exists; <paramref name="defaultValue"/> otherwise.
        /// </returns>
        internal static T GetExtendedProperty<T>(this DataColumn column, string property, T defaultValue = default)
        {
            if (column.ExtendedProperties[property] is T)
            {
                return (T)column.ExtendedProperties[property];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets an extended property from the <see cref="DataColumn"/>.
        /// </summary>
        ///
        /// <typeparam name="T">
        /// The type of the value to get.
        /// </typeparam>
        ///
        /// <param name="column">
        /// The column to pretty print.
        /// </param>
        ///
        /// <param name="property">
        /// The extended property to set.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        internal static void SetExtendedProperty<T>(this DataColumn column, string property, T value)
        {
            column.ExtendedProperties[property] = value;
        }
    }

    /// <summary>
    /// An extension class providing <see cref="DataRow"/> utility methods for pretty printing to a string.
    /// </summary>
    internal static class DataRowExtensions
    {
        /// <summary>
        /// Gets the width (in characters) of this row as it would appear on the pretty printed table by aggregating
        /// the widths of each individual column.
        /// </summary>
        ///
        /// <param name="row">
        /// The input row.
        /// </param>
        ///
        /// <returns>
        /// The width (in characters) of this row.
        /// </returns>
        internal static int GetWidth(this DataRow row)
        {
            //return row.Table.Columns.Cast<DataColumn>().Aggregate(0, (a, c) => a + c.GetWidth() + 1) - 1;
            return GetWidthSum(row);
        }

        private static int GetWidthSum(DataRow row)
        {
            int sum = 0;
            foreach (DataColumn column in row.Table.Columns)
            {
                sum += column.GetWidth() + 1;
            }
            return sum - 1;
        }

        /// <summary>
        /// Gets the height (in characters) of this row as it would appear on the pretty printed table by aggregating
        /// the heights of each individual column.
        /// </summary>
        ///
        /// <param name="row">
        /// The input row.
        /// </param>
        ///
        /// <returns>
        /// The height (in characters) of this row.
        /// </returns>
        internal static int GetHeight(this DataRow row)
        {
            return 1;
        }
    }

    /// <summary>
    /// An extension class providing <see cref="DataTable"/> utility methods for pretty printing to a string.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Gets the border around the entire table.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <returns>
        /// The border around the entire table.
        /// </returns>
        public static Border GetBorder(this DataTable table)
        {
            return table.GetExtendedProperty("Border", Border.All);
        }

        /// <summary>
        /// Sets the border around the entire table.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetBorder(this DataTable table, Border value)
        {
            table.SetExtendedProperty("Border", value);
        }

        /// <summary>
        /// Gets whether to show the column header section which shows the column names.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <returns>
        /// <c>true</c> if the column header is to be shown; <c>false</c> otherwise.
        /// </returns>
        public static bool GetShowColumnHeader(this DataTable table)
        {
            return table.GetExtendedProperty("ShowColumnHeader", true);
        }

        /// <summary>
        /// Sets whether to show the column header section which shows the column names.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetShowColumnHeader(this DataTable table, bool value)
        {
            table.SetExtendedProperty("ShowColumnHeader", value);
        }

        /// <summary>
        /// Gets whether to show the <see cref="DataTable.TableName"/>.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <returns>
        /// <c>true</c> if the table title is to be shown; <c>false</c> otherwise.
        /// </returns>
        public static bool GetShowTableName(this DataTable table)
        {
            return table.GetExtendedProperty("ShowTableName", true);
        }

        /// <summary>
        /// Sets whether to show the <see cref="DataTable.TableName"/>.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetShowTableName(this DataTable table, bool value)
        {
            table.SetExtendedProperty("ShowTableName", value);
        }

        /// <summary>
        /// Gets the text alignment of the title determined by the <see cref="DataTable.TableName"/> property.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <returns>
        /// Gets the title text alignment.
        /// </returns>
        public static TextAlignment GetTitleTextAlignment(this DataTable table)
        {
            return table.GetExtendedProperty("TitleTextAlignment", TextAlignment.Center);
        }

        /// <summary>
        /// Sets the text alignment of the title determined by the <see cref="DataTable.TableName"/> property.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        public static void SetTitleTextAlignment(this DataTable table, TextAlignment value)
        {
            table.SetExtendedProperty("TitleTextAlignment", value);
        }

        /// <summary>
        /// Converts the <see cref="DataTable"/> into pretty printed string which can be displayed on the console.
        /// </summary>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <returns>
        /// The pretty printed table.
        /// </returns>
        public static string ToPrettyPrintedString(this DataTable table)
        {
            //int x2 = table.Columns.Cast<DataColumn>().Last().GetDataX2();
            //int y2 = table.Columns.Cast<DataColumn>().Last().GetDataY2();

            int x2 = GetLastDataColumn(table).GetDataX2();
            int y2 = GetLastDataColumn(table).GetDataY2();

            char[] newLineChars = Environment.NewLine.ToCharArray();
            char[,] canvas = new char[y2 + 1, x2 + 1 + newLineChars.Length];

            // Fill the table with spaces and new lines at the end of each row
            for (var y = 0; y < y2 + 1; ++y)
            {
                for (var x = 0; x < x2 + 1; ++x)
                {
                    canvas[y, x] = ' ';
                }

                for (var i = 0; i < newLineChars.Length; ++i)
                {
                    canvas[y, x2 + 1 + i] = newLineChars[i];
                }
            }

            // Draw the table border
            canvas.DrawBorder(0, 0, x2, y2, table.GetBorder());

            // Keep track of the x and y coordinates we are drawing on
            int x1 = 0;
            int y1 = 0;

            if (table.GetShowTableName())
            {
                ++y1;

                var titleAlignment = table.GetTitleTextAlignment();
                canvas.DrawText(2, y1, x2 - 2, y1, table.TableName, titleAlignment);

                ++y1;
            }

            // Cache the column widths for performance
            //var cachedColumnWidths = table.Columns.Cast<DataColumn>().Select(c => c.GetWidth()).ToList();
            var cachedColumnWidths = GetColumnWidths(table);

            if (table.GetShowColumnHeader())
            {
                x1 = 0;

                for (var i = 0; i < table.Columns.Count; ++i)
                {
                    // Draw the header border
                    canvas.DrawBorder(x1, y1, x1 + cachedColumnWidths[i] + 1, y1 + 2, table.Columns[i].GetHeaderBorder());

                    if (table.Columns[i].GetShowColumnName())
                    {
                        // Draw the header name
                        canvas.DrawText(x1 + 2, y1 + 1, x1 + 1 + cachedColumnWidths[i] - 2, y1 + 1, table.Columns[i].ColumnName, table.Columns[i].GetColumnNameAlignment());
                    }

                    x1 += cachedColumnWidths[i] + 1;
                }

                y1 += 2;
            }

            x1 = 0;

            for (var i = 0; i < table.Columns.Count; ++i)
            {
                // Draw the data border
                canvas.DrawBorder(x1, y1, x1 + cachedColumnWidths[i] + 1, y1 + table.Rows.Count + 1, table.Columns[i].GetDataBorder());

                x1 += cachedColumnWidths[i] + 1;
            }

            ++y1;

            for (var i = 0; i < table.Rows.Count; ++i)
            {
                x1 = 2;

                for (var j = 0; j < table.Columns.Count; ++j)
                {
                    var dataText = table.Columns[j].GetDataTextFormat().Invoke(table.Columns[j], table.Rows[i]);

                    // Draw the cell text
                    canvas.DrawText(x1, y1, x1 + cachedColumnWidths[j] - 3, y1, dataText, table.Columns[j].GetDataAlignment());

                    x1 += cachedColumnWidths[j] + 1;
                }

                ++y1;
            }

            var buffer = new char[canvas.GetLength(0) * canvas.GetLength(1)];
            Buffer.BlockCopy(canvas, 0, buffer, 0, buffer.Length * sizeof(char));

            return new string(buffer);
        }


        private static DataColumn GetLastDataColumn(DataTable table)
        {
            if (table.Columns.Count > 0)
            {
                DataColumn lastColumn = table.Columns[table.Columns.Count - 1];
                return lastColumn;
            }
            else
            {
                // 如果表格没有列，返回null或者抛出异常  
                return null; // 或者 throw new ArgumentException("Table must have at least one column.");  
            }
        }

        private static List<int> GetColumnWidths(DataTable table)
        {
            List<int> columnWidths = new List<int>();
            foreach (DataColumn column in table.Columns)
            {
                columnWidths.Add(column.GetWidth());
            }
            return columnWidths;
        }

        /// <summary>
        /// Gets an extended property from the <see cref="DataTable"/> with a default value if it does not exist.
        /// </summary>
        ///
        /// <typeparam name="T">
        /// The type of the value to get.
        /// </typeparam>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <param name="property">
        /// The extended property to get.
        /// </param>
        ///
        /// <param name="defaultValue">
        /// The default value to return if the extended property does not exist.
        /// </param>
        ///
        /// <returns>
        /// The value of the extended property if it exists; <paramref name="defaultValue"/> otherwise.
        /// </returns>
        internal static T GetExtendedProperty<T>(this DataTable table, string property, T defaultValue = default)
        {
            if (table.ExtendedProperties[property] is T)
            {
                return (T)table.ExtendedProperties[property];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Sets an extended property from the <see cref="DataTable"/>.
        /// </summary>
        ///
        /// <typeparam name="T">
        /// The type of the value to get.
        /// </typeparam>
        ///
        /// <param name="table">
        /// The table to pretty print.
        /// </param>
        ///
        /// <param name="property">
        /// The extended property to set.
        /// </param>
        ///
        /// <param name="value">
        /// The value to set.
        /// </param>
        internal static void SetExtendedProperty<T>(this DataTable table, string property, T value)
        {
            table.ExtendedProperties[property] = value;
        }
    }

    /// <summary>
    /// Enumerates the alignment of the text represented by a <see cref="DataTable"/> which is to be pretty printing to
    /// a string.
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Text is centered.
        /// </summary>
        Center,

        /// <summary>
        /// Text is aligned to the left.
        /// </summary>
        Left,

        /// <summary>
        /// Text is aligned to the right.
        /// </summary>
        Right,
    }

    /// <summary>
    /// A utility class with miscellaneous methods.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Swaps the values of <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        ///
        /// <typeparam name="T">
        /// The type of the values to swap.
        /// </typeparam>
        ///
        /// <param name="x">
        /// The value to swap with <paramref name="y"/>.
        /// </param>
        ///
        /// <param name="y">
        /// The value to swap with <paramref name="x"/>.
        /// </param>
        internal static void Swap<T>(ref T x, ref T y)
        {
            T z = x;

            x = y;
            y = z;
        }
    }
}
