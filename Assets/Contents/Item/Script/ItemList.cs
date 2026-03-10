using System.Collections.Generic;
using System.Linq;

public class ItemList
{
     public IEnumerable<string> ItemKeys => itemKeys;
     
     IEnumerable<string> itemKeys;

     public static ItemList Create(IEnumerable<string> keys)
     {
          var itemList = new ItemList();
          itemList.itemKeys = keys;

          return itemList;
     }
}