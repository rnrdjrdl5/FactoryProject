using System.Collections.Generic;
using System.Linq;

public class ItemList
{
     public IEnumerable<Item> Items => items;
     
     IEnumerable<Item> items;

     public static ItemList Create(IEnumerable<Item> items)
     {
          var itemList = new ItemList();
          itemList.items = items;

          return itemList;
     }
}