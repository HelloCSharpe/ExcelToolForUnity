public class Table
{
    public static void Init()
    {
        TableManager.Instance.LoadTable<Table_Plot>("Tables/Plot");
        TableManager.Instance.LoadTable<Table_Shop>("Tables/Shop");
    }
}
