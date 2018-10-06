﻿namespace BlazorComponents.ChartJS
{
    public class ChartTypes
    {
        //bar,
        //line,
        //pie,
        //radar,
        //bubble

        public static ChartTypes BAR = new ChartTypes("bar");
        public static ChartTypes LINE = new ChartTypes("line");
        public static ChartTypes PIE = new ChartTypes("pie");
        public static ChartTypes RADAR = new ChartTypes("radar");
        public static ChartTypes BUBBLE = new ChartTypes("bubble");

        private readonly string _chartType;

        private ChartTypes(string chartType)
        {
            _chartType = chartType;
        }

        public override string ToString()
        {
            return _chartType;
        }
    }
}