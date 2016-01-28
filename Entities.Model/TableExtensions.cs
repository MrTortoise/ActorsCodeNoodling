using System;
using System.Collections.Generic;
using System.Linq;
using Entities.DataStructures;
using Entities.Inventory;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    public static class TableExtensions
    {
        public static string GetField(this Table table, string fieldName)
        {
            return table.Rows.Single(r => r["Field"] == fieldName)["Value"];
        }

        public static InventoryType[] GetInventoryTypes(this Table table)
        {
            var inventoryTypes = new List<InventoryType>();
            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["Name"];
                int capacity = int.Parse(tableRow["Capacity"]);
                CargoSize size = (CargoSize) Enum.Parse(typeof (CargoSize), tableRow["CargoSize"]);

                var inventoryType = new InventoryType(name, capacity, size);
                inventoryTypes.Add(inventoryType);
            }

            return inventoryTypes.ToArray();
        }

        public static Point2Int[] GetPoints(this Table table)
        {
            var points = new Point2Int[table.RowCount];
            for (int i = 0; i < table.RowCount; i++)
            {
                int x = int.Parse(table.Rows[i]["X"]);
                int y = int.Parse(table.Rows[i]["Y"]);
                var point = new Point2Int(x,y);
                points[i] = point;
            }

            return points;
        }
    }
}