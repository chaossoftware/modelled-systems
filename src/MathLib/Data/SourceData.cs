﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MathLib.IO;

namespace MathLib.Data
{
    public class SourceData
    {
        private const string NumberRegex = "\\s+";

        private double[,] dataColumns;

        public SourceData(string filePath, int startOffset, int readLines)
        {
            ReadFromFile(filePath, startOffset, readLines);

            FileName = Path.GetFileName(filePath);
            Folder = Path.GetDirectoryName(filePath);
            LinesCount = dataColumns.GetLength(0);
            ColumnsCount = dataColumns.GetLength(1);

            SetTimeSeries(0, 0, LinesCount, 1, false);
        }

        public SourceData(string filePath) : this (filePath, 0, -1)
        {
        }

        public Timeseries TimeSeries { get; protected set; }

        public int LinesCount { get; protected set; }

        public int ColumnsCount { get; protected set; }

        public double Step { get; protected set; }

        public string FileName { get; protected set; }

        public string Folder { get; protected set; }

        /// <summary>
        /// Set current time series from column and data range.
        /// </summary>
        /// <param name="colIndex">index of column</param>
        /// <param name="startPoint">start point for time series</param>
        /// <param name="endPoint">end point for time series</param>
        /// <param name="pts">use each N point from range</param>
        /// <param name="timeInFirstColumn">specify whether to use first column values as time or not</param>
        public void SetTimeSeries(int colIndex, int startPoint, int endPoint, int pts, bool timeInFirstColumn)
        {
            int max = (endPoint - startPoint) / pts;
            TimeSeries = new Timeseries();

            for (int i = 0; i < max; i++)
            {
                int row = startPoint + i * pts;
                var x = timeInFirstColumn ? dataColumns[row, 0] : i + 1;
                var y = dataColumns[row, colIndex];
                TimeSeries.AddDataPoint(x, y);
            }
                    
            Step = TimeSeries.DataPoints[1].X - TimeSeries.DataPoints[0].X;
        }

        public string GetTimeSeriesString()
        {
            var timeSeriesOut = new StringBuilder();

            foreach (double value in TimeSeries.YValues)
            {
                timeSeriesOut.AppendLine(value.ToString(NumFormat.General, CultureInfo.InvariantCulture));
            }

            return timeSeriesOut.ToString();
        }

        public override string ToString() =>
            $"File: {FileName}\nLines: {LinesCount}\nColumns: {ColumnsCount}";

        private void ReadFromFile(string file, int startOffset, int readLines)
        {
            int i, j;

            var sourceData = File.ReadAllLines(file);

            // Determine how many numbers in line.
            var columns = Regex.Split(sourceData[startOffset].Trim(), NumberRegex).Length;

            var length = readLines == -1 ? sourceData.Length - startOffset : readLines;

            dataColumns = new double[length, columns];

            for (i = startOffset; i < length + startOffset; i++)
            {
                var numbers = Regex.Split(sourceData[i].Trim(), NumberRegex);

                for (j = 0; j < columns; j++)
                {
                    if (double.TryParse(numbers[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        dataColumns[i - startOffset, j] = value;
                    }
                    else
                    {
                        throw new ArgumentException($"Unable to parse value (Line: {i + 1}, Column: {j} [value: {numbers[j]}])");
                    }
                }
            }
        }
    }
}
