﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IndustryThing.Output
{
    class MarketInfo
    {
        public MarketInfo(db.Db dataBase, calculator.T2Builder t2mods, ApiImport.MainImport import, Market.Market market)
        {
          //  StreamWriter sw = new StreamWriter("C:\\Users\\PCPCPC\\Google Drive\\Eve\\marketInfo.txt"); // use this once i work out how to import a txt into a google spreadsheet
            StreamWriter sw = new StreamWriter("marketInfo.txt");
            ApiImport.ContainerII office = import.buildCorpAssets.assets.GetContainer("1022964286749");
            int i = 0;

            sw.WriteLine("Name" + "\t" + "Buildcost" + "\t" + "Haulingcost" + "\t" + "Market sell" + "\t" + "Amount on market" + "\t" + "Stock in xanadu" + "\t" + "Stock in chanuur"
                + "\t" + "Order price" + "\t" + "Avg daily volume" + "\t" + "Item category" + "\t" + "Max output");
            while (i < t2mods.OutputName.Length)
            {
                sw.WriteLine(
                    t2mods.OutputName[i] //name
                  + "\t" + t2mods.OutputTotalCost[i] / t2mods.Output[i, 1] //cost per item
                 + "\t" + dataBase.types.GetRepackagedVolume(t2mods.Output[i, 0]) * 800 // hauling cost per item (800 is ITL's price per m3 from delve to jita, hardcoding it because im lazy
               + "\t" + market.FindPrice(dataBase.settings.MarketRegion, "sell", t2mods.Output[i, 0]) // gets the sale value of the item
               + "\t" + import.marketOrders.ItemsOnMarket(t2mods.Output[i,0]) //amount we have on the market
                + "\t" + import.empireDonkey.assets.FindItem(t2mods.Output[i,0]) // amount on Reluah
                   + "\t" + office.FindItem(t2mods.Output[i,0]) // ammount on chanuur
                   + "\t" + import.marketOrders.SellOrderPrice(t2mods.Output[i, 0]) // value of our sell order
                    + "\t" + market.FindAverageVolume(dataBase.settings.MarketRegion,t2mods.Output[i,0], 30) // average volume sold per day(last 30 days)
                    +"\t"+ dataBase.categoryIDs.GetName(dataBase.groupIDs.CategoryID(dataBase.types.GroupID(t2mods.Output[i,0])),0) // items category
                    +"\t"+ t2mods.Output[i,1] //amount of items produce per cycle
                    );
                i++;
            }
            sw.WriteLine("market orders cached until" + "\t" + import.marketOrders.CachedUntil);
            sw.WriteLine("build corp assets cached until" + "\t" + import.buildCorpAssets.CachedUntil);
            sw.WriteLine("empire donkey corp assets cached until" + "\t" + import.empireDonkey.CachedUntil);
            sw.Close();
            System.Diagnostics.Process.Start(@"marketInfo.txt");

        }
    }
}

/*
            string[] name = t2builder.OutputName;
            int[,] output = t2builder.Output;
            decimal[] totalCost = t2builder.OutputTotalCost;
            decimal[] totalValue = t2builder.OutputTotalValue;

            sw.WriteLine("<table>");
            sw.WriteLine("<caption><b>" + tableName + "</b></caption>");
            sw.WriteLine("<tr><td>Name</td><td>Amount</td><td>Value</td><td>Cost</td><td>Profit</td><td>Profit/m^3 ratio</td></tr>");
            int i = 0;
            while (i < name.Length)
            {
                decimal profit = totalValue[i] - totalCost[i];
                decimal profitRatio = profit / (dataBase.types.GetRepackagedVolume(output[i, 0]) * output[i, 1]);
                sw.WriteLine("<tr><td>" + 
                    string.Format("{0:n0}", name[i]) + "</td><td>" +
                    string.Format("{0:n0}", output[i, 1]) + "</td><td>" +
                    string.Format("{0:n0}", totalValue[i]) + "</td><td>" +
                    string.Format("{0:n0}", totalCost[i]) + "</td><td>" +
                    string.Format("{0:n0}", profit) + "</td><td>" +
                    string.Format("{0:n0}", profitRatio) + "</td></tr>");
                i++;
            }
            decimal valueSum = totalValue.Sum();
            decimal costSum = totalCost.Sum();
            i = 0;
            decimal profitSum = 0;
            while (i < name.Length)
            {
                if ((totalValue[i] - totalCost[i]) > 0) profitSum += totalValue[i] - totalCost[i];
                i++;
            }
            sw.WriteLine("<tr><td><b>Sum</b></td><td></td><td><b>" + string.Format("{0:n0}", valueSum) + "</b></td><td><b>" + string.Format("{0:n0}", costSum) + "</b></td><td><b>" + string.Format("{0:n0}", profitSum) + "</b></td></b><td></td></tr>");
            sw.WriteLine("</table>");
*/