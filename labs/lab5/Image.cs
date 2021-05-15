using ScottPlot;
using System.Collections.Generic;
using System;

namespace lab5
{
    public static class Image
    {
        
        public struct MarketSegmentation
        {
            public int building;
            public int automobile;
            public int household;
            public int furniture;
            public int machinery;

        }

        public static  MarketSegmentation CountCustomersInCategory(List<Customer> customers)
        {
            MarketSegmentation segmentation = new MarketSegmentation();
            for (int i = 0; i < customers.Count; i++)
            {
                Customer customer = customers[i];
                switch (customer.marketSegmentation)
                {
                    case "HOUSEHOLD":
                        segmentation.household++;
                        break;
                    case "AUTOMOBILE":
                        segmentation.automobile++;
                        break;
                    case "BUILDING":
                        segmentation.building++;
                        break;
                    case "MACHINERY":
                        segmentation.machinery++;
                        break;
                    case "FURNITURE":
                        segmentation.furniture++;
                        break;
                }

            }
            return segmentation;

        }

        public static ScottPlot.Plot FormBarGraph(MarketSegmentation segmentation)
        {
            ScottPlot.Plot plt = new ScottPlot.Plot(600, 400);
            string[] labels = {"household", "automobile", "building", "machinery", "furniture"};
            int barCount = labels.Length;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(barCount);
            double[] ys = new double[] { segmentation.household, segmentation.automobile, segmentation.building, segmentation.machinery, segmentation.furniture };
            double[] yError = DataGen.RandomNormal(rand, barCount, 50, 10);
            plt.PlotBar(xs, ys, yError);
            plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
            plt.XTicks(xs, labels);
            return plt;
        
        }
    }
}