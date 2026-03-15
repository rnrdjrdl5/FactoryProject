public class ExItem
{
    public Item Item => item;
    
    Item item;

    public static ExItem Create(Item item)
    {
        var exItem = new ExItem();
        exItem.item = item;

        return exItem;
    }
}