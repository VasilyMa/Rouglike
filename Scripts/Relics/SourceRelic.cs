[System.Serializable]
public abstract class SourceRelic : IDissolvable
{
    public abstract void InvokeRelic();
    public virtual void Dissolve()
    {
        PlayerEntity.Instance.Currency.Favour++;
        //todo add currency
    }
}
