using System.Collections.Generic;
using System.Linq;

public class ExItemList
{
     public IEnumerable<ExItem> ExItems => exItems;
     
     IEnumerable<ExItem> exItems;

     public static ExItemList Create(IEnumerable<ExItem> exItems)
     {
          var itemList = new ExItemList();
          itemList.exItems = exItems;

          return itemList;
     }
}