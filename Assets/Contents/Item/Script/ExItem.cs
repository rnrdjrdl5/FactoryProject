public class ExItem
{
    public Item Item => item;
    public bool IsEquip => isEquip;
    
    Item item;
    bool isEquip;

    public static ExItem Create(Item item, bool isEquip)
    {
        var exItem = new ExItem();
        exItem.item = item;
        exItem.isEquip = isEquip;

        return exItem;
    }
}