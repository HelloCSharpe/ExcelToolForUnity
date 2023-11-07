public abstract class ITable
{
    public virtual void Load(string[] columns) { }

    public virtual void OnLoaded() { }
}
